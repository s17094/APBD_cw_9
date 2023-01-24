using Microsoft.AspNetCore.Mvc.Filters;

using Crawler.Exceptions.Doctor;
using Crawler.Exceptions.Prescription;

namespace Crawler.Filters;

public class RestExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DoctorException doctorException)
        {
            context.Result = doctorException.GetResponse();
        }
        if (context.Exception is PrescriptionException prescriptionException)
        {
            context.Result = prescriptionException.GetResponse();
        }
    }
}