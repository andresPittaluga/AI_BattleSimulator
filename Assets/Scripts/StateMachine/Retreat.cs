using System.Collections.Generic;
using UnityEngine;

public abstract class Retreat : State
{
    protected Boid _boid;

    protected List<Pf_Node> _path = new();
    protected int _index = 0;
    protected override void OnEnter()
    {
        _path.Clear();
        _index = 0;

        SpecialAbility();

        FindFarthest();

        _boid.particles.PlayNervousParticles();
    }
    protected override void OnUpdate(float deltaTime)
    {
        _boid.Move();

        if(_boid.AreEnemiesInArea(_boid.healDistance))
            _boid.AddForce(_boid.Arrive(FollowPath()) 
                        + _boid.ObstacleAvoidance() 
                        + _boid.Separate());
        else
            _boid.PathAchieved();
    }
    protected override void OnExit() {}
    public virtual void RefreshAbilityCD() {}
    protected abstract void SpecialAbility();
    private void FindFarthest() => ConstructPath(Pf_NodeManager.Instance.GetExactFarthestNode(_boid.GetExactClosestEnemy().position));
    public void SetPath(List<Pf_Node> path)
    {
        _path = path;
    }
    public Vector3 FollowPath()
    {
        if (_index >= _path.Count || _path == null || _path[_index] == null)
        {
            _boid.PathAchieved();
            return _boid.transform.position;
        }

        Vector3 dir = _path[_index].transform.position - _boid.transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.25f)
            _index++;

        if (_index < _path.Count)
            return _path[_index].transform.position;
        else
        {
            _boid.PathAchieved();
            return _boid.transform.position;
        }
    }
    public void ConstructPath(Pf_Node desired)
    {
        if (_boid.transform == null) return;

        _index = 0;

        if (Observe.LineOfSight(_boid.transform.position, desired.transform.position, _boid.obstacleMask))
        {
            if (_path.Count >= 1)
                _path.Clear();

            _path.Add(desired);
        }
        else
        {
            _path.Clear();

            Pf_Node initialNode = Pf_NodeManager.Instance.GetClosestNode(_boid.transform.position);
            SetPath(Pathfinding.CalculateTheta(initialNode, desired, _boid.obstacleMask));
        }
    }

}
