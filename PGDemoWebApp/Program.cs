using PGDataAccess;
using PGDemoWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("PaymentGatewayApi"));
builder.Services.Configure<ProxySettings>(builder.Configuration.GetSection("ProxySettings"));

builder.Services.AddHttpClient<IPaymentGatewayApiService, PaymentGatewayApiService>();
builder.Services.AddScoped<ISpsLegacyFormHandlerService, SpsLegacyFormHandlerService>();
builder.Services.AddScoped<PGDataServiceClient>();
// Para usar TempData (para el email en el callback)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

var env = builder.Environment.EnvironmentName;
Console.WriteLine($"Entorno actual: {env}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Para usar TempData
app.UseSession();

app.MapRazorPages();

app.Run();