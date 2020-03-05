using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{

    private Rigidbody rb;
    private bool canJump;
    private bool inAir;
    private float health;
    private float myID;

    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float jumpSpeed = 50;
    [SerializeField]
    private GameObject ball;

    public float startHealth = 100;
    

    private Vector3 Velocity
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float ID
    {
        get { return myID; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canJump = false;
        inAir = true;
        health = startHealth;
        myID = Random.Range(0, 99999999);
    }

    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            canJump = false;
            Velocity = Vector3.up * jumpSpeed;
        }
        else if (!inAir)
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetAxis("Horizontal") > 0)
            {
                moveDirection += Vector3.right;
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                moveDirection += Vector3.left;
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                moveDirection += Vector3.forward;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                moveDirection += Vector3.back;
            }

            Velocity = moveDirection.normalized * moveSpeed;

            if (Input.GetButtonDown("Fire1") && moveDirection != Vector3.zero)
            {
                /*GameObject myBall = */PhotonNetwork.Instantiate(ball.name, transform.position + moveDirection, Quaternion.identity).GetComponent<BallController>().SetVars(myID, moveDirection.normalized);
                //myBall.GetComponent<BallController>().SetVars(this, moveDirection.normalized);
            }
        }

        if (health <= 0)
        {
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && other.GetComponent<BallController>() && other.GetComponent<BallController>().owner != myID)
        {
            Health -= 10;
            PhotonNetwork.Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = true;
            inAir = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = true;
            inAir = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            inAir = true;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else
        {
            Health = (float)stream.ReceiveNext();
        }
    }

}
