using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/customers", ([FromServices] CustomerRepository repo) =>
{
    return repo.GetAll();
});

app.MapGet("/customers/{id}", ([FromServices] CustomerRepository repo, int id) =>
{
    var customer = repo.GetById(id);
    return customer is not null ? Results.Ok(customer) : Results.NotFound();
});

app.MapPost("/customers", ([FromServices] CustomerRepository repo, Customer customer) =>
{
    repo.Create(customer);
    return Results.Created($"/customers/{customer.Id}", customer);
});

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
//
// app.UseAuthorization();
//
// app.MapControllers();

app.Run();

record Customer(int Id, string FullName);

class CustomerRepository
{
    private readonly Dictionary<int, Customer> _customers = new();

    public void Create(Customer customer)
    {
        if (customer is null)
        {
            return;
        }

        _customers[customer.Id] = customer;
    }

    public async Task<IEnumerable<Customer>> GetAll()
    {
         return _customers.Values;
    }

    public async Task<Customer> GetById(int id)
    {
        var customer =  _customers[id];
    }
}