using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{
    int common_Case = 0;
    int active_Player;
    int dice_1, dice_2 = 2;
    public GameManager gm;
    public Propriete[] Case= new Propriete[40];
    public List<Player> Players = new List<Player>();
    
    public void Creer()
    {
        Case = gm.Plateau.GetComponentsInChildren<Propriete>();
        foreach (var propriete in Case.OrderBy(propriete => propriete.id));
        int i = 0;
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
   
            if (p == PhotonNetwork.player)
            {
                gm.debugText.text = p.name + " / " + PhotonNetwork.player.ID; // PLAYER ID PERMETTRA DE METTRE UN NUMEROS AU JOUEUR
            }

            i++;
        }
        gm.carte.Creer();
    }    

    public Propriete getProprieter(int i)
    {
        return Case[i];
    }
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            int[] temp = new int[4];
            for (int i = 0; i < 4; i++)
            {
                temp[i]=Players[i].IDCase;
            }
                
            
            int[] Temp = new int[40];
            for (int i = 0; i < 40; i++)
            {
                Temp[i]=Case[i].Maison;
            }
            
            stream.SendNext(temp);
            stream.SendNext(Temp);
        }
        else
        {
            int[] temp = (int[])stream.ReceiveNext();
            int[] Temp = (int[])stream.ReceiveNext();
            Debug.Log(Temp[5]);
            
            for (int i = 0; i < 4; i++)
            {
                
                Players[i].move(Case[temp[i]]);
            }

            for (int i = 0; i < Temp.Length; i++)
            {
                if (Temp[i] > Case[i].Maison)
                {
                    int k = Temp[i] - Case[i].Maison;
                    for (int j = 1; j <= k; j++)
                    {
                        Case[i].CreerMaison();
                    }
                }
            }
        }
    }
    
    //To Add money in the common pot
    public void AddToCommon(int val)
    {
        common_Case += val;
    }
    //Give money from the common case to the player
    public void ResetCommon()
    {
        Players[active_Player].CreditPlayer(common_Case);
        common_Case = 0;
    }

    //Change the active player
    public void ChangePlayer()
    {
        active_Player++;
        if (active_Player >= Players.Count)
            active_Player = 0;
    }
    public Player getActivePlayer()
    {
        return Players[active_Player];
    }

    //randomize the order of the player
    public void setPlayerList(List<Player> list)
    {
        int[] diceroll = new int[4];
        for(int i = 0; i < 4; i++)
        {
            Roll();
            diceroll[i] = getDiceRoll();
        }
        List<Player> li = new List<Player>();
        for (int i = 0; i < 4; i++)
        {
            int maxInd = diceroll.ToList().IndexOf(diceroll.Max<int>());
            li.Add(list[maxInd]);
            diceroll[maxInd] = 0;
        }
        playerList = li;
        foreach (Player p in playerList)
        {
            gm.debugText.text += p.name;
        }
    }

    //Remove a defeated player
    public void RemovePlayer()
    {
        Players.RemoveAt(active_Player);
        active_Player--;
        ChangePlayer();
    }

    //Make a dice roll
    public bool Roll() {
        dice_1 = Random.Range(1, 7);
        dice_1 = Random.Range(1, 7);
        return dice_1 == dice_2;
    }
    //to get the value of the roll
    public int getDiceRoll()
    {
        return dice_1 + dice_2;
    }
}
