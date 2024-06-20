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
    public GameObject displayPanel;
    public Text messageText;
    public bool teamMode = false;

    public int[] kills;
    public bool noRespawn = false;
    public GameObject eliminationPanel;

    public bool mazeGnerate = false;
    private void Start()
    {
        if (noRespawn == true)
        {
            eliminationPanel.SetActive(false);
        }

        displayPanel.SetActive(false);
        for (int i = 0; i < names.Length; i++)
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

    public void RunMessage(string win, string lose)
    {
        this.GetComponent<PhotonView>().RPC("DisplayMessage", RpcTarget.All, win, lose);
        UpdateKills(win);

    }

    void UpdateKills(string win)
    {
        for (int i = 0; i < names.Length; i++)
        {
            if (win == names[i].text)
            {
                kills[i]++;
            }
        }
    }

    [PunRPC]
    void DisplayMessage(string win, string lose)
    {
        displayPanel.SetActive(true);
        messageText.text = win + " killed " + lose;
        StartCoroutine(SwitchOffMessage());
    }
    IEnumerator SwitchOffMessage()
    {
        yield return new WaitForSeconds(3);
        this.GetComponent<PhotonView>().RPC("MessageOff", RpcTarget.All);
    }

    [PunRPC]
    void MessageOff()
    {
        displayPanel.SetActive(false);
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
