using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.Mapper;
using Catan.Domain.Entities;
using Catan.Domain.Data;
using System.Numerics;
using Catan.Application.Dtos.GamePieces;

namespace Catan.Application.Tests
{
    public class MappingTests
	{
		private readonly IConfigurationProvider _configuration;
		private readonly IMapper _mapper;

		public MappingTests()
		{
			_configuration = new MapperConfiguration(config =>
				config.AddProfile<MappingProfile>());

			_mapper = _configuration.CreateMapper();
		}

		[Test]
		public void ShouldHaveValidConfiguration()
		{
			_configuration.AssertConfigurationIsValid();
		}

		[Test]
		public void CanMapRoadDto_Success()
		{
			var player = new Player("teo");
			var road = new Road(player, 2);

			var roadDto = _mapper.Map<RoadDto>(road);
			Console.WriteLine(roadDto.PlayerId.ToString());
			Console.WriteLine(road.Player.Id);

			Assert.That(road.Player.Id, Is.EqualTo(roadDto.PlayerId));
			Assert.That(roadDto.Position, Is.EqualTo(road.Position));
		}

		[Test]
		public void CanMapSettlementDto_Success()
		{
			var player = new Player("teo");
			var settlement = new Settlement(player, false, 2);

			var settlementDto = _mapper.Map<SettlementDto>(settlement);

			Assert.That(settlement.Position, Is.EqualTo(settlementDto.Position));
			Assert.That(settlement.IsCity, Is.EqualTo(settlementDto.IsCity));
			Assert.That(settlement.Player.Id, Is.EqualTo(settlementDto.PlayerId));
		}

		[Test]
		public void CanMapHexTileDto_Success()
		{
			var hexTile = new HexTile(Resource.Wood);

			var hexTileDto = _mapper.Map<HexTile>(hexTile);

			Assert.That(hexTile.Resource, Is.EqualTo(hexTileDto.Resource));
			Assert.That(hexTile.Number, Is.EqualTo(hexTileDto.Number));
		}

		[Test]
		public void CanMapPlayerDto_Success()
		{
			var player = new Player("teo");

			var playerDto = _mapper.Map<PlayerDto>(player);

			Assert.That(player.Id, Is.EqualTo(playerDto.Id));
			Assert.That(player.Name, Is.EqualTo(playerDto.Name));
			Assert.That(player.IsActive, Is.EqualTo(playerDto.IsActive));
			Assert.That(player.ResourceCount, Is.EqualTo(playerDto.ResourceCount));

			Assert.That(player.Settlements.Count, Is.EqualTo(playerDto.Settlements.Count));

			for (var i = 0; i < playerDto.Settlements.Count; i++)
			{
				var settlement = player.Settlements[i];
				var settlementDto = playerDto.Settlements[i];

				Assert.That(settlement.Position, Is.EqualTo(settlementDto.Position));
				Assert.That(settlement.IsCity, Is.EqualTo(settlementDto.IsCity));
				Assert.That(settlement.Player.Id, Is.EqualTo(settlementDto.PlayerId));
			}
		}

		[Test]
		public void CanMapMapDto_Success()
		{
			var map = new Map();

			var mapDto = _mapper.Map<MapDto>(map);

			Assert.That(map.ThiefPosition, Is.EqualTo(mapDto.ThiefPosition));

			Assert.That(map.Settlements.Length, Is.EqualTo(mapDto.Settlements.Length));

			for (var i = 0; i < map.Settlements.Length; i++)
			{
				var settlement = map.Settlements[i];
				var settlementDto = mapDto.Settlements[i];

				Assert.That(settlement.Position, Is.EqualTo(settlementDto.Position));
				Assert.That(settlement.IsCity, Is.EqualTo(settlementDto.IsCity));
				Assert.That(settlement.Player.Id, Is.EqualTo(settlementDto.PlayerId));
			}

			Assert.That(map.Roads.Length, Is.EqualTo(mapDto.Roads.Length));

			for(var i = 0;i < map.Roads.Length; i++)
			{
				var road = map.Roads[i];
				var roadDto = mapDto.Roads[i];

				Assert.That(road.Player.Id, Is.EqualTo(roadDto.PlayerId));
				Assert.That(roadDto.Position, Is.EqualTo(road.Position));
			}


		}
	}
}
