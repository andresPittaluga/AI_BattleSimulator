using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLead : State
{
    LeaderFollower _follower;

    private List<Pf_Node> _path = new();
    private int _index = 0;
    public FollowLead(LeaderFollower follower)
    {
        _follower = follower;
    }

    protected override void OnEnter()
    {
        _index = 0;

        if (!_follower.IsLeaderInArea(_follower.leaderMaxDistance))
            FindLeader();
        //else
        //    _follower.PrettyPrint("Entro a Flock");
    }
    protected override void OnUpdate(float deltaTime)
    {
        _follower.Move();

        if (!_follower.IsLeaderInArea(_follower.leaderMaxDistance))
        {
            if (_path.Count < 1)
                FindLeader();
            else
            {
                //_follower.PrettyPrint("Pathin!");
                _follower.AddForce(_follower.Arrive(FollowPath() + _follower.ObstacleAvoidance()));
            }
        }
        else
            Flocking();
    }
    protected override void OnExit()
    {
    }
    private void Flocking()
    {
        if (_follower.leader == null) return;

        _follower.AddForce(_follower.Separate()
                         + _follower.Arriving(_follower.leader, _follower.arriveWeight)
                         + _follower.ObstacleAvoidance() /** _obstacleAvoidWeight*/);

        if(_path.Count >= 1) _path.Clear();
    }
    private void FindLeader()
    {
        if (_follower.leader == null) return;

        //_follower.PrettyPrint("Busco al Lider");
        ConstructPath(Pf_NodeManager.Instance.GetExactClosestNode(_follower.leader.transform.position));
    }
    public void SetPath(List<Pf_Node> path)
    {
        _path = path;
    }
    public Vector3 FollowPath()
    {
        if (_index >= _path.Count || _path == null)
        {
            _path.Clear();
            return _follower.transform.position;
        }

        Vector3 dir = _path[_index].transform.position - _follower.transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.25f)
            _index++;

        if (_index < _path.Count)
            return _path[_index].transform.position;
        else
        {
            _path.Clear();
            return _follower.transform.position;
        }
    }
    public void ConstructPath(Pf_Node desired)
    {
        if (_follower.leader == null) return;

        _index = 0;

        if (Observe.LineOfSight(_follower.transform.position, desired.transform.position, _follower.obstacleMask))
        {
            if(_path.Count >= 1)
                _path.Clear();

            _path.Add(desired);
        }
        else
        {
            _path.Clear();

            Pf_Node initialNode = Pf_NodeManager.Instance.GetExactClosestNode(_follower.transform.position);
            SetPath(Pathfinding.CalculateTheta(initialNode, desired, _follower.obstacleMask));
        }
    }
}
