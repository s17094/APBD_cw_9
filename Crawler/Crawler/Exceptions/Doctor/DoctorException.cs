using Microsoft.AspNetCore.Mvc;

namespace Crawler.Exceptions.Doctor;

public abstract class DoctorException : Exception
{
    protected internal abstract IActionResult GetResponse();
}