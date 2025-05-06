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

public class ReservationService(DataContext context, IMapper mapper) : IReservationService
{
    public async Task<Response<GetReservationDTO>> CreateAsync(CreateReservationDTO createReservation)
    {
        createReservation.ReservationDate = createReservation.ReservationDate.ToUniversalTime();

        if (await context.Reservations.AnyAsync(
            r => r.ReservationDate == createReservation.ReservationDate &&
            r.TableId == createReservation.TableId
        ))
            return new Response<GetReservationDTO>(HttpStatusCode.BadRequest, $"Reservation at {createReservation.ReservationDate} already exists, choose different time");

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

            query = query.Where(r => r.ReservationDate >= date && r.ReservationDate < nextDate);
        }

        var totalRecords = await query.CountAsync();

        var reservations = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var getReservations = mapper.Map<List<GetReservationDTO>>(reservations);

        return new PagedResponse<List<GetReservationDTO>>(getReservations, pageNumber, pageSize, totalRecords);
    }
}