using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Node> _neighbours;
    Dictionary<int, float> distanceMapping;

    public List<Node> neighbours
    {
        get { return _neighbours; }
        set { _neighbours = value; }
    }

    private void Awake()
    {
        distanceMapping = new Dictionary<int, float>();
        CalculateDistances();
    }

    private void CalculateDistances()
    {
        foreach(Node node in neighbours)
        {
            distanceMapping.Add(node.GetInstanceID(), (node.transform.position - transform.position).magnitude);
        }
    }

    public Queue<Node> GetDijkstraPath(Node _actual, Node _goal)
    {
        Queue<Node> result = new Queue<Node>();
        Node actualNode = _actual;
        result.Enqueue(_actual);

        while (actualNode != _goal)
        {
            actualNode = ShortestNode(actualNode, _goal);
            result.Enqueue(actualNode);
        }

        return result;
    }

    public static Node ShortestNode(Node _node, Node _goal)
    {
        List<float> measuredDistances = new List<float>();

        foreach (Node node in _node.neighbours)
        {
            float distanceToGoal = (_goal.transform.position - node.transform.position).magnitude;
            float distanceToNeighbour = (node.transform.position - _node.transform.position).magnitude;
            measuredDistances.Add(distanceToGoal + distanceToNeighbour);
        }

        float minDistance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < measuredDistances.Count; i++)
        {
            if (measuredDistances[i] < minDistance)
            {
                minDistance = measuredDistances[i];
                index = i;
            }
        }

        return _node.neighbours[index];
    }
}
