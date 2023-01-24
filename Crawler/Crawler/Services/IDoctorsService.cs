using Crawler.Models;

namespace Crawler.Services;

public interface IDoctorsService
{
    Task<HashSet<Doctor>> GetDoctors();
    Task<Doctor> GetDoctor(int id);
    Task<Doctor> CreateDoctor(Doctor doctor);
    Task<Doctor> UpdateDoctor(int id, Doctor doctor);
    Task<Doctor> DeleteDoctor(int id);
}