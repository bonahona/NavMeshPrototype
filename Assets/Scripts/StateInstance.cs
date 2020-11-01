using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateInstance 
{
    public BehaviourState State;

    public StateInstance(BehaviourState state)
    {
        State = state;
    }

    public AgentInput Execute(BehaviourContext context)
    {
        return State.Execute(this, context);
    }
}
