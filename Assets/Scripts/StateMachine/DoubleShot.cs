using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShot : Attack
{
    //LeaderFollower _follower;
    public DoubleShot(LeaderFollower follower)
    {
        _boid = follower;
        //_follower = follower;
    }
    protected override IEnumerator AttackRoutine()
    {
        _distance = Vector3.zero;

        while (_boid.AreEnemiesInArea(out Transform enemy, _boid.pursuitDistance))
        {
            if (enemy != null)
                FireBullet(enemy.position);

            yield return new WaitForSeconds(.15f);

            if (enemy != null)
                FireBullet(enemy.position);

            yield return new WaitForSeconds(_boid.attackCD);
        }

        _boid.LostTrack();
    }
}
