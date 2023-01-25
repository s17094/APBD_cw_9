using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Crawler.Models;
using Crawler.Services;

namespace Crawler.Controllers;

[Authorize]
[Route("api/doctors")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorsService _doctorsService;

    public DoctorsController(IDoctorsService doctorsService)
    {
        _doctorsService = doctorsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors()
    {
        var doctor = await _doctorsService.GetDoctors();
        return Ok(doctor);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctor(int id)
    {
        var doctor = await _doctorsService.GetDoctor(id);
        return Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor(Doctor doctor)
    {
        var newDoctor = await _doctorsService.CreateDoctor(doctor);
        return Ok(newDoctor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(int id, Doctor doctor)
    {
        var updatedDoctor = await _doctorsService.UpdateDoctor(id, doctor);
        return Ok(updatedDoctor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var deletedDoctor = await _doctorsService.DeleteDoctor(id);
        return Ok(deletedDoctor);
    }

}