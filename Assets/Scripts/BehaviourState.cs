using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourState: ScriptableObject
{
    public abstract StateInstance CreateInstance(BehaviourContext context);
    public abstract AgentInput Execute(StateInstance instance, BehaviourContext context);
}
