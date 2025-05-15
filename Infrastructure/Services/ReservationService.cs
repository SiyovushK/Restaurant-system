using System.Net;
using AutoMapper;
using Domain.DTOs.ReservationDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReservationService(DataContext context, IMapper mapper, ICustomerService customerService) : IReservationService
{
    public async Task<Response<GetReservationDTO>> CreateAsync(CreateReservationDTO createReservation)
    {
        createReservation.ReservationDateFrom = createReservation.ReservationDateFrom.ToUniversalTime();
        createReservation.ReservationDateTo = createReservation.ReservationDateTo.ToUniversalTime();

        var customerResult = await customerService.GetByIdAsync(createReservation.CustomerId);
        if (!customerResult.IsSuccess)
        {
            return new Response<GetReservationDTO>(HttpStatusCode.BadRequest, "Customer does not exist");
        }

        if (createReservation.ReservationDateFrom >= createReservation.ReservationDateTo)
        {
            return new Response<GetReservationDTO>(HttpStatusCode.BadRequest, "Reservation start time must be before end time");
        }

        var dayStart = createReservation.ReservationDateFrom.Date;
        var dayEnd = dayStart.AddDays(1);

        // var check = await context.Reservations.AnyAsync(r =>
        //     r.TableId == createReservation.TableId &&
        //     (r.ReservationDateFrom <= createReservation.ReservationDateFrom && r.ReservationDateFrom < createReservation.ReservationDateTo ||
        //     r.ReservationDateFrom <= createReservation.ReservationDateFrom && r.ReservationDateFrom <= createReservation.ReservationDateTo) &&
        //     r.ReservationDateFrom >= dayStart &&
        //     r.ReservationDateFrom < dayEnd
        // );

        // if (!check)
        // {
        //     return new Response<GetReservationDTO>(HttpStatusCode.BadRequest,
        //         $"Reservation for TableId {createReservation.TableId} overlaps with an existing reservation. Please choose a different time.");
        // }

        var reservation = mapper.Map<Reservation>(createReservation);

        await context.Reservations.AddAsync(reservation);
        var result = await context.SaveChangesAsync();

        if (result == 0)
            return new Response<GetReservationDTO>(HttpStatusCode.InternalServerError, "Reservation couldn't be added");

        var getReservation = mapper.Map<GetReservationDTO>(reservation);

        return new Response<GetReservationDTO>(getReservation);
    }

    public async Task<Response<string>> DeleteAsync(int reservationId)
    {
        var reservation = await context.Reservations.FirstOrDefaultAsync(r => r.Id == reservationId);
        if (reservation == null)
            return new Response<string>(HttpStatusCode.NotFound, "Reservation not found");

        context.Reservations.Remove(reservation);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Reservation couldn't be deleted")
            : new Response<string>("Reservation deleted successfully");
    }

    public async Task<PagedResponse<List<GetReservationDTO>>> GetAllAsync(ReservationFilter filter)
    {
        var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
        var pageSize = filter.PageSize < 10 ? 10 : filter.PageSize;

        var query = context.Reservations.AsQueryable();

        if (filter.TableId != null)
        {
            query = query.Where(r => r.TableId == filter.TableId);
        }

        if (filter.CustomerId != null)
        {
            query = query.Where(r => r.CustomerId == filter.CustomerId);
        }

        if (filter.ReservationDate != null)
        {
            var date = filter.ReservationDate.Value.Date;
            var nextDate = date.AddDays(1);

            query = query.Where(r => r.ReservationDateFrom >= date && r.ReservationDateFrom < nextDate);
        }   

        var totalRecords = await query.CountAsync();

        var reservations = await query
            .OrderBy(r => r.ReservationDateFrom)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var getReservations = mapper.Map<List<GetReservationDTO>>(reservations);

        return new PagedResponse<List<GetReservationDTO>>(getReservations, pageNumber, pageSize, totalRecords);
    }
}