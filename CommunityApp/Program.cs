using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CommunityApp.Data;
using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Data.Seeders;
using CommunityApp.Policies;
using CommunityApp.Services;
using CommunityApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    builder.Configuration["AdminPassword"] = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
}

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "true"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireClaim("IsManager", "true"));
    options.AddPolicy("CommunityManager", policy => policy.Requirements.Add(new CommunityManagerRequirement()));
    options.AddPolicy("LeaseTenant", policy => policy.Requirements.Add(new LeaseTenantRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, CommunityManagerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, LeaseTenantAuthorizationHandler>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<ICommunityManagerRepository, CommunityManagerRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILeaseRepository, LeaseRepository>();
builder.Services.AddScoped<ILeaseTenantRepository, LeaseTenantRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<CommunityService>();
builder.Services.AddScoped<CommunityManagerService>();
builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LeaseService>();
builder.Services.AddScoped<LeaseTenantService>();
builder.Services.AddScoped<PaymentService>();

builder.Services.AddScoped<AdminUserSeed>();
builder.Services.AddScoped<ManagerUsersSeed>();
builder.Services.AddScoped<CommunitiesSeed>();
builder.Services.AddScoped<CommunityManagersSeed>();
builder.Services.AddScoped<HomesSeed>();
builder.Services.AddScoped<LeasesSeed>();
builder.Services.AddScoped<PaymentsSeed>();
builder.Services.AddScoped<DatabaseSeeder>();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseDatabaseSeeder();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();

app.Run();
