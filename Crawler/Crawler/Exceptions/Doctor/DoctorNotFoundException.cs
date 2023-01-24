using Microsoft.AspNetCore.Mvc;

using Crawler.Models.DTO;

namespace Crawler.Exceptions.Doctor;

public class DoctorNotFoundException : DoctorException
{

    private readonly int _doctorId;

    public DoctorNotFoundException(int doctorId)
    {
        _doctorId = doctorId;
    }

    protected internal override IActionResult GetResponse()
    {
        var message = new ErrorMessageDto("Not found doctor with id = " + _doctorId);
        return new ObjectResult(message)
        {
            StatusCode = 404
        };
    }
}