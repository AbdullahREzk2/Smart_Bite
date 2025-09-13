var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDBHelperServices();

builder.Services.AddAuthanticationServices();

builder.Services.AddDiseaseServices();

builder.Services.AddAllergyServices();

builder.Services.AddDiseaseNutrientServices();

builder.Services.AddFoodServices();

builder.Services.AddMedicationServices();

builder.Services.AddCalorieServices();

builder.Services.AddAiPredictionServices();

builder.Services.AddReportServices();

builder.Services.AddMealServices();

builder.Services.AddLimitServices();

builder.Services.AddProductServices();

builder.Services.AddNutritionPredictionServices();

builder.Services.AddHealthServices();

builder.Services.AddDailyJobServices();

builder.Services.AddOtherServices();

builder.Services.AddJWTServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();


app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
