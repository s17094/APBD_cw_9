using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crawler.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public int IdDoctor { get; set; }

    [ForeignKey(nameof(IdDoctor))]
    public virtual Doctor Doctor { get; set; }

    public int IdPatient { get; set; }

    [ForeignKey(nameof(IdPatient))]
    public virtual Patient Patient { get; set; }

    public virtual ICollection<PrescriptionMedicament> Medicaments { get; set; }

}