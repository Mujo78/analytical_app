using Microsoft.EntityFrameworkCore;
using server;
using server.Data;
using server.Repository;
using server.Repository.IRepository;
using server.Repository.IRepository.IUser;
using server.Services;
using server.Services.IServices;
using StackExchange.Profiling.Storage;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EntityDBContext>(options =>
    options.UseSqlServer(connString, sql => sql.CommandTimeout(600)));

var myAllowOrigin = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowOrigin,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod().AllowCredentials();
        });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddControllers();

builder.Services.AddScoped<IUserEFRepository, UserEFRepository>();
builder.Services.AddScoped<IUserDapperRepository, UserDapperRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPostRepository, PostEFRepository>();
builder.Services.AddScoped<IPostRepository, PostDapperRepository>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<ICommentRepository, CommentEFRepository>();
builder.Services.AddScoped<ICommentRepository, CommentDapperRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.TrackConnectionOpenClose = true;
    options.PopupShowTimeWithChildren = true;
    options.ShouldProfile = _ => true;
}).AddEntityFramework();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseMiniProfiler();
app.UseMiddleware<ResourceMetricsMiddleware>();
app.UseHttpsRedirection();
app.UseCors(myAllowOrigin);
app.MapControllers();
app.Run();