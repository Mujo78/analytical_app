using System;

namespace server.Services.IServices;

public interface IDBService
{
    Task CreateIndexes();
    Task DropIndexes();
}
