using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState: ScriptableObject
{
    public bool UseAvoidObstacles = true;

    public virtual StateInstance CreateInstance(Agent agent)
    {
        return new StateInstance { Agent = agent };
    }

    public abstract Steering GetSteering(StateInstance stateInstance);

    protected void Repath(StateInstance stateInstance, float repathTimer)
    {
        NavMesh.CalculatePath(stateInstance.Agent.transform.position, stateInstance.Target.transform.position, int.MaxValue, stateInstance.Path);
        for(var i = 0; i < stateInstance.Path.corners.Length; i ++) {
            stateInstance.Path.corners[i].y = 0;
        }

        stateInstance.Index = 0;
        stateInstance.Timer = repathTimer;
    }

    protected void AvoidObstacles(StateInstance stateInstance, Steering steering)
    {
        var agentPosition = stateInstance.Agent.transform.position;
        var agentDirection = stateInstance.Agent.CurrentMovementDirection;

        var distance = stateInstance.Agent.GetMovementSpeed();

        LayerMask obstacleMask = 1 << 9;

        Debug.DrawLine(agentPosition, agentPosition + agentDirection * distance, Color.red, 0.5f);
        if(Physics.Raycast(agentPosition, agentDirection, out var hit, distance, obstacleMask, QueryTriggerInteraction.Ignore)) {

            var position = hit.point + hit.normal * distance;
            var direction = (position - agentPosition).normalized;
            steering.MovementDirection = direction;
            steering.RotationDirection = direction;

            stateInstance.ProxyPosition = position;
        }
    }
}
