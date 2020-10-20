using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LerpMovement", menuName = "Movement/Lerp")]
public class LerpMovement : MovementBase
{
    public float Acceleration = 1;
    public float Rotation = 1f;

    public override Vector3 GetMovementDirection(Vector3 current, Vector3 target)
    {
        return Vector3.Lerp(current, target, Acceleration * Time.deltaTime);
    }

    public override float GetRotationDirection(float current, float target)
    {
        return Mathf.LerpAngle(current, target, Rotation * Time.deltaTime);
    }
}
