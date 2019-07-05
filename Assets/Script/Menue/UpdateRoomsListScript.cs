//Florent WASSEN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRoomsListScript : Photon.PunBehaviour {

    [SerializeField]
    private Transform containerRoomsList;

    [SerializeField]
    private GameObject RoomItemPrefab;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnReceivedRoomListUpdate()
    {
        //Update the list
        //Delete all Child
        /*for(int i= containerParent.childCount-1; i>=0; i--)
        {
            Destroy(containerParent.GetChild(i).gameObject);
        }*/

        foreach(Transform tr in containerRoomsList)
        {
            Destroy(tr.gameObject);
        }

        RoomInfo[] roomsInfo = PhotonNetwork.GetRoomList();
        foreach(RoomInfo ri in roomsInfo)
        {
            GameObject roomItem = Instantiate(RoomItemPrefab, containerRoomsList);
            roomItem.GetComponentInChildren<UnityEngine.UI.Text>().text = ri.Name+" ("+ri.PlayerCount+"/"+(ri.MaxPlayers==0 ? 20 : ri.MaxPlayers)+")";
            if (ri.IsOpen)
                roomItem.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(delegate { PhotonNetwork.JoinRoom(ri.Name); });
            else
                Destroy(roomItem.GetComponentInChildren<UnityEngine.UI.Button>().gameObject);
        }

    }
}
