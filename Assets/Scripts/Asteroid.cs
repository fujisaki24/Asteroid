using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] asteroids;
    public ParticleSystem explosion;
    public float maxLifetime = 30.0f;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    public float size = 0.5f;
    public float minSize = 0.1f;
    public float maxSize = 1f;

    public float speed = 50.0f;
    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        //explosion.Stop();
        sprite.sprite = asteroids[Random.Range(0, asteroids.Length)];

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        rb.mass = this.size;
    }

    //Applying force on the passed direction
    public void SetTrajectory(Vector2 direction)
    {
        rb.AddForce(direction * this.speed);
    }

    public void Update()
    {
        if(this.gameObject.activeInHierarchy)
        {
            Destroy(this.gameObject,maxLifetime);
        }
    }


    void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;
        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;

        half.SetTrajectory(Random.insideUnitCircle.normalized*this.speed);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            if (this.size * 0.5f >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
                //audioSource.PlayOneShot(explosion_clip, 0.5f);
                explosion.Play();
                FindObjectOfType<GameManager>().AsteroidHitSound();
            }
            else
            {
                //audioSource.PlayOneShot(explosion_clip, 0.5f);
                explosion.Play();
                FindObjectOfType<GameManager>().AsteroidHitSound();
                
            }
            FindObjectOfType<GameManager>().AsteroidHitScore(this.size);
            Destroy(this.gameObject);
        }
    }


}
