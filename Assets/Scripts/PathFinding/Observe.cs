using UnityEngine;

public class Observe
{
    public static bool LineOfSight(Vector3 from, Vector3 to, LayerMask obstacleLayer)
    {
        var dir = to - from;
        return !Physics.Raycast(from, dir, dir.magnitude, obstacleLayer);
    }

    public static bool FieldOfView(Transform from, Transform to, float viewAngle, float viewRadius, LayerMask obstacleLayer)
    {
        var forward = from.forward;
        var dir = to.position - from.position;

        if (dir.magnitude > viewRadius) return false;

        var angle = Vector3.Angle(forward, dir);

        if (angle < viewAngle * .5f && LineOfSight(from.position, to.position, obstacleLayer)) return true;

        return false;
    }

    public static void DrawLineOfSightGizmo(Vector3 from, Vector3 to, LayerMask nonWalkable)
    {
        var dir = to - from;

        bool hasLineOfSight = !Physics.Raycast(from, dir, dir.magnitude, nonWalkable);

        if(!hasLineOfSight) return;

        Gizmos.color = hasLineOfSight ? Color.green : Color.red;
        Gizmos.DrawLine(from, to);
    }

    public static void DrawFieldOfViewGizmo(Vector3 from, Vector3 forward, float viewAngle, float viewRadius, Color color)
    {
        Gizmos.color = color;

        Gizmos.DrawRay(from, Quaternion.Euler(0, -viewAngle / 2, 0) * forward * viewRadius);
        Gizmos.DrawRay(from, Quaternion.Euler(0, viewAngle / 2, 0) * forward * viewRadius);

        //Gizmos.DrawWireSphere(from, viewRadius);
    }

    public static string ToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        int a = Mathf.RoundToInt(color.a * 255);

        return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }
}
