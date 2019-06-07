using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour
{
    string casename;
    int id;
    int price;
    public enum group {purple, turquoise, magenta, orange, red, yellow, green, blue, station, company, luck, community, taxes, park, gotoprison, prison, start}
    group caseGroup;
    bool isMortgage;
    int basePrice;
    int upgradeTier;
    int housePrice;

    public string Name
    {
        get
        {
            return casename;
        }
    }
    public int Id
    {
        get { return id; }
    }
    public group Groupe
    {
        get { return caseGroup; }
    }
    public bool IsMortgage
    {
        get { return IsMortgage; }
        set { isMortgage = value; }
    }
    //price to buy this property
    public int Price
    {
        get { return price; }
    }
    //price went you end on this case
    public int getPrice(bool groupFull)
    {
        if(caseGroup == group.station)
        {
            return basePrice * (int)Mathf.Pow(2, upgradeTier);
        }
        if(caseGroup == group.company)
        {
            return basePrice * upgradeTier /** ActivePlayer.diceRoll*/;
        }
        switch (upgradeTier)
        {
            case 0:
                if (groupFull)
                {
                    return basePrice * 2;
                }
                else return basePrice;
            case 1:
                return basePrice * 5;
            case 2:
                return basePrice * 15;
            case 3:
                return basePrice * 45;
            case 4:
                return basePrice * 60;
            case 5:
                return basePrice * 70;
        }
        return basePrice;
    }

    public int UpgradeTier
    {
        get { return upgradeTier; }
    }
    public void addHouse()
    {
        upgradeTier++;
    }
    public void removeHouse()
    {
        upgradeTier--;
    }

    public int GetHousePrice
    {
        get { return housePrice; }
    }
}
