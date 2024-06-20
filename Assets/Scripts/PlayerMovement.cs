using Photon.Pun;
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

    public float JumpForce;
    /// <summary>
    /// Private RigidBody of the player
    /// </summary>
    Rigidbody rb;

    /// <summary>
    /// Private animator form the player
    /// </summary>
    
    Animator animator;

    private bool canJump = true;
    // Start is called before the first frame update

    public bool isDead = false;


    private Vector3 startPos;
    private bool respawned = false;
    private GameObject respawnPanel;

    public bool gameOver = false;    public bool noRespawn;    private bool startChecking = false;
    private GameObject Canvas;    bool canMove = true;
    private void Awake()
    {
        respawnPanel = GameObject.Find("RespawnPanel");
        Canvas = GameObject.Find("Canvas");
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        startPos = transform.position;       // respawnPanel = GameObject.Find("RespawnPanel");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead == false)
        {
            respawnPanel.SetActive(false);
            if (canMove)
            {
                MovementPlayer();
                RotationPlayer();
            }
        }
    }

    /// <summary>
    /// Handle player Movement based on user input
    /// </summary>
    void MovementPlayer()
    {
        rb.MovePosition(rb.position + (transform.forward * Input.GetAxis("Vertical")
        + transform.right * Input.GetAxis("Horizontal")) * speed * Time.deltaTime);

        animator.SetFloat("BlendV", Input.GetAxis("Vertical"));
        animator.SetFloat("BlendH", Input.GetAxis("Horizontal"));
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

    private void Update()
    {
        if (isDead == false)
        {
       
            if (Input.GetButtonDown("Jump") && canJump==true)
            {
                canJump = false;
                rb.AddForce(Vector3.up * JumpForce * Time.deltaTime, ForceMode.VelocityChange);
                StartCoroutine(JumpAgain());
            }
        }
        if (isDead == true && respawned == false && gameOver == false && noRespawn == false)
        {
            respawned = true;
            respawnPanel.SetActive(true);            respawnPanel.GetComponent<RespawnTimer>().enabled = true;
            StartCoroutine(RespawnWait());
        }
        if (isDead == true && respawned == false && gameOver == false && noRespawn == true)
        {
            respawned = true;
            GetComponent<DisplayColor>().NoRespawnExit();
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && startChecking == false)
        {
            startChecking = true;
            InvokeRepeating("CheckForWinner", 3, 3);
        }
    }
    void CheckForWinner()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Canvas.GetComponent<KillCount>().NoRespawnWinner(GetComponent<PhotonView>().Owner.NickName);
        }
    }
    IEnumerator JumpAgain()
    {
        yield return new WaitForSeconds(1);
        canJump = true; 
    }

    IEnumerator RespawnWait()
    {
        yield return new WaitForSeconds(3);
        isDead = false;
        respawned = false;
        transform.position = startPos;
        GetComponent<DisplayColor>().Respawn(GetComponent<PhotonView>().Owner.NickName);
    }
    [PunRPC]
    void StopMovementItem(float time)
    {
        StartCoroutine(stopMovementPlayer(time));
    }

    IEnumerator stopMovementPlayer(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    [PunRPC]
    void InvisibleTime(float time)
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        StartCoroutine(playerInvisible(time));
    }

    IEnumerator playerInvisible(float time)
    {
        
        yield return new WaitForSeconds(time);
        this.GetComponent<PhotonView>().RPC("InvisibleTimeActive", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void InvisibleTimeActive()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(2).gameObject.SetActive(true);

    }

}
