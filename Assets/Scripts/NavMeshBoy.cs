using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshBoy : MonoBehaviour
{
    public enum MovementType
    {
        None,
        Patrol,
        Random
    }

    public float Speed = 5;
    public MovementType Type;

    public List<Transform> PatrolPath;
    private int CurrentPatrolPathIndex;

    public float MaxMovementDistance;

    private NavMeshAgent NavMeshAgent;
    private Agent Target;

    [HideInInspector]
    public float CurrentRotationDirection;

    public Vector3 CurrentMovementDirection;
    public Vector3 TargetMovementDirection;

    public MovementBase Movement;

    private Vector3 CurrentMoveDirection;
    private Vector3 CurrentLookDirection;

    private Vector3 StartPosition;

    // Start is called before the first frame update
    private void Start()
    {
        StartPosition = transform.position;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        SetupNavMeshAgent(NavMeshAgent);
        Target = FindObjectOfType<Player>().GetComponent<Agent>();

        if(Movement == null) {
            Movement = ScriptableObject.CreateInstance<DummyMovement>();
        }
    }

    private void SetupNavMeshAgent(NavMeshAgent agent)
    {
        agent.updateRotation = false;
        //agent.updatePosition = false;
    }

    // Update is called once per frame
    private void Update()
    {

        if (Type == MovementType.Patrol) {
            Patrol();
        }else if(Type == MovementType.Random) {
            MoveRandom();
        }
        var nextMovePosition = NavMeshAgent.nextPosition;
        var nextLookPosition = NavMeshAgent.pathEndPosition;

        CurrentMoveDirection = (nextMovePosition - transform.position);
        CurrentMoveDirection.y = 0;
        CurrentMoveDirection.Normalize();

        CurrentLookDirection = (nextLookPosition - transform.position);
        CurrentLookDirection.y = 0;
        CurrentLookDirection.Normalize();

        var angle = Mathf.Atan2(CurrentLookDirection.x, CurrentLookDirection.z) * Mathf.Rad2Deg;

        TargetMovementDirection = CurrentMoveDirection;
        CurrentMovementDirection = Movement.GetMovementDirection(CurrentMovementDirection, TargetMovementDirection);
        var offset = CurrentMovementDirection * Speed;
        transform.Translate(offset * Time.deltaTime, Space.World);

        CurrentRotationDirection = Movement.GetRotationDirection(CurrentRotationDirection, angle);

        transform.rotation = Quaternion.Euler(0, CurrentRotationDirection, 0);
    }

    private void Patrol()
    {
        if (PatrolPath.Count == 0) {
            NavMeshAgent.SetDestination(Target.transform.position);
        } else {
            NavMeshAgent.SetDestination(PatrolPath[CurrentPatrolPathIndex].position);
            if (NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && NavMeshAgent.remainingDistance < 1) {
                CurrentPatrolPathIndex = (CurrentPatrolPathIndex + 1) % PatrolPath.Count;
            }
        }
    }

    private void MoveRandom()
    {
        if (NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && NavMeshAgent.remainingDistance < 1) {
            var randomLength = Random.Range(0, MaxMovementDistance);
            var randomAngle = Random.Range(0, Mathf.PI * 2);

            var offset = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)) * randomLength;

            var target = StartPosition + offset;
            NavMeshAgent.SetDestination(target);

        }
    }
}