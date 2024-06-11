using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public LayerMask wallLayer;

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = new Node(startPos);
        Node targetNode = new Node(targetPos);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.Equals(targetNode))
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor) || Physics.CheckSphere(neighbor.position, 0.5f, wallLayer))
                {
                    continue;
                }

                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return new List<Node>(); // Return an empty path if no path is found
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (!currentNode.Equals(startNode))
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (Vector3 direction in directions)
        {
            Vector3 neighborPos = node.position + direction;
            if (!Physics.CheckSphere(neighborPos, 0.5f, wallLayer))
            {
                neighbors.Add(new Node(neighborPos));
            }
        }

        return neighbors;
    }

    private float GetDistance(Node nodeA, Node nodeB)
    {
        return Vector3.Distance(nodeA.position, nodeB.position);
    }
}

public class Node
{
    public Vector3 position;
    public float gCost;
    public float hCost;
    public Node parent;

    public Node(Vector3 pos)
    {
        position = pos;
        gCost = 0;
        hCost = 0;
        parent = null;
    }

    public float FCost
    {
        get { return gCost + hCost; }
    }

    public override bool Equals(object obj)
    {
        Node other = obj as Node;
        return other != null && position == other.position;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}
