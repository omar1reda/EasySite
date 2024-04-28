        using EasySite.Repository.Context;
        using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
        using Microsoft.AspNetCore.Identity;
        using Microsoft.EntityFrameworkCore;
        using Core.Entites;
        using EasySite.DataSeeding;
        using Microsoft.Extensions.DependencyInjection.Extensions;
        using EasySite.Core.Entites;
        using EasySite.DTOs;
        using EasySite.Helper.SendEmail;

        using EasySite.Core.I_Repository;
        using EasySite.Services;
        using Microsoft.AspNetCore.Authentication.JwtBearer;
        using Microsoft.IdentityModel.Tokens;
        using System.Text;
        using Microsoft.AspNetCore.Mvc;
using EasySite.Errors;
using Microsoft.Extensions.Options;
using EasySite.Helper;
using EasySite.Repository.Repository;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using EasySite.SignalR;

var builder = WebApplication.CreateBuilder(args);

////  ======> api  السماح باستخدام ال

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                   .AllowAnyMethod()
//                   .AllowAnyHeader();
//        });
//});




// Add services to the container.

builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:7056")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

// ===> SignalR
builder.Services.AddSignalR();

        // ===> Get Id && MaceAdderes
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext<AppDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("EasySite"));
        });
        // Redise ===>
        builder.Services.AddSingleton<IConnectionMultiplexer>(option =>
        {
            var conection = builder.Configuration.GetConnectionString("Redise");
            return ConnectionMultiplexer.Connect(conection);
        });


        /// ==> AddAuthentication && Token
        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {

        }).AddEntityFrameworkStores<AppDbContext>();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };

            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("User"));
                
            });


            // ???????? ====> HttpClient 
            builder.Services.AddHttpClient();

            builder.Services.AddControllers();

            ///===> snd email
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));
                    builder.Services.AddTransient<IMailSettings, MailSettings>();


        /// ===> Genrate Token 
        builder.Services.AddScoped<ITokenService, TokenService>();




        // Validation Error Response
        builder.Services.Configure<ApiBehaviorOptions>(Option =>
        {
            Option.InvalidModelStateResponseFactory = (actionContext) =>
            {
                var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                .SelectMany(P => P.Value.Errors)
                .Select(E => E.ErrorMessage)
                .ToArray();

                var ValidationErrorResponse = new ApiValidationErrorResponse()
                {
                    Errors = errors
                };
                return new BadRequestObjectResult(ValidationErrorResponse);
            };
        });


        // Auto Mapper ==>
       builder.Services.AddAutoMapper(typeof(MappingProfiles));

      ////  Generice Repository ==>
       builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
       builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
       builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
       builder.Services.AddSingleton(typeof(IResponseCachingService), typeof(ResponseCachingService));

/////// ???? ?? Athrization ?? ?? Swegger===============>
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "dotnetClaimAuthorization", Version = "v1" });


    c.AddSecurityDefinition("Bearar", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
            In=ParameterLocation.Header,
            Description="ples Enter Token",
            Type=SecuritySchemeType.Http,
            BearerFormat="JWT",
            Scheme="bearer"       
        });



    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
                    new OpenApiSecurityScheme
                    {
                        Reference= new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearar"
                        }
                    },
                    new string[]{}
        }

    });
});

var app = builder.Build();


        //___________Update-Database ==> 
        #region Update-Database && Logger Factory Service
        using var Scope = app.Services.CreateScope();

        var Services = Scope.ServiceProvider;

        var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
        try
        {
            // ---- Update - Database ==> Dbcontext )
            var DbContext = Services.GetService<AppDbContext>();
            await DbContext.Database.MigrateAsync();
            // ----- Update-Database ==> Identity )
            //var identityContext = Services.GetService<IdentityContext>();
            //await identityContext.Database.MigrateAsync();

   

            // Seed Users=====> Register
            var UserManger = Services.GetRequiredService<UserManager<AppUser>>();

            var RoleManger = Services.GetRequiredService<RoleManager<IdentityRole>>();
            await AppApplecationSeed.AddAllRolesAndAddAdmin(RoleManger, UserManger);
        }
        catch (Exception ex)
        {
            var logger = LoggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "An Error in Migration ##########################");
        }


#endregion



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
        {
            app.UseMiddleware<ExiptionMedelWhare>();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.MapHub<ChatHub>("/chatHub");

app.UseRouting();
//app.UseCors("AllowAllOrigins");

        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();
        app.UseStaticFiles();
        app.Run();
