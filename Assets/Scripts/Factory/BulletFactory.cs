using UnityEngine;
public class BulletFactory : Factory<Bullet>
{
    [SerializeField] private Bullet _prefab;
    private ObjectPool<Bullet> _pool;
    private void Awake()
    {
        _pool = new ObjectPool<Bullet>(InstantiatePrefab, TurnOn, TurnOff, _startAmount);
    }
    public override Bullet Create()
    {
        Bullet bullet = _pool.Get();
        bullet.Pool = _pool;
        return bullet;
    }
    private Bullet InstantiatePrefab()
    {
        return Instantiate(_prefab);
    }
    private void TurnOn(Bullet bullet)
    {
        bullet.TurnOn();
    }
    private void TurnOff(Bullet bullet)
    {
        bullet.TurnOff();
    }
}

