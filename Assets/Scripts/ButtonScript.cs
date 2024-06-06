using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonScript : MonoBehaviour
{
    private GameObject[] players;
    private int myId;
   
    public void SelectButton(int buttonNumber)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().IsMine==true)
            {
                myId = players[i].GetComponent<PhotonView>().ViewID;
                break;
            }
        }
        GetComponent<PhotonView>().RPC("SelectColor",RpcTarget.AllBuffered, buttonNumber, myId);
    }
    [PunRPC]
    void SelectColor(int buttonNumber, int myId)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {

        }
        this.transform.gameObject.SetActive(false);
    }
}
