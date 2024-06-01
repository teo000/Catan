namespace Catan.Domain.Common
{
	public static class GraphFunctions
	{
		public static (int, List<(int, int)>) FindLongestRoad(List<(int, int)> edges)
		{
			Dictionary<int, List<int>> adjacencyList = new Dictionary<int, List<int>>();
			foreach (var (start, end) in edges)
			{
				if (!adjacencyList.ContainsKey(start))
				{
					adjacencyList[start] = new List<int>();
				}
				if (!adjacencyList.ContainsKey(end))
				{
					adjacencyList[end] = new List<int>();
				}
				adjacencyList[start].Add(end);
				adjacencyList[end].Add(start);
			}

			int longestRoadLength = 0;
			List<(int, int)> longestRoadEdges = new List<(int, int)>();

			(int, List<(int, int)>) Dfs(int node, HashSet<int> visited, List<(int, int)> currentPath)
			{
				visited.Add(node);
				int maxLength = 0;
				List<(int, int)> maxPath = new List<(int, int)>();

				foreach (var neighbor in adjacencyList[node])
				{
					if (!visited.Contains(neighbor))
					{
						var edge = (node, neighbor);
						currentPath.Add(edge);
						var (length, path) = Dfs(neighbor, visited, currentPath);
						currentPath.RemoveAt(currentPath.Count - 1);

						length += 1;
						if (length > maxLength)
						{
							maxLength = length;
							maxPath = new List<(int, int)>(path);
							maxPath.Insert(0, edge);
						}
					}
				}

				visited.Remove(node); 
				return (maxLength, maxPath);
			}

			foreach (var node in adjacencyList.Keys)
			{
				HashSet<int> visited = new HashSet<int>();
				List<(int, int)> currentPath = new List<(int, int)>();
				var (length, path) = Dfs(node, visited, currentPath);

				if (length > longestRoadLength)
				{
					longestRoadLength = length;
					longestRoadEdges = path;
				}
			}

			return (longestRoadLength, longestRoadEdges);
		}
	}
}
