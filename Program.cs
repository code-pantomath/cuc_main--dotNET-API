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


var connectionString = builder.Configuration.GetConnectionString("MyAppDbConnectionString"); // This is how to get something from the ( appsetting.json ) File. through the ( builder.Configuration )
builder.Services.AddDbContext<MyAppDbContext>(opts =>
{
    opts.UseSqlServer(connectionString);
});


builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(x =>
//                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddCors();

builder.Services.AddCors(opts => opts.AddPolicy("AllowAll", p =>
{
    p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowCuc", p =>
    {
        p.WithOrigins("https://cheapudemy.com", "https://www.cheapudemy.com", "https://cheapudemy-com--support-server.herokuapp.com/", "https://www.cheapudemy-com--support-server.herokuapp.com", "https://herokuapp.com", "https://www.herokuapp.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            ;
    });
});


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("_myAllowSpecificOrigins",
//                      policy =>
//                      {
//                          policy.WithOrigins("http://localhost:3000", "http:127.0.0.1:3000", "http://localhost", "http://127.0.0.1").AllowAnyOrigin();
//                      });
//});


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
//app.UseCors("AllowAll");
app.UseCors("AllowCuc");
// allow credentials

//app.UseCors(builder =>
//{
//    builder
//    .WithOrigins("https://cheapudemy.com", "https://www.cheapudemy.com")
//    .AllowAnyMethod()
//    .AllowAnyMethod()
//    ;
//});

//app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://cheapudemy.com", "https://www.cheapudemy.com"));


 
//Prevent requests that or not from our webite.?
app.Use(async (ctx, next) =>
{

    //var queryitems = ctx.Request.Query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();
    //List<KeyValuePair<string, string>> queryParams = new List<KeyValuePair<string, string>>();

    //Console.Write("\n\n {0} {1} {2} {3} \n\n", "sdsd:::==  ", ctx.Request.Host.Host + "  ||  ", ctx.Request.Host.Value?.ToLower() + " || ", ctx.Connection.RemoteIpAddress);
    if ((!ctx.Request.Headers.Referer.ToString().StartsWith("https://cheapudemy.com/") && !ctx.Request.Headers.Referer.ToString().StartsWith("https://www.cheapudemy.com/") && !ctx.Request.Headers.Referer.ToString().Equals("https://cheapudemy-com--support-server.herokuapp.com/app/api/v1")) || !ctx.Request.IsHttps)
    {
        //await Task.CompletedTask;
        await ctx.Response.WriteAsync("Unauthorized!");
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        ctx.Connection.RequestClose();
        ctx.Abort();
    }
    else
    {
        await next.Invoke(ctx);
    }
});

app.Use(async (ctx, next) =>
{
    Console.Write($"\n\n REFR:: {ctx.Request.Headers.Referer.ToString()} \n\n");
    await next.Invoke(ctx);
});

//app.Use((ctx, next) =>
//{
//    ctx.Response.Headers["Access-Control-Allow-Origin"] = "https://cheapudemy.com";
//    return next.Invoke();
//});



app.UseCookiePolicy();


app.UseAuthorization();

app.MapControllers();

// How to make Coustom middlewares :

//app.Use(async (ctx, next) =>
//{
//    // Do some thing....

//    // Do work that can write to the Response.

//    //
//    await next.Invoke();

//    // Then

//    // here you can Do logging or other work that doesn't write to the Response.
//});

//

// Coustom made CORS Middkeware


app.Run();
//app.Run(async (ctx) =>
//{
//    // Do anything here as the last middleware that will run in the pipe-line...
//    await ctx.Response.WriteAsync("Hello from the final delegate !.");
//});



// app.Run() middleware will end the request, and app.Use() middleware will pass the request to next middleware !
//For app.Use, it adds a middleware delegate to the application's request pipeline.

//For the difference between app.Run and app.UseEndpoints, it is the difference between app.Run and app.Use.app.Run will end the request, and app.Use will pass the request to next middleware.

//For app.UseEndpoints, it is app.Use with EndpointMiddleware.

//// so The main difference is once a middle ware added with App.Run has completed its execution the pipeline will terminate and the response will be returned to the caller.
//// ans so you can register/write any other .Use() middelware after it!.

//////
///
