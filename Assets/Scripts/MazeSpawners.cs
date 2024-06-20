using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MazeSpawners : MonoBehaviour
{
    public Transform[] weaponSpawnPoints;
    public Transform[] spawnPoints;
    public Transform[] spawnsMagic;
    // Start is called before the first frame update

    public Transform[] WeaponsSpawns()
    {
        return weaponSpawnPoints;
    }
    public Transform[] SpawnPoints()
    {
        return spawnPoints;
    }
    public Transform[] SpawnMagic()
    {
        return spawnsMagic;
    }
}
