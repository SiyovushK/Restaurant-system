using System.Net;
using AutoMapper;
using Domain.DTOs.CustomerDTOs;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CustomerService(DataContext context, IMapper mapper) : ICustomerService
{
    public async Task<Response<GetCustomerDTO>> CreateAsync(CreateCustomerDTO createCustomer)
    {
        if (await context.Customers.AnyAsync(t => 
                t.FirstName == createCustomer.FirstName &&
                t.LastName == createCustomer.LastName &&
                t.Phone == createCustomer.Phone
        ))
            return new Response<GetCustomerDTO>(HttpStatusCode.BadRequest, "Customer already exists");

        var customer = mapper.Map<Customer>(createCustomer);

        await context.Customers.AddAsync(customer);
        var result = await context.SaveChangesAsync();

        var getCustomer = mapper.Map<GetCustomerDTO>(customer);

        return result == 0
            ? new Response<GetCustomerDTO>(HttpStatusCode.InternalServerError, "Customer couldn't be added")
            : new Response<GetCustomerDTO>(getCustomer);
    }

    public async Task<Response<string>> DeleteAsync(int customerId)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(t => t.Id == customerId);
        if (customer == null)
            return new Response<string>(HttpStatusCode.BadRequest, "Customer is not found");

        context.Customers.Remove(customer);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Customer couldn't be deleted")
            : new Response<string>("Customer deleted successfully");
    }
}