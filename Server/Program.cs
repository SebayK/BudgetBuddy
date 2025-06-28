using System.Text;
using System.Text.Json.Serialization;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Middlewares;
using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BudgetContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
  .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; });

builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BudgetService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<GoalService>();
builder.Services.AddScoped<IncomeService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AccountTypeService>();
builder.Services.AddScoped<ShareBudgetsService>();
builder.Services.AddHttpClient<CurrencyConverterService>();

builder.Services.AddIdentity<User, IdentityRole>()
  .AddEntityFrameworkStores<BudgetContext>()
  .AddDefaultTokenProviders();

builder.Services
  .AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters() {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidAudience = builder.Configuration["JWTKey:Audience"],
      ValidIssuer = builder.Configuration["JWTKey:Issuer"],
      ClockSkew = TimeSpan.Zero,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"])),
    };
    options.Events = new JwtBearerEvents {
      OnAuthenticationFailed = context => {
        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
        return Task.CompletedTask;
      }
    };
  });
builder.Services.AddCors(options =>
{
  options.AddPolicy("LocalhostPolicy", builder =>
  {
    builder
      .WithOrigins("http://localhost:57474") // podaj konkretne adresy
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials();
  });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
  c.SwaggerDoc("v1", new OpenApiInfo {
    Title = "BudgetBuddy API",
    Version = "v1",
    Description = "API do zarządzania budżetem domowym"
  });
  // Konfiguracja autoryzacji
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Wprowadź token JWT"
  });
  c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
      new OpenApiSecurityScheme {
        Reference = new OpenApiReference {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      Array.Empty<string>()
    }
  });
});

var app = builder.Build();


if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BudgetBuddy API V1"); });
}

// Add global error handling middleware
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext context) => {
  var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
  return Results.Problem(exception?.Message);
});

app.UseCors("LocalhostPolicy");
app.UseMiddleware<UserMiddleware>();
app.UseAuthorization();
app.MapControllers();


app.Run();