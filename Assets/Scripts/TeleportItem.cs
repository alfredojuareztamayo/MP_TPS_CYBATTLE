using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeleportItem : MonoBehaviour
{
    public GameObject teleportSpawn;
    Vector3 posTelport;
    // Start is called before the first frame update
    void Start()
    {
        posTelport = teleportSpawn.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PhotonView>().RPC("Teleport", RpcTarget.AllBuffered, posTelport);
        }
    }
}
