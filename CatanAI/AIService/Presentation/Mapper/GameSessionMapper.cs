using AIService.Entities.Data;
using AIService.Entities.Game;

namespace AIService.Presentation.Mapper
{
	//public static class GameSessionMapper
	//{
	//	public static GameState ToGameState(GameSessionDto dto)
	//	{
	//		return new GameState
	//		{
	//			Id = dto.Id,
	//			Map = MapMapper.ToMap(dto.Map),
	//			Players = dto.Players.Select(PlayerMapper.ToPlayer).ToList(),
	//			GameStatus = Enum.Parse<GameStatus>(dto.GameStatus, true),
	//			TurnPlayerIndex = dto.Players.FindIndex(p => p.Id == dto.TurnPlayer.Id),
	//			TurnEndTime = dto.TurnEndTime,
	//			Round = dto.Round,
	//			Dice = DiceRollMapper.ToDiceRoll(dto.Dice),
	//			Trades = dto.Trades.Select(TradeMapper.ToPlayerTrade).ToList(),
	//			Winner = dto.Winner != null ? PlayerMapper.ToPlayer(dto.Winner) : null,
	//			LongestRoad = dto.LongestRoad != null ? LongestRoadMapper.ToLongestRoad(dto.LongestRoad) : null,
	//			ThiefMovedThisTurn = dto.ThiefMovedThisTurn
	//		};
	//	}
	//}
}
