using Microsoft.EntityFrameworkCore;

using Crawler.Models;

namespace Crawler.Context;

public class MainDbContext : DbContext
{
    public MainDbContext()
    {

    }

    public MainDbContext(DbContextOptions options) : base(options)
    {

    }

    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Medicament> Medicaments { get; set; }
    public virtual DbSet<Patient> Patients { get; set; }
    public virtual DbSet<Prescription> Prescriptions { get; set; }
    public virtual DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>(opt =>
        {
            opt.HasData(
                new Doctor { IdDoctor = 1, FirstName = "Maciej", LastName = "Bobrowski",
                    Email = "maciej.bobrowski@gmail.com"},
                new Doctor { IdDoctor = 2, FirstName = "Adrian", LastName = "Sobolewski",
                    Email = "adrian.sobolewski@gmail.com"}
            );
        });

        modelBuilder.Entity<Patient>(opt =>
        {
            opt.HasData(
                new Patient { IdPatient = 1, FirstName = "Bartosz", LastName = "Różański",
                    BirthDate = new DateTime(1996, 4, 13)},
                new Patient { IdPatient = 2, FirstName = "Izabela", LastName = "Rostek",
                    BirthDate = new DateTime(1994, 2, 10)},
                new Patient { IdPatient = 3, FirstName = "Tomasz", LastName = "Zieliński",
                    BirthDate = new DateTime(1962, 11, 6)},
                new Patient { IdPatient = 4, FirstName = "Hanna", LastName = "Laskowska",
                    BirthDate = new DateTime(1942, 6, 1)}
            );
        });

        modelBuilder.Entity<Prescription>(opt =>
        {
            opt.HasData(
                new Prescription { IdPrescription = 1, Date = new DateTime(2023, 1, 23),
                    DueDate = new DateTime(2023, 1, 30), IdDoctor = 1, IdPatient = 1},
                new Prescription { IdPrescription = 2, Date = new DateTime(2023, 1, 23),
                    DueDate = new DateTime(2023, 1, 30), IdDoctor = 1, IdPatient = 2},
                new Prescription { IdPrescription = 3, Date = new DateTime(2023, 1, 25),
                    DueDate = new DateTime(2023, 2, 10), IdDoctor = 2, IdPatient = 3},
                new Prescription { IdPrescription = 4, Date = new DateTime(2023, 1, 23),
                    DueDate = new DateTime(2023, 3, 23), IdDoctor = 2, IdPatient = 4},
                new Prescription { IdPrescription = 5, Date = new DateTime(2023, 1, 28),
                    DueDate = new DateTime(2023, 3, 10), IdDoctor = 2, IdPatient = 4}
            );
        });

        modelBuilder.Entity<Medicament>(opt =>
        {
            opt.HasData(
                new Medicament { IdMedicament = 1, Name = "Mupirocynav2",
                    Description = "Opis leku Mupirocynav2", Type = "Wirusobójczy"},
                new Medicament { IdMedicament = 2, Name = "Amfenikole",
                    Description = "Opis leku Amfenikole", Type = "Wirusobójczy"},
                new Medicament { IdMedicament = 3, Name = "Pleuormutylyn",
                    Description = "Opis leku Pleuormutylyn", Type = "Bakteriobójczy"},
                new Medicament { IdMedicament = 4, Name = "Mupirocyna",
                    Description = "Opis leku Mupirocyna", Type = "Wirusobójczy"},
                new Medicament { IdMedicament = 5, Name = "Kwas fusydowy",
                    Description = "Opis leku Kwas fusydowy", Type = "Bakteriobójczy"},
                new Medicament { IdMedicament = 6, Name = "Tetracyklin",
                    Description = "Opis leku Tetracyklin", Type = "Bakteriobójczy"},
                new Medicament { IdMedicament = 7, Name = "Penicylina",
                    Description = "Opis leku Penicylina", Type = "Bakteriobójczy"}
            );
        });

        modelBuilder.Entity<PrescriptionMedicament>(opt =>
        {
            opt.HasData(
                new PrescriptionMedicament { IdMedicament = 2, IdPrescription = 1, Dose = 1, Details = "dziennie"},
                new PrescriptionMedicament { IdMedicament = 3, IdPrescription = 2, Details = "jak na ulotce"},
                new PrescriptionMedicament { IdMedicament = 6, IdPrescription = 3, Dose = 2, Details = "dziennie"},
                new PrescriptionMedicament { IdMedicament = 7, IdPrescription = 3, Dose = 2, Details = "dziennie"},
                new PrescriptionMedicament { IdMedicament = 4, IdPrescription = 4, Dose = 3, Details = "dziennie"},
                new PrescriptionMedicament { IdMedicament = 5, IdPrescription = 4, Dose = 1, Details = "dziennie"},
                new PrescriptionMedicament { IdMedicament = 1, IdPrescription = 5, Dose = 3, Details = "dziennie"},
                new PrescriptionMedicament { IdMedicament = 5, IdPrescription = 5, Dose = 1, Details = "dziennie"}
            );
        });

    }
}