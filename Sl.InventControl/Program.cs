using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.Hosting.WindowsServices;
using MudBlazor.Services;
using Sl.InventControl.Data;
using Sl.InventControl.Service;
using System.Reflection;
using System.Security.Principal;

var workingDirectory = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var builder = WebApplication.CreateBuilder(new WebApplicationOptions() {
    ContentRootPath = workingDirectory,
    Args = args,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
});

var config = new SettingsModel(builder.Configuration);
var groupSids = new List<string>();
foreach (var group in config.Management.AccessGroups) {
    var sid = (new NTAccount(group)).Translate(typeof(SecurityIdentifier)).Value;
    groupSids.Add(sid);
}

// Add services to the container.
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options => {
    // By default, all incoming requests will be authorized according to the default policy.

    options.AddPolicy("GroupsCheck", policy => {
        policy.RequireRole(groupSids.ToArray());
    });
    options.DefaultPolicy = options.GetPolicy("GroupsCheck") ?? options.DefaultPolicy;
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddSingleton<DbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
