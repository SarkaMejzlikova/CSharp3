using ToDoList.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    // Configure DI
    builder.Services.AddControllers();
    // přidání databázového kontextu do DI kontejneru
    builder.Services.AddDbContext<ToDoItemsContext>();
}

var app = builder.Build();
{
    // Configure Middleware (HTTP request pipeline)
    app.MapControllers();
}


app.Run();
