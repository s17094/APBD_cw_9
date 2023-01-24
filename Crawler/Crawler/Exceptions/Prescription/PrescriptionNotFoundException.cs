using Microsoft.AspNetCore.Mvc;

using Crawler.Models.DTO;

namespace Crawler.Exceptions.Prescription;

public class PrescriptionNotFoundException : PrescriptionException
{

    private readonly int _prescriptionId;

    public PrescriptionNotFoundException(int prescriptionId)
    {
        _prescriptionId = prescriptionId;
    }

    protected internal override IActionResult GetResponse()
    {
        var message = new ErrorMessageDto("Not found prescription with id = " + _prescriptionId);
        return new ObjectResult(message)
        {
            StatusCode = 404
        };
    }
}