using Microsoft.EntityFrameworkCore;
using server;
using server.Data;
using server.Repository;
using server.Repository.IRepository;
using server.Repository.IRepository.IComment;
using server.Repository.IRepository.IPost;
using server.Repository.IRepository.IUser;
using server.Services;
using server.Services.IServices;

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
                .AllowAnyMethod()
                .AllowCredentials().WithExposedHeaders("x-miniprofiler-ids");
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

builder.Services.AddScoped<IPostEFRepository, PostEFRepository>();
builder.Services.AddScoped<IPostDapperRepository, PostDapperRepository>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<ICommentEFRepository, CommentEFRepository>();
builder.Services.AddScoped<ICommentDapperRepository, CommentDapperRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IDBRepository, DBRepository>();
builder.Services.AddScoped<IDBService, DBService>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.TrackConnectionOpenClose = true;
    options.EnableServerTimingHeader = true;
    options.PopupShowTimeWithChildren = true;
    options.MaxUnviewedProfiles = 0;
    options.ShouldProfile = _ => true;
}).AddEntityFramework();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments(new PathString("/profiler")))
    {
        if (context.Request.Headers.TryGetValue("Origin", out var origin))
        {
            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
                context.Response.Headers.Append("Access-Control-Allow-Methods", "OPTIONS, GET");
                await context.Response.CompleteAsync();
                return;
            }
        }
    }

    await next();
});

app.UseRouting();
app.UseMiniProfiler();
app.UseMiddleware<ResourceMetricsMiddleware>();
app.UseHttpsRedirection();
app.UseCors(myAllowOrigin);
app.MapControllers();
app.Run();