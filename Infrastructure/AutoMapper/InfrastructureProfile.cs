using System.Runtime;
using AutoMapper;
using Domain.DTOs.CustomerDTOs;
using Domain.DTOs.ReservationDTOs;
using Domain.DTOs.TableDTOs;
using Domain.Entities;

namespace Infrastructure.AutoMapper;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<Table, GetTableDTO>();
        CreateMap<CreateTableDTO, Table>();
        CreateMap<UpdateTableDTO, Table>();

        CreateMap<Customer, GetCustomerDTO>();
        CreateMap<CreateCustomerDTO, Customer>();
        CreateMap<UpdateCustomerDTO, Customer>();

        CreateMap<Reservation, GetReservationDTO>();
        CreateMap<CreateReservationDTO, Reservation>();
        CreateMap<UpdateReservationDTO, Reservation>();
    }
}