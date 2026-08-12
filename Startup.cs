using System.Text;
using App.Email.Generator;
using App.EmailSender.Service;
using App.Onion.Application.Interfaces;
using App.Onion.Domain.Interfaces.IMedicalRecordNumberGenerator;
using App.Onion.Domain.Interfaces.PatientRepository;
using App.Onion.Infrastructure.Persistence;
using App.Onion.Infrastructure.Persistence.Repositories;
using App.Passsword.Encoder;
using App.PassswordPolicy;
using App.Password.Generator;
using App.Security;
using dddnet8.AuditLog.Entities;
using dddnet8.AuditLog.Interfaces;
using dddnet8.AuditLog.Services;
using dddnet8.Domain.Appointments.Interfaces;
using dddnet8.Domain.Appointments.Service;
using dddnet8.Domain.AssignedStaff.Interfaces;
using dddnet8.Domain.Authentication;
using dddnet8.Domain.Authentication.token;
using dddnet8.Domain.BackOfficeEmail;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.PlanningModuleNotifications;
using dddnet8.Domain.RoomCoordinates.Interfaces;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Specializations.Interfaces;
using dddnet8.Domain.Specializations.Services;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.Services;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.Services;
using dddnet8.Domain.SystemUsers;
using dddnet8.Domain.Timetable;
using dddnet8.Infraestructure;
using dddnet8.Infraestructure.Appointments;
using dddnet8.Infraestructure.AssignedStaffs;
using dddnet8.Infraestructure.AuditLog.OperationRequests;
using dddnet8.Infraestructure.AuditLog.Patients;
using dddnet8.Infraestructure.AuditLog.Staffs;
using dddnet8.Infraestructure.AuditLog.Users;
using dddnet8.Infraestructure.Email;
using dddnet8.Infraestructure.MappingProfiles;
using dddnet8.Infraestructure.OperationRequests;
using dddnet8.Infraestructure.OperationTypes;
using dddnet8.Infraestructure.Password;
using dddnet8.Infraestructure.PlanningModuleNotifications;
using dddnet8.Infraestructure.RequiredStaffs;
using dddnet8.Infraestructure.Staffs;
using dddnet8.Infraestructure.SystemUsers;
using dddnet8.Infraestructure.Password;
using dddnet8.Infraestructure.RoomCoordinates;
using dddnet8.Infraestructure.Specializations;
using dddnet8.Infraestructure.Staff;
using dddnet8.Infraestructure.SurgeryRooms;
using dddnet8.Infraestructure.Timetable;
using dddnet8.Infraestructure.UtilsBootstrapper.Appointments;
using dddnet8.Infraestructure.UtilsBootstrapper.AssignedStaffs;
using dddnet8.Infraestructure.UtilsBootstrapper.MaintanceSlots;
using dddnet8.Infraestructure.UtilsBootstrapper.OperationRequests;
using dddnet8.Infraestructure.UtilsBootstrapper.OperationTypes;
using dddnet8.Infraestructure.UtilsBootstrapper.Patients;
using dddnet8.Infraestructure.UtilsBootstrapper.RequiredStaff;
using dddnet8.Infraestructure.UtilsBootstrapper.RoomCoordinates;
using dddnet8.Infraestructure.UtilsBootstrapper.Specializations;
using dddnet8.Infraestructure.UtilsBootstrapper.Staffs;
using dddnet8.Infraestructure.UtilsBootstrapper.SurgeryRooms;
using dddnet8.Infraestructure.UtilsBootstrapper.SystemUsers;
using dddnet8.Infraestructure.UtilsBootstrapper.Timetables;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SurgicalManagement.Domain.Domain;

