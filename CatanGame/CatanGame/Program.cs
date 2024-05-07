using CatanGame.Entities;
using CatanGame.Data;
using CatanGame.Common;

var teo = new Player("teo");
var rares = new Player("rares");
var diana = new Player("diana");
var casu = new Player("casu");

var players = new List<Player>() {
	teo, rares, diana, casu
};

var gameSession = new GameSession(players);



while (gameSession.GameStatus == GameStatus.InProgress)
{
	var turnPlayer = gameSession.GetTurnPlayer();

	Console.WriteLine("It's " + turnPlayer.Name + "'s turn");


	Result<Settlement> settlementResult;
	do
	{
		Console.Write("Settlement position: ");
		int settlementPos = Convert.ToInt32(Console.ReadLine());

		settlementResult = gameSession.PlaceSettlement(settlementPos, isBeginning: true);

		Console.WriteLine(settlementResult.Error);
	} while (!settlementResult.IsSuccess);

	Console.WriteLine("Settlement placed successfully");


	Result<Road> roadResult;
	do
	{
		Console.Write("Road position: ");
		int roadPos = Convert.ToInt32(Console.ReadLine());

		roadResult = gameSession.PlaceRoad(roadPos);

		Console.WriteLine(roadResult.Error);
	} while (!roadResult.IsSuccess);

	Console.WriteLine("Road placed successfully");

	gameSession.ChangeTurn();

}


static void PrintListofLists(List<List<int>> listOfLists)
{
	foreach (List<int> innerList in listOfLists)
	{
		Console.WriteLine(string.Join(", ", innerList));
	}
}

static void PrintListOfTuples(List<(int, int)> listOfTuples)
{
	int index = 0;
	foreach ((int, int) tuple in listOfTuples)
	{
		Console.WriteLine($"{index}: ({tuple.Item1}, {tuple.Item2})");
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

//PrintListOfTuples(GameMapData.RoadEnds);
//Console.WriteLine();

//PrintDictionaryOfLists(GameMapData.AdjacentSettlements);

