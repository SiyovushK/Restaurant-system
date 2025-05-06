namespace Domain.DTOs.ReservationDTOs;

public class GetReservationDTO
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public int CustomerId { get; set; }
    public DateTime? ReservationDateFrom { get; set; }
    public DateTime? ReservationDateTo { get; set; }
}