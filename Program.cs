using AutoMapper;
using LetsPlayTogether.Models;
using LetsPlayTogether.Models.Steam;
using LetsPlayTogether.Models.Steam.Responses;
using LetsPlayTogether.Services;
using Player = LetsPlayTogether.Models.Player;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "localhost",
        policy  =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
        });
});

ConfigureServices(builder.Services);
ConfigureAutoMapper(builder.Services);

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseHsts();
}

// app.UseHttpsRedirection();
// app.UseStaticFiles();
app.UseCors("localhost");
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();


void ConfigureServices(IServiceCollection serviceCollection) {
    serviceCollection.AddTransient<ISteamService, SteamService>();
}

void ConfigureAutoMapper(IServiceCollection serviceCollection) {
    var config = new MapperConfiguration(cfg => {
        cfg.CreateMap<GetPlayerSummaries, Player>()
            .ConstructUsing((s, ctx) =>
                ctx.Mapper.Map<Player>(s.Response.Players.FirstOrDefault()));
        cfg.CreateMap<SteamPlayer, Player>()
            .ForMember(d => d.Id, s => s.MapFrom(x => x.SteamId))
            .ForMember(d => d.Nickname, s => s.MapFrom(x => x.PersonaName))
            .ForMember(d => d.AvatarUrl, s => s.MapFrom(x => x.Avatar));
        
        cfg.CreateMap<GetOwnedGames, List<Game>>()
            .ConstructUsing((s, ctx) =>
                ctx.Mapper.Map<List<Game>>(s.Response.Games));
        cfg.CreateMap<SteamGame, Game>()
            .ForMember(d => d.Id, s => s.MapFrom(x => x.AppId));
        
        cfg.CreateMap<AppDetails, Game>()
            .ConstructUsing((s, ctx) =>
                ctx.Mapper.Map<Game>(s.Data));
    });
    
    var mapper = new Mapper(config);
    serviceCollection.AddSingleton<IMapper>(mapper);
}