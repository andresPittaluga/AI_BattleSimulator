using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriAttack : Attack
{
    float _angleSpread = 45;
    //Leader _leader;
    public TriAttack(Leader lead)
    {
        _boid = lead;
        //_leader = lead;
    }
    public void SetYOffset(float offset) => _angleSpread = offset;
    protected override IEnumerator AttackRoutine()
    {
        _distance = Vector3.zero;

        while (_boid.AreEnemiesInArea(out Transform enemy, _boid.pursuitDistance))
        {
            FireBullet(enemy.position);
            FireBullet(Quaternion.Euler(0, _angleSpread, 0) * enemy.position);
            FireBullet(Quaternion.Euler(0, -_angleSpread, 0) * enemy.position);

            yield return new WaitForSeconds(_boid.attackCD);
        }

        _boid.LostTrack();
    }
}
