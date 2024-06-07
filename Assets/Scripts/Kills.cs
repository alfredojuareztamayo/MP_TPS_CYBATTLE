using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Kills : IComparable<Kills>
{
    public string playerName;
    public int playerKills;

    public Kills(string newPlayerName, int newPlayerKills)
    {
       playerName = newPlayerName;
       playerKills = newPlayerKills;
    }

    public int CompareTo(Kills other)
    {
       return other.playerKills - playerKills; 
    }

}
