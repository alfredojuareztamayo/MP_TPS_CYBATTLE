using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class DisplayColor : MonoBehaviourPunCallbacks
{
    public int[] buttonNumbers;
    public int[] viewId;
    public Color32[] colors;

    private GameObject namesObject;
    private GameObject waitForPlayer;

    private void Start()
    {
        namesObject = GameObject.Find("namesBG");
        waitForPlayer = GameObject.Find("Waiting BG");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetComponent<PhotonView>().IsMine == true && waitForPlayer.activeInHierarchy == false)
            {
                RemoveData();
                RoomExit();
            }
        }
    }

    private void RoomExit()
    {
        StartCoroutine(GetReadyToLeave());
    }

    private void RemoveData()
    {
        GetComponent<PhotonView>().RPC("RemoveMe", RpcTarget.AllBuffered);
    }

    public void ChooseColor()
    {
        GetComponent<PhotonView>().RPC("AssignColor", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void AssignColor()
    {
        for (int i = 0; i < viewId.Length; i++)
        {
            if (this.GetComponent<PhotonView>().ViewID == viewId[i])
            {
                this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[i];
                namesObject.GetComponent<NickNameScript>().names[i].gameObject.SetActive(true);
                namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.SetActive(true);
                namesObject.GetComponent<NickNameScript>().names[i].text = this.GetComponent<PhotonView>().Owner.NickName;
                break;
            }
        }
    }

    [PunRPC]
    void RemoveMe()
    {
        for (int i = 0; i < namesObject.gameObject.GetComponent<NickNameScript>().names.Length; i++)
        {
            if (this.GetComponent<PhotonView>().Owner.NickName == namesObject.GetComponent<NickNameScript>().names[i].text)
            {
                namesObject.GetComponent<NickNameScript>().names[i].gameObject.SetActive(false);
                namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator GetReadyToLeave()
    {
        yield return new WaitForSeconds(1);
        namesObject.GetComponent<NickNameScript>().Leaving();
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
    }
}
