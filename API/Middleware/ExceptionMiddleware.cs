using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utility;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await MasterException(ex,context);
            }
            // Call the next delegate/middleware in the pipeline
            
        }

        private async Task MasterException(Exception ex, HttpContext context)
        {
            var code = HttpStatusCode.InternalServerError;
            var error = new ErrorResponse();
            error.StatusCode = code.ToString();
            error.Message = ex.Message;
            error.DeveloperMessage = ex.StackTrace;

            switch (ex)
            {
                case ExceptionManagementHelper exception:
                    {
                        error.StatusCode = HttpStatusCode.NotFound.ToString();
                        break;
                    } 
                case UnauthorizedAccessException unauthorizedAccessException :
                    {
                        error.StatusCode = HttpStatusCode.Unauthorized.ToString();
                        error.Message = unauthorizedAccessException.Message;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(result);
        }
    }
}
