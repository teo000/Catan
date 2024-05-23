﻿using AutoMapper;
using Catan.Application.Dtos;
using Catan.Domain.Entities;

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
			CreateMap<Road, RoadDto>()
					.ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id));

			CreateMap<Trade, TradeDto>()
				.ForMember(dest => dest.PlayerToGiveId, opt => opt.MapFrom(src => src.PlayerToGive.Id))
				.ForMember(dest => dest.PlayerToReceiveId, opt => opt.MapFrom(src => src.PlayerToReceive.Id))
				.ForMember(dest => dest.ResourceToGive, opt => opt.MapFrom(src => src.ResourceToGive.ToString()))
				.ForMember(dest => dest.ResourceToReceive, opt => opt.MapFrom(src => src.ResourceToReceive.ToString()))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

			CreateMap<Map, MapDto>()
				   .ForMember(dest => dest.HexTiles, opt => opt.MapFrom(src => src.HexTiles))
				   .ForMember(dest => dest.Settlements, opt => opt.MapFrom(src => src.Settlements))
				   .ForMember(dest => dest.Roads, opt => opt.MapFrom(src => src.Roads));

			CreateMap<Player, PlayerDto>()
					.ForMember(dest => dest.ResourceCount, opt => opt.MapFrom(src => src.ResourceCount))
					.ForMember(dest => dest.Settlements, opt => opt.MapFrom(src => src.Settlements))
					.ForMember(dest => dest.Roads, opt => opt.MapFrom(src => src.Roads));

			CreateMap<GameSession, GameSessionDto>()
				   .ForMember(dest => dest.Map, opt => opt.MapFrom(src => src.GameMap))
				   .ForMember(dest => dest.GameStatus, opt => opt.MapFrom(src => src.GameStatus.ToString()))
				   .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));
		}
	}
}