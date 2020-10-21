using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float Frequense = 1;
    public float Amplitude = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Mathf.Sin(Time.time * Frequense) * Amplitude * Time.deltaTime); 
    }
}
