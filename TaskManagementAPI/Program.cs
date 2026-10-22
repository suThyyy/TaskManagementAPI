
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Kết nối SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 2. Đăng ký Controllers
builder.Services.AddControllers();

// 3. Đăng ký OpenAPI
builder.Services.AddOpenApi();

// 4. Cấu hình CORS cho React
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 5. Cấu hình HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

// 6. Kích hoạt CORS
app.UseCors("ReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
