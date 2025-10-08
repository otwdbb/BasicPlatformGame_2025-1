using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;              // needed for IEnumerator + coroutines

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    //
    private float inputHorizontal;
    private int maxNumJumps;
    private int numJumps;
    //because this is public we have access to it in the unity editor
    public float horizontalMoveSpeed;
    public float jumpForce;
   
   // public bool canDash = false;

    public GameObject doubleJumpHatLocation;
    public GameObject BootLocation;


    // Dash (locked until you grab Boots)
    public bool canDash = false;                 // stays false until Boots pickup
    public KeyCode dashKey = KeyCode.LeftShift;  // key to dash
    public float dashSpeed = 18f;                // horizontal burst speed
    public float dashTime = 0.15f;               // how long the burst lasts
    public float dashCooldown = 0.6f;            // cooldown between dashes

    private bool isDashing = false;
    private float nextDashTime = 0f;
    private int facing = 1; // 1 = right, -1 = left



















    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //I can only get this component becuase the rigidbody2d is attached to the player
        //this script is also attached to the player
        rb = GetComponent<Rigidbody2D>();
        

        maxNumJumps = 1;
        numJumps = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            movePlayerLateral();
            jump();
        }

        // 2) Start a dash only if:
        //    - you've unlocked it (canDash)
        //    - you're not already dashing
        //    - the cooldown has expired
        //    - the dash key was just pressed
        if (canDash && !isDashing && Time.time >= nextDashTime && Input.GetKeyDown(dashKey))
        {
            StartCoroutine(DashRoutine()); 
        }


        



        //movePlayerLateral();
        //jump();
    }

    private void movePlayerLateral()
    {
        //if A/D/<-/-> are pressed move the player accordingly
        //"Horizontal" is defined in the input section of the project settings
        //the line below will return:
        //0 - no button pressed
        //1 - right arrow or d pressed
        //2 - left arrow or a pressed.
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        flipPlayerSprite(inputHorizontal);
        rb.linearVelocity = new Vector2(horizontalMoveSpeed * inputHorizontal, rb.linearVelocity.y);
    }

    private void flipPlayerSprite(float inputHorizontal)
    {
        //this is how we will make the player face the direction they are moving
        //if (inputHorizontal > 0)
        //{
        //    transform.eulerAngles = new Vector3(0, 0, 0);
        //}
        //else if (inputHorizontal < 0)
        //{
        //    transform.eulerAngles = new Vector3(0, 180, 0);
        //}

        // face right

        if (inputHorizontal > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facing = 1;
        }
        else if (inputHorizontal < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            facing = -1;
        }
        // if 0f, keep current facing

    }

    private void jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && numJumps <= maxNumJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            numJumps++;
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;

        // Pause gravity for a clean horizontal burst
        float oldGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        
        rb.linearVelocity = new Vector2(facing * dashSpeed, 0f);

        yield return new WaitForSeconds(dashTime);

        // Restore physics and set cooldown
        rb.gravityScale = oldGravity;
        isDashing = false;
        nextDashTime = Time.time + dashCooldown;
    }

  


    //Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collision will contain information about the object that the player collided with 
        //Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Ground"))
        {
            numJumps = 1;
        }
        else if (collision.gameObject.CompareTag("obBottom"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    //Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //double jump
        if(collision.gameObject.CompareTag("DoubleJump"))
        {
            GameObject hat = collision.gameObject;
            equipDoubleJumpHat(hat);
            maxNumJumps = 2;
        }
        else if(collision.gameObject.CompareTag("Boots"))
        {
            //Debug.Log("Collieded with boots");
            GameObject boots = collision.gameObject;
            equipBoots(boots);
            //we want it to be a regular collider and we want the boots to stand on the ground
            //boots.GetComponent<BoxCollider2D>().isTrigger = false;

            canDash = true;

            // 3) Stop the equipped boots from colliding or simulating physics
            var col = boots.GetComponent<Collider2D>();
            if (col) col.enabled = false;

            var rb2 = boots.GetComponent<Rigidbody2D>();
            if (rb2) rb2.simulated = false;



            // bool = dash;
        }
    }

    private void equipDoubleJumpHat(GameObject hat)
    {
        hat.transform.position = doubleJumpHatLocation.transform.position;
        // i want the hat to be a child
        hat.gameObject.transform.SetParent(this.gameObject.transform);
    }

    private void equipBoots(GameObject boots)
    {
        //move to where i want
        boots.transform.position = BootLocation.transform.position;
        //set to be child and stay with me
        //i want the hat to be a child
        boots.gameObject.transform.SetParent(this.gameObject.transform);




        //collided with boots
        //need to dash or increase speed
        //public bool to be able to dash
        //We need a new function called dash
        //call it in update
        //look for dash key event - shift
        // and if the boolean is true


    }
 
}   

