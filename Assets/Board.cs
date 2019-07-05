using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{

    
    public Propriete[] Case= new Propriete[40];
    public static Board instance;
    public List<playerPref> Players = new List<playerPref>();
    
    
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
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            int[] temp = new int[4];
            for (int i = 0; i < 4; i++)
            {
                temp[i]=Players[i].IDCase;
            }
                
            
            int[] Temp = new int[40];
            for (int i = 0; i < 40; i++)
            {
                Temp[i]=Case[i].Maison;
            }
            
            stream.SendNext(temp);
            stream.SendNext(Temp);
        }
        else
        {
            int[] temp = (int[])stream.ReceiveNext();
            int[] Temp = (int[])stream.ReceiveNext();
            Debug.Log(Temp[5]);
            
            for (int i = 0; i < 4; i++)
            {
                
                Players[i].move(Case[temp[i]]);
            }

            for (int i = 0; i < Temp.Length; i++)
            {
                if (Temp[i] > Case[i].Maison)
                {
                    int k = Temp[i] - Case[i].Maison;
                    for (int j = 1; j <= k; j++)
                    {
                        Case[i].CreerMaison();
                    }
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
