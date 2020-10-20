using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LinearMovement", menuName = "Movement/Linear")]
public class LinearMovement : MovementBase
{
    public float Acceleration = 1f;
    public float RotationSpeed = 1f;

    public override Vector3 GetMovementDirection(Vector3 current, Vector3 target)
    {
        return Vector3.Lerp(current, target, Acceleration * Time.deltaTime);
    }

    public override float GetRotationDirection(float current, float target)
    {
        var rotationDelta = Mathf.DeltaAngle(current, target);

        var delta = RotationSpeed * Time.deltaTime;
        if(rotationDelta < 0) {
            delta = -delta;
        }

        if(Mathf.Abs(delta) <= rotationDelta) {
            return target;
        }

        var result = current + delta;
        if(result > 360) {
            result -= 360;
        }else if(result < 0) {
            result += 360;
        }

        return result;
    }
}
