using Crawler.Models;

namespace Crawler.Services;

public interface IPrescriptionService
{
    Task<Prescription> GetPrescription(int id);
}