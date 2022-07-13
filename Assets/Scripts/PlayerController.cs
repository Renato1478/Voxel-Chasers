using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager theGM;

    public Rigidbody theRB;

    public float jumpForce = 10f;
    public float speed = 50f;

    public Transform modelHolder;
    public LayerMask whatIsGround;
    private bool onGround;

    private string runDir = "straight";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (theGM.canMove)
        {
            if (Input.GetKey(KeyCode.A))
                runDir = "left";
            if (Input.GetKey(KeyCode.D))
                runDir = "right";
            if (Input.GetKey(KeyCode.W))
                runDir = "straight";
            if (Input.GetKey(KeyCode.S))
                runDir = "back";

            if (runDir == "back")
            {
                transform.position -= new Vector3(0f, 0f, speed * Time.deltaTime);
            }
            else if (runDir == "straight")
            {
                transform.position -= new Vector3(0f, 0f, -speed * Time.deltaTime);
            }
            else if (runDir == "left")
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
            }
            else if (runDir == "right")
            {
                transform.position -= new Vector3(-speed * Time.deltaTime, 0f, 0f);
            }
            

            onGround = Physics.OverlapSphere(modelHolder.position, 0.2f, whatIsGround).Length > 0;

            if(onGround)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    theRB.velocity = new Vector3(0f, jumpForce, 0f);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hazards" || other.tag == "Enemies")
        {
            theGM.HitHazard();

            theRB.constraints = RigidbodyConstraints.None;

            theRB.velocity = new Vector3(Random.Range(GameManager._worldSpeed / 2f, -GameManager._worldSpeed / 2f), 2.5f, -GameManager._worldSpeed / 2f);
        }
    }
}
