using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PursueState", menuName = "States/Pursue")]
public class PursueState : BaseState
{
    public override StateInstance CreateInstance(Agent agent)
    {
        var target = GameObject.FindObjectOfType<Player>();
        return new TargetStateInstance { Agent = agent, Target = target.GetComponent<Agent>() };
    }

    public override Steering GetSteering(StateInstance stateInstance)
    {
        var targetStateInstance = stateInstance as TargetStateInstance;

        var pathFound = NavMesh.CalculatePath(targetStateInstance.Agent.transform.position, targetStateInstance.Target.transform.position, int.MaxValue, stateInstance.Path);

        if(stateInstance.Path != null && stateInstance.Path.corners.Length > 1) {
            var corners = stateInstance.Path.corners;
            for (int i = 0; i < corners.Length - 1; i ++) {
                Debug.DrawLine(corners[i], corners[i + 1], Color.magenta);
            }
        }

        var direction = (targetStateInstance.Target.transform.position - targetStateInstance.Agent.transform.position).normalized;
        var result = new Steering {
            MovementDirection = direction,
            RotationDirection = direction
        };

        if (UseAvoidObstacles) {
            AvoidObstacles(stateInstance, result);
        }

        return result;
    }
}
