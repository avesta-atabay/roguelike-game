using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoobingAnimation : MonoBehaviour
{
    public float frequency; //speed of movement
    public float magnitude; //range of movement
    public Vector3 direction;  //direction of movement
    Vector3 initialPosition;
    Pickup pickup;

    void Start()
    {
        pickup = GetComponent<Pickup>();

        initialPosition = transform.position;
    }
    void Update()
    {
        if(pickup && !pickup.hasBeenCollected)
        {
            transform.position = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }
}
