using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    
    public Propriete[] Case= new Propriete[40];
    public static Board instance;
    void Start()
    {
        instance = this;
        Case = transform.GetComponentsInChildren<Propriete>();
        foreach (var propriete in Case.OrderBy(propriete => propriete.id));
    }

    public Propriete getProprieter(int i)
    {
        return Case[i];
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
