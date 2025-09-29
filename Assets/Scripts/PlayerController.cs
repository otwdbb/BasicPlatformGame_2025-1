using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
   
    public bool canDash = false;

    public GameObject doubleJumpHatLocation;
    public GameObject BootLocation;
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
        movePlayerLateral();
        jump();
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
        if (inputHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (inputHorizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }

    private void jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && numJumps <= maxNumJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            numJumps++;
        }
    }

   private void dash()
    {

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
            boots.GetComponent<BoxCollider2D>().isTrigger = false;

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

