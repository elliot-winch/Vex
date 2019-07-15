using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public Action OnTurnEnd;

    public Player CurrentFocusedPlayer { get; private set; }
    public Team Team { get; private set; }

    private List<Player> playersInTurn;

    public void Begin(Team team, List<Player> playersToAct)
    {
        this.Team = team;
        this.playersInTurn = playersToAct;

        foreach (var p in playersInTurn)
        {
            p.BeginPhase();

            p.OnTurnComplete += FocusNextPlayer;
            p.OnTurnComplete += CheckEnd;
        }

        FocusPlayer(playersInTurn.First());
    }

    public void End()
    {
        foreach (var p in playersInTurn)
        {
            p.OnTurnComplete -= FocusNextPlayer;
            p.OnTurnComplete -= CheckEnd;

            p.EndPhase();
        }

        OnTurnEnd?.Invoke();
    }

    private void CheckEnd()
    {
        if(WillEnd())
        {
            End();
        }
    }

    private bool WillEnd()
    {
        return playersInTurn.All(p => p.TurnIsComplete);
    }

    #region Modifying
    //Players that are added to the board during the turn need to alert the turn they are to be considered
    public void AddPlayer(Player player)
    {
        playersInTurn.Add(player);

        CheckEnd();
    }

    //Players that are removed during the turn need to alert the turn they are no longer to be considered
    public void RemovePlayer(Player player)
    {
        playersInTurn.Remove(player);

        CheckEnd();
    }
    #endregion

    #region Focus
    public void FocusNextPlayer()
    {
        FocusPlayer(playersInTurn.NextWhere(CurrentFocusedPlayer, p => p.TurnIsComplete == false));
    }

    public void FocusPlayer(Player player)
    {
        CurrentFocusedPlayer?.Unfocus();
        CurrentFocusedPlayer = player;
        player?.Focus();
    }
    #endregion
}
