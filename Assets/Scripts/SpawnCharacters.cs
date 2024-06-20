using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnCharacters : MonoBehaviour
{
    public GameObject character;
    public Transform[] spawnPoints;
    public GameObject[] weapons;
    public Transform[] weaponSpawnPoints;
    public float weaponRespawnTime = 10;
    public bool isMazeSpawn = false;

    // Start is called before the first frame update 
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if(isMazeSpawn == false)
            {
                StartCoroutine(SpawnCharacter(1f));
            }
            else
            {
                StartCoroutine(SpawnCharacter(3f));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnWeaponsStart()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            PhotonNetwork.Instantiate(weapons[i].name, weaponSpawnPoints[i].position, weaponSpawnPoints[i].rotation);

        }
    }
    /*public void SpawnPlayer()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber % spawnPoints.Length];
            PhotonNetwork.Instantiate(character.name, spawnPoint.position, spawnPoint.rotation);
        }
    }*/
    IEnumerator SpawnCharacter(float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Instantiate(character.name, spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation);

    }
}
