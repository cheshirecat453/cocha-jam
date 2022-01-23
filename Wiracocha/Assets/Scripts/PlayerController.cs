using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private enum State {idle, run, jump, falling, hurt};
    private State state = State.idle;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private int health = 3;
    [SerializeField] private Text healthAmmount;
    [SerializeField] private int pineapples = 0;
    [SerializeField] private Text pineappleText;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent <Rigidbody2D>();
        anim = GetComponent <Animator>();
        coll = GetComponent <Collider2D>();
        //healthAmmount.text = health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.hurt)
        {
            movement();
        }
        
        VelocityState();
        anim.SetInteger("State", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            pineapples += 1;
            //pineappleText.text = pineapples.ToString();
        }
        if (collision.tag == "PowerUp")
        {
            //Debug.Log("Power UP");
            Destroy(collision.gameObject);
            jumpForce = 20f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" )
        {
            if (state == State.falling)
            {
                Destroy(other.gameObject);
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy is to right
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //enemy is to left
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y); 
                }
            }
        }

    }

    private void VelocityState() 
    {
        if (state == State.jump)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
             
        }
        else if (state == State.falling)
            {
                if (coll.IsTouchingLayers(Ground))
                {
                    state = State.idle;
                }
            }
        else if (Mathf.Abs(rb.velocity.x) > 2f) 
        {
            state = State.run;
        }
        else {
            state = State.idle;
        }
    }

    private void movement() 
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection > 0)
        {
            rb.velocity = new Vector2(Speed,rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else if (hDirection < 0)
        {
            rb.velocity = new Vector2(-Speed,rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }


        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;

        }
    }

    private void HandleHealth()
    {
        //health -= 1;
        //healthAmmount.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 10;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    
}
