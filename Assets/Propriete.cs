using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propriete : MonoBehaviour
{
    // Start is called before the first frame update
    public string name;
    public int id;
    public int Thunes;
    public int Maison = 0;
    public Transform ZoneMaison;
    public bool RueMaison = false;
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
    public Material CouleurZone ;
    public GameObject PrefabMaison;
    public GameObject PrefabHotel;
    private List<GameObject> MaisonInst = new List<GameObject>();
    public void CreerMaison()
    {
        
        if (Maison == 4)
        {
            foreach (GameObject g in MaisonInst)
            {
                g.SetActive(false);
            }
            GameObject maison = Instantiate(PrefabHotel, ZoneMaison);
            maison.GetComponent<Renderer>().material = CouleurZone;
        }
        else if(Maison <4)
        {
            GameObject maison = Instantiate(PrefabMaison, ZoneMaison);
            maison.transform.localPosition += new Vector3(-30 + (20 * Maison), 0, 0);
            maison.GetComponent<Renderer>().material = CouleurZone;
            MaisonInst.Add(maison);
            
        }
        Maison++;
    }
    
    void Start()
    {
        name = gameObject.name;
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
    void Update()
    {
        
    }
}
