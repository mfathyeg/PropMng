using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.IO;

namespace PropMng.Api.Infrastructure
{
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {
        
        public override void OnException(ExceptionContext context)
        {
            var desc = (ControllerActionDescriptor)context.ActionDescriptor;
            var error = new  
            {
                CreatedDate = DateTime.Now,
                UserName = context.HttpContext.User.Identity?.Name,
                ControllerCode = desc.ControllerName,
                ActionCode = desc.ActionName,
                Details = JsonConvert.SerializeObject(context.Exception)
            };

           
                var dt = DateTime.Now;
                var path = $"D:/Errors/PropManagerErrors";
                Directory.CreateDirectory(path);

                var txt = $"{path}/{dt.Hour:00}{dt.Minute:00}{dt.Second:00}{dt.Millisecond:000}_{error.UserName}.txt";
                File.AppendAllText(txt, JsonConvert.SerializeObject(error));
        }
    }
}
