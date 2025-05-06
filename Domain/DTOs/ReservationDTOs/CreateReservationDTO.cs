namespace Domain.DTOs.ReservationDTOs;

public class CreateReservationDTO
{
    public int TableId { get; set; }
    public int CustomerId { get; set; }
    public DateTime ReservationDateFrom { get; set; }
    public DateTime ReservationDateTo { get; set; }
}