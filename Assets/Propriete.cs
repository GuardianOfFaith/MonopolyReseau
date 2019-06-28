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
        Taxe
    };

    public TypeCase Type;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
