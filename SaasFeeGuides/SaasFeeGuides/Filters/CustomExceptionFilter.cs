using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SaasFeeGuides.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CustomExceptionFilterAttribute(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {

            var detaiiledMessage = string.Empty;
            if (_hostingEnvironment.IsDevelopment())
            {
                detaiiledMessage = context.Exception?.ToString();                
            }
            if (context.Exception is Exceptions.BadRequestException ex)
            {
                context.Result = new ObjectResult(
                    new
                    {
                        message = ex.Message,
                        detailedMessage = detaiiledMessage
                    })
                {
                    StatusCode = ex.ErrorCode
                };
                return;
            }
            context.Exception?.Trace();
            var result = new ObjectResult(
                   new
                   {
                       message = "Internal server error",
                       detailedMessage = detaiiledMessage
                   })
            {
                StatusCode = 500
            };

            context.Result = result;
        }
    }
}
