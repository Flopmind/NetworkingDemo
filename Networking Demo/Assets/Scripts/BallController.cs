using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviourPunCallbacks, IPunObservable
{

    public float moveSpeed = 10;
    public float owner;
    public Vector3 direction = Vector3.zero;
    public float timerLength;

    private float timer;

    void Start()
    {
        timer = timerLength;
    }

    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        GetComponent<Rigidbody>().velocity = direction.normalized * moveSpeed;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() && other.GetComponent<PlayerController>().ID != owner)
        {
            timer = 0;
        }
    }

    [PunRPC]
    public void RPCSetVars(float pc, Vector3 direct)
    {
        owner = pc;
        direction = direct;
    }

    public void SetVars(float pc, Vector3 direct)
    {
        object[] vars = { pc, direct };
        GetComponent<PhotonView>().RPC("RPCSetVars", RpcTarget.All, vars);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            timer = (float)stream.ReceiveNext();
            if (timer <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            stream.SendNext(timer);
        }
    }
}
