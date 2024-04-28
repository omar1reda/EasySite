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

var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddDbContext<AppDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("EasySite"));
        });


        /// ==> AddAuthentication && Token
        builder.Services.AddIdentity<AppUser, IdentityRole>()
                       .AddEntityFrameworkStores<AppDbContext>();
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

        // ip
        builder.Services.AddHttpContextAccessor();

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
            await AppIdentitySeed.SeedUserAsync(UserManger);
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
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();
        app.UseStaticFiles();
        app.Run();
