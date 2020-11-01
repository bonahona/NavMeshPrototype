using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Agent))]
public class AiAgent : MonoBehaviour
{
    public List<BehaviourState> States = new List<BehaviourState>();
    public NavMeshAgent ProxyObjectPrefab;

    [HideInInspector]
    public NavMeshAgent ProxyObject;
    private Agent Agent;
    private BehaviourContext Context;
    private StateInstance CurrentState;

    private void Start()
    {
        Agent = GetComponent<Agent>();
        var target = FindObjectOfType<Player>().GetComponent<Agent>();
        ProxyObject = Instantiate(ProxyObjectPrefab, transform.position, transform.rotation);

        Context = new BehaviourContext { Agent = Agent, AiAgent = this, Target = target };

        if (States.Count > 0) {
            CurrentState = States.First().CreateInstance(Context);
        }
    }

    private void Update()
    {
        if(CurrentState == null) {
            return;
        }

        var result = CurrentState.Execute(Context);
        Agent.SetMovementDirection(result.MovementDirection);
        Agent.SetRotationDirection(result.RotationDirection);

    }
}
