using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    [Header("<color=#BB1C11>Steering Agent Stats")]
    [SerializeField] protected float _maxSpeed;
    [SerializeField] protected float _maxForce;
    [SerializeField] protected float _stopDistance = 0.1f;

    protected Vector3 _velocity;

    [Header("<color=orange>Physics")]
    [SerializeField] public LayerMask obstacleMask;
    [SerializeField] protected float _obstacleAvoidRadius = 10;
    [SerializeField] protected float _viewRadius = 10;

    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = (target - transform.position).normalized * _maxSpeed;
        return CalculateSteering(desired);
    }

    protected Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - _velocity;
        return Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);
    }

    public void AddForce(Vector3 force) => _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);

    public void StopMovement() => _velocity = Vector3.zero;
    public virtual Vector3 Arrive(Vector3 targetPos)
    {
        float dist = Vector3.Distance(transform.position, targetPos);

        if (dist < _stopDistance) dist = 0;
        if (dist > _viewRadius) return Seek(targetPos);

        return Seek(targetPos);
    }

    public void Move()
    {
        if (_velocity == Vector3.zero) return;
        transform.forward = _velocity;
        transform.position += _velocity * Time.deltaTime;
    }
    public Vector3 Pursuit(SteeringAgent target)
    {
        if (target == null) return Vector3.zero;

        else
            return Seek(target.transform.position + target._velocity);
    }

    public Vector3 Evade(SteeringAgent target)
    {
        if (target == null) return Vector3.zero;

        else
            return -Pursuit(target);
    }
    public Vector3 ObstacleAvoidance()
    {
        Debug.DrawLine(transform.position, transform.position + (transform.forward * _obstacleAvoidRadius));

        if (Physics.SphereCast(transform.position, .25f, transform.forward, out RaycastHit hit, _obstacleAvoidRadius, obstacleMask))
        {
            var obstacle = hit.transform;
            Vector3 dirToObj = obstacle.position - transform.position;
            float angleInBetween = Vector3.SignedAngle(transform.forward, dirToObj, Vector3.up);
            Vector3 desired = angleInBetween >= 0 ? -transform.right : transform.right;
            desired.Normalize();
            desired *= _maxSpeed;

            Vector3 steering = Vector3.ClampMagnitude(desired - _velocity, _maxForce);

            return steering;
        }

        return Vector3.zero;
    }

    public float GetSpeed() => _maxSpeed;
    public float SetSpeed(float s) => _maxSpeed = s;
}
