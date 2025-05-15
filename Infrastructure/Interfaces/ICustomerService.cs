using Domain.DTOs.CustomerDTOs;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface ICustomerService
{
    Task<Response<GetCustomerDTO>> CreateAsync(CreateCustomerDTO createCustomer);
    Task<Response<string>> DeleteAsync(int customerId);
    Task<Response<GetCustomerDTO>> GetByIdAsync(int customerId);
    Task<Response<List<GetCustomerDTO>>> GetAllAsync();
}