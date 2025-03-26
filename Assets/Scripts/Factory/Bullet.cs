using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("<color=#1A21D1>Stats")]
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private int _damage;
    [SerializeField] LayerMask _hitteableMask;
    [SerializeField] LayerMask _obstacleMask;
    private Vector3 _dir;

    public ObjectPool<Bullet> Pool;
    public void Initialize(Vector3 origin, Vector3 target, Material mat, LayerMask hitMask)
    {
        GetComponent<MeshRenderer>().material = mat;

        transform.position = origin;
        _dir = target - origin;

        _hitteableMask = hitMask;
    }

    private void Update()
    {
        transform.position += _dir.normalized * _speed * Time.deltaTime;

        CollisionCheck();
    }
    private void CollisionCheck()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, .5f, _hitteableMask + _obstacleMask);

        if (colls.Length > 0)
        {
            if (colls[0].TryGetComponent<IDamageable>(out IDamageable enemy))
            {
                enemy.TakeDamage(_damage);
            }

            TurnOff();
        }
    }
    public void TurnOn()
    {
        gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
