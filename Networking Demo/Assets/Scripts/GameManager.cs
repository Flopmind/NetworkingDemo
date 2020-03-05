using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        Vector3 spawnLocation = new Vector3(Random.Range(-9, 9), 1.5f, Random.Range(-9, 9));
        GameObject obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnLocation, Quaternion.identity);
    }

}
