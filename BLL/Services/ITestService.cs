using System;
using System.Threading.Tasks;
using DLL.Model;
using DLL.Repository;
using DLL.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public interface ITestService
    {
        Task SaveAllData();
        Task UpdateBalance();
    }

    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;


        public TestService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task UpdateBalance()
        {
            Random random = new Random();
            int amount = random.Next(-100, 100);

            Order objOrder = new Order()
            {
                Amount = amount
            };

            await _unitOfWork.OrderRepository.InsertAsync(objOrder);
            
            if(await _unitOfWork.DbSaveChangeAsync())
            {
               await _unitOfWork.CustomerBalanceRepository.UpdateCustomerBalanceAsync(amount);
                //var custbalance = await _unitOfWork.CustomerBalanceRepository.GetSingleAsync(x => x.CustomerBalanceId == 1);
                //custbalance.Balance += amount;
                //_unitOfWork.CustomerBalanceRepository.Update(custbalance);
                //await _unitOfWork.DbSaveChangeAsync();
            }

           
        }

        public async Task SaveAllData()
        {

            AppUser user = null;
            AppRole role = null;

            for (int i=1; i < 6; i++)
            {
                user = new AppUser()
                {
                    UserName = "test" + i + "@gmail.com",
                    Email = "test" + i + "@gmail.com",
                };
                 var result = await _userManager.CreateAsync(user, "tapos1234$..T");
               

                if (result.Succeeded)
                {
                    
                    if (i % 2 == 0)
                    {
                         role = await _roleManager.FindByNameAsync("teacher");

                        if (role == null)
                        {
                            await _roleManager.CreateAsync(new AppRole()
                            {
                                Name = "teacher"
                            });
                            role = new AppRole()
                            {
                                Name = "teacher"
                            };
                        }
                    }
                    else
                    {
                        role = await _roleManager.FindByNameAsync("staff");

                        if (role == null)
                        {
                            await _roleManager.CreateAsync(new AppRole()
                            {
                                Name = "staff"
                            });
                            role = new AppRole()
                            {
                                Name = "staff"
                            };
                        }
                        
                    }
                   

                }

                await _userManager.AddToRoleAsync(user, role.Name);
            }

            //var user = new AppUser()
            //{

            //    UserName = "a.aa@gmail.com",
            //    Email = "a.aa@gmail.com"
            //};
            //var result = await _userManager.CreateAsync(user, "tapos1234$..T");

            //if (result.Succeeded)
            //{
            //    var role = await _roleManager.FindByNameAsync("teacher");

            //    if (role == null)
            //    {
            //        await _roleManager.CreateAsync(new AppRole()
            //        {
            //            Name = "teacher"
            //        });
            //    }
            //}

            //await _userManager.AddToRoleAsync(user, "teacher");

            //var user1 = new AppUser()
            //{

            //    UserName = "b.aa@gmail.com",
            //    Email = "b.aa@gmail.com"
            //};
            //var result1 = await _userManager.CreateAsync(user1, "tapos1234$..T");

            //if (result1.Succeeded)
            //{
            //    var role = await _roleManager.FindByNameAsync("staff");

            //    if (role == null)
            //    {
            //        await _roleManager.CreateAsync(new AppRole()
            //        {
            //            Name = "staff"
            //        });
            //    }
            //}

            //await _userManager.AddToRoleAsync(user1, "staff");

        }

        
    }
}