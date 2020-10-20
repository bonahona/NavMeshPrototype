using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshBoy : MonoBehaviour
{
    public List<Transform> PatrolPath;
    private int CurrentPatrolPathIndex;

    private NavMeshAgent NavMeshAgent;
    private Agent Target;

    // Start is called before the first frame update
    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        SetupNavMeshAgent(NavMeshAgent);
        Target = FindObjectOfType<Player>().GetComponent<Agent>();
    }

    private void SetupNavMeshAgent(NavMeshAgent agent)
    {
        agent.updateRotation = false;
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
    }
}
