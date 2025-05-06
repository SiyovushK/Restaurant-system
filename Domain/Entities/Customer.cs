using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public virtual List<Reservation> Reservations { get; set; }
}