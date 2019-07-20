using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    int money;
    public int IDCase = 0;
    public Propriete Case;
    public GameState gs;
    List<Propriete> properties { set; get; }
    //List<int> cards { get; }
    int isInPrison;
    public int id;
    public int doubleRolls = 0;
    
    void Start()
    {
        gs = GameManager.instance.gs;
        Case = gs.getProprieter(IDCase);
        transform.SetParent(Case.transform);
        transform.localScale = new Vector3(10,20,10);
        switch (name)
        {
            case "p1":
                transform.localPosition = new Vector3(-20, 0, 0);
                break;
            case "p2":
                transform.localPosition = new Vector3(-20, -30, 0);
                break;
            case "p3":
                transform.localPosition = new Vector3(20, 0, 0);
                break;
            case "p4":
                transform.localPosition = new Vector3(20, -30, 0);
                break; 
        }
    }
    
    
    //public int Id
    //{
    //    set { id = value; }
    //    get { return id; }
    //}

    public int Id_case
    {
        set { IDCase = value; }
        get { return IDCase; }
    }

    public int IsInPrison {
        get { return isInPrison; }
        set { isInPrison = value; }
    }

    public int Money
    {
        get { return money; }
    }

    //TO CREDIT OR DEBIT THIS PLAYER
    public void CreditPlayer(int value)
    {
        money = Money + value;
    }
    public void DebitPlayer(int value)
    {
        money = Money - value;
    }
    

    //TO ADD AND REMOVE CARD TO PLAYER
    public void AddCard(int card)
    {
        throw new NotImplementedException();
        //cards.Add(card);
    }
    public bool HasCard(int card)
    {
        throw new NotImplementedException();
        //return cards.Contains(card);
    }
    public void UseCard(int card)
    {
        throw new NotImplementedException();
        //cards.Remove(card);
    }

    //TO ADD AND REMOVE PROPERTY TO PLAYER
    public void AddProperty(Propriete prop)
    {
        DebitPlayer(prop.Prix);
        properties.Add(prop);
    }
    public void SellProperty(Propriete prop)
    {
        CreditPlayer(prop.Prix);
        properties.Remove(prop);
    }
    //MORTGAGE PROPERTY
    public void MortgageProperty(Propriete prop)
    {
        CreditPlayer(prop.Prix / 2);
        prop.IsMortgage = true;
    }
    //DISENCUMBER PROPERTY
    public void DisencumberProperty(Propriete prop)
    {
        DebitPlayer(prop.Prix / 2);
        prop.IsMortgage = false;
    }
    //ToCheckIsGroupIsFull
    public bool isGroupFull(Propriete prop)
    {
        throw new NotImplementedException();
    }

    //HANDLER
    public bool isBuildable(Propriete prop)
    {
        if (isGroupFull(prop))
        {
            if (hasHouse(prop) < 5)
                return true;
            else return false;
        }
        else
            return false;
    }
    public int hasHouse(Propriete prop)
    {
        return prop.UpgradeTier;
    }
    //BUY HOUSE
    public void AddHouse(Propriete prop, int num)
    {
        for( int i = 0; i< num; i++)
            prop.addHouse();
        DebitPlayer(prop.GetHousePrice * num);
    }
    //SELL HOUSE
    public void RemoveHouse(Propriete prop, int num)
    {
        for (int i = 0; i < num; i++)
            prop.removeHouse();
        CreditPlayer(prop.GetHousePrice * num / 2);
    }

    public void SetPrisonner()
    {
        IsInPrison = 3;
        IDCase = 10;
        AllerEnPrison();
    }



    void Awake()
    {
        IDCase = 0;
        money = 15000;
        properties = new List<Propriete>();
        isInPrison = 0;
    }
    
    public int move(int movement)
    {
        IDCase = (IDCase + movement) % 40;
        Case = gs.getProprieter(IDCase);
        if (Case.Type == Propriete.TypeCase.Prison)
        {
            transform.SetParent(Case.transform.GetChild(0));
            switch (name)
            {
                
                case "p1":
                    transform.localPosition = new Vector3(-60, 0, 0);
                    break;
                case "p2":
                    transform.localPosition = new Vector3(-30, 0 , 0);
                    break;
                case "p3":
                    transform.localPosition = new Vector3(0, 0, 0);
                    break;
                case "p4":
                    transform.localPosition = new Vector3(30, 0, 0);
                    break; 
            }
        }
        else if (Case.Type == Propriete.TypeCase.Allez_en_Prison)
        {
            AllerEnPrison();
        }
        else
        {
            transform.SetParent(Case.transform);
            switch (name)
            {
                case "p1":
                    transform.localPosition = new Vector3(-20, 0, 0);
                    break;
                case "p2":
                    transform.localPosition = new Vector3(-20, -30, 0);
                    break;
                case "p3":
                    transform.localPosition = new Vector3(20, 0, 0);
                    break;
                case "p4":
                    transform.localPosition = new Vector3(20, -30, 0);
                    break; 
            }
        }
        
        return IDCase;
    }

    public int move(Propriete _case)
    {
        IDCase = _case.id;
        Case = _case;
        if (Case.Type == Propriete.TypeCase.Prison)
        {
            transform.SetParent(Case.transform.GetChild(0));
            switch (name)
            {
                
                case "p1":
                    transform.localPosition = new Vector3(-60, 0, 0);
                    break;
                case "p2":
                    transform.localPosition = new Vector3(-30, 0 , 0);
                    break;
                case "p3":
                    transform.localPosition = new Vector3(0, 0, 0);
                    break;
                case "p4":
                    transform.localPosition = new Vector3(30, 0, 0);
                    break; 
            }
        }
        else if (Case.Type == Propriete.TypeCase.Allez_en_Prison)
        {
            AllerEnPrison();
        }
        else
        {
            transform.SetParent(Case.transform);
            switch (name)
            {
                case "p1":
                    transform.localPosition = new Vector3(-20, 0, 0);
                    break;
                case "p2":
                    transform.localPosition = new Vector3(-20, -30, 0);
                    break;
                case "p3":
                    transform.localPosition = new Vector3(20, 0, 0);
                    break;
                case "p4":
                    transform.localPosition = new Vector3(20, -30, 0);
                    break; 
            }
        }
        
        return IDCase;
    }
    
    public void AllerEnPrison()
    {
        IDCase = 10;
        Case = gs.getProprieter(IDCase);
        transform.SetParent(Case.transform.GetChild(2));
        switch (name)
        {
            case "p1":
                transform.localPosition = new Vector3(-20, 0, 0);
                break;
            case "p2":
                transform.localPosition = new Vector3(-20, -30, 0);
                break;
            case "p3":
                transform.localPosition = new Vector3(20, 0, 0);
                break;
            case "p4":
                transform.localPosition = new Vector3(20, -30, 0);
                break; 
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            gs.dice1.gameObject.SetActive(true);
            gs.dice2.gameObject.SetActive(true);
            if (id == gs.getActivePlayer().id)
            {
                if (gs.Roll())
                {
                    doubleRolls++;
                    if (doubleRolls==3)
                    {
                        AllerEnPrison();
                        doubleRolls = 0;
                        return;
                    }
                
                }
                else
                {
                    doubleRolls = 0;
                    move(gs.getDiceRoll());
                }
            }
            
            
            //AllerEnPrison();
        }
    }
}
