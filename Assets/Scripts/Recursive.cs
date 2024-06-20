using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursive : Maze
{
    public override void Generate()
    {
        Generate(Random.Range(1,width), Random.Range(1, depth));
    }
    
    void Generate(int x, int z)
    {
        if(CountSquareNeighbours(x, z) >=2) return;
        map[x, z] = 0;
        direction.Shuffle();
        Generate(x + direction[0].x,z + direction[0].z);
        Generate(x + direction[1].x,z + direction[1].z);
        Generate(x + direction[2].x,z + direction[2].z);
        Generate(x + direction[3].x,z + direction[3].z);
    }

}
