using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

app.MapPost("/products", (ProductDto productDto, ApplicationDbContext context) =>
{
    var category = context.Categories.Where(c => c.Id == productDto.CategoryId).First();
    var product = new Product
    {
        Code = productDto.Code,
        Name = productDto.Name,
        Description = productDto.Description,
        Category = category
    };
    if (productDto.Tags != null)
    {
        product.Tags = new List<Tag>();
        foreach (var item in productDto.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }
    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created($"/products/{product.Id}", product.Id);
});


app.MapGet("/products/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetBy(code);
    if (product != null)
    {
        Console.WriteLine("O produto foi executado");
        return Results.Ok(product);
    }
    return Results.NotFound();
});

app.MapGet("/products", () =>
{
    var product = ProductRepository.GetAll();
    if (product != null)
    {
        return Results.Ok(product);
    }
    return Results.NotFound();
});

app.MapPut("/products", (Product product) =>
{
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;

    Results.Ok();
});

app.MapDelete("/products/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetBy(code);
    ProductRepository.Remove(product);

    Results.Ok();
});

if (app.Environment.IsStaging())
    app.MapGet("/configuration/database", (IConfiguration configuration) =>
    {
        return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");
    });

app.Run();
