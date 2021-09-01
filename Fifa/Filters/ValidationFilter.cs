using Fifa.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Filters
{
    public  class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                var erroresinmodel = context.ModelState
                    .Where(er => er.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

                var errorresponse = new ErrorResponse();
                foreach(var error in erroresinmodel)
                {
                    foreach(var suberror in error.Value)
                    {
                        var errorModel = new ErrorModel
                        {
                            FieldName = error.Key,
                            Messsage = suberror
                        };
                        errorresponse.Errors.Add(errorModel);
                    }
                }

                context.Result = new BadRequestObjectResult(errorresponse);
                return;
            }

            await next();
        }
    }
}
