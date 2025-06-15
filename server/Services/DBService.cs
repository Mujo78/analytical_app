using System;
using server.Repository.IRepository;
using server.Services.IServices;

namespace server.Services;

public class DBService(IDBRepository dBRepository) : IDBService
{
    private readonly IDBRepository _dBRepository = dBRepository;

    public async Task CreateIndexes()
    {
        await _dBRepository.CreateIndexes();
    }

    public async Task DropIndexes()
    {
        await _dBRepository.DropIndexes();
    }
}
