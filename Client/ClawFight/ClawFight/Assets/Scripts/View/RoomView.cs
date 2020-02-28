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
    StringBuilder sb = new StringBuilder();
    const string formatString = "PlayerID: {0}  PlayerTeam: {1}";
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
    }
    private void OnDisable()
    {
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_OTHER_PLAYER_IN_ROOM, OnPlayerJoinRoom);
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_PLAYER_JOIN_TEAM, OnPlayerJoinTeam);
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
            sb.Append(string.Format(formatString, "Self", none));
        }
        else if (mainPlayerData.eTeam == ETeam.TeamA)
        {
            sb.Append(string.Format(formatString, "Self", teamA));
        }
        else
        {
            sb.Append(string.Format(formatString, "Self", teamB));
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
                sb.Append(string.Format(formatString, pd.ID, none));
            }
            else if (pd.eTeam == ETeam.TeamA)
            {
                sb.Append(string.Format(formatString, pd.ID, teamA));
            }
            else
            {
                sb.Append(string.Format(formatString, pd.ID, teamB));
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
        GameManager.instance.connectProxy.SendMessage(EMessageType.CSP_JoinTeam, msg);
    }
    public void Ready_Yes_No() {

    }
    
}
