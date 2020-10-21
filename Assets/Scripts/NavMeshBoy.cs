using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshBoy : MonoBehaviour
{
    public float Speed = 5;

    public List<Transform> PatrolPath;
    private int CurrentPatrolPathIndex;

    private NavMeshAgent NavMeshAgent;
    private Agent Target;

    [HideInInspector]
    public float CurrentRotationDirection;

    public Vector3 CurrentMovementDirection;
    public Vector3 TargetMovementDirection;

    public MovementBase Movement;

    // Start is called before the first frame update
    private void Start()
    {
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
        agent.updatePosition = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (PatrolPath.Count == 0) {
            NavMeshAgent.SetDestination(Target.transform.position);
        } else {
            NavMeshAgent.SetDestination(PatrolPath[CurrentPatrolPathIndex].position);
            if(NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && NavMeshAgent.remainingDistance < 1) {
                CurrentPatrolPathIndex = (CurrentPatrolPathIndex + 1) % PatrolPath.Count;
            }
        }

        var direction = (NavMeshAgent.nextPosition - transform.position);
        direction.y = 0;
        direction.Normalize();

        var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        TargetMovementDirection = direction;
        CurrentMovementDirection = Movement.GetMovementDirection(CurrentMovementDirection, TargetMovementDirection);
        var offset = CurrentMovementDirection * Speed;
        transform.Translate(offset * Time.deltaTime, Space.World);

        CurrentRotationDirection = Movement.GetRotationDirection(CurrentRotationDirection, angle);

        //transform.position = NavMeshAgent.nextPosition;
        transform.rotation = Quaternion.Euler(0, CurrentRotationDirection, 0);

    }
}
