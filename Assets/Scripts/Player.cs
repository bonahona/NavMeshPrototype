using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Agent))]
public class Player : MonoBehaviour
{
    private Agent Agent;

    private void Start()
    {
        Agent = GetComponent<Agent>();
    }

    void Update()
    {
        TakeInput();
    }

    private void TakeInput()
    {
        var direction = new Vector3();

        if (Input.GetKey(KeyCode.W)) {
            direction.z = 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            direction.z = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            direction.x = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            direction.x = 1;
        }

        direction.Normalize();

        Agent.SetMovementDirection(direction);
        Agent.SetRotationDirection(direction);
    }
}
