using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public string teamName;
    public List<Player> allPlayers;

    private void Start()
    {
        allPlayers.ForEach(x => x.Team = this);
    }
}
