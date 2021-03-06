﻿using Fyrvall.DataEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[BonaDataEditor]
public class Agent : MonoBehaviour
{
    public float MovementSpeed = 1f;

    public MovementBase Movement;

    [HideInInspector]
    public Vector3 TargetMovementDirection;
    [HideInInspector]
    public Vector3 CurrentMovementDirection;

    [HideInInspector]
    public float TargetRotationDirection;

    [HideInInspector]
    public float CurrentRotationDirection;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (Movement == null) {
            Movement = ScriptableObject.CreateInstance<DummyMovement>();
        }
    }

    public void SetMovementDirection(Vector3 direction)
    {
        direction.y = 0;
        TargetMovementDirection = direction.normalized;
    }

    public void SetRotationDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0) {
            direction.Normalize();
            TargetRotationDirection = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CurrentMovementDirection = Movement.GetMovementDirection(CurrentMovementDirection, TargetMovementDirection);
        var offset = CurrentMovementDirection * MovementSpeed * Time.deltaTime;

        CurrentRotationDirection = Movement.GetRotationDirection(CurrentRotationDirection, TargetRotationDirection);

        transform.rotation = Quaternion.Euler(0, CurrentRotationDirection, 0);
        transform.position = transform.position + offset;
    }

    public float GetMovementSpeed()
    {
        return (CurrentMovementDirection * MovementSpeed).magnitude;
    }
}
