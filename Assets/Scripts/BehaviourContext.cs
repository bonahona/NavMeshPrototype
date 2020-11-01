using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourContext
{
    public Agent Agent;
    public AiAgent AiAgent;
    public Agent Target;

    public void SetTarget(Vector3 target)
    {
        AiAgent.ProxyObject.SetDestination(target);
    }
}
