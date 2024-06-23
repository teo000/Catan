using Catan.Application.Dtos;
using Catan.Application.Dtos.GamePieces;

namespace Catan.Application.Responses;

public class MoveResponse : BaseResponse
{
	//private MoveResponse(bool success, string? moveType = null, SettlementDto? settlement = null, RoadDto? road = null, CityDto? city = null, List<string>? errors = null)
	//{
	//	Success = success;
	//	ValidationErrors = new List<string>(errors);
	//	Settlement = settlement;
	//	Road = road;
	//	City = city;
	//	MoveType = moveType;
	//}
	public MoveResponse(SettlementDto settlement)
	{
		Success = true;
		Settlement = settlement;
		MoveType = "PlaceSettlement";
	}
	public MoveResponse(RoadDto road)
	{
		Success = true;
		Road = road;
		MoveType = "PlaceRoad";
	}
	public MoveResponse(CityDto city)
	{
		Success = true;
		City = city;
		MoveType = "PlaceCity";
	}

	public MoveResponse(DevelopmentCardDto card)
	{
		Success = true;
		DevelopmentCard = card;
		MoveType = "BuyDevelopmentCard";
	}


	public MoveResponse(List<string> errors)
	{
		Success = false;
		ValidationErrors = new List<string>(errors);
	}

	public string? MoveType { get; }
	public SettlementDto? Settlement { get; }
	public RoadDto? Road { get; }
	public CityDto? City { get; }
	public DevelopmentCardDto? DevelopmentCard { get; }
	
	//public static MoveResponse PlaceSettlement(SettlementDto settlement)
	//{
	//	return new MoveResponse(true, "PlaceSettlement", settlement: settlement);
	//}

	//public static MoveResponse PlaceRoad(RoadDto road)
	//{
	//	return new MoveResponse(true, "PlaceRoad", road: road);
	//}

	//public static MoveResponse PlaceCity(CityDto city)
	//{
	//	return new MoveResponse(true, "PlaceCity", city: city);
	//}

	//public static MoveResponse Failure(List<string> errors)
	//{
	//	return new MoveResponse(false, errors: errors);
	//}

}
