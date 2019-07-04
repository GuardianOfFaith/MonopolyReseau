using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    GameState gs;
    
    public GameObject player;
    public int seed;
    
    // Start is called before the first frame update
    void Start()
    {
        gs = new GameState();
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
