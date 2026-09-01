using Prem_4_Login.Web.IServices;
using Prem_4_Login.Web.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout =
        TimeSpan.FromMinutes(60);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});


builder.Services.AddHttpClient("API", client =>
{
    var baseUrl =
        builder.Configuration["ApiSettings:BaseUrl"];

    client.BaseAddress =
        new Uri(baseUrl!);

    client.DefaultRequestHeaders
        .Accept
        .Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
                "application/json"));
});


builder.Services.AddScoped<
    IAuthApiService,
    AuthApiService>();

builder.Services.AddScoped<
    IUserApiService,
    UserApiService>();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


app.Run();