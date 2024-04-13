using Zadanie5.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var _animals = new List<Animal>
{
    new Animal { IdAnimal = 1, Category = "Kot", Name = "Frazer", HairColor = "Grey", WeightKg = 8 },
    new Animal { IdAnimal = 2, Category = "Pies", Name = "Franek" , HairColor = "Ginger", WeightKg = 5},
    new Animal {IdAnimal = 3 , Category = "Wąż" , Name = "Bazyliszek", HairColor = null , WeightKg = 5},
    new Animal { IdAnimal = 4 , Category = "Kot", Name = "Filemon", HairColor = "Black", WeightKg = 7},
    new Animal{IdAnimal = 5, Category = "Pies", Name = "Azor", HairColor = "White", WeightKg = 10}

};

app.MapGet("/api/animals", () => Results.Ok(_animals))
    .WithName("GetAnimals")
    .WithOpenApi();


app.Run();

 