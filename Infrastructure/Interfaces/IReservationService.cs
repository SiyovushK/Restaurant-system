using Domain.DTOs.ReservationDTOs;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface IReservationService
{
    Task<Response<GetReservationDTO>> CreateAsync(CreateReservationDTO createReservation);
    Task<Response<string>> DeleteAsync(int reservationId);
    Task<PagedResponse<List<GetReservationDTO>>> GetAllAsync(ReservationFilter filter);
}