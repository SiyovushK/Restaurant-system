using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    [Required]
    public int TableId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public DateTime? ReservationDateFrom { get; set; }
    public DateTime? ReservationDateTo { get; set; }

    public virtual Table Table { get; set; }
    public virtual Customer Customer { get; set; }
}