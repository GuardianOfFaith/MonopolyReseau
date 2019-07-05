using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Carte : MonoBehaviour
{
    
    public List<EffetCarte> CartesChance = new List<EffetCarte>();
    public List<EffetCarte> CartesCommunaute = new List<EffetCarte>();
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Text;
    
    public void Creer()
    {
        for (int i = 0; i < 16; i++)
        {
            CartesChance.Add(new EffetCarte(EffetCarte.CarteType.Chance));
        }
        for (int i = 0; i < 16; i++)
        {
            CartesCommunaute.Add(new EffetCarte(EffetCarte.CarteType.Communauté));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DrawChance();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            DrawCommunauté();
        }
    }

    public void DrawChance()
    {
        EffetCarte et = CartesChance[0];
        CartesChance.Remove(CartesChance[0]);
        CartesChance.Add(CartesChance[0]);

        Title.text = et.Title;
        Text.text = et.Text;

        //EFFET
    }
    
    public void DrawCommunauté()
    {
        EffetCarte et = CartesCommunaute[0];
        CartesCommunaute.Remove(CartesCommunaute[0]);
        CartesCommunaute.Add(CartesCommunaute[0]);

        Title.text = et.Title;
        Text.text = et.Text;

        //EFFET
    }

}

public class EffetCarte
{
    public enum CarteType
    {
        Chance,
        Communauté
    };

    public string Title;
    public string Text;
    public int amount;
    
    public CarteType Type;

    public enum Effet
    {
       Bouger,
       Payer,
       Recevoir,
       Prison
    }

    public Effet effet;
    
    public EffetCarte(CarteType ct)
    {
        Type = ct;
        if (Type == CarteType.Communauté)
        {
            Title = "Vous recevez de l'argent";
            amount = Random.Range(10,51);
            amount = amount * 1000;
            Text = "Recevez " + amount;
        }
        else
        {
            int rand = Random.Range(0, 4);
            switch (rand)
            {
                case 0:
                    effet = Effet.Bouger;
                    Title = "Deplacement forcer";
                    int id = Random.Range(0, 40);
                    
                    Text = "Vous partez pour " + GameManager.instance.gs.Case[id].name;
                    amount = id;
                    break;
                case 1:
                    effet = Effet.Payer;
                    Title = " Visite de l'inspecteur des impots";
                    amount = Random.Range(10,31);
                    amount = amount * 1000;
                    Text = " Vous n'avez pas déclarer quelque chose, payez " + amount;
                    break;
                case 2:
                    effet = Effet.Recevoir;
                    Title = " Lotterie ";
                    amount = Random.Range(10,31);
                    amount = amount * 1000;
                    Text = " Vous gagnez " + amount + " à la lotterie";
                    break;
                case 3:
                    Title = "Detournement de fond";
                    Text = "Vous vous faite attrapé pour detournement de fond, allé en prison 3 tour";
                    break;
            }
        }
    }
}
