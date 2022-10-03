using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World 4!");

app.MapGet("/user", () => new { Name = "Luan Douglas", Age = 23 });

app.MapGet("/AddHeader", (HttpResponse response) =>
{
    response.Headers.Add("Teste", "Luan Douglas");
    return "Luan Douglas Adicionado";
});

app.MapPost("/saveproduto", (Produto produto) =>
{
    return produto.Code + " - " + produto.Name;
});

// api.app.com/users?datestart={date}&dateend={date}
// api.app.com/users/{code}

app.MapGet("/getproduto", ([FromQuery] string dateStart, [FromQuery] string dateEnd) =>
{
    return dateStart + " - " + dateEnd;
});

app.MapGet("/getproduto/{code}", ([FromRoute] string code) =>
{
    return code;
});

app.MapGet("/getproduto/byheader", (HttpRequest request) =>
{
    return request.Headers["product-code"].ToString();
});




app.Run();


public class Produto
{
    public string Code { get; set; }
    public string Name { get; set; }
}