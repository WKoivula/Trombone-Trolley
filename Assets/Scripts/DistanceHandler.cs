using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//measure how far the player has reached
public class DistanceHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public Transform startpoint;
    [SerializeField] public Transform endpoint;

    public float distanceTraveled;
    void Start()
    {
        distanceTraveled = (endpoint.position - startpoint.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        distanceTraveled = (endpoint.position - startpoint.position).magnitude;
    }
}
