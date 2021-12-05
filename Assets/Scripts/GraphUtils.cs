using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphUtils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Vector2> GetNeighbors(Dictionary<Vector2, List<Vector2>> graph, Vector2 src)
    {
        return graph[src];
    }

    public int GetDistance(Dictionary<Vector2, List<Vector2>> graph, Vector2 src, Vector2 dst)
    {
        var visitedNodes = new List<Vector2>();
        var currentNodes = new List<Vector2>{src};
        var distance = 0;
        while (true)
        {
            if (currentNodes.Contains(dst))
            {
                break;
            }
            var nextNodes = new List<Vector2>();
            foreach (var node in currentNodes)
            {
                foreach (var neighbor in GetNeighbors(graph, node))
                {
                    if (!visitedNodes.Contains(neighbor))
                    {
                        visitedNodes.Add(neighbor);
                        nextNodes.Add(neighbor);
                    }
                }
            }
            if (nextNodes.Count == 0)
            {
                distance = int.MaxValue;
                break;
            }
            currentNodes.Clear();
            currentNodes.AddRange(nextNodes);
            nextNodes.Clear();
            distance++;
        }
        return distance;
    }

    public Vector2 GetNextNodeRandom(Dictionary<Vector2, List<Vector2>> graph, Vector2 src)
    {
        var options = GetNeighbors(graph, src);
        if (options.Count == 0)
        {
            return Vector2.zero;
        }
        var index = Random.Range(0, options.Count);
        return options[index];
    }

    public Vector2 GetNextNodeClosestTo(Dictionary<Vector2, List<Vector2>> graph, Vector2 src, Vector2 dst)
    {
        var options = GetNeighbors(graph, src);
        var distances = new List<int>();
        foreach (var option in options)
        {
            distances.Add(GetDistance(graph, option, dst));
        }
        var validOptions = new List<Vector2>();
        for (int i = 0; i < options.Count; i++)
        {
            if (distances[i] == distances.Min())
            {
                validOptions.Add(options[i]);
            }
        }
        if (validOptions.Count == 0)
        {
            return Vector2.zero;
        }
        var index = Random.Range(0, validOptions.Count);
        return validOptions[index];
    }

    public Vector2 GetNextNodeFurthestTo(Dictionary<Vector2, List<Vector2>> graph, Vector2 src, Vector2 dst)
    {
        var options = GetNeighbors(graph, src);
        var distances = new List<int>();
        foreach (var option in options)
        {
            distances.Add(GetDistance(graph, option, dst));
        }
        var validOptions = new List<Vector2>();
        for (int i = 0; i < options.Count; i++)
        {
            if (distances[i] == distances.Max())
            {
                validOptions.Add(options[i]);
            }
        }
        if (validOptions.Count == 0)
        {
            return Vector2.zero;
        }
        var index = Random.Range(0, validOptions.Count);
        return validOptions[index];
    }
}
