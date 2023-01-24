using Crawler.Context;
using Crawler.Exceptions.Doctor;
using Crawler.Models;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Services;

public class DoctorsService : IDoctorsService
{

    private readonly MainDbContext _context;

    public DoctorsService(MainDbContext context)
    {
        _context = context;
    }

    public async Task<HashSet<Doctor>> GetDoctors()
    {
        var doctors = await _context.Doctors
            .Include(d => d.Prescriptions)
            .ToListAsync();

        return doctors.ToHashSet();
    }

    public async Task<Doctor> GetDoctor(int id)
    {
        var doctor = await _context.Doctors
            .Where(d => d.IdDoctor == id)
            .Include(d => d.Prescriptions)
            .FirstOrDefaultAsync();

        if (doctor == null)
        {
            throw new DoctorNotFoundException(id);
        }

        return doctor;
    }

    public async Task<Doctor> CreateDoctor(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();

        return doctor;
    }

    public async Task<Doctor> UpdateDoctor(int id, Doctor updatedDoctor)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.IdDoctor == id);

        if (doctor == null)
        {
            throw new DoctorNotFoundException(id);
        }

        doctor.FirstName = updatedDoctor.FirstName;
        doctor.LastName = updatedDoctor.LastName;
        doctor.Email = updatedDoctor.Email;
        doctor.Prescriptions = updatedDoctor.Prescriptions;

        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();

        return doctor;
    }

    public async Task<Doctor> DeleteDoctor(int id)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.IdDoctor == id);

        if (doctor == null)
        {
            throw new DoctorNotFoundException(id);
        }

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();

        return doctor;
    }
}