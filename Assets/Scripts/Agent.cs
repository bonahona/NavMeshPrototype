using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    public float MovementSpeed = 1f;
    public float Acceleration = 1f;
    public float RotationSpeed = 1f;

    [HideInInspector]
    public Vector3 TargetRotationDirection;

    [HideInInspector]
    public Vector3 TargetMovementDirection;

    [HideInInspector]
    public Vector3 CurrentMovementDirection;
    [HideInInspector]
    public Vector3 CurrentRotationDirection;

    protected NavMeshAgent NavMeshAgent;

    // Start is called before the first frame update
    public virtual void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();   
    }

    public void SetMovementDirection(Vector3 direction)
    {
        TargetMovementDirection = direction.normalized;
    }

    public void SetRotationDirection(Vector3 direction)
    {
        direction.y = 0;
        TargetRotationDirection = direction;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CurrentMovementDirection = Vector3.Lerp(CurrentMovementDirection, TargetMovementDirection, Acceleration * Time.deltaTime);
        var offset = CurrentMovementDirection * MovementSpeed * Time.deltaTime;

        CurrentRotationDirection = Vector3.Lerp(CurrentRotationDirection, TargetRotationDirection, RotationSpeed * Time.deltaTime);

        if (CurrentRotationDirection.sqrMagnitude > 0) {
            transform.rotation = Quaternion.LookRotation(CurrentRotationDirection);
        }

        NavMeshAgent.Move(offset);
    }

    public float GetMovementSpeed()
    {
        return (CurrentMovementDirection * MovementSpeed).magnitude;
    }
}
