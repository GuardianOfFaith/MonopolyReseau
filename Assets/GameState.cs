using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameState : MonoBehaviour
{
    int common_Case = 0;
    int active_Player=1;
    int dice_1, dice_2 = 2;
    public GameManager gm;
    public Propriete[] Case= new Propriete[40];
    public List<Player> Players = new List<Player>();
    public Sprite[] dices = new Sprite[6];
    public Image dice1;
    public Image dice2;
    
    
    public void Creer()
    {
        Case = gm.Plateau.GetComponentsInChildren<Propriete>();
        foreach (var propriete in Case.OrderBy(propriete => propriete.id));
        gm.carte.Creer();
    }    

    public Propriete getProprieter(int i)
    {
        return Case[i];
    }

    public bool once = true;
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            int[] temp = new int[gm.playerCount];
            
            
            for (int i = 0; i < gm.playerCount; i++)
            {
                temp[i]=Players[i].IDCase;               
            }
                
            
            int[] Temp = new int[40];
            for (int i = 0; i < 40; i++)
            {
                Temp[i]=Case[i].Tier;
            }
                      
            stream.SendNext(temp);
            stream.SendNext(Temp);
            gm.debugText.text = "" + active_Player;
            stream.SendNext(active_Player);
            
            if (once)
            {
                string[] names = new string[gm.playerCount];
                string[] sprite = new string[gm.playerCount];
                string[] argent = new string[gm.playerCount];

                for (int i = 0; i < gm.playerCount; i++)
                {
                    names[i] = gm.gui[i].name;
                    sprite[i] = gm.gui[i].sprite;
                    argent[i] = gm.gui[i].argent;
                }
                stream.SendNext(names);
                stream.SendNext(sprite);
                stream.SendNext(argent);
            }
        }
        else
        {
            int[] temp = (int[])stream.ReceiveNext();
            int[] Temp = (int[])stream.ReceiveNext();
            int player = (int)stream.ReceiveNext();
            string[] names = (string[])stream.ReceiveNext();
            string[] sprite = (string[])stream.ReceiveNext();
            string[] argent = (string[])stream.ReceiveNext();

            active_Player = player;
            gm.debugText.text = "" + active_Player;
            
            Debug.Log(Players.Count+" "+gm.playerCount+" "+temp.Length);
            
            for (int i = 0; i < gm.playerCount; i++)
            {   
                Players[i].move(Case[temp[i]]);
            }
     
            for (int i = 0; i < Temp.Length; i++)
            {
                if (Temp[i] > Case[i].Tier)
                {
                    int k = Temp[i] - Case[i].Tier;
                    for (int j = 1; j <= k; j++)
                    {
                        Case[i].CreerMaison();
                    }
                }
                
                if (Temp[i] < Case[i].Tier)
                {
                    int k = Case[i].Tier - Temp[i] ;
                    for (int j = 1; j <= k; j++)
                    {
                        Case[i].RetirerMaison();
                    }
                }
            }
            
           if (once)
            {
                for (int i = 0; i < gm.playerCount; i++)
                {
                    gm.gui[i].name=names[i];
                    gm.gui[i].sprite=sprite[i];
                    gm.gui[i].argent=argent[i];
                }
                gm.InitGui(gm.gui);
                once = false;
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

    public void NextTurn()
    {
        if (getActivePlayer().IsInPrison > 0)
            getActivePlayer().IsInPrison--;
        GetComponent<PhotonView>().RPC("ChangePlayer",PhotonTargets.All, "jup", "and jup!");
    }
    //Change the active player
    [PunRPC]
    public void ChangePlayer(string a,string b)
    {
        active_Player++;
        if (active_Player >= Players.Count+1)
            active_Player = 1;
    }
    public Player getActivePlayer()
    {
        return Players[active_Player-1];
    }

    //randomize the order of the player
    public void setPlayerList(List<Player> list)
    {
        int[] diceroll = new int[gm.playerCount];
        for(int i = 0; i < gm.playerCount; i++)
        {
            Roll();
            diceroll[i] = getDiceRoll();
        }
        List<Player> li = new List<Player>();
        for (int i = 0; i < gm.playerCount; i++)
        {
            int maxInd = diceroll.ToList().IndexOf(diceroll.Max<int>());
            li.Add(list[maxInd]);
            diceroll[maxInd] = 0;
        }

        int j = 0;
        foreach (Player p in li)
        {
            p.id = j+1;
            //gm.debugText.text += " " + li[j].name + " " + (j+1); 
            j++;
        }
        Players = li;
       
    }

    //Remove a defeated player
    public void RemovePlayer()
    {
        Players.RemoveAt(active_Player);
        active_Player--;
        ChangePlayer("","");
    }

    //Make a dice roll
    public bool Roll() {
        
        dice_1 = Random.Range(1, 7);
        dice_2 = Random.Range(1, 7);
        dice1.sprite = dices[dice_1-1];
        dice2.sprite = dices[dice_2-1];
        
        return dice_1 == dice_2;
    }
    //to get the value of the roll
    public int getDiceRoll()
    {
        return dice_1 + dice_2;
    }
}
