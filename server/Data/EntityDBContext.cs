using System;
using Microsoft.EntityFrameworkCore;

namespace server.Data;

public class EntityDBContext(DbContextOptions<EntityDBContext> options) : DbContext(options)
{

}
