using DndPersonality;
using DndPersonality.DAL;
using DndPersonality.DAL.Interfaces;
using DndPersonality.DAL.Repositories;
using DndPersonality.Models;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.Subrace;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var AllowLocalHost = "_allowLocalHost";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(p => p.UseSqlServer(builder.Configuration["DatabaseConnection"]));

builder.Services.AddScoped<IEntitiesRepository<CharacterClass>, ClassesRepository>();
builder.Services.AddScoped<IEntitiesRepository<Item>, ItemsRepository>();
builder.Services.AddScoped<IEntitiesRepository<Race>, RacesRepository>();
builder.Services.AddScoped<IEntitiesRepository<Subrace>, SubracesRepository>();
builder.Services.AddScoped<IEntitiesRepository<Armor>, ArmorsRepository>();
builder.Services.AddScoped<IEntitiesRepository<Language>, LanguagesRepository>();
builder.Services.AddScoped<IEntitiesRepository<Skill>, SkillsRepository>();
builder.Services.AddScoped<IEntitiesRepository<Tool>, ToolsRepository>();
builder.Services.AddScoped<IEntitiesRepository<Weapon>, WeaponsRepository>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

builder.Services.AddScoped<IOpenAIService, OpenAIService>();

builder.Services.AddAutoMapper(typeof(CharacterProfile));

if (!builder.Configuration["AllowedCors"].IsNullOrEmpty())
{
    builder.Services.AddCors(p =>
    {
        p.AddPolicy(name: AllowLocalHost,
            policy =>
            {
                policy.WithOrigins(builder.Configuration["AllowedCors"] ?? string.Empty)
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader();
            });
    });
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.RequireHttpsMetadata = bool.Parse(builder.Configuration["Auth0:RequireHttpsMetadata"] ?? "false");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<Context>())
{
    context?.Database.EnsureCreated();
}

app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(AllowLocalHost);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();