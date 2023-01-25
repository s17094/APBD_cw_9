using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Crawler.Services;

namespace Crawler.Controllers;

[Authorize]
[Route("api/prescriptions")]
[ApiController]
public class PrescriptionController : ControllerBase
{

    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescription(int id)
    {
        var prescription = await _prescriptionService.GetPrescription(id);
        return Ok(prescription);
    }
}