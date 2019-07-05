//Florent WASSEN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoomScript : Photon.PunBehaviour {

    [SerializeField]
    private int minNbPlayerToLaunch;

    [SerializeField]
    private UnityEngine.UI.Button LaunchButton;

    [SerializeField]
    private Transform containerPlayersList;

    [SerializeField]
    private GameObject PlayerItemPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Quand on arrive dans le menu
    public void OnEnable()
    {
        if (PhotonNetwork.isMasterClient) //PhotonNetwork.player == newMasterClient
        {
            LaunchButton.gameObject.SetActive(true);
        }
        else
        {
            LaunchButton.gameObject.SetActive(false);
        }

        RefreshPlayersList();
    }

    public void Launch()
    {
        PhotonNetwork.room.IsOpen = false;
        
        ExitGames.Client.Photon.Hashtable setRoomProperties = new ExitGames.Client.Photon.Hashtable();
        setRoomProperties.Add("seed", Random.Range(0,9999));
        PhotonNetwork.room.SetCustomProperties(setRoomProperties);

        PhotonNetwork.LoadLevel("SampleScene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void GoToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if(PhotonNetwork.isMasterClient) //PhotonNetwork.player == newMasterClient
        {
            LaunchButton.gameObject.SetActive(true);
        }
        else
        {
            LaunchButton.gameObject.SetActive(false);
        }
    }

    void RefreshPlayersList()
    {
        // Delete all players in the list
        foreach (Transform tr in containerPlayersList)
        {
            Destroy(tr.gameObject);
        }

        // Add all players in the list
        foreach (PhotonPlayer pPlayer in PhotonNetwork.playerList)
        {
            GameObject playerItem = Instantiate(PlayerItemPrefab, containerPlayersList);
            UnityEngine.UI.Text text = playerItem.GetComponentInChildren<UnityEngine.UI.Text>();
            text.text = pPlayer.NickName;

            if (pPlayer.IsMasterClient)
                text.color = Color.yellow;
            else if (pPlayer.IsLocal)
                text.color = Color.blue;
            else
                text.color = Color.white;

        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        //Add in chat newPlayer is connected
        RefreshPlayersList();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        //Add in chat otherPlayer leave the room
        RefreshPlayersList();
    }
}
