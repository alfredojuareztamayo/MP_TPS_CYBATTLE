using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text minutesText, secondsText;
    public int minutes, seconds;
    public GameObject Canvas;
    [HideInInspector]
    public bool timeStop = false;

    public void BegingTimer()
    {
        GetComponent<PhotonView>().RPC("Count", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void Count()
    {
        BegingCounting();
    }
    void BegingCounting()
    {
        CancelInvoke();
        InvokeRepeating("TimeCountDown", 1, 1);
    }

    void TimeCountDown()
    {
        if (this.gameObject.GetComponent<NickNameScript>().noRespawn == false)
        {

            if (seconds > 10)
            {
                seconds -= 1;
                secondsText.text = seconds.ToString();
            }
            else if (seconds > 0 && seconds < 11)
            {
                seconds -= 1;
                secondsText.text = "0" + seconds.ToString();
            }
            else if (seconds == 0 && minutes > 0)
            {
                secondsText.text = "0" + seconds.ToString();
                minutes -= 1;
                seconds = 59;
                secondsText.text = seconds.ToString();
                minutesText.text = minutes.ToString();
            }
            if (seconds == 0 && minutes <= 0)
            {
                if (this.gameObject.GetComponent<NickNameScript>().teamMode == false)
                {
                    Canvas.GetComponent<KillCount>().countDown = false;
                    Canvas.GetComponent<KillCount>().TimeOver();
                    timeStop = true;
                }
                if (this.gameObject.GetComponent<NickNameScript>().teamMode == true)
                {
                    Canvas.GetComponent<TeamKillCount>().countDown = false;
                    Canvas.GetComponent<TeamKillCount>().TimeOver();
                    timeStop = true;
                }

            }
        }
        else
        {
            minutesText.text = "";
            secondsText.text = "";
        }
    }
}
