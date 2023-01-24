using Crawler.Context;
using Crawler.Exceptions.Prescription;
using Crawler.Models;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly MainDbContext _context;

    public PrescriptionService(MainDbContext context)
    {
        _context = context;
    }

    public async Task<Prescription> GetPrescription(int id)
    {
        var prescription = await _context.Prescriptions
            .Where(p => p.IdPrescription == id)
            .Include(p => p.Patient)
            .Include(p => p.Doctor)
            .Include(p => p.Medicaments)
            .FirstOrDefaultAsync();

        if (prescription == null)
        {
            throw new PrescriptionNotFoundException(id);
        }

        return prescription;
    }
}