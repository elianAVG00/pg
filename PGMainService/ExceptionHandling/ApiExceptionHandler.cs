using System.Text;
using System.Web.Configuration;
using System.Web.Http.ExceptionHandling;
using PGMainService.Exceptions;
using PGMainService.Results;

namespace PGMainService.ExceptionHandling
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var baseException = context.Exception.GetBaseException();
            var apiException = baseException as ApiException;

            if (apiException != null)
            {
                context.Result = new BadRequestCustomApiResult(
                    apiException,
                    Encoding.UTF8,
                    context.Request);
            }
            else
            {
                string showerrorbool = WebConfigurationManager.AppSettings["ShowCriticalError"];
                string errorToShow = "";
                if (showerrorbool == "on")
                {
                    if (baseException.InnerException == null)
                    {
                        errorToShow = baseException.Message.ToString() + "";
                    }
                    else
                    {
                        errorToShow = baseException.Message.ToString() + baseException.Message.ToString();
                    }

                }
                else
                {
                    errorToShow =
                        "Ocurrió un error en el proceso. Por favor contáctese con el administrador del sistema.";
                }

                context.Result = new InternalServerErrorResult(
                    errorToShow,
                    Encoding.UTF8,
                    context.Request);
            }
        }
    }
}