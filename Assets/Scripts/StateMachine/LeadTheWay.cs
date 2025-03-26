using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadTheWay : State
{
    Leader _leader;

    private List<Pf_Node> _path = new();
    private int _index = 0;
    public LeadTheWay(Leader lead)
    {
        _leader = lead;
    }
    protected override void OnUpdate(float deltaTime)
    {
        _leader.Move();

        if (_path.Count >= _index)
            _leader.AddForce(_leader.Arrive(FollowPath()) + _leader.ObstacleAvoidance() + _leader.Separate());
        else
            _leader.PathAchieved();
    }
    public void SetPath(List<Pf_Node> path)
    {
        _path = path;
    }
    public Vector3 FollowPath()
    {
        if (_index >= _path.Count || _path == null)
            return _leader.transform.position;

        Vector3 dir = _path[_index].transform.position - _leader.transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.25f)
            _index++;

        if (_index < _path.Count)
            return _path[_index].transform.position;
        else
            return _leader.transform.position;
    }
    public void ConstructPath(Pf_Node desired)
    {
        _index = 0;

        if (Observe.LineOfSight(_leader.transform.position, desired.transform.position, _leader.obstacleMask))
        {
            _leader.PrettyPrint("Node On Sight, there is no need to build a path");
            _path.Clear();
            _path.Add(desired);
        }
        else
        {
            _leader.PrettyPrint("I Can't See the Node: Building Pathfinding");
            _path.Clear();

            Pf_Node initialNode = Pf_NodeManager.Instance.GetClosestNode(_leader.transform.position);
            SetPath(Pathfinding.CalculateTheta(initialNode, desired, _leader.obstacleMask));
        }
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }
}
