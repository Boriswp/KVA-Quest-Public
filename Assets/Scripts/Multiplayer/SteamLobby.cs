using Mirror;
using Org.BouncyCastle.Tls;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MyNetworkManager;

public class SteamLobby : MonoBehaviour { 

    private NetworkManager networkManager;

    public GameObject MenuUi = null;
    public GameObject LobbyUI = null;
    public GameObject invitedLobbyUI = null;

    public TextMeshProUGUI playerHostName;
    public TextMeshProUGUI playerClientName;

    public TextMeshProUGUI playerInvHostName;
    public TextMeshProUGUI playerInvClientName;

    public GameObject startButton;

    public GameObject inviteButton;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequest;
    protected Callback<LobbyEnter_t> lobbyEnter;
    protected Callback<LobbyDataUpdate_t> dataUpdate;

    private ulong steamId;

    private const string HostAddressKey = "HostAdress";

    private void Awake()
    {
        onPLayerConnect += OnPlayerConnect;
        onPLayerDisconnect += OnPlayerDisconnect;
    }

    private void OnDisable()
    {
        onPLayerConnect -= OnPlayerConnect;
        onPLayerDisconnect -= OnPlayerDisconnect;

    }

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        if (!SteamManager.Initialized) { return; }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
    }


    private void SetLobbyUi() {
        MenuUi.SetActive(false);
        LobbyUI.SetActive(true);
    }

    private void SetInviteLobbyUi()
    {
        MenuUi.SetActive(false);
        invitedLobbyUI.SetActive(true);
    }

    private void SetMenu()
    {
        MenuUi.SetActive(true);
        LobbyUI.SetActive(false);
    }

    public void ExitHost() {
        NetworkManager.singleton.StopHost();
        MenuUi.SetActive(true);
        LobbyUI.SetActive(false);
    }

    public void ExitLobby() {
        NetworkManager.singleton.StopClient();
        MenuUi.SetActive(true);
        invitedLobbyUI.SetActive(false);
    }

    public void OpenFriendsMenu() {
        SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(steamId));
    }

    private void OnPlayerConnect() 
    {
        var steamIdLobby = new CSteamID(steamId);
        var lobbyCount = SteamMatchmaking.GetNumLobbyMembers(steamIdLobby);
        if (lobbyCount > 1)
        {
            var userId = SteamMatchmaking.GetLobbyMemberByIndex(steamIdLobby, 1);
            playerClientName.text = SteamFriends.GetFriendPersonaName(userId);
            startButton.SetActive(true);
            inviteButton.SetActive(false);
        }
    }

    private void OnPlayerDisconnect()
    {
        startButton.SetActive(false);
        inviteButton.SetActive(true);
        playerClientName.text = "Waiting...";
    }



    private void OnLobbyCreated(LobbyCreated_t callback) {

        if (callback.m_eResult != EResult.k_EResultOK) {
            SetMenu();
            return;
        }
        steamId = callback.m_ulSteamIDLobby;
        networkManager.StartHost();
        var name = SteamFriends.GetPersonaName().ToString();
        playerHostName.text = name;
        SteamMatchmaking.SetLobbyData(new CSteamID(steamId), HostAddressKey,SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(steamId), "name", name);
   
    }

    public void HostLobby() {
        SetLobbyUi();
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback) {

        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);

    }


    private void OnLobbyEnter(LobbyEnter_t callback) {

        if (NetworkServer.active) {
            return;
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby),HostAddressKey);
        playerInvHostName.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");
        var name = SteamFriends.GetPersonaName().ToString();
        playerInvClientName.text = name;
        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
        SetInviteLobbyUi();
    }


}
