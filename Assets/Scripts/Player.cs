using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using EZCameraShake;

public class Player : MonoBehaviour
{

    [Header("Mobile UI Buttons")]
    public Button shoot_button;

    [Header("Prefabs")]
    public Bullet bulletPrefab;
    public ParticleSystem thruster;

    [Header("Physics")]
    public Rigidbody2D rb;

    [Header("Other Properties")]
    public Color color;
    public float thrustspeed;
    public float turnspeed;
    public int health = 100;

    [Header("Mobile Component")]
    public Joystick joystick;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip thruster_clip;
    

    [Header("Debug")]
    public bool isThrusting;
    public bool keyboardControls;
    public float turnDirection;

    public GameManager le_GM;  // GameManager to be called also it's a Lebron James joke.
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
        thruster.Stop();
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(joystick.Horizontal);


        if(Input.GetKeyDown(KeyCode.K))
        {
            keyboardControls = !keyboardControls;
            if(keyboardControls)
            {
                Debug.Log("Keyboard controls active");
            }
            else
            {
                Debug.Log("Touch controls active");
            }
            
        }

        //Keyboard Controls
        if (keyboardControls)
        {
            isThrusting = Input.GetKey(KeyCode.W);
            if (Input.GetKey(KeyCode.A))
                turnDirection = 1.0f;
            else if (Input.GetKey(KeyCode.D))
                turnDirection = -1.0f;
            else
                turnDirection = 0.0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }
        //Touch Controls
        else
        {
            //The joystick functionality is used to calculate the axes
            if (joystick.Vertical > 0.9f)
            {
                isThrusting = true;
            }
            else if (joystick.Vertical < 0.9f || joystick.Vertical <= 0.0f)
            {
                isThrusting = false;
            }
            else
            {
                isThrusting = false;
            }

            if (joystick.Horizontal < -0.9f)
            {
                turnDirection = -joystick.Horizontal * turnspeed;
            }
            else if (joystick.Horizontal > 0.9f)
            {
                turnDirection = -joystick.Horizontal * turnspeed;
            }
            else
            {
                turnDirection = 0.0f;
            }
        }

    }

    public void OnThrustDown()
    {
        //Debug.Log("Hello move");
        isThrusting = true;
    }

    public void OnThrustUp()
    {
        isThrusting = false;
    }

    //Public function to pass it to a UI button
    public void ShootButton()
    {
        if(this.gameObject.activeSelf)
        {
            Shoot();
        }
        else
        {
            //Do nothing
        }
        
    }

    private void FixedUpdate()
    {
        if(isThrusting)
        {
            rb.AddForce(transform.up * thrustspeed);
            thruster.Play();
            audioSource.clip = thruster_clip;
            if (audioSource.isPlaying)
            {
                //DO nothing
            }
            else
            {
                audioSource.Play();
                audioSource.loop = true;
            }
        }
        else
        {
            thruster.Stop();
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop= false;
        }
        //if(turnDirection != 0.0f)
        //{
            rb.AddTorque(turnDirection * turnspeed);
        //}
    }

    //Function for the player to shoot the bullets
    public void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab,this.transform.position,this.transform.rotation);
        le_GM.PlayerShootSound();
        bullet.OnBullet(this.transform.up);
        

    }

    //Function for the player to get killed by the asteroid hit
    void Die()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity=0.0f;
        this.gameObject.SetActive(false);
        this.health = 100;
        //The game manager function call
        le_GM.PlayerDied();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //When the players gets collided with the asteroid
        if(collision.collider.CompareTag("Asteroid"))
        {
            health -= 20;
            if(health<=0)
            {
                Die();
            }
            else
            { 
                le_GM.PlayerHitSound();
            }
            
            CameraShaker.Instance.ShakeOnce(10f, 5f, .1f, 1f);
        }

        //When the player hits any of the boundaries

        if(collision.collider.CompareTag("TopBoundary"))
        {
            Vector3 temp = this.transform.position;
            temp.y =(-temp.y)+0.3f;
            this.transform.position = temp; 
        }

        if (collision.collider.CompareTag("BottomBoundary"))
        {
            Vector3 temp = this.transform.position;
            temp.y = (-temp.y) - 0.3f;
            this.transform.position = temp;
        }

        if (collision.collider.CompareTag("LeftBoundary"))
        {
            Vector3 temp = this.transform.position;
            temp.x = (-temp.x)-0.3f;
            this.transform.position = temp;
        }

        if (collision.collider.CompareTag("RightBoundary"))
        {
            Vector3 temp = this.transform.position;
            temp.x = (-temp.x) + 0.3f;
            this.transform.position = temp;
        }

    }


}
