using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Helpers;

namespace BLL.Services
{
    public interface IAccountService
    {
        Task<LogInResponse> LogIn(LogInRequest logInRequest);
        void Test(ClaimsPrincipal user);
        Task<SuccessResponse> LogOut(ClaimsPrincipal user);
        Task<LogInResponse> RefreshToken(RefreshTokenRequest refreshToken);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly TaposRSA _taposRSA;
        private readonly IDistributedCache _distributedCache;

        public AccountService(UserManager<AppUser> userManager, IConfiguration config,TaposRSA taposRSA,IDistributedCache distributedCache)
        {
            this._userManager = userManager;
            this._config = config;
            this._taposRSA = taposRSA;
            this._distributedCache = distributedCache;
        }
        public async Task<LogInResponse> LogIn(LogInRequest logInRequest)
        {
            AppUser user = await _userManager.FindByNameAsync(logInRequest.UserName);

            if (user == null)
            {
                throw new ExceptionManagementHelper("user not found");   
            }

            var matchUser = await _userManager.CheckPasswordAsync(user, logInRequest.Password);

            if (!matchUser)
            {
                throw new ExceptionManagementHelper("username/password missmatch");
            }
            return await GenerateJSONWebTokenAsync(user);
        }

        public async Task<SuccessResponse> LogOut(ClaimsPrincipal user)
        {
            var userid = user.FindFirst(c=>c.Type=="userid")?.Value;

            var accessTokenKey = userid.ToString() + "_accessToken";
            var refreshTokenKey = userid.ToString() + "_refreshToken";

            await _distributedCache.RemoveAsync(accessTokenKey);
            await _distributedCache.RemoveAsync(refreshTokenKey);

            return new SuccessResponse()
            {
                Message = "Successfully Logout",
                StatusCode = HttpStatusCode.OK.ToString(),
                //DeveloperMessage

            };
        }

        public async Task<LogInResponse> RefreshToken(RefreshTokenRequest refreshToken)
        {
            var decryptRSA = _taposRSA.Decrypt(refreshToken.Token, "v1");

            if (decryptRSA == null)
            {
                throw new ExceptionManagementHelper("Refresh Token Not Found");

            }

            var refreshTokenObj = JsonConvert.DeserializeObject<RefreshTokenResponse>(decryptRSA);

            var refreshTokenKey = refreshTokenObj.UserId.ToString() + "_refreshToken";

            var cacheData = await _distributedCache.GetStringAsync(refreshTokenKey);

            if (cacheData == null)
            {
                throw new ExceptionManagementHelper("Refresh Token Not Found");
            }

            if (cacheData != refreshToken.Token)
            {
                throw new ExceptionManagementHelper("Refresh Token Not Found");
            }

            var user= await _userManager.FindByIdAsync(refreshTokenObj.UserId.ToString());

            return await GenerateJSONWebTokenAsync(user);

        }

        public void Test(ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var userName = user.FindFirst(CustomJwtClaimName.UserName)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            var email = user.FindFirst(CustomJwtClaimName.Email)?.Value;
            var mm = user.Claims.FirstOrDefault(x => x.Type == "mm")?.Value;
        }

        private async Task<LogInResponse> GenerateJSONWebTokenAsync(AppUser userInfo)
        {
            var response = new LogInResponse();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userRole = (await _userManager.GetRolesAsync(userInfo)).FirstOrDefault();

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,userInfo.Id.ToString()),
                new Claim(CustomJwtClaimName.UserName,userInfo.UserName.ToString()),
                new Claim(CustomJwtClaimName.Email,userInfo.Email.ToString()),
                new Claim(CustomJwtClaimName.UserId,userInfo.Id.ToString()),
                new Claim(ClaimTypes.Role, userRole)
            };

            var accessTokenLifeTime = _config.GetValue<int>("Jwt:AccessTokenLifeTime");

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims:claims,
              expires: DateTime.Now.AddMinutes(accessTokenLifeTime),
              signingCredentials: credentials);

            var refreshTokenResponse = new RefreshTokenResponse()
            {
                UserId = userInfo.Id,
                ID = Guid.NewGuid().ToString()
            };
            var encRefTokenResponse = _taposRSA.EncryptData(JsonConvert.SerializeObject(refreshTokenResponse), "v1");

            response.Token= new JwtSecurityTokenHandler().WriteToken(token);
            response.Expire = accessTokenLifeTime * 60;
            response.RefreshToken = encRefTokenResponse;

            await StoreTokenInformation(userInfo.Id, response.Token, response.RefreshToken);

            return response;
        }


        private async Task StoreTokenInformation(long userid, string accessToken, string refreshToken)
        {
            var accessTokenOption = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.GetValue<int>("Jwt:AccessTokenLifeTime")));
            var refreshTokenOption = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.GetValue<int>("Jwt:RefreshTokenLifeTime")));

            var accessTokenKey = userid.ToString() + "_accessToken";
            var refreshTokenKey = userid.ToString() + "_refreshToken";

            await _distributedCache.SetStringAsync(accessTokenKey,accessToken,accessTokenOption);
            await _distributedCache.SetStringAsync(refreshTokenKey, refreshToken, refreshTokenOption);
        }
    }
}
