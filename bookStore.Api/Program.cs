using bookStore.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

bookStore.DataAccess.DbSession.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<bookStore.BusinessLogic.Interfaces.ISession, bookStore.BusinessLogic.SessionBL>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
