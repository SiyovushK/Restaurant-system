namespace Domain.Filters;

public class ReservationFilter
{
    public int? TableId { get; set; }
    public int? CustomerId { get; set; }
    public DateTime? ReservationDate { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}