namespace dddnet8
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5173", "http://74.234.33.121") // Specify allowed origins
                        .WithMethods("GET", "POST", "PUT", "DELETE") // Allowed methods
                        .WithHeaders("Content-Type", "Authorization") // Allowed headers
                        .AllowCredentials() // Allow credentials if necessary
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // Preflight cache
                });
            });

            // Configure Entity Framework Core and SqlServer
            ConfigureDatabaseConnection(services);

            // Dependency injection for services and repositories
            ConfigureDependencies(services);

            // Configure authentication
            ConfigureAuthentication(services);

            // Configure authorization
            ConfigureAuthorization(services);

            // Swagger docs generation
            services.AddSwaggerGen();

            // Add support for controllers
            services.AddControllers();
        }

        private void ConfigureDatabaseConnection(IServiceCollection services)
        {
            // DONT TOUCH. IF IT AINT BROKE DONT FIX IT!!!
            var envConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            string? connectionString;
            if (!string.IsNullOrEmpty(envConnectionString))
            {
                connectionString = envConnectionString;
                Console.WriteLine($"LOG: Using connection string from environment variable"); // Do **NOT** print it
            }
            else
            {
                connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (connectionString.IsNullOrEmpty())
                {
                    throw new InvalidOperationException(
                        "Connection string not found in appsettings.json or environment variable");
                }

                Console.WriteLine($"LOG: Using default connection string from appsettings.json: {connectionString}");
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            // JWT Authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["JwtSettings:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // Remove token time delay
                    };
                });

            // Cookie Authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/api/login";
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            if (context.Request.Path.StartsWithSegments("/api"))
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return Task.CompletedTask;
                            }

                            context.Response.Redirect(context.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddFacebook(options =>
                {
                    options.AppId = _configuration["Facebook:AppId"];
                    options.AppSecret = _configuration["Facebook:AppSecret"];
                    options.Scope.Add("public_profile");
                    options.Scope.Add("email");
                });
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole(UserRole.Admin.ToString()));
                options.AddPolicy("Nurse", policy => policy.RequireRole(UserRole.Nurse.ToString()));
                options.AddPolicy("Doctor", policy => policy.RequireRole(UserRole.Doctor.ToString()));
                options.AddPolicy("Patient", policy => policy.RequireRole(UserRole.Patient.ToString()));
                options.AddPolicy("Technician", policy => policy.RequireRole(UserRole.Technician.ToString()));
            });
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddScoped<IStaffRepository, StaffRepository>();
            ConfigurePlanningModuleDependencies(services);
            ConfigureOperationRequestDependencies(services);
            ConfigureSpecializationDependencies(services);
            ConfigureStaffRepositoryDependencies(services);
            ConfigureAuthDependencies(services);
            ConfigurePatientDependencies(services);
            ConfigureSystemUserDependencies(services);
            ConfigurePasswordDependencies(services);
            ConfigureLoginDependencies(services);
            ConfigureEmailDependencies(services);
            ConfigureOperationTypesDependencies(services);
            ConfigureRequiredStaffDependencies(services);
            ConfigureAuditLogDependencies(services);

            ConfigureUtilsAndBoostrapper(services);
            
            services.AddScoped<ApplicationBootstrapper>();

            // Add IUnitOfWork registration
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private void ConfigureUtilsAndBoostrapper(IServiceCollection services)
        {
            services.AddScoped<SpecializationsUtils>();
            services.AddScoped<OperationTypeUtils>();
            services.AddScoped<RequiredStaffUtils>();
            services.AddScoped<OperationRequestUtils>();
            
            services.AddScoped<StaffUtils>();
            services.AddScoped<SystemUserUtils>();
            services.AddScoped<TimetableUtils>();
            services.AddScoped<SurgeryRoomsUtils>();
            services.AddScoped<PatientUtils>();
            services.AddScoped<MaintenanceSlotsUtils>();

            services.AddScoped<OperationRequestUtils>();
            services.AddScoped<AssignedStaffUtils>();
            services.AddScoped<AppointmentUtils>();
            
            services.AddScoped<RoomCoordinatesUtils>();



            
            services.AddScoped<ApplicationBootstrapper>();
            
        }

        private void ConfigurePlanningModuleDependencies(IServiceCollection services)
        {
            services.AddHttpClient<IPlanningModuleNotificationService, PlanningModuleNotificationService>(client =>
            {
                var baseUrl = _configuration["PlanningModule:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("PlanningModule:BaseUrl is not configured");
                }

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        private void ConfigureOperationRequestDependencies(IServiceCollection services)
        {
            services.AddScoped<IOperationRequestPolicy, OperationRequestPolicy>();
            services.AddScoped<IOperationTypeRepository, OperationTypeRepository>();
            services.AddScoped<IOperationRequestRepository, OperationRequestRepository>();
            services.AddScoped<IOperationRequestService, OperationRequestService>();
            services.AddAutoMapper(typeof(OperationRequestProfile));
        }

        private void ConfigureAuditLogDependencies(IServiceCollection services)
        {
            services.AddScoped<ILogRepository<PatientLog>, PatientLogRepository>();
            services.AddScoped<ILogService<Patient>, PatientLogService>();
            services.AddScoped<ILogService<Staff>, StaffLogService>();
            services.AddScoped<ILogRepository<StaffLog>, StaffLogRepository>();
            services.AddScoped<ILogService<SystemUser>, UserLogService>();
            services.AddScoped<ILogRepository<UserLog>, UserLogRepository>();
            services.AddScoped<ILogService<OperationRequest>, OperationRequestLogService>();
            services.AddScoped<ILogRepository<OperationRequestLog>, OperationRequestLogRepository>();
            services.AddScoped<ITimeAssignedStaffRepository, AssignedStaffRepository>();


            services.AddScoped<ISurgeryRoomService, SurgeryRoomService>();
            services.AddScoped<ISurgeryRoomRepository, SurgeryRoomRepository>();
            services.AddScoped<IOperationTypeCodeGenerator, OperationTypeCodeGenerator>();

            services.AddScoped<IOperationRequestCodeGenerator, OperationRequestCodeGenerator>();
        }

        private void ConfigureSpecializationDependencies(IServiceCollection services)
        {
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<ISpecializationService, SpecializationService>();
        }
        private void ConfigureStaffRepositoryDependencies(IServiceCollection services)
        {
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<ILicenseNumberGenerator, LicenseNumberGenerator>();
        }

        private void ConfigureAuthDependencies(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        private void ConfigureEmailDependencies(IServiceCollection services)
        {
            services.AddSingleton<ISmtpClientWrapper>(provider =>
            {
                return new SmtpClientWrapper(
                    _configuration["EmailSettings:SmtpHost"],
                    int.Parse(_configuration["EmailSettings:SmtpPort"]),
                    _configuration["EmailSettings:FromAddress"],
                    _configuration["EmailSettings:FromPassword"]
                );
            });
            services.AddScoped<IEmailService, EmailService>();
        }

        private void ConfigurePatientDependencies(IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IMedicalRecordNumberGenerator, MedicalRecordNumberGenerator>();
        }

        private void ConfigureLoginDependencies(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<ILoginAttemptsService, LoginAttemptsService>();
        }

        private void ConfigurePasswordDependencies(IServiceCollection services)
        {
            services.AddScoped<IPasswordPolicy, PasswordPolicy>();
            services.AddScoped<IPasswordGenerator, PasswordGenerator>();
            services.AddScoped<IPasswordEncoder, PasswordEncoder>();
        }

        private void ConfigureSystemUserDependencies(IServiceCollection services)
        {
            services.AddScoped<ISystemUserService, SystemUserService>();
            services.AddScoped<ISystemUserRepository, SystemUserRepository>();
            services.AddScoped<IBackOfficeEmailGenerator, BackofficeEmailGenerator>();
        }

        private void ConfigureOperationTypesDependencies(IServiceCollection services)
        {
            services.AddScoped<IOperationTypeService, OperationTypeService>();
            services.AddScoped<IOperationTypeRepository, OperationTypeRepository>();

            services.AddScoped<IRoomCoordinateRepository, RoomCoordinateRepository>();
        }

        private void ConfigureRequiredStaffDependencies(IServiceCollection services)
        {
            services.AddScoped<IRequiredStaffRepository, RequiredStaffRepository>();

            services.AddScoped<IPlanningService, PlanningService>();
            services.AddScoped<IMaintenanceSlotRepository, MaintenanceSlotRepository>();
            services.AddScoped<ITimetableRepository, TimetableRepository>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            services.AddScoped<ITimeSlotService, TimeSlotService>();

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentService, AppointmentService>();

        }


        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
                }

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hospital Management API v1"); });

            app.UseCors("AllowSpecificOrigin"); // Use the specific CORS policy
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}