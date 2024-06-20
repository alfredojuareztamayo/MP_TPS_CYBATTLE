using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

public class Maze : MonoBehaviourPunCallbacks
{
    [Tooltip("This is the depth of your maze")]
    public int depth = 30;
    [Tooltip("This is the Width of your maze")]
    public int width = 30;

    public byte[,] map;

    [Tooltip("This is the scale of your maze")]
    public int scale = 6;

    [Tooltip("This is your main character, always will spawn at the start of the maze")]
    public GameObject player;

    public List<MapLocation> direction = new List<MapLocation>()
    {
        new MapLocation(0, 1),
        new MapLocation(0, -1),
        new MapLocation(1, 0),
        new MapLocation(-1, 0)
    };

    public GameObject MazeWall;
    public Transform[] SpawnPoints;
    public Transform[] WeaponsPoints;
    public Transform[] PickUpMagic;

    public bool isMaze = false; //Boolean para verifiaasdbaidabd

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitialiseMap();
            Generate();
            DrawMap();
            InstantiateSpawnPoints();
            InstantiateWeaponsPoints();
            SendMazeDataToOthers();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void ReceiveMazeData(byte[] mazeData)
    {
        map = DeserializeMaze(mazeData);
        DrawMap();
        InstantiateSpawnPoints();
        InstantiateWeaponsPoints();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Esta funcion sirve
    /// </summary>
    void SendMazeDataToOthers()
    {
        byte[] mazeData = SerializeMaze(map);
        photonView.RPC("ReceiveMazeData", RpcTarget.OthersBuffered, mazeData);
    }

    void InitialiseMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;
            }
        }
    }

    /// <summary>
    /// Virtual void Generate el mapa procedural
    /// </summary>
    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    map[x, z] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Dibuja el mapa
    /// </summary>
    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x * scale, -0.5f, z * scale);
                if (map[x, z] == 1)
                {
                    GameObject wall = Instantiate(MazeWall);
                    wall.transform.position = pos;
                }
            }
        }
    }

    void InstantiateSpawnPoints()
    {
        int numberOfSpawns = SpawnPoints.Length;
        int iter = 0;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x * scale, 3, z * scale);
                if (numberOfSpawns > 0)
                {
                    if (map[x, z] == 0)
                    {
                        SpawnPoints[iter].transform.position = pos;
                        iter++;
                        numberOfSpawns--;
                        x += (width / SpawnPoints.Length);
                        z += (depth / SpawnPoints.Length);
                    }
                }
            }
        }
    }

    void InstantiateWeaponsPoints()
    {
        int numberOfSpawns = WeaponsPoints.Length;
        int iter = 0;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x * scale, 0 - (scale / 2), z * scale);
                if (numberOfSpawns > 0)
                {
                    if (map[x, z] == 0)
                    {
                        WeaponsPoints[iter].transform.position = pos;
                        PickUpMagic[iter].transform.position = pos;
                        iter++;
                        numberOfSpawns--;
                        x--;
                        x += (width / WeaponsPoints.Length);
                        z--;
                        z += (depth / WeaponsPoints.Length);
                    }
                }
            }
        }
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;

        return count;
    }

    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x - 1, z - 1] == 0) count++;
        return count;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int CountAllNeighbours(int x, int z)
    {
        return CountDiagonalNeighbours(x, z) + CountSquareNeighbours(x, z);
    }

    byte[] SerializeMaze(byte[,] maze)
    {
        int size = maze.GetLength(0) * maze.GetLength(1);
        byte[] result = new byte[size];
        Buffer.BlockCopy(maze, 0, result, 0, size);
        return result;
    }

    byte[,] DeserializeMaze(byte[] data)
    {
        byte[,] result = new byte[width, depth];
        Buffer.BlockCopy(data, 0, result, 0, data.Length);
        return result;
    }
}
