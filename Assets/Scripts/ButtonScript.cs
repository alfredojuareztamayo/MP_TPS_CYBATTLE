using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonScript : MonoBehaviour
{
    private GameObject[] players;
    private int myId;
    private GameObject panel;
    private GameObject namesObject;


    private void Start()
    {
        Cursor.visible = true;
        panel = GameObject.Find("ChoosePanel");
        namesObject = GameObject.Find("namesBG");
    }
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
        Cursor.visible = false;
        panel.SetActive(false);
    }
    [PunRPC]
    void SelectColor(int buttonNumber, int myId)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<DisplayColor>().viewId[buttonNumber] = myId;
            players[i].GetComponent<DisplayColor>().ChooseColor();
        }
        namesObject.GetComponent<Timer>().BegingTimer();
        this.transform.gameObject.SetActive(false);
    }
}
