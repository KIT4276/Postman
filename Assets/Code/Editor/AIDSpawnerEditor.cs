using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIDSpawner))]
public class AIDSpawnerEditor : UnityEditor.Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(AIDSpawner spawner, GizmoType gizmo)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(spawner.transform.position, 0.5f);
    }
}
