using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletspeed;

    [SerializeField] private Rigidbody2D rb;

    public void OnBullet(Vector2 direction)
    {
        rb.AddForce(direction * bulletspeed);
        Destroy(this.gameObject,10.0f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Asteroid") || collision.collider.CompareTag("TopBoundary") 
            || collision.collider.CompareTag("BottomBoundary") || collision.collider.CompareTag("LeftBoundary")
            || collision.collider.CompareTag("RightBoundary"))
        {
            Destroy(this.gameObject);
        }
    }

}
