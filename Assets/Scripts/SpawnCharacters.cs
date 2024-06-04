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

    // Start is called before the first frame update 
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(SpawnCharacter());
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
    IEnumerator SpawnCharacter()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Instantiate(character.name, spawnPoints[PhotonNetwork.CountOfPlayers - 1].position, spawnPoints[PhotonNetwork.CountOfPlayers - 1].rotation);

    }
}
