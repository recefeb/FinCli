using FinCli.Core;

var builder = WebApplication.CreateBuilder(args);

string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
string appFolderPath = Path.Combine(appDataPath, "FinCli");
Directory.CreateDirectory(appFolderPath);
string dataFilePath = Path.Combine(appFolderPath, "transactions.json");

builder.Services.AddSingleton(new TransactionService(dataFilePath));


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
