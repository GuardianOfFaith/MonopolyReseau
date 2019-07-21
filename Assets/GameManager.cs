using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    public TextMeshProUGUI debugText2;
    public Carte carte;
    public GameObject[] playersInfo;
    public int playerCount;
    public Transform Plateau;
    public GuiContainer[] gui;
    public int playerID;
    
    // Start is called before the first frame update
    void Start()
    {
        debugText.text = "";
        instance = this;
        seed=(int)PhotonNetwork.room.CustomProperties["seed"];
        Debug.Log(PhotonNetwork.playerList.Length);
        playerCount = PhotonNetwork.playerList.Length;
        gui=new GuiContainer[playerCount];
        Random.InitState(seed);
        gs =  gameObject.GetComponent<GameState>();
        gs.Creer();
        instantiatePlayer(playerCount);
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
        gs.ChangePlayer("","");
        OnGui();
    }
    
    
    public void InitGui()
    {
        int i = 0;

        foreach (GameObject go in playersInfo)
        {
            if (i >= playerCount)
            {
                go.SetActive(false);
      
            }
            else
            {
                string s1 = "";
                string s4 = "";
                if (i < PhotonNetwork.playerList.Length)
                {
                    s1 = PhotonNetwork.playerList[i].NickName+" "+gs.Players[i].name;
                    s4 = PhotonNetwork.playerList[i].NickName;
                    if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList[i].NickName == PhotonNetwork.playerName)
                    {   
                        Debug.Log(PhotonNetwork.playerList[i].NickName +"  "+ PhotonNetwork.playerName);
                        debugText2.text =""+ gs.Players[i].id;
                        playerID = gs.Players[i].id;
                    }
                    go.transform.Find("UserName").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.playerList[i].NickName+" "+gs.Players[i].name;;
                
                }
                else
                {
                    s1 = "IA";
                    s4 = "IA";
                    go.transform.Find("UserName").GetComponent<TextMeshProUGUI>().text = "IA";
                }

                string s2 = "SpriteP" + (i + 1);
                string s3 = gs.Players[i].Money.ToString();
                go.transform.Find("Icone").GetComponent<Image>().sprite = Resources.Load<Sprite>("SpriteP" + (i + 1));
                go.transform.Find("Money").GetComponentInChildren<TextMeshProUGUI>().text = gs.Players[i].Money.ToString();
                gui[i]=new GuiContainer(s1,s2,s3,s4);
            }    
            i++;
        }
        OnGui();
    }
    
    public void InitGui(GuiContainer[] g)
    {
        int i = 0;

        foreach (GameObject go in playersInfo)
        {
            if (i >= playerCount)
            {
                go.SetActive(false);
                
            }
            else
            {
                if (g[i].Nickname== PhotonNetwork.playerName)
                {   
                    Debug.Log(PhotonNetwork.playerList[i].NickName +"  "+ PhotonNetwork.playerName);
                    debugText2.text =""+ gs.Players[i].id;
                    playerID = gs.Players[i].id;
                }
                string s1 = g[i].name;
                string s2 = g[i].sprite;
                string s3 = g[i].argent;
          
                go.transform.Find("UserName").GetComponent<TextMeshProUGUI>().text = s1;          
                go.transform.Find("Icone").GetComponent<Image>().sprite = Resources.Load<Sprite>(s2);
                go.transform.Find("Money").GetComponentInChildren<TextMeshProUGUI>().text = s3;
                i++;
            }
            
        }
        OnGui();
    }
    void OnGui()
    {
        int i = 0;
        foreach(GameObject go in playersInfo)
        {
            if (i >= playerCount)
            {
                return;
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

public class GuiContainer
{
    public string name;
    public string sprite;
    public string argent;
    public string Nickname;
    public GuiContainer(string _name, string _sprite, string _argent,string nickname)
    {
        name = _name;
        sprite = _sprite;
        argent = _argent;
        Nickname = nickname;
    }

}
