using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using LetsPlayTogether.Models;
using LetsPlayTogether.Models.Steam;
using LetsPlayTogether.Models.Steam.Responses;
using LetsPlayTogether.Services;
using Game = LetsPlayTogether.Models.DTO.Game;
using Player = LetsPlayTogether.Models.Player;
using PollAppVotes = LetsPlayTogether.Models.DTO.PollAppVotes;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddCors(options => {
    options.AddPolicy(name: "localhost",
        policy => {
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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("localhost");
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();


void ConfigureServices(IServiceCollection serviceCollection) {
    serviceCollection.AddTransient<ISteamService, SteamService>();
    serviceCollection.AddSingleton<IGameRepository, GameRepository>();
    serviceCollection.AddSingleton<IRepository<Poll>, PollRepository>();
    serviceCollection.AddSingleton<IPollService, PollService>();
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
        cfg.CreateMap<SteamGame, DataAccess.Models.Game>();

        // VVV move to another place (consts?)
        var mpIds = new List<int> { 1, 9, 38 };

        cfg.CreateMap<AppDetails, Game>()
            .ConstructUsing((s, ctx) =>
                ctx.Mapper.Map<Game>(s.Data))
            .ForMember(d => d.AppId, s => s.MapFrom(x => x.Data.AppId))
            .ForMember(d => d.IsOk, s => s.MapFrom(x => x.Success))
            .ForMember(d => d.Tags, s => s.MapFrom(
                x => string.Join(", ", x.Data.Categories.Where(c => mpIds.Contains(c.Id)).Select(o => o.Description)) ));

        cfg.CreateMap<Game, DataAccess.Models.Game>();
        cfg.CreateMap<DataAccess.Models.Game, Game>();
        cfg.CreateMap<Game, SteamGame>();
        cfg.CreateMap<SteamGame, Game>();
        cfg.CreateMap<DataAccess.Models.PollAppVotes, LetsPlayTogether.Models.DTO.PollAppVotes>();
        cfg.CreateMap<LetsPlayTogether.Models.DTO.PollAppVotes, DataAccess.Models.PollAppVotes>();
        cfg.CreateMap<DataAccess.Models.Poll, LetsPlayTogether.Models.DTO.Poll>()
            .ForMember(d => d.Id, s => s.MapFrom(x => x.Id.ToString()));
        cfg.CreateMap<DataAccess.Models.ResultVotes, LetsPlayTogether.Models.DTO.ResultVotes>();
    });

    var mapper = new Mapper(config);
    serviceCollection.AddSingleton<IMapper>(mapper);
}