using System;

namespace server.Repository.IRepository;

public interface IDBRepository
{
    Task CreateIndexes();
    Task DropIndexes();
}
