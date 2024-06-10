using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{
    public List<Kills> higestkills = new List<Kills>();
    public Text[] names;
    public Text[] killsAmts;
    private GameObject killsPanel;
    private GameObject namesObject;
    private bool killsCountOn = false;
    public bool countDown = true;
    public GameObject winnerPanel;
    public Text winnerText;

    // Start is called before the first frame update
    void Start()
    {
        killsPanel = GameObject.Find("KillCountPanel");
        namesObject = GameObject.Find("namesBG");
        killsPanel.SetActive(false);
        winnerPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && countDown == true)
        {
            Debug.Log("Im pressed K");
            if(killsCountOn == false)
            {
                killsPanel.SetActive(true);
                killsCountOn = true;
                higestkills.Clear();
                for(int i = 0; i <names.Length; i++)
                {
                    higestkills.Add(new Kills(namesObject.GetComponent<NickNameScript>().names[i].text, namesObject.GetComponent<NickNameScript>().kills[i]));
                }
                higestkills.Sort();
                for(int i = 0; i < names.Length; i++)
                {
                    names[i].text = higestkills[i].playerName;
                    killsAmts[i].text = higestkills[i].playerKills.ToString();

                }
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i].text == "Name")
                    {
                        names[i].text = "";
                        killsAmts[i].text = "";
                    }
                }
            }
            else if (killsCountOn == true)
            {
                killsPanel.SetActive(false);
                killsCountOn = false;
            }
        }
    }
    public void TimeOver()
    {
        killsPanel.SetActive(true);
        winnerPanel.SetActive(true);
        killsCountOn = true;
        higestkills.Clear();
        for (int i = 0; i < names.Length; i++)
        {
            higestkills.Add(new Kills(namesObject.GetComponent<NickNameScript>().names[i].text, namesObject.GetComponent<NickNameScript>().kills[i]));
        }
        higestkills.Sort();
        winnerText.text = higestkills[0].playerName;
        for (int i = 0; i < names.Length; i++)
        {
            names[i].text = higestkills[i].playerName;
            killsAmts[i].text = higestkills[i].playerKills.ToString();
        }
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].text == "Name")
            {
                names[i].text = "";
                killsAmts[i].text = "";
            }
        }
    }
    public void NoRespawnWinner(string name)
    {
        winnerPanel.SetActive(true);
        winnerText.text = name;
    }
}
