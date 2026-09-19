using System.Reflection.Metadata;
using System.Text;
using FDP.Data;
using FDP.Interface;
using FDP.Middleware;
using FDP.Profiles;
using FDP.Repository;
using FDP.Services;
using FDP.Services.Auth;
using FDP.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using AutoMapper;
using FDP.Hubs;
using StackExchange.Redis;
using Serilog;


var builder=WebApplication.CreateBuilder(args);


Log.Logger=new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.File(
                "Logs/log.txt",
                rollingInterval:RollingInterval.Day
            )
            .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters=new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey=new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                        ),
                        
                        ValidateIssuer=true,
                        ValidIssuer=builder.Configuration["Jwt:Issuer"],

                        ValidateAudience=true,
                        ValidAudience=builder.Configuration["Jwt:Audience"],

                        ValidateLifetime=true,

                        ClockSkew=TimeSpan.Zero
                    };

                    options.Events=new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken=context.Request.Query["access_token"];

                            var path=context.HttpContext.Request.Path;

                            if(!string.IsNullOrEmpty(accessToken)&&
                                path.StartsWithSegments("/notificationHub"))
                            {
                                context.Token=accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        Name="Authorization",
        Type=SecuritySchemeType.Http,
        Scheme="bearer",
        BearerFormat="JWT",
        In=ParameterLocation.Header,
        Description="Enter your token"
    });
    options.AddSecurityRequirement(Document=>
    new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer",Document)]=[]
    });


});


builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRestaurantRepository,RestaurantRepository>();
builder.Services.AddScoped<IRestaurantService,RestaurantService>();
builder.Services.AddScoped<IMenuRepository,MenuRepository>();
builder.Services.AddScoped<IMenuService,MenuService>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IMenuItemService,MenuItemService>();
builder.Services.AddScoped<ICartRepository,CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
builder.Services.AddScoped<INotificationService,NotificationService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService,ReviewService>();
builder.Services.AddScoped<IPaymentRepository,PaymentRepository>();
builder.Services.AddScoped<IPaymentService,PaymentService>();
builder.Services.AddScoped<IPaymentProvider,MockPaymentProvider>();
builder.Services.AddScoped<IDeliveryRepository,DeliveryRepository>();
builder.Services.AddScoped<IDeliveryService,DeliveryService>();

builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    var connectionString=builder.Configuration["Redis:ConnectionString"];

    return ConnectionMultiplexer.Connect(connectionString!);
});


builder.Services.AddScoped<IRedisService, RedisService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserProfile>();
});

builder.Services.AddSignalR();

var app=builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/notificationHub");

// app.UseHttpsRedirection();

app.Run();
