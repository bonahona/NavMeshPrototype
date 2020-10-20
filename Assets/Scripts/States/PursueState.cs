using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PursueState", menuName = "States/Pursue")]
public class PursueState : BaseState
{
    public float DistanceTreshold;
    public float RepathTreshold;
    public float RepathTimer;

    public override StateInstance CreateInstance(Agent agent)
    {
        var target = FindObjectOfType<Player>();
        var result =  new StateInstance { Agent = agent, Target = target.GetComponent<Agent>() };

        Repath(result, RepathTimer);

        return result;
    }

    public bool ShouldRepath(StateInstance stateInstance)
    {
        if((stateInstance.Target.transform.position - stateInstance.Agent.transform.position).sqrMagnitude > (RepathTreshold * RepathTreshold)) {
            return true;
        }

        return false;
    }

    public override Steering GetSteering(StateInstance stateInstance)
    {
        var position = stateInstance.Agent.transform.position;
        var targetPosition = position;

        stateInstance.Timer -= Time.deltaTime;
        if(stateInstance.Timer <= 0) {
            if (ShouldRepath(stateInstance)) {
                Repath(stateInstance, RepathTimer);
            }
        }

        if (!stateInstance.ProxyPosition.HasValue) {
            if (stateInstance.Path != null && stateInstance.Path.corners.Length > 1) {
                var corners = stateInstance.Path.corners;
                for (int i = 0; i < corners.Length - 1; i++) {
                    Debug.DrawLine(corners[i], corners[i + 1], Color.magenta);
                }
            }
        } else {
            Debug.DrawLine(position, stateInstance.ProxyPosition.Value, Color.green);
        }


        if (stateInstance.ProxyPosition.HasValue) {
            targetPosition = stateInstance.ProxyPosition.Value;
            var distance = (targetPosition - position);
            if(distance.sqrMagnitude < DistanceTreshold * DistanceTreshold) {
                stateInstance.ProxyPosition = null;
            }

        } else {
            position.y = 0;
            targetPosition.y = 0;
            if (stateInstance.Path.status == NavMeshPathStatus.PathComplete) {

                var distance = (stateInstance.Path.corners[stateInstance.Index] - position);

                if (distance.sqrMagnitude < DistanceTreshold * DistanceTreshold) {
                    stateInstance.Index++;
                }

                if (stateInstance.Index == stateInstance.Path.corners.Length) {
                    Repath(stateInstance, RepathTimer);
                }

                targetPosition = stateInstance.Path.corners[stateInstance.Index];
            }
        }

        var direction = (targetPosition - stateInstance.Agent.transform.position).normalized;
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
