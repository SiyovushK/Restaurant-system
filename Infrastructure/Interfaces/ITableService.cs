using Domain.DTOs.TableDTOs;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface ITableService
{
    Task<Response<GetTableDTO>> CreateAsync(CreateTableDTO createTable);
    Task<Response<string>> DeleteAsync(int tableId);
    Task<PagedResponse<List<GetTableDTO>>> GetAllAsync(int pageNumber, int pageSize);
    Task<Response<int>> GetTablesCountAsync();
}