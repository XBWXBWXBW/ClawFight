using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;
using message;
using UnityEngine;
using UnityEngine.UI;
public class RoomView : ViewBase
{
    public Text infoText;
    public GameObject teamAButton, teamBButton;
    StringBuilder sb = new StringBuilder();
    const string formatString = "PlayerID: {0}  PlayerTeam: {1}  IsReady: {2}";
    const string teamA = "TeamA", teamB = "TeamB", none = "NoTeam";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        EventManager.instance.RegistEvent(HallEvents.HALLEVENT_OTHER_PLAYER_IN_ROOM, OnPlayerJoinRoom);
        EventManager.instance.RegistEvent(HallEvents.HALLEVENT_PLAYER_JOIN_TEAM, OnPlayerJoinTeam);
        EventManager.instance.RegistEvent(HallEvents.HALLEVENT_PLAYER_READY_TO_PLAY, OnPlayerReadyToPlay);
    }
    private void OnDisable()
    {
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_OTHER_PLAYER_IN_ROOM, OnPlayerJoinRoom);
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_PLAYER_JOIN_TEAM, OnPlayerJoinTeam);
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_PLAYER_READY_TO_PLAY, OnPlayerReadyToPlay);
    }

    private void OnPlayerReadyToPlay()
    {
        UpdateInfo();
        teamAButton.SetActive(false);
        teamBButton.SetActive(false);
    }

    private void OnPlayerJoinTeam()
    {
        UpdateInfo();
    }

    private void OnPlayerJoinRoom()
    {
        UpdateInfo();
    }
    private void UpdateInfo() {
        sb.Length = 0;
        PlayerData mainPlayerData = PlayerManager.instance.mainPlayer.playerData;
        if (mainPlayerData.eTeam == ETeam.None)
        {
            sb.Append(string.Format(formatString, "Self", none, mainPlayerData.isReady));
        }
        else if (mainPlayerData.eTeam == ETeam.TeamA)
        {
            sb.Append(string.Format(formatString, "Self", teamA, mainPlayerData.isReady));
        }
        else
        {
            sb.Append(string.Format(formatString, "Self", teamB, mainPlayerData.isReady));
        }
        sb.Append('\n');

        foreach (var e in PlayerManager.instance.playerInRoom_Dict)
        {
            PlayerData pd = e.Value.playerData;
            if (pd.ID == mainPlayerData.ID)
            {
                continue;
            }
            if (pd.eTeam == ETeam.None)
            {
                sb.Append(string.Format(formatString, pd.ID, none, pd.isReady));
            }
            else if (pd.eTeam == ETeam.TeamA)
            {
                sb.Append(string.Format(formatString, pd.ID, teamA, pd.isReady));
            }
            else
            {
                sb.Append(string.Format(formatString, pd.ID, teamB, pd.isReady));
            }
            sb.Append('\n');
        }
        infoText.text = sb.ToString();
    }
    public void JoinTeamA()
    {
        JoinTeam(ETeam.TeamA);
    }
    public void JoinTeamB() {
        JoinTeam(ETeam.TeamB);
    }
    public void JoinTeam(ETeam eTeam) {
        CSP_JoinTeam msg = new CSP_JoinTeam();
        msg.PlayerID = PlayerManager.instance.mainPlayer.playerData.ID;
        msg.Team = (int)eTeam;
        ConnectManager.instance.connectProxy.SendMessage(EMessageType.CSP_JoinTeam, msg);
    }
    public void ReadyToPlay_Yes_No() {
        PlayerData pd = PlayerManager.instance.mainPlayer.playerData;
        if (pd.eTeam == ETeam.None) {
            return;
        }
        
        CSP_ReadyToPlay msg = new CSP_ReadyToPlay();
        msg.PlayerID = pd.ID;
        ConnectManager.instance.connectProxy.SendMessage(EMessageType.CSP_ReadyToPlay, msg);
    }
    
}
