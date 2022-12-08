using Microsoft.EntityFrameworkCore;
//using Serilog;
using CheapUdemy.Configs;
using CheapUdemy.Contracts;
using CheapUdemy.Data;
using CheapUdemy.Repository;
//using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


// Add services to the container.

//builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi); ////!!!/// 


var connectionString = builder.Configuration.GetConnectionString("MyAppDbConnectionString");
builder.Services.AddDbContext<MyAppDbContext>(opts =>
{
    opts.UseSqlServer(connectionString);
});


builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(x =>
//                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddCors(opts => opts.AddPolicy("AllowAll", p =>
{
    p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowCuc", p =>
    {
        p.WithOrigins("https://example-domain.io", "https://example-domain--2.io")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            ;
    });
});


builder.Services.AddAutoMapper(typeof(AutoMapperConfig));


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
//builder.Services.AddScoped<IPaymentsRepository, PaymentsRepository>();


//builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //Guid guid = Guid.NewGuid();
    //Console.WriteLine(guid);
}

app.UseDefaultFiles();
app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowCuc");


app.Use(async (ctx, next) =>
{
    Console.Write($"\n\n Req from REFR:: {ctx.Request.Headers.Referer.ToString()} \n\n");
    await next.Invoke(ctx);
});


app.UseCookiePolicy();
app.UseAuthorization();
app.MapControllers();


app.Run();
