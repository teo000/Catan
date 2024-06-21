using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.Dtos.GamePieces;
using Catan.Domain.Entities;
using Catan.Domain.Entities.GamePieces;
using Catan.Domain.Entities.Harbors;
using Catan.Domain.Entities.GameMap;
using Catan.Domain.Entities.Misc;
using Catan.Domain.Entities.Trades;

namespace Catan.Application.Mapper
{
    public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
			CreateMap<HexTile, HexTileDto>()
					.ForMember(dest => dest.Resource, opt => opt.MapFrom(src => src.Resource.ToString()));

			CreateMap<Settlement, SettlementDto>()
					.ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id));
			CreateMap<City, CityDto>()
					.ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id));
			CreateMap<Road, RoadDto>()
					.ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id));

			CreateMap<DevelopmentCard, DevelopmentCardDto>()
				.ForMember(dest => dest.DevelopmentType, opt => opt.MapFrom(src => src.DevelopmentType.ToString()));

			CreateMap<LongestRoad, LongestRoadDto>()
				.ForMember(dest => dest.Roads, opt => opt.MapFrom(src => src.Roads))
				.ForMember(dest => dest.Player, opt => opt.MapFrom(src => src.Player));

			CreateMap<DiceRoll, DiceRollDto>();

			CreateMap<PlayerTrade, TradeDto>()
				.ForMember(dest => dest.PlayerToGiveId, opt => opt.MapFrom(src => src.PlayerToGive.Id))
				.ForMember(dest => dest.PlayerToReceiveId, opt => opt.MapFrom(src => src.PlayerToReceive.Id))
				.ForMember(dest => dest.ResourceToGive, opt => opt.MapFrom(src => src.ResourceToGive.ToString()))
				.ForMember(dest => dest.ResourceToReceive, opt => opt.MapFrom(src => src.ResourceToReceive.ToString()))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


			CreateMap<SpecialHarbor, SpecialHarborDto>()
				.ForMember(dest => dest.Resource, opt => opt.MapFrom(src => src.Resource.ToString()));


			CreateMap<Map, MapDto>()
				   .ForMember(dest => dest.HexTiles, opt => opt.MapFrom(src => src.HexTiles))
				   .ForMember(dest => dest.Settlements,
						opt => opt.MapFrom(src => src.Buildings
						.Where(building => building != null)
						.Where(building => building is Settlement)
						.ToList()))
				   .ForMember(dest => dest.Cities,
						opt => opt.MapFrom(src => src.Buildings
						.Where(building => building != null)
						.Where(building => building is City)
						.ToList()))
				   .ForMember(dest => dest.Roads,
						opt => opt.MapFrom(src => src.Roads.Where(road => road != null).ToList()))
				   .ForMember(dest => dest.SpecialHarbors, 
						opt => opt.MapFrom(src => src.Harbors
						.Where(harbor => harbor is SpecialHarbor)
						.ToList()));

			CreateMap<Player, PlayerDto>()
					.ForMember(dest => dest.ResourceCount, opt => opt.MapFrom(src => src.ResourceCount))
					.ForMember(dest => dest.Settlements, opt => opt.MapFrom(src => src.Settlements))
					.ForMember(dest => dest.Cities, opt => opt.MapFrom(src => src.Cities))
					.ForMember(dest => dest.Roads, opt => opt.MapFrom(src => src.Roads))
					.ForMember(dest => dest.DevelopmentCards, opt => opt.MapFrom(src => src.DevelopmentCards))
					.ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.ToString().ToLower()));

			CreateMap<GameSession, GameSessionDto>()
				   .ForMember(dest => dest.Map, opt => opt.MapFrom(src => src.GameMap))
				   .ForMember(dest => dest.GameStatus, opt => opt.MapFrom(src => src.GameStatus.ToString()))
				   .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players))
				   .ForMember(dest => dest.TurnPlayer, opt => opt.MapFrom(src => src.GetTurnPlayer()))
				   .ForMember(dest => dest.Dice, opt => opt.MapFrom(src => src.Dice))
				   .ForMember(dest => dest.Trades, opt => opt.MapFrom(src => src.Trades))
				   .ForMember(dest => dest.Winner, opt => opt.MapFrom(src => src.Winner));

			CreateMap<Lobby, LobbyDto>()
				.ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players))
				.ForMember(dest => dest.GameSession, opt => opt.MapFrom(src => src.GameSession));

		}
	}
}
