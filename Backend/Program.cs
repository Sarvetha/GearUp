using GearUp.Mappings;
using GearUp.Models;
using GearUp.Services.IService;
using GearUp.Services.Service;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<GearUpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartInterface, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton<SmtpClient>(sp =>
{
    var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io")
    {
        Port = 2525,
        Credentials = new NetworkCredential("5aef8111db7a16", "71986a5347cd6f"),
        EnableSsl = true,
    };
    return smtpClient;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
