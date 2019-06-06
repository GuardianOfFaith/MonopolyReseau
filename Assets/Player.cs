using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int id;
    int id_case;
    int money;
    //List<int> properties { get; }
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
    public void AddProperty(int prop)
    {
        throw new NotImplementedException();
        //properties.Add(prop);
    }
    public void SellProperty(int prop)
    {
        throw new NotImplementedException();
        //properties.Remove(prop);
    }

    //MORTGAGE PROPERTY
    //DISENCUMBER PROPERTY

    //HANDLER
    public bool isBuildable(int prop)
    {
        throw new NotImplementedException();
    }
    public int hasHouse(int prop)
    {
        throw new NotImplementedException();
    }
    //BUY HOUSE
    public void AddHouse(int prop, int num)
    {
        throw new NotImplementedException();
    }
    //SELL HOUSE
    public void RemoveHouse(int prop, int num)
    {
        throw new NotImplementedException();
    }
}
