using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class AiAgent : MonoBehaviour
{
    private Agent Agent;
    public BaseState State;

    private StateInstance StateInstance;

    void Start()
    {
        Agent = GetComponent<Agent>();    
        if(State != null) {
            StateInstance = State.CreateInstance(Agent);
        }
    }

    void Update()
    {
        if(State != null) {
            var steering = State.GetSteering(StateInstance);
            Agent.SetMovementDirection(steering.MovementDirection);
            Agent.SetRotationDirection(steering.RotationDirection);
        }
    }
}
