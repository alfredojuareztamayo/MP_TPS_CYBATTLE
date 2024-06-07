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
    // Start is called before the first frame update
    void Start()
    {
        killsPanel = GameObject.Find("KillCountPanel");
        namesObject = GameObject.Find("namesBG");
        killsPanel.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Im pressed K");
            if(killsCountOn == false)
            {
                killsPanel.SetActive(true);
                killsCountOn = true;
                higestkills.Clear();
                for(int i = 0; i <names.Length; i++)
                {
                    higestkills.Add(new Kills(namesObject.GetComponent<NickNameScript>().names[i].text,Random.Range(1,2900)));
                }
                higestkills.Sort();
                for(int i = 0; i < names.Length; i++)
                {
                    names[i].text = higestkills[i].playerName;
                    killsAmts[i].text = higestkills[i].playerKills.ToString();

                }
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i].text == "name")
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
}
