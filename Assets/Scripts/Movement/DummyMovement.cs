using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MovementBase
{
    public override Vector3 GetMovementDirection(Vector3 current, Vector3 target)
    {
        return target;
    }

    public override float GetRotationDirection(float current, float target)
    {
        return target;
    }
}
