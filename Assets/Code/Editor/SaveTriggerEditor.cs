using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveTrigger))]
public class SaveTriggerEditor : UnityEditor.Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SaveTrigger trigger, GizmoType gizmo)
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(trigger.transform.position, new Vector3(1, 1, 1));
    }
}
