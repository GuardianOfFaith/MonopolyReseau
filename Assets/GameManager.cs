using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameState gs;
    public static GameManager instance;
    public GameObject player;
    public int seed;
    public TextMeshProUGUI debugText;
    public Carte carte;
    
    public Transform Plateau;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        seed=(int)PhotonNetwork.room.CustomProperties["seed"];
        Random.InitState(seed);
        gs =  gameObject.GetComponent<GameState>();
        gs.Creer();
        player = Resources.Load<GameObject>("Player");
        instantiatePlayer(4);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiatePlayer(int count)
    {
        List<Player> list = new List<Player>();
        list.Clear();
        
        //create GUI For player information display
        for(int i = 0; i < count; i++)
        {
            list.Add(Instantiate(player).GetComponent<Player>());
            list[i].name = "p"+(i+1);
        }
        gs.setPlayerList(list);
    }

    public void ThrowDice(int double_count)
    {
        bool isDouble = gs.Roll();
        if (isDouble && double_count == 2)
            gs.getActivePlayer().SetPrisonner();
        else
            move(gs.getDiceRoll());

        //play animation
    }

    //MOVE PLAYER
    public void move(int value)
    {
        throw new NotImplementedException();
        int pos = gs.getActivePlayer().Id_case += value;

        if (pos > 39) //MAX CASE NUMBER //TODO
        {
            gs.getActivePlayer().CreditPlayer(2000); //TODO
            pos = pos % 39; //TODO
        }
        Debug.Log("position = " + pos);
        //CALL EVENT
        //TODO
    }

    public void EndTurn()
    {
        if (gs.getActivePlayer().IsInPrison > 0)
            gs.getActivePlayer().IsInPrison--;
        gs.ChangePlayer();
    }
}
