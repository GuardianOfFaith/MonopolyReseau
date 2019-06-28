using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //int id;
    int id_case;
    int money;
    List<Properties> properties { set; get; }
    //List<int> cards { get; }
    int isInPrison;

    //public int Id
    //{
    //    set { id = value; }
    //    get { return id; }
    //}

    public int Id_case
    {
        set { id_case = value; }
        get { return id_case; }
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
    public void AddProperty(Properties prop)
    {
        DebitPlayer(prop.Price);
        properties.Add(prop);
    }
    public void SellProperty(Properties prop)
    {
        CreditPlayer(prop.Price);
        properties.Remove(prop);
    }
    //MORTGAGE PROPERTY
    public void MortgageProperty(Properties prop)
    {
        CreditPlayer(prop.Price / 2);
        prop.IsMortgage = true;
    }
    //DISENCUMBER PROPERTY
    public void DisencumberProperty(Properties prop)
    {
        DebitPlayer(prop.Price / 2);
        prop.IsMortgage = false;
    }
    //ToCheckIsGroupIsFull
    public bool isGroupFull(Properties prop)
    {
        throw new NotImplementedException();
    }

    //HANDLER
    public bool isBuildable(Properties prop)
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
    public int hasHouse(Properties prop)
    {
        return prop.UpgradeTier;
    }
    //BUY HOUSE
    public void AddHouse(Properties prop, int num)
    {
        for( int i = 0; i< num; i++)
            prop.addHouse();
        DebitPlayer(prop.GetHousePrice * num);
    }
    //SELL HOUSE
    public void RemoveHouse(Properties prop, int num)
    {
        for (int i = 0; i < num; i++)
            prop.removeHouse();
        CreditPlayer(prop.GetHousePrice * num / 2);
    }

    public void SetPrisonner()
    {
        IsInPrison = 3;
        id_case = 10;
    }



    void Awake()
    {
        id_case = 0;
        money = 15000;
        properties = new List<Properties>();
        isInPrison = 0;
    }
}
