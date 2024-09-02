using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        playerCollector.radius = player.CurrentMagnet;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //check if the other game object has the ICollectible interface
        if(col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            //pulling animation
            //gets the rigidbody2D component on the item
            Rigidbody2D rb= col.gameObject.GetComponent<Rigidbody2D>();
            //vector2 pointing from the item to the plyer
            Vector2 forceDirection = (transform.position -col.transform.position).normalized;
            //applies force to the item in the forece direction with pullSpeed
            rb.AddForce(forceDirection * pullSpeed);

            //if it does, call the collect method
            collectible.Collect();
        }
    }
}
