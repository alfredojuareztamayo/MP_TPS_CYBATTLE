using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// movement speed (velocity) of the player
    /// </summary>
    public float speed = 3.5f;
    /// <summary>
    /// speed of rhe rotation of the player
    /// </summary>
    public float rotationSpeed = 100f;
    /// <summary>
    /// Private RigidBody of the player
    /// </summary>
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementPlayer();
        RotationPlayer();
    }

    /// <summary>
    /// Handle player Movement based on user input
    /// </summary>
    void MovementPlayer()
    {
        rb.MovePosition(rb.position + (transform.forward * Input.GetAxis("Vertical")
        + transform.right * Input.GetAxis("Horizontal")) * speed * Time.deltaTime);
    }
    /// <summary>
    /// Handle player Rotation based on user Input
    /// </summary>
    void RotationPlayer()
    {
        Vector3 checkMove = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")).normalized;
        Vector3 rotateY = new Vector3(0,Input.GetAxis("Mouse X")*rotationSpeed*Time.deltaTime,0);
       // (checkMove != Vector3.zero) ? rb.MoveRotation(rb.rotation*Quaternion.Euler(rotateY)):
       if(checkMove != Vector3.zero)
       {
        rb.MoveRotation(rb.rotation*Quaternion.Euler(rotateY));
       }
    }
}
