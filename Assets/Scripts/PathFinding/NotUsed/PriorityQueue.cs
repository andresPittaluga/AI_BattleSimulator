using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    Dictionary<Pf_Node, float> _allNodes = new Dictionary<Pf_Node, float>();

    public int Count { get { return _allNodes.Count; }}

    public void Put(Pf_Node node, float cost)
    {
        if (_allNodes.ContainsKey(node)) _allNodes[node] = cost;
        else _allNodes.Add(node, cost);
    }

    public Pf_Node Get()
    {
        Pf_Node node = null;
        float lowestValue = Mathf.Infinity;

        // [node, 10] [node, 2]
        foreach (var item in _allNodes)
        {
            if(item.Value < lowestValue)
            {
                lowestValue = item.Value;
                node = item.Key;
            }
        }
        _allNodes.Remove(node);
        return node;
    }
}
