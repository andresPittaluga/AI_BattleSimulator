using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : State
{
    protected Boid _boid;
    private Coroutine _currentAttackRoutine;
    protected override void OnEnter()
    {
        //_boid.PrettyPrint("I see the enemy");
        if (_currentAttackRoutine == null)
        {
            _currentAttackRoutine = _boid.StartCoroutine(AttackRoutine());
        }
    }
    protected override void OnUpdate(float deltaTime)
    {
        _boid.Move();

        if (_boid.AreEnemiesInArea(_boid.evadeDistence))
            _boid.AddForce(_boid.Evade(_boid.GetClosestEnemyAgent()) * _boid.evadeWeight + _boid.ObstacleAvoidance() * _boid.obstacleAvoidanceWeight + _boid.Separate());
        else if (_boid.AreEnemiesInArea(_boid.pursuitDistance))
            _boid.AddForce(_boid.Pursuit(_boid.GetClosestEnemyAgent()) * _boid.evadeWeight + _boid.ObstacleAvoidance() * _boid.obstacleAvoidanceWeight + _boid.Separate());
        else
            _boid.LostTrack();
    }
    protected override void OnExit()
    {
        if (_currentAttackRoutine != null)
        {
            _boid.StopCoroutine(_currentAttackRoutine);
            _currentAttackRoutine = null;
        }

        //_boid.PrettyPrint("I've lost the enemy");
    }
    protected Vector3 _distance;
    protected abstract IEnumerator AttackRoutine();
    protected void FireBullet(Vector3 targetPosition)
    {
        Bullet bullet = _boid.BulletFactory.Create();
        bullet.Initialize(_boid.transform.position, targetPosition, _boid.teamMat, _boid.enemyMask);
    }
    public void Stop()
    {
        //_boid.StopCoroutine(AttackRoutine());
        _boid.LostTrack();
    }
}
