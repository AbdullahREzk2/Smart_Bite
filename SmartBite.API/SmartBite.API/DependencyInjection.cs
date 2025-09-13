using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartBite.BAL.Authantication;
using SmartBite.BAL.CalorieCalculator;
using SmartBite.BAL.FoodTransactions;
using SmartBite.BAL.HealthDataPreparing;
using SmartBite.BAL.HealthProfileOperations;
using SmartBite.BAL.JWTHelper;
using SmartBite.BAL.LimitsOperations;
using SmartBite.BAL.Mapping;
using SmartBite.BAL.MealOperations;
using SmartBite.BAL.Medication;
using SmartBite.BAL.OthersServiceManager;
using SmartBite.BAL.ProductOperations;
using SmartBite.BAL.ReportManeger;
using SmartBite.BAL.Services;
using SmartBite.DAL;
using SmartBite.DAL.Allergy;
using SmartBite.DAL.Authantication;
using SmartBite.DAL.Calories;
using SmartBite.DAL.Disease;
using SmartBite.DAL.DiseaseNutrient;
using SmartBite.DAL.Food;
using SmartBite.DAL.HealthProfile;
using SmartBite.DAL.Medication;
using SmartBite.DAL.OthersServices;
using SmartBite.DAL.Services;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {

        // For Using ConnectionString -->
        public static IServiceCollection AddDBHelperServices(this IServiceCollection services)
        {
            services.AddScoped<dbHelper>();
            return services;
        }


        // For Authantication Service -->
        public static IServiceCollection AddAuthanticationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthanticationService, AuthanticationService>();
            services.AddScoped<IAuthanticationManager, AuthanticationManager>();
            services.AddAutoMapper(typeof(PersonMap));
            return services;
        }


        // For Disease Service -->
        public static IServiceCollection AddDiseaseServices(this IServiceCollection services)
        {
            services.AddScoped<IDiseaseService, DiseaseService>();
            return services;
        }


        // For Disease Service -->
        public static IServiceCollection AddAllergyServices(this IServiceCollection services)
        {
            services.AddScoped<IAllergyService, AllergyService>();
            return services;
        }


        // For DiseaseNutrient Service -->
        public static IServiceCollection AddDiseaseNutrientServices(this IServiceCollection services)
        {
            services.AddScoped<IDiseaseNutrientService, DiseaseNutrientService>();
            return services;
        }


        // For Food Service --> 
        public static IServiceCollection AddFoodServices(this IServiceCollection services)
        {
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IFoodTransactionManager, FoodTransactionManager>();
            services.AddAutoMapper(typeof(TinyFoodItemMap));
            return services;
        }


        // For Medication Service --> 
        public static IServiceCollection AddMedicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMedicationService, MedicationService>();
            services.AddScoped<IMedicationManager, MedicationManager>();
            services.AddAutoMapper(typeof(MedicineMap));
            return services;
        }


        // For CalorieClaculator Service -->
        public static IServiceCollection AddCalorieServices(this IServiceCollection services)
        {
            services.AddScoped<ICalorieCalculatorManager, CalorieCalculatorManager>();
            services.AddScoped<ICaloriesService, CaloriesService>();
            return services;
        }


        // For AI Classfication -->
        public static IServiceCollection AddAiPredictionServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAiPredictionService, AiPredictionService>();
            return services;
        }


        // For generate Report -->
        public static IServiceCollection AddReportServices(this IServiceCollection services)
        {
            services.AddScoped<IReportService, ReportService>();
            return services;
        }


        // For Meal Operation -->
        public static IServiceCollection AddMealServices(this IServiceCollection services)
        {
            services.AddScoped<IMealService, MealService>();
            return services;
        }


        // For Limit Operation -->
        public static IServiceCollection AddLimitServices(this IServiceCollection services)
        {
            services.AddScoped<ILimitService, LimitService>();
            services.AddAutoMapper(typeof(LimitMap));
            return services;
        }


        // For product Service -->
        public static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductManager, ProductManager>();
            return services;
        }


        // For Nutrition Service -->
        public static IServiceCollection AddNutritionPredictionServices(this IServiceCollection services)
        {
            services.AddHttpClient<AiPredictionNutritionService>(client =>
            {
                client.BaseAddress = new Uri("https://habibaamr18-nutrition-api.hf.space"); // 👉 Replace with your FastAPI URL
            });

            return services;
        }


        // For Health Data Prepare -->
        public static IServiceCollection AddHealthServices(this IServiceCollection services)
        {
            services.AddScoped<IHealthPrepareservice, HealthPrepareservice>();
            services.AddScoped<IHealthProfileService, HealthProfileService>();
            services.AddScoped<IHealthProfileManager, HealthProfileManager>();
            services.AddAutoMapper(typeof(HealthProfileDataMap));
            return services;
        }


        // For Background Services -->
        public static IServiceCollection AddDailyJobServices(this IServiceCollection services)
        {
            services.AddScoped<IDailyJobsRepository, DailyJobsRepository>();
            services.AddScoped<IDailyJobsManager, DailyJobsManager>();
            services.AddHostedService<DailyJobsService>();
            return services;
        }


        // For Others ( not important ) Services -->
        public static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            services.AddScoped<IOthersService, OthersService>();
            services.AddScoped<IOthersServiceManeger, OthersServiceManeger>();
            return services;
        }


        // For JWT
        public static IServiceCollection AddJWTServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJWTService, JwtService>();
            services.Configure<JWTModel>(configuration.GetSection("JWT"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("JWT").Get<JWTModel>()
                    ?? throw new InvalidOperationException("Missing or invalid JWT configuration");

                if (string.IsNullOrEmpty(jwtSettings.Key))
                    throw new InvalidOperationException("JWT Key is required");
                if (string.IsNullOrEmpty(jwtSettings.Issuer))
                    throw new InvalidOperationException("JWT Issuer is required");
                if (string.IsNullOrEmpty(jwtSettings.Audience))
                    throw new InvalidOperationException("JWT Audience is required");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            return services;
        }


    }
}
