using Microsoft.AspNetCore.Mvc;

namespace Crawler.Exceptions.Prescription;

public abstract class PrescriptionException : Exception
{
    protected internal abstract IActionResult GetResponse();
}