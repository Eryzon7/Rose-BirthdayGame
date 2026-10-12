using System;
using UnityEngine;

public class Wallnut : MonoBehaviour
{
    public float speed = 5f;
    public bool randomDirection = true;
    public int direction = 1; // 1 = right, -1 = left (used if randomDirection is off)

    public int boundsLeft = -14;
    public int boundsRight = 15;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = UnityEngine.Random.Range(7, 16);

        // Initial push, so it starts moving right at birth
        rb.linearVelocity = new Vector2(direction * speed, 0f);   // Unity 6+
        // rb.velocity = new Vector2(direction * speed, 0f);      // older versions
    }
   
    void FixedUpdate()
    {
        // Keep it rolling at a steady speed
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        // rb.velocity = new Vector2(direction * speed, rb.velocity.y);  // older versions
        if (this.gameObject.transform.position.x < boundsLeft || this.gameObject.transform.position.x > boundsRight)
        {
            Destroy(gameObject);
        }
    }

    public void Break()
    {
        /*if (brokenPrefab != null)
            Instantiate(brokenPrefab, transform.position, transform.rotation);

        if (breakEffect != null)
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        */
        Destroy(gameObject);
    }
    
}
