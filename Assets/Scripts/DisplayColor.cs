using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DisplayColor : MonoBehaviour
{
    public int[] buttonNumbers;
    public int[] viewId;
    public Color32[] colors;

    private GameObject namesObject;

    private void Start()
    {
        namesObject = GameObject.Find("namesBG");
    }


    public void ChooseColor()
    {
        GetComponent<PhotonView>().RPC("AssignColor", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void AssignColor()
    {
        for(int i = 0; i < viewId.Length; i++)
        {
            if(this.GetComponent<PhotonView>().ViewID == viewId[i])
            {
                this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[i];
                namesObject.GetComponent<NickNameScript>().names[i].gameObject.SetActive(true);
                namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.SetActive(true);
                namesObject.GetComponent<NickNameScript>().names[i].text = this.GetComponent<PhotonView>().Owner.NickName;
                break;
            }
        }
    }
}
