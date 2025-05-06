namespace Domain.DTOs.ReservationDTOs;

public class CreateReservationDTO
{
    public int TableId { get; set; }
    public int CustomerId { get; set; }
    public DateTime ReservationDate { get; set; }
}