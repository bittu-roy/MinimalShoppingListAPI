//1. using in memory database using get package manager


using Microsoft.EntityFrameworkCore;
using MinimalShoppingListAPI;

//using builder to build a web application.
var builder = WebApplication.CreateBuilder(args);

//to use the endpoints
builder.Services.AddEndpointsApiExplorer();

//to have some API Documentation
builder.Services.AddSwaggerGen();

//adding db application to our current application so that we can use a databse in our application
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ShoppingListApi"));

var app = builder.Build();

//endpoint for HTTP GET using Minimal API
app.MapGet("/shoppinglist", async (ApiDbContext db) =>
          await db.Groceries.ToListAsync());

//endpoint for HTTP GET to get a particular id.
app.MapGet("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = db.Groceries.FindAsync(id);

    return grocery != null ? Results.Ok(grocery): Results.NotFound();
});

//endpoint for HTTP POST using Minimal API
app.MapPost("/shoppinglist", async (Grocery grocery, ApiDbContext db) =>
{
    db.Groceries.Add(grocery);

    await db.SaveChangesAsync();

    return Results.Created($"/shoppinglist/{grocery.Id}", grocery);
});

//api endpoint to delete 
app.MapDelete("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);
    
    if(grocery != null)
    {
        db.Groceries.Remove(grocery);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

//api endpoint to update
app.MapPut("/shoppinglist/{id}", async (int id, Grocery grocery, ApiDbContext db) =>
{
    var groceryInDb = await db.Groceries.FindAsync(id);

    if (groceryInDb != null)
    {
        groceryInDb.Name = grocery.Name;
        groceryInDb.Purchased = grocery.Purchased;

        await db.SaveChangesAsync();
        return Results.Ok(groceryInDb);
    }

    return Results.NotFound();

});

//development scenario
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//We checked the HTTP box, so our app is configured to HTTPS
app.UseHttpsRedirection();

app.Run();