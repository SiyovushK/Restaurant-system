using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Table
{
    public int Id { get; set; }
    [Required]
    public int Number { get; set; }
    [Required]
    public int Seats { get; set; }
 
    public virtual List<Reservation> Reservations { get; set; }
}