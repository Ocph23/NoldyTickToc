using GameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {

        public static void Main(string[] args)
        {



        }
    }
}
//{
//    public class Program
//    {

//        public static void Main(string[] args)
//        {





//          var  board = new[,] { { null, "H", "H" }, { "H", null, "M" }, { "M", null, "M" } };

//            var dfs = new DFS(new Player("H", PlayerType.Human), new Player("M", PlayerType.Human));
//            dfs.Nodes = new List<Node>();
//            dfs.Nodes.Add(new Node( 0, new Position(0, 0)));
//            dfs.Nodes.Add(new Node(1, new Position(0, 1)));
//            dfs.Nodes.Add(new Node(2, new Position(0, 2)));
//            dfs.Nodes.Add(new Node(3, new Position(1, 0)));
//            dfs.Nodes.Add(new Node(4, new Position(1, 1)));
//            dfs.Nodes.Add(new Node(5, new Position(1, 2)));
//            dfs.Nodes.Add(new Node(6, new Position(2, 0)));
//            dfs.Nodes.Add(new Node(7, new Position(2, 1)));
//            dfs.Nodes.Add(new Node(8, new Position(2, 2)));

//           var result= dfs.Analysis(board, "H").Result;

//        }
//    }








//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var vertices = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8};
//        var goalStates = new[,] { { 0, 4, 8 }, { 2, 4, 6 }, { 1, 4, 7 }, { 3, 4, 5 }, { 0, 3, 6 }, { 2, 5, 8 }, { 0, 1, 2 }, { 6, 7, 8 } };
//        var edges = new[]{Tuple.Create(1,2), Tuple.Create(1,3),
//            Tuple.Create(2,4), Tuple.Create(3,5), Tuple.Create(3,6),
//            Tuple.Create(4,7), Tuple.Create(5,7), Tuple.Create(5,8),
//            Tuple.Create(5,6), Tuple.Create(8,9), Tuple.Create(9,10)};

//        var graph = new Graph<int>(vertices, edges);
//        var algorithms = new Algorithms();

//        var path = new List<int>();

//        Console.WriteLine(string.Join(", ", algorithms.DFS(graph, 1, v => path.Add(v))));
//       // # 1, 3, 6, 5, 8, 9, 10, 7, 4, 2

//        Console.WriteLine(string.Join(", ", path));
//       // # 1, 3, 6, 5, 8, 9, 10, 7, 4, 2
//    }
//}

//public class Algorithms
//{
//    public HashSet<T> DFS<T>(Graph<T> graph, T start)
//    {
//        var visited = new HashSet<T>();

//        if (!graph.AdjacencyList.ContainsKey(start))
//            return visited;

//        var stack = new Stack<T>();
//        stack.Push(start);

//        while (stack.Count > 0)
//        {
//            var vertex = stack.Pop();

//            if (visited.Contains(vertex))
//                continue;

//            visited.Add(vertex);

//            foreach (var neighbor in graph.AdjacencyList[vertex])
//                if (!visited.Contains(neighbor))
//                    stack.Push(neighbor);
//        }

//        return visited;
//    }
//    public HashSet<T> DFS<T>(Graph<T> graph, T start, Action<T> preVisit = null)
//    {
//        var visited = new HashSet<T>();

//        if (!graph.AdjacencyList.ContainsKey(start))
//            return visited;

//        var stack = new Stack<T>();
//        stack.Push(start);

//        while (stack.Count > 0)
//        {
//            var vertex = stack.Pop();

//            if (visited.Contains(vertex))
//                continue;

//            if (preVisit != null)
//                preVisit(vertex);

//            visited.Add(vertex);

//            foreach (var neighbor in graph.AdjacencyList[vertex])
//                if (!visited.Contains(neighbor))
//                    stack.Push(neighbor);
//        }

//        return visited;
//    }
//}

//public class Graph<T>
//{
//    public Graph() { }
//    public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
//    {
//        foreach (var vertex in vertices)
//            AddVertex(vertex);

//        foreach (var edge in edges)
//            AddEdge(edge);
//    }

//    public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

//    public void AddVertex(T vertex)
//    {
//        AdjacencyList[vertex] = new HashSet<T>();
//    }

//    public void AddEdge(Tuple<T, T> edge)
//    {
//        if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
//        {
//            AdjacencyList[edge.Item1].Add(edge.Item2);
//            AdjacencyList[edge.Item2].Add(edge.Item1);
//        }
//    }
//}






//public class Tree<K, V>    where K : class, IComparable<K>

//where V : class

//{

//    private Node<K, V> root;



//    public V DFS(K key)

//    {

//        Stack<Node<K, V>> stack = new Stack<Node<K, V>>();



//        while (stack.Any())

//        {

//            var node = stack.Pop();



//            if (node.key == key)

//            {

//                return node.value;

//            }

//            foreach (var child in node.children)

//            {

//                stack.Push(child);

//            }

//        }

//        return default(V);

//    }



//    private class Node<K, V>

//        where K : class, IComparable<K>

//        where V : class

//    {

//        public K key;

//        public V value;

//        public Node<K, V>[] children;

//    }

//    //}
//}
