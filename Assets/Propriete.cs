using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Propriete : MonoBehaviour
{
    public string Nom;
    public int id;
    public int Prix;
    public int Tier = 0;
    public Transform ZoneMaison;
    public bool RueMaison = false;
    public int Owner = 0;
    public TextMeshProUGUI ownerName;
    public enum TypeCase
    {
        Depart,
        Violet,
        BleuClair,
        Pourpre,
        Orange,
        Rouge,
        Jaune,
        Vert,
        BleuFonce,
        Gare,
        Public,
        Chance,
        Communauté,
        Parc,
        Prison,
        Allez_en_Prison,
        Taxe
    };
    public TypeCase Type;
    int housePrice;
    bool isMortgage;
    
    //AFFICHAGE
    public Material CouleurZone ;
    public GameObject PrefabMaison;
    public GameObject PrefabHotel;
    private List<GameObject> MaisonInst = new List<GameObject>();
    public int instantier=0;
    public GameObject Hotel=null;
    
    public void CreerMaison()
    {
        if (Tier > 4)
        {
            return;
        }
        if (Tier == 4)
        {
            foreach (GameObject g in MaisonInst)
            {
                g.SetActive(false);
            }

            if (Hotel == null)
            {
                GameObject maison = Instantiate(PrefabHotel, ZoneMaison);
                maison.GetComponent<Renderer>().material = CouleurZone;
                Hotel = maison;
            }
            else
            {
                Hotel.SetActive(true);
            }
            
        }
        else if(Tier <4)
        {
            if (instantier==Tier)
            {
                GameObject maison = Instantiate(PrefabMaison, ZoneMaison);
                maison.transform.localPosition += new Vector3(-30 + (20 * Tier), 0, 0);
                maison.GetComponent<Renderer>().material = CouleurZone;
                MaisonInst.Add(maison);
                instantier++;
            }
            else
            {
                MaisonInst[Tier].SetActive(true);
            }
        }
        Tier++;
    }
    
    public void RetirerMaison()
    {

        if (Tier == 0)
        {
            return;
        }
        if (Tier > 4)
        {
            foreach (GameObject g in MaisonInst)
            {
                g.SetActive(true);
            }
            Hotel.SetActive(false);
        }
        else if(Tier <=4 && Tier >0)
        {
            MaisonInst[Tier-1].SetActive(false);      
        }
        Tier--;
    }
    
    void Start()
    {
        Nom = gameObject.name;
        PrefabMaison = Resources.Load<GameObject>("Maison");
        PrefabHotel = Resources.Load<GameObject>("Hotel");
 
        switch (Type)
        {
            case TypeCase.Violet:
                CouleurZone = Resources.Load<Material>("Z1");
                RueMaison = true;
                ZoneMaison = transform.GetChild(0);
                break;
            case TypeCase.BleuClair:
                CouleurZone = Resources.Load<Material>("Z2");
                RueMaison = true;
                ZoneMaison = transform.GetChild(0);
                break;
            case TypeCase.Pourpre:
                CouleurZone = Resources.Load<Material>("Z3");
                RueMaison = true;
                ZoneMaison = transform.GetChild(0);
                break;
            case TypeCase.Orange:
                CouleurZone = Resources.Load<Material>("Z4");
                RueMaison = true;
                ZoneMaison = transform.GetChild(0);
                break;
            case TypeCase.Rouge:
                CouleurZone = Resources.Load<Material>("Z5");
                RueMaison = true;
                ZoneMaison = transform.GetChild(0);
                break;
            case TypeCase.Jaune:
                CouleurZone = Resources.Load<Material>("Z6");
                RueMaison = true;
                ZoneMaison = transform.GetChild(0);
                break;
            case TypeCase.Vert:
                CouleurZone = Resources.Load<Material>("Z7");
                RueMaison = true;
                ZoneMaison = transform.GetChild(0);
                break;
            case TypeCase.BleuFonce:
                CouleurZone = Resources.Load<Material>("Z8");
                RueMaison = true;
                ZoneMaison = transform.GetChild(0);
                break;
            
        }
    }

    // Update is called once per frame
    public string Name
    {
        get
        {
            return name;
        }
    }
    public int Id
    {
        get { return id; }
    }
    public TypeCase type
    {
        get { return Type; }
    }
    public bool IsMortgage
    {
        get { return IsMortgage; }
        set { isMortgage = value; }
    }
    //price to buy this property
    public int prix
    {
        get { return Prix; }
    }
    //price went you end on this case
    public int getPrice(bool groupFull)
    {
        if(type == TypeCase.Gare)
        {
            return Prix * (int)Mathf.Pow(2, Tier);
        }
        if(type == TypeCase.Public)
        {
            return Prix * Tier /** ActivePlayer.diceRoll*/;
        }
        switch (Tier)
        {
            case 0:
                if (groupFull)
                {
                    return Prix * 2;
                }
                else return Prix;
            case 1:
                return Prix * 5;
            case 2:
                return Prix * 15;
            case 3:
                return Prix * 45;
            case 4:
                return Prix * 60;
            case 5:
                return Prix * 70;
        }
        return Prix;
    }

    public int UpgradeTier
    {
        get { return Tier; }
    }
    public void addHouse()
    {
        CreerMaison();
    }
    public void removeHouse()
    {
        Tier--;
    }

    public int GetHousePrice
    {
        get { return housePrice; }
    }
}
