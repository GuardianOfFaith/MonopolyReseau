using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    int common_Case = 0;
    int active_Player;
    List<Player> playerList;
    int dice_1, dice_2 = 1;

    //To Add money in the common pot
    public void AddToCommon(int val)
    {
        common_Case += val;
    }
    //Give money from the common case to the player
    public void ResetCommon()
    {
        playerList[active_Player].CreditPlayer(common_Case);
        common_Case = 0;
    }

    //Change the active player
    public void ChangePlayer()
    {
        active_Player++;
        if (active_Player >= playerList.Count)
            active_Player = 0;
    }

    //Remove a defeated player
    public void RemovePlayer()
    {
        playerList.RemoveAt(active_Player);
        active_Player--;
        ChangePlayer();
    }

    //Make a dice roll
    public bool Roll() {
        dice_1 = Random.Range(1, 7);
        dice_1 = Random.Range(1, 7);
        return dice_1 == dice_2;
    }
}
