using System.Net;
using AutoMapper;
using Domain.DTOs.TableDTOs;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TableService(DataContext context, IMapper mapper) : ITableService
{
    public async Task<Response<GetTableDTO>> CreateAsync(CreateTableDTO createTable)
    {
        if (await context.Tables.AnyAsync(t => t.Number == createTable.Number))
            return new Response<GetTableDTO>(HttpStatusCode.BadRequest, "Table with such number already exists");

        var table = mapper.Map<Table>(createTable);

        await context.Tables.AddAsync(table);
        var result = await context.SaveChangesAsync();

        var getTable = mapper.Map<GetTableDTO>(table);

        return result == 0
            ? new Response<GetTableDTO>(HttpStatusCode.InternalServerError, "Table couldn't be added")
            : new Response<GetTableDTO>(getTable);
    }

    public async Task<Response<string>> DeleteAsync(int tableId)
    {
        var table = await context.Tables.FirstOrDefaultAsync(t => t.Id == tableId);
        if (table == null)
            return new Response<string>(HttpStatusCode.BadRequest, "Table is not found");

        context.Tables.Remove(table);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Table couldn't be deleted")
            : new Response<string>("Table deleted successfully");
    }

    public async Task<PagedResponse<List<GetTableDTO>>> GetAllAsync(int pageNumber, int pageSize)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 10 ? 10 : pageSize;

        var query = context.Tables.AsQueryable();

        var totalRecords = await query.CountAsync();

        var tables = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var getTables = mapper.Map<List<GetTableDTO>>(tables);

        return new PagedResponse<List<GetTableDTO>>(getTables, pageNumber, pageSize, totalRecords);
    }

    public async Task<Response<int>> GetTablesCountAsync()
    {
        var count = await context.Tables.CountAsync();

        return new Response<int>(count);
    }
}