using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerPref : MonoBehaviour
{
    public int IDCase = 0;
    public Propriete Case;
    public void start()
    {
        Case = Board.instance.getProprieter(IDCase);
        transform.SetParent(Case.transform);
        transform.localScale = new Vector3(10,50,10);
        switch (name)
        {
            case "p1":
                transform.localPosition = new Vector3(-20, 0, 0);
                break;
            case "p2":
                transform.localPosition = new Vector3(-20, -30, 0);
                break;
            case "p3":
                transform.localPosition = new Vector3(20, 0, 0);
                break;
            case "p4":
                transform.localPosition = new Vector3(20, -30, 0);
                break; 
        }
    }

    public int move(int movement)
    {
        IDCase = (IDCase + movement) % 40;
        Case = Board.instance.getProprieter(IDCase);
        if (Case.Type == Propriete.TypeCase.Prison)
        {
            transform.SetParent(Case.transform.GetChild(0));
            switch (name)
            {
                
                case "p1":
                    transform.localPosition = new Vector3(-60, 0, 0);
                    break;
                case "p2":
                    transform.localPosition = new Vector3(-30, 0 , 0);
                    break;
                case "p3":
                    transform.localPosition = new Vector3(0, 0, 0);
                    break;
                case "p4":
                    transform.localPosition = new Vector3(30, 0, 0);
                    break; 
            }
        }
        else if (Case.Type == Propriete.TypeCase.Allez_en_Prison)
        {
            AllerEnPrison();
        }
        else
        {
            transform.SetParent(Case.transform);
            switch (name)
            {
                case "p1":
                    transform.localPosition = new Vector3(-20, 0, 0);
                    break;
                case "p2":
                    transform.localPosition = new Vector3(-20, -30, 0);
                    break;
                case "p3":
                    transform.localPosition = new Vector3(20, 0, 0);
                    break;
                case "p4":
                    transform.localPosition = new Vector3(20, -30, 0);
                    break; 
            }
        }
        
        return IDCase;
    }

    public void AllerEnPrison()
    {
        IDCase = 10;
        Case = Board.instance.getProprieter(IDCase);
        transform.SetParent(Case.transform.GetChild(2));
        switch (name)
        {
            case "p1":
                transform.localPosition = new Vector3(-20, 0, 0);
                break;
            case "p2":
                transform.localPosition = new Vector3(-20, -30, 0);
                break;
            case "p3":
                transform.localPosition = new Vector3(20, 0, 0);
                break;
            case "p4":
                transform.localPosition = new Vector3(20, -30, 0);
                break; 
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            move(Random.Range(1, 7));
            //AllerEnPrison();
        }
    }
}
