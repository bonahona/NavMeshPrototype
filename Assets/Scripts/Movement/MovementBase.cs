using Fyrvall.DataEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BonaDataEditor(DisplayName = "Movement")]
public abstract class MovementBase : ScriptableObject
{
    public abstract Vector3 GetMovementDirection(Vector3 current, Vector3 target);
    public abstract float GetRotationDirection(float current, float target);
}
