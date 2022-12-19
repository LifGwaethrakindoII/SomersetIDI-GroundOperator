using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public Transform node;
    public List<int> connectedIndexes;
}

[System.Serializable]
public struct EdgeData
{
    public int nodeIndex;
    public float distance;
}

public class DijkstraCalculator : MonoBehaviour
{
    public List<NodeData> nodesData;
#if UNITY_EDITOR
    [Header("Editor:")]
    //public float radius = 1;
#endif

    private IEnumerator<Vector3> path;

    private void OnDrawGizmos()
    {
        foreach (NodeData data in nodesData)
        {
            if (data.node != null)
            {
                //Gizmos.DrawWireSphere(data.node.position, radius);
                for(int i = 0; i < data.connectedIndexes.Count; i++)
                {
                    int index = data.connectedIndexes[i];
                    Transform node = nodesData[index].node;
                    if (node != null) Gizmos.DrawLine(data.node.position, node.position);
                }
            }
        }
    }

    private Queue<Vector3> CalculateDijkstraPath(int _start, int _goal)
    {
        Queue<Vector3> path = new Queue<Vector3>();
        HashSet<int> visitedNodes = new HashSet<int>();
        NodeData actual = nodesData[_start];
        NodeData goal = nodesData[_goal];
        int actualNode = _start;
        path.Enqueue(actual.node.position);
        visitedNodes.Add(actualNode);

        

        while (actualNode != _goal)
        {
            List<float> mappedDistances = new List<float>(nodesData.Count);
            float minDistance = Mathf.Infinity;
            int index = 0;

            for (int i = 0; i < actual.connectedIndexes.Count; i++)
            {
                int nodeIndex = actual.connectedIndexes[i];
                Transform node = nodesData[index].node;
                float goalDistance = (actual.node.position - goal.node.position).magnitude;
                float distance = (actual.node.position - node.position).magnitude;
                if ((distance + goalDistance) < minDistance && visitedNodes.Add(nodeIndex))
                {
                    actualNode = index;
                    minDistance = distance;
                    index = i;
                }
            }

            actual = nodesData[index];
            path.Enqueue(actual.node.position);
        }

        return path;
    }

    public IEnumerator<Vector3> IterateDijkstraPath(int _start, int _goal)
    {
        _start = Mathf.Clamp(_start, 0, nodesData.Count - 1);
        _goal = Mathf.Clamp(_goal, 0, nodesData.Count - 1);

        HashSet<int> visitedNodes = new HashSet<int>();
        NodeData actual = nodesData[_start];
        NodeData goal = nodesData[_goal];
        yield return actual.node.position;
        visitedNodes.Add(_start);

        while (actual != goal)
        {
            List<float> mappedDistances = new List<float>(actual.connectedIndexes.Count);
            float minDistance = Mathf.Infinity;
            int index = 0;

            for (int i = 0; i < actual.connectedIndexes.Count; i++)
            {
                int nodeIndex = actual.connectedIndexes[i];
                Transform node = nodesData[nodeIndex].node;
                float goalDistance = (node.position - goal.node.position).magnitude;
                float distance = (actual.node.position - node.position).magnitude;
                if ((distance + goalDistance) < minDistance && !visitedNodes.Contains(nodeIndex))
                {
                    minDistance = distance;
                    index = nodeIndex;
                }
            }

            actual = nodesData[index];
            visitedNodes.Add(index);
            yield return actual.node.position;
        }
    }
}