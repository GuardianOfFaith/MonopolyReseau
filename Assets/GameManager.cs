using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameState gs;
    public static GameManager instance;
    public int seed;
    public TextMeshProUGUI debugText;
    public Carte carte;
    public GameObject[] playersInfo; 
    
    public Transform Plateau;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        seed=(int)PhotonNetwork.room.CustomProperties["seed"];
        Random.InitState(seed);
        gs =  gameObject.GetComponent<GameState>();
        gs.Creer();
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
        for (int i = 0; i < count; i++)
        {
            list.Add(Instantiate(Resources.Load<GameObject>("p"+(i+1))).GetComponent<Player>());
            list[i].name = "p"+(i+1);
        }
        gs.setPlayerList(list);
        InitGui();
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
        OnGui();
    }

    public void InitGui()
    {
        int i = 0;


        foreach (GameObject go in playersInfo)
        {
            if (i < PhotonNetwork.playerList.Length)
            {
                go.transform.Find("UserName").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.playerList[i].NickName;
            }
            else
            {
                go.transform.Find("UserName").GetComponent<TextMeshProUGUI>().text = "IA";
            }
            go.transform.Find("Icone").GetComponent<Image>().sprite = Resources.Load<Sprite>("SpriteP" + (i + 1));
            i++;
        }
        OnGui();
    }
    void OnGui()
    {
        int i = 0;
        foreach(GameObject go in playersInfo)
        {
            if (i == 3)
            {
                gs.Players[i].IsInPrison = 2;
            }
            if (gs.Players[i].IsInPrison > 0)
            {
                go.transform.Find("Prison").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                go.transform.Find("Prison").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            }
            go.transform.Find("Money").GetComponentInChildren<TextMeshProUGUI>().text = gs.Players[i].Money.ToString();
            i++;
        }
    }
}
