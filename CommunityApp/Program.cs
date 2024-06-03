using CommunityApp.Data;
using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Policies;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
});

builder.Services.AddSingleton<IAuthorizationHandler, CommunityManagerAuthorizationHandler>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<ICommunityManagerRepository, CommunityManagerRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CommunityService>();
builder.Services.AddScoped<CommunityManagerService>();
builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();

app.Run();
