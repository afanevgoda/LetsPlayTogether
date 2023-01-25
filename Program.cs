using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using LetsPlayTogether.Models.DTO;
using LetsPlayTogether.Models.DTO.Polls;
using LetsPlayTogether.Models.Steam;
using LetsPlayTogether.Models.Steam.Responses;
using LetsPlayTogether.Services;
using MongoDB.Bson;

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

// if (!app.Environment.IsDevelopment()) {
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("localhost");
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();


void ConfigureServices(IServiceCollection serviceCollection) {
    serviceCollection.AddSingleton<IGameRepository, GameRepository>();
    serviceCollection.AddSingleton<IRepository<Poll>, PollRepository>();
    serviceCollection.AddTransient<ISteamService, SteamService>();
    serviceCollection.AddTransient<IPollService, PollService>();
}

void ConfigureAutoMapper(IServiceCollection serviceCollection) {
    var config = new MapperConfiguration(cfg => {
        cfg.CreateMap<GetPlayerSummariesDto, PlayerDto>()
            .ConstructUsing((s, ctx) =>
                ctx.Mapper.Map<PlayerDto>(s.Response.Players.FirstOrDefault()));
        cfg.CreateMap<SteamPlayerDto, PlayerDto>()
            .ForMember(d => d.Id, s => s.MapFrom(x => x.SteamId))
            .ForMember(d => d.Nickname, s => s.MapFrom(x => x.PersonaName))
            .ForMember(d => d.AvatarUrl, s => s.MapFrom(x => x.Avatar));

        cfg.CreateMap<GetOwnedGamesDto, List<GameDto>>()
            .ConstructUsing((s, ctx) =>
                ctx.Mapper.Map<List<GameDto>>(s.Response.Games));
        cfg.CreateMap<SteamGameDto, Game>();

        // VVV move to another place (constants?)
        // var mpIds = new List<int> { 1, 9, 38 };

        cfg.CreateMap<AppDetailsDto, GameDto>()
            .ConstructUsing((s, ctx) =>
                ctx.Mapper.Map<GameDto>(s.Data))
            .ForMember(d => d.AppId, s => s.MapFrom(x => x.Data.AppId))
            .ForMember(d => d.IsOk, s => s.MapFrom(x => x.Success));

        cfg.CreateMap<GameDto, Game>();
        cfg.CreateMap<Game, GameDto>();
        cfg.CreateMap<GameDto, SteamGameDto>();
        cfg.CreateMap<SteamGameDto, GameDto>();
        cfg.CreateMap<PollAppVotes, PollAppVotesDto>();
        cfg.CreateMap<PollAppVotesDto, PollAppVotes>();
        cfg.CreateMap<Poll, PollDto>()
            .ForMember(d => d.Id, s => s.MapFrom(x => x.Id.ToString()));
        cfg.CreateMap<PollDto, Poll>()
            .ForMember(d => d.Id, s => s.MapFrom(x => ObjectId.Parse(x.Id)));
        cfg.CreateMap<ResultVotes, ResultVotesDto>();
        cfg.CreateMap<GameDto, PollMatchedGame>();
        cfg.CreateMap<Game, PollMatchedGameDto>();
        cfg.CreateMap<PollMatchedGameDto, PollMatchedGame>();
        cfg.CreateMap<PollMatchedGame, PollMatchedGameDto>();
    });

    var mapper = new Mapper(config);
    serviceCollection.AddSingleton<IMapper>(mapper);
}