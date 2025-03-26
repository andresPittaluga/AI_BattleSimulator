using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public static List<Pf_Node> CalculateTheta(Pf_Node startingNode, Pf_Node goalNode, LayerMask obstacleLayer)
    {
        var listNode = ConstructPathAStar(startingNode, goalNode);

        int current = 0;

        while (current + 2 < listNode.Count)
        {
            if (Observe.LineOfSight(listNode[current].transform.position, listNode[current + 2].transform.position, obstacleLayer))
                listNode.RemoveAt(current + 1);
            else
                current++;
        }
        return listNode;
    }
    public static List<Pf_Node> ConstructPathAStar(Pf_Node startingNode, Pf_Node goalNode)
    {
        if (!startingNode || !goalNode) return null;

        PriorityQueue frontier = new();
        frontier.Put(startingNode, startingNode.cost);//Enqueue = Add

        Dictionary<Pf_Node, Pf_Node> cameFrom = new();
        cameFrom.Add(startingNode, null);

        Dictionary<Pf_Node, float> costSoFar = new();
        costSoFar.Add(startingNode, startingNode.cost);

        while (frontier.Count > 0)
        {
            Pf_Node current = frontier.Get();//Get + Remove

            if (current == goalNode)
            {
                List<Pf_Node> path = new();
                Pf_Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }
                path.Reverse();
                return path;
            }

            foreach (Pf_Node next in current.GetNeighbors())
            {
                if (next.isBlocked) continue;
                float dist = Vector3.Distance(goalNode.transform.position, next.transform.position);

                float newCost = costSoFar[current] + dist;
                float priority = newCost + dist;
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Put(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (newCost < costSoFar[next])
                {
                    frontier.Put(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }

        return null;
    }


}