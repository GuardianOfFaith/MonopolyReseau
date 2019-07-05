//Florent WASSEN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : Photon.PunBehaviour {

    [SerializeField]
    private string currentVersion;

    [SerializeField]
    private string playerNamePrefsKey;
    [SerializeField]
    public UnityEngine.UI.InputField playerNameInputField;

    //CreateMenu
    [SerializeField]
    private UnityEngine.UI.Toggle PrivateToggle;

    [SerializeField]
    private UnityEngine.UI.Toggle JoinExistToggle;

    [SerializeField]
    private UnityEngine.UI.InputField RoomNameInputField;

    [SerializeField]
    private UnityEngine.UI.Dropdown MaxPlayerDropdown;

    [SerializeField]
    private UnityEngine.UI.InputField MazeSizeInputField;

    private GameObject currentPanel;
    public GameObject ConnexionPanel;
    public GameObject MainPanel;
    public GameObject CreateRoomPanel;
    public GameObject JoinRoomPanel;
    public GameObject LoadingPanel;
    public GameObject InRoomPanel;

    [SerializeField]
    private GameObject messageBox;

    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 1.0f;
        if(PhotonNetwork.offlineMode)
            SetCurrentPanel(MainPanel);
        else
            SetCurrentPanel(ConnexionPanel);    // Start with this panel

        if(PlayerPrefs.HasKey(playerNamePrefsKey))
        {
            string playerName = PlayerPrefs.GetString(playerNamePrefsKey);
            PhotonNetwork.playerName = playerName;
            playerNameInputField.text = playerName;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Pas besoin du lobby au début, uniquement pour avoir la liste des parties
        PhotonNetwork.autoJoinLobby = false;

        // PhotonNetwork.LoadLevel() sur le MasterClient change la map pour tout les joueurs
        PhotonNetwork.automaticallySyncScene = true;

        currentPanel = ConnexionPanel;  //Make Something currentPanel
        //Deactive All Panel
        foreach (Transform tr in transform)
        {
            tr.gameObject.SetActive(false);
        }
    }

    //For All Panel
    void SetCurrentPanel(GameObject panelToActivate)
    {
        currentPanel.SetActive(false);
        currentPanel = panelToActivate;
        currentPanel.SetActive(true);
    }

    void ShowMsgBox(string msg)
    {
        messageBox.GetComponentInChildren<UnityEngine.UI.Text>().text = msg;
        messageBox.SetActive(true);
    }

    void OnGUI()
    {
        //GUI.color = Color.black;

        //Show Connexion State
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(Screen.width / 2 - 400 / 2, Screen.height - 21, 400, 21), PhotonNetwork.connectionStateDetailed.ToString());

        if(currentPanel == ConnexionPanel || currentPanel == MainPanel) {
            // Show Version
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.Label(new Rect(3, Screen.height - 21, 400, 21), "Ver: "+currentVersion);
        }
        if(currentPanel == MainPanel)
        {
            //Show Number of Current Player connected To master
            GUI.skin.label.alignment = TextAnchor.MiddleRight;
            GUI.Label(new Rect(Screen.width-400-3, Screen.height - 21, 400, 21), "Nombre de joueur(s) sur le jeu: " + PhotonNetwork.countOfPlayers+"/"+20);

            GUI.Label(new Rect(Screen.width-400-3, Screen.height - 21*2+5, 400, 21), "Nombre de partie(s): " + PhotonNetwork.countOfRooms);
        }
        if(currentPanel == InRoomPanel)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleRight;
            GUI.Label(new Rect(Screen.width - 400 - 3, Screen.height - 21, 400, 21), "Nombre de joueur(s) dans la partie: " + PhotonNetwork.room.PlayerCount + "/" + (PhotonNetwork.room.MaxPlayers>0 ? PhotonNetwork.room.MaxPlayers.ToString() : "20"));
        }
    }

    // CreateRoomPanel and JoinRoomPanel and OptionsPanel and ConnexionPanel
    public void GoToMainMenu()
    {
        SetCurrentPanel(MainPanel);
    }

    // Only For Connexion Panel
    public void SetPlayerName(string playerName)
    {
        PhotonNetwork.playerName = playerName;
        PlayerPrefs.SetString(playerNamePrefsKey, playerName);
    }

    public void Connect()
    {
        if (PhotonNetwork.playerName.Length < 1) return;

        SetCurrentPanel(LoadingPanel);

        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(currentVersion);
    }

    public void Quit()
    {
        Application.Quit();
    }

    // MainPanel
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void GoToCreateRoom()
    {
        SetCurrentPanel(CreateRoomPanel);
    }

    public void GoToJoinRoom()
    {
        SetCurrentPanel(LoadingPanel);
        PhotonNetwork.JoinLobby();
    }

    // CreateRoomPanel
    public void OpenRoom()
    {
        string roomName = RoomNameInputField.text;
        if (roomName.Length < 1) return;

        RoomOptions ro = new RoomOptions();
        ro.IsVisible = !PrivateToggle.isOn;//!isPrivate;
        ro.IsOpen = true;
        ro.MaxPlayers = (byte)(MaxPlayerDropdown.value+2);  // Use MaxPlayer in dropDown
        ro.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();

        if (MazeSizeInputField.text.Length > 0 && int.Parse(MazeSizeInputField.text) >= 2) //2 = nombre détage
        {
            ro.CustomRoomProperties.Add("mazeSize", int.Parse(MazeSizeInputField.text));
        }

        if(!JoinExistToggle.isOn) {
            if(PhotonNetwork.CreateRoom(roomName, ro, TypedLobby.Default))
            {

            }
            else
                ShowMsgBox("Impossible de crée la room");
        }
        else
        {
            if (PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default))
            {

            }
            else
                ShowMsgBox("Impossible de crée ou rejoindre la room");
        }
    }

    // JoinRoomPanel
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeftLobby()
    {
        SetCurrentPanel(LoadingPanel);
        PhotonNetwork.LeaveLobby();
    }

    // Photon Callbacks
    public override void OnConnectedToMaster()
    {
        GoToMainMenu();
    }

    public override void OnDisconnectedFromPhoton()
    {
        SetCurrentPanel(ConnexionPanel);
        //Afficher un message
        ShowMsgBox("Vous avez été déconnecté de Photon");
    }

    public override void OnJoinedRoom()
    {
        SetCurrentPanel(InRoomPanel);
    }

    public override void OnLeftRoom()
    {
        SetCurrentPanel(MainPanel);
    }

    public override void OnJoinedLobby()
    {
        SetCurrentPanel(JoinRoomPanel);
    }

    public override void OnLeftLobby()
    {
        GoToMainMenu();
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        ShowMsgBox("Connexion à Photon échouer");
        print(cause.ToString());
    }

    public override void OnPhotonMaxCccuReached()
    {
        //Afficher un message
        ShowMsgBox("Nombre d'utilisateur dépasser");
        //Just for moment
        print("MaxCCUReached");
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        ShowMsgBox("Création de la partie échouer");
        print(codeAndMsg.ToString());
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        ShowMsgBox("Impossible de rejoindre la partie\n"+"Quelqu'un avec le meme pseudo ?");
        print(codeAndMsg.ToString());
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        ShowMsgBox("Impossible de trouver une partie");
        print(codeAndMsg.ToString());
    }

    //PhotonNetwork.ReconnectAndRejoin();
}
