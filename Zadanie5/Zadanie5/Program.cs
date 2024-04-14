using Microsoft.AspNetCore.Http.HttpResults;
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
    new Animal {IdAnimal = 3 , Category = "Kot" , Name = "Garfield", HairColor = "Ginger" , WeightKg = 14},
    new Animal { IdAnimal = 4 , Category = "Kot", Name = "Filemon", HairColor = "Black", WeightKg = 7},
    new Animal{IdAnimal = 5, Category = "Pies", Name = "Azor", HairColor = "White", WeightKg = 10}

};

var _visits = new List<Visit>
{
    new Visit { VisitID = 1, AnimalIdVisit = 1, VisitDate = "12-03-2023", Description = "Choroba", Price = 150 },
    new Visit { VisitID = 2, AnimalIdVisit = 1, VisitDate = "12-03-2024", Description = "Choroba1", Price = 160 }
};

app.MapGet("/api/animals", () => Results.Ok(_animals))
    .WithName("GetAnimals")
    .WithOpenApi();

app.MapGet("/api/animals/{id:int}", (int id) =>
{
    var animal = _animals.FirstOrDefault(s => s.IdAnimal == id);
    return animal == null ? Results.NotFound($"Animal with id {id} not found") : Results.Ok(animal);
}).WithName("GetAnimal").WithOpenApi();

app.MapPost("/api/animals", (Animal animal) =>
{
    if (_animals.Any(o=>o.IdAnimal == animal.IdAnimal))
    {
        return Results.StatusCode(StatusCodes.Status409Conflict);
    }
    _animals.Add(animal);
    return Results.StatusCode(StatusCodes.Status201Created);

}).WithName("CreateAnimal").WithOpenApi();

app.MapPut("/api/animals/{id:int}", (int id, Animal animal) =>
{
    var animalToEdit = _animals.FirstOrDefault(s => s.IdAnimal == id);
    if (animalToEdit == null)
    {
        return Results.NotFound($"Animal with id {id} not found");
    }
    _animals.Remove(animalToEdit);
    _animals.Add(animal);
    return Results.NoContent();
}).WithName("UpdateAnimal").WithOpenApi();

app.MapDelete("/api/animals/{id:int}", (int id) =>
{
    var animalToDelete = _animals.FirstOrDefault(s => s.IdAnimal == id);
    if (animalToDelete == null)
    {
        return Results.NoContent();
    }

    _animals.Remove(animalToDelete);
    return Results.NoContent();
}).WithName("DeleteAnimal").WithOpenApi();


//Proste pobranie listy wizyt dla danego zwierzecia
app.MapGet("/api/animals/{id:int}/visits", (int id) =>
{
    var animalVisits = _visits.FindAll(s => s.AnimalIdVisit == id);
    if (animalVisits.Count <=0 )
    {
        return Results.NotFound($"Visits for animal with id {id} not found ");
    }
    return Results.Ok(animalVisits);
    
}).WithName("GetAnimalVisits").WithOpenApi();

//Proste utworzenie wizyty dla danego zwierzecia
app.MapPost("/api/animals/{id:int}/visits", (int id, Visit visit) =>
{
    var animal = _animals.FirstOrDefault(s => s.IdAnimal == id);
    if (animal==null)
    {
        return Results.NotFound($"Animal with id {id} not found");
    }
    if (_visits.Any(o => o.VisitID == visit.VisitID))
    {
        return Results.StatusCode(StatusCodes.Status409Conflict);
    }

    visit.AnimalIdVisit = id;
    _visits.Add(visit);

    return Results.StatusCode(StatusCodes.Status201Created);

}).WithName("CreateVisits").WithOpenApi();


app.UseHttpsRedirection();
app.Run();

 