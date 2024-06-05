using CatanGame.Entities;
using CatanGame.Data;
using CatanGame.Common;
using System.Linq;


void InitialPhasePlaceSettlementAndRoad(GameSession gameSession, Player turnPlayer)
{
	Console.WriteLine("It's " + turnPlayer.Name + "'s turn");


	Result<Settlement> settlementResult;
	do
	{
		Console.Write("Settlement position: ");
		int settlementPos = Convert.ToInt32(Console.ReadLine());

		settlementResult = gameSession.PlaceSettlement(settlementPos, isInitialPhase: true);

		Console.WriteLine(settlementResult.Error);
	} while (!settlementResult.IsSuccess);

	Console.WriteLine("Settlement placed successfully");

	var lastPlacedSettlementPos = settlementResult.Value.Position;

	Result<Road> roadResult;
	do
	{
		Console.Write("Road position: ");
		int roadPos = Convert.ToInt32(Console.ReadLine());

		roadResult = gameSession.PlaceRoad(roadPos, isInitialPhase: true, lastPlacedSettlementPos: lastPlacedSettlementPos);

		Console.WriteLine(roadResult.Error);
	} while (!roadResult.IsSuccess);

	Console.WriteLine("Road placed successfully");
}

void PlaceSettlement(GameSession gameSession)
{
	Result<Settlement> settlementResult;

	Console.Write("Settlement position: ");
	var x = Console.ReadLine();
	int settlementPos = Convert.ToInt32(x);

	settlementResult = gameSession.PlaceSettlement(settlementPos);

	Console.WriteLine(settlementResult.Error);


	Console.WriteLine("Settlement placed successfully");
}


void PlaceRoad(GameSession gameSession)
{
	Result<Road> roadResult;

	Console.Write("Road position: ");
	int roadPos = Convert.ToInt32(Console.ReadLine());

	roadResult = gameSession.PlaceRoad(roadPos);

	Console.WriteLine(roadResult.Error);

	Console.WriteLine("Road placed successfully");
}

var teo = new Player("teo");
//var rares = new Player("rares");
//var diana = new Player("diana");
//var casu = new Player("casu");

var players = new List<Player>() {
	teo/*, rares, diana, casu*/
};

var gameSession = new GameSession(players);


//foreach (var player in players)
//{
//	InitialPhasePlaceSettlementAndRoad(gameSession, player);
//}

//players.Reverse();

//foreach (var player in players)
//{
//	InitialPhasePlaceSettlementAndRoad(gameSession, player);
//}


//while (gameSession.GameStatus == GameStatus.InProgress)
//{
//	var turnPlayer = gameSession.GetTurnPlayer();

//	Console.WriteLine("It's " + turnPlayer.Name + "'s turn");

//	Console.WriteLine("Place road (r), settlement (s), or quit (q):  ");



//	char action;

//	do
//	{

//		action = (char)Console.Read();
//		Console.ReadLine();

//		switch (action)
//		{
//			case 'r':
//				PlaceRoad(gameSession); break;
//			case 's':
//				PlaceSettlement(gameSession); break;
//			//case 'c':
//			//	PlaceSettlement(gameSession); break;
//			default:
//				break;
//		}

//		PlaceSettlement(gameSession);

//	} while (action != 'q');

//	gameSession.ChangeTurn();

//}


static void PrintListofLists(List<List<int>> listOfLists)
{
	int i = 0;
	foreach (List<int> innerList in listOfLists)
	{
		Console.WriteLine(i + ": " + string.Join(", ", innerList));
		i++;
	}
}

static void PrintListOfTuples(List<(int, int)> listOfTuples)
{
	int index = 0;
	foreach ((int, int) tuple in listOfTuples)
	{
		Console.WriteLine($"[{tuple.Item1}, {tuple.Item2}],");
		index++;
	}
}

static void PrintDictionaryOfLists(Dictionary<int, List<int>> dictOfLists)
{
	foreach (int key in dictOfLists.Keys)
	{
		Console.WriteLine($"{key}: " + string.Join(", ", dictOfLists[key]));
	}
}

//PrintListofLists(GameMapData.SettlementAdjacentTiles);
//Console.WriteLine(GameMapData.SettlementAdjacentTiles.Count);
//Console.WriteLine();

PrintListOfTuples(GameMapData.RoadEnds);
//Console.WriteLine();

//PrintDictionaryOfLists(GameMapData.AdjacentSettlements);

