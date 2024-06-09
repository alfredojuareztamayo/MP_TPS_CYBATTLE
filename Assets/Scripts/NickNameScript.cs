using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNameScript : MonoBehaviourPunCallbacks
{
    public Text[] names;
    public Image[] healthBars;
    private GameObject waitObject;


    private void Start()
    {
        for(int i = 0; i < names.Length; i++)
        {
            names[i].gameObject.SetActive(false);
            healthBars[i].gameObject.SetActive(false);
        }
        waitObject = GameObject.Find("Waiting BG");
    }
    public void Leaving()
    {
        StartCoroutine("BackToLobby");
    }
    IEnumerator BackToLobby()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.LoadLevel("Lobby");
    }

    //This is for the Waiting screen
    public void ReturnToLobby()
    {
        waitObject.SetActive(false);
        RoomExit();
    }
    void RoomExit()
    {
        StartCoroutine(ToLobby());
    }
    IEnumerator ToLobby()
    {
        yield return new WaitForSeconds(0.4f);
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
