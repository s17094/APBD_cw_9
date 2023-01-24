using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Crawler.Models;

[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }

    [ForeignKey(nameof(IdMedicament))]
    public virtual Medicament medicament { get; set; }

    public int IdPrescription { get; set; }

    [ForeignKey(nameof(IdPrescription))]
    public virtual Prescription prescription { get; set; }

    public int? Dose { get; set; }

    [Required]
    [MaxLength(100)]
    public string Details { get; set; }
}