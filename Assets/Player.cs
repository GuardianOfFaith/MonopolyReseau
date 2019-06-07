using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int id;
    int id_case;
    int money;
    List<Properties> properties { get; }
    //List<int> cards { get; }
    bool isInPrison { get; }

    public int Id
    {
        get { return id; }
    }

    public int Id_case
    {
        get { return id_case; }
    }

    public int Money
    {
        get { return money; }
    }

    //MOVE PLAYER
    public void move(int value)
    {
        throw new NotImplementedException();
        id_case = Id_case + value;
        if (Id_case > 39) //MAX CASE NUMBER //TODO
        {
            CreditPlayer(20000); //TODO
            id_case = Id_case % 39; //TODO
        }

        //CALL EVENT
        //TODO
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
}
