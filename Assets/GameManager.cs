using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    public GameObject[] guiButton;

    public bool isRollingDice = false;
    public int countDouble = 0;
    public bool isBuying = false;

    // Start is called before the first frame update
    void Start()
    {
        debugText.text = "";
        instance = this;
        seed = (int)PhotonNetwork.room.CustomProperties["seed"];
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
        isRollingDice = true;
        
        InitGui();
    }

    //MOVE PLAYER
    public void move()
    {
        gs.Roll();
        isRollingDice = false;
        if (gs.isDiceDouble())
        {
            isRollingDice = true;
            gs.getActivePlayer().doubleRolls++;
            if (gs.getActivePlayer().doubleRolls == 3)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    gs.getActivePlayer().AllerEnPrison();
                    gs.getActivePlayer().doubleRolls = 0;
                }
                else
                {
                    gs.moveplayerPrison(gs.getActivePlayer().id-1);
                }

                if (gs.getProprieter(gs.getActivePlayer().IDCase).Owner == 0)
                    isBuying = true;
                else
                {
                    Player p = gs.getActivePlayer();
                    Propriete P = gs.getProprieter(gs.getActivePlayer().IDCase);
                    
                    gs.PlayerPay(p.id, P.Owner, P.prix*(P.Tier));
                }

                refreshGui();
                
                return;
            }
        }           
        if (PhotonNetwork.isMasterClient)
        {
            gs.getActivePlayer().move(gs.getDiceRoll());
            gs.getActivePlayer().doubleRolls = 0;
        }
        else
        {
            gs.moveplayer(gs.getActivePlayer().id-1,gs.getDiceRoll());  
        }
        if (gs.getProprieter(gs.getActivePlayer().IDCase).Owner == 0 || gs.getProprieter(gs.getActivePlayer().IDCase).Owner == gs.getActivePlayer().id)
            isBuying = true;
        else
        {
            Player p = gs.getActivePlayer();
            Propriete P = gs.getProprieter(gs.getActivePlayer().IDCase);
            gs.PlayerPay(p.id, P.Owner, P.prix*(P.Tier+1));
        }
        refreshGui();
    }

    public void EndTurn()
    {
        if (gs.getActivePlayer().IsInPrison > 0)
            gs.getActivePlayer().IsInPrison--;
        gs.ChangePlayer("","");
        OnGui();
    }
    
    public void BuyingMod(bool val)
    {
        isBuying = val;
        refreshGui();
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

    public void refreshGui()
    {
        OnGui();
    }

    //public void endGame()
    //{
    //    foreach(GameObject g in guiButton)
    //    {
    //        g.SetActive(false);
    //    }
    //    guiButton[3].GetComponentInChildren<TextMeshProUGUI>().text = "Partie Terminée\nJoueur " + gs.Players[0].name + " à gagné";
    //    guiButton[3].GetComponentInChildren<Button>().onClick.AddListener(reloadMenu);
    //}
    //public void reloadMenu()
    //{
    //    SceneManager.LoadScene(0);
    //}


    void OnGui()
    {
        int i = 0;
        foreach(GameObject go in playersInfo)
        {
            if (i >= playerCount)
            {
                break;
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
        Debug.Log(playerID-1+" "+gs.Players.Count);
        if (playerID == 0)
        {
            return;
            
        }
        if (gs.Players[playerID-1] == gs.getActivePlayer())
        {
            if (isRollingDice && countDouble < 3)
                guiButton[0].SetActive(true);
            else if (countDouble == 3)
            {
                guiButton[3].SetActive(true);
                guiButton[3].GetComponent<TextMeshPro>().text = "Allez en prison !";
            }
            else if (isBuying)
            {
                Propriete prop = gs.getProprieter(gs.getActivePlayer().IDCase);
                if (prop.Type != Propriete.TypeCase.Allez_en_Prison && prop.Type != Propriete.TypeCase.Chance && prop.Type != Propriete.TypeCase.Communauté && prop.Type != Propriete.TypeCase.Depart && prop.Type != Propriete.TypeCase.Parc && prop.Type != Propriete.TypeCase.Prison && prop.Type != Propriete.TypeCase.Taxe && prop.Owner == 0 && gs.getActivePlayer().Money > prop.prix)
                {
                    guiButton[2].SetActive(true);
                    guiButton[2].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    guiButton[2].transform.Find("Buy").gameObject.SetActive(true);
                    if (prop.Name != null  && prop.Prix != null && prop.Type != null)
                        guiButton[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = prop.Name + "\nPrix : " + prop.Prix + "\nGroupe : " + prop.Type.ToString();
                }
                else if (prop.Owner == playerID && gs.Players[i].Money > prop.Prix * 0.5f * prop.Tier)
                {
                    guiButton[2].SetActive(true);
                    guiButton[2].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    guiButton[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = prop.Name + "\nAjouter une Maison : " + prop.Prix * 0.5f *prop.Tier;
                    guiButton[2].transform.Find("Construct").gameObject.SetActive(true);
                }
            }
            //else if (false)
            //{

            //}
            else
            {
                guiButton[1].SetActive(true);
            }
            isRollingDice = false;
        }
        else
        {
            foreach( GameObject g in guiButton)
            {
                g.SetActive(false);
            }
        }
    }

    public void buyProp()
    {
        gs.BuyPropiete(gs.getActivePlayer().IDCase);
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
