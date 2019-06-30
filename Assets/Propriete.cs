using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propriete : MonoBehaviour
{
    // Start is called before the first frame update
    public string name;
    public int id;
    public int Thunes;

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
    
    
    void Start()
    {
        name = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
