using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS_Cam;
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
    public bool isDouble = false;
    public RTS_Camera cam;
    public Carte cartes;
    public bool DebugStop = false;
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

    private void Update()
    {
        gm.debugText.text ="Count "+ Players.Count;
    }

    public bool once = true;
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            int[] temp = new int[gm.playerCount];
            
            
            for (int i = 0; i < gm.playerCount; i++)// Player Position
            {
                temp[i]=Players[i].IDCase;           
            }
                
            
            int[] Temp = new int[40]; // Display houses on properties
            
            string[] Tempname = new String[40]; // Display owner name on properties
            
            for (int i = 0; i < 40; i++)
            {
                Temp[i]=Case[i].Tier;
                if (Case[i].ownerName)
                {
                    Tempname[i] = Case[i].ownerName.text;
                }
                else
                {
                    Tempname[i] = "";  
                }
                
            }
            
            int[] temp2 = new int[gm.playerCount];// Synch number of double Rolls
            
            
            for (int i = 0; i < gm.playerCount; i++)
            {
                temp2[i]=Players[i].doubleRolls;               
            }

                      
            stream.SendNext(temp);
            stream.SendNext(Temp);
            stream.SendNext(Tempname);
            stream.SendNext(temp2);
            gm.debugText.text = "" + active_Player;
            stream.SendNext(active_Player);
            
            if (once)
            {
                string[] names = new string[gm.playerCount];
                string[] sprite = new string[gm.playerCount];
                string[] argent = new string[gm.playerCount];
                string[] Nickname = new string[gm.playerCount];
                for (int i = 0; i < gm.playerCount; i++)
                {
                    names[i] = gm.gui[i].name;
                    sprite[i] = gm.gui[i].sprite;
                    argent[i] = gm.gui[i].argent;
                    Nickname[i] = gm.gui[i].Nickname;
                }
                stream.SendNext(names);
                stream.SendNext(sprite);
                stream.SendNext(argent);
                stream.SendNext(Nickname);
            }
            
        }
        else
        {
            int[] temp = (int[])stream.ReceiveNext();
            int[] Temp = (int[])stream.ReceiveNext();
            string[] Tempname = (string[])stream.ReceiveNext();
            int[] temp2 = (int[])stream.ReceiveNext();
            int player = (int)stream.ReceiveNext();
            string[] names = (string[])stream.ReceiveNext();
            string[] sprite = (string[])stream.ReceiveNext();
            string[] argent = (string[])stream.ReceiveNext();
            string[] Nickname = (string[])stream.ReceiveNext();
            active_Player = player;
            gm.debugText.text = "" + active_Player;
            
            Debug.Log(Players.Count+" "+gm.playerCount+" "+temp.Length);
            
            for (int i = 0; i < gm.playerCount; i++)
            {   
                Players[i].move(Case[temp[i]]);
            }
     
            for (int i = 0; i < Temp.Length; i++)
            {
                if (Case[i].ownerName)
                {
                    Case[i].ownerName.text = Tempname[i];  
                }
                
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
            
            for (int i = 0; i < gm.playerCount; i++)
            {   
                Players[i].doubleRolls=temp2[i];
            }
            
           if (once)
            {
                for (int i = 0; i < gm.playerCount; i++)
                {
                    gm.gui[i].name=names[i];
                    gm.gui[i].sprite=sprite[i];
                    gm.gui[i].argent=argent[i];
                    gm.gui[i].Nickname = Nickname[i];
                }
                gm.InitGui(gm.gui);
                gm.refreshGui();
                once = false;
                cam.targetFollow = Players[active_Player - 1].transform;
                
            }
           gm.refreshGui();
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

    [PunRPC]
    public void MasterMovePlayer(int player, int Case)
    {
        getActivePlayer().move(Case);
        getActivePlayer().doubleRolls = 0;
    }

    public void moveplayer(int player, int Case)
    {
        GetComponent<PhotonView>().RPC("MasterMovePlayer",PhotonTargets.MasterClient, player, Case);  
        
    }

    [PunRPC]
    public void MasterBuyPropiete(int _Case)
    {
        Case[_Case].CreerMaison();
        Case[_Case].ownerName.text = "p"+getActivePlayer().id;
    }
    
    [PunRPC]
    public void AllBuyPropiete(int _Case)
    {
        Case[_Case].Owner = getActivePlayer().id;
        getActivePlayer().properties.Add(Case[_Case]);
        Debug.Log("payin");
        getActivePlayer().money -= Case[_Case].prix;
        Debug.Log( getActivePlayer().money+" "+Case[_Case].name);
        gm.refreshGui();
    }
    
    public void BuyPropiete(int Case)
    {
        GetComponent<PhotonView>().RPC("MasterBuyPropiete",PhotonTargets.MasterClient,  Case);  
        GetComponent<PhotonView>().RPC("AllBuyPropiete",PhotonTargets.All,  Case);    
    }
    
    [PunRPC]
    public void CheckPlayerPay(int payer,int receiver,int amount)
    {
        Players[payer - 1].money = Mathf.Clamp(Players[payer - 1].Money-amount,0,100000);
        Players[receiver - 1].money += amount;
        gm.refreshGui();
    }
    
    public void PlayerPay(int payer,int receiver,int amount)
    {
        GetComponent<PhotonView>().RPC("CheckPlayerPay",PhotonTargets.All,  payer,receiver,amount);  
    }

    [PunRPC]
    public void AllGainMoney(int payer,int amount)
    {
        Players[payer - 1].money += amount;
        gm.refreshGui();
    }
    
    public void GainMoney(int payer,int amount)
    {
        GetComponent<PhotonView>().RPC("AllGainMoney",PhotonTargets.All,  payer,amount); 
        
    }

    [PunRPC]
    public void AllDrawCard(EffetCarte.CarteType type)
    {
        switch (type)
        {
            case EffetCarte.CarteType.Chance:
                cartes.DrawnChance();
                break;
            case EffetCarte.CarteType.Communauté:
                cartes.DrawnComu();
                break;
        }
    }
    
    public void DrawCard(EffetCarte.CarteType type)
    {
        GetComponent<PhotonView>().RPC("AllDrawCard",PhotonTargets.All,  type);  
    }
    
    [PunRPC]
    public void AllLooseMoney(int payer,int amount)
    {
        Players[payer - 1].money = Mathf.Clamp(Players[payer - 1].Money-amount,0,100000);
        gm.refreshGui();
    }
    
    public void LooseMoney(int payer,int amount)
    {
        GetComponent<PhotonView>().RPC("AllLooseMoney",PhotonTargets.All,  payer,amount);  
    }

    
    [PunRPC]
    public void MasterGotoPrison(int player)
    {
        getActivePlayer().AllerEnPrison();
        getActivePlayer().doubleRolls = 0;
    }

    public void moveplayerPrison(int player)
    {
        GetComponent<PhotonView>().RPC("MasterGotoPrison",PhotonTargets.MasterClient, player);
    }
    
    

    
    public void NextTurn()
    {
        if (getActivePlayer().IsInPrison > 0)
            getActivePlayer().IsInPrison--;
        gm.isRollingDice = false;
        GetComponent<PhotonView>().RPC("ChangePlayer", PhotonTargets.All, "jup", "and jup!"); 
        gm.refreshGui();
    }
    //Change the active player
    [PunRPC]
    public void ChangePlayer(string a, string b)
    {
        Debug.Log("Reroll");
        //if (getActivePlayer().Money < 0)
        //{
        //    Players.Remove(getActivePlayer());
        //}
        //else
        //{
            active_Player++;
        //}
        //if(Players.Count > 1)
        //{
            if (active_Player >= Players.Count + 1)
            {
                active_Player = 1;
            }

            //if (Players[active_Player - 1].canPlay)
            //{
                if (active_Player == gm.playerID)
                {
                    gm.isRollingDice = true;
                    Roll();
                }
                else
                {
                    gm.guiButton[3].SetActive(false);
                }
                cam.targetFollow = Players[active_Player - 1].transform;
                gm.refreshGui();
            //}
            //else
            //{
            //    NextTurn();
            //}
        //}
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

        if (li.Count == 1)
        {
            DebugStop = true;
        }
        Players = li;
        cam.targetFollow = Players[active_Player - 1].transform;
    }

    //Remove a defeated player
    public void RemovePlayer()
    {
        Players.RemoveAt(active_Player);
        active_Player--;
        ChangePlayer("","");
    }

    //Make a dice roll
    public void Roll() {
        isDouble = false;
        dice_1 = Random.Range(1, 7);
        dice_2 = Random.Range(1, 7);
        dice1.sprite = dices[dice_1-1];
        dice2.sprite = dices[dice_2-1];

        if (dice_1 == dice_2)
            isDouble = true;
    }

    public bool isDiceDouble()
    {
        return dice_1 == dice_2;
    }
    //to get the value of the roll
    public int getDiceRoll()
    {
        return dice_1 + dice_2;
    }

    public void Buy()
    {
        if (getActivePlayer().Money > getProprieter(getActivePlayer().IDCase).prix)
            BuyPropiete(getActivePlayer().IDCase);
    }
}
