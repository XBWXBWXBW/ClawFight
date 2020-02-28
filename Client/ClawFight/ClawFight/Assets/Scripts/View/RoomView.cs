using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;
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
    }
    private void OnDisable()
    {
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_OTHER_PLAYER_IN_ROOM, OnPlayerJoinRoom);
    }

    private void OnPlayerJoinRoom()
    {
        sb.Length = 0;
        foreach (var e in PlayerManager.instance.playerInRoom_Dict) {
            PlayerData pd = e.Value.playerData;
            if (pd.eTeam == ETeam.None) {
                sb.Append(string.Format(formatString, pd.ID, none));
            } else if (pd.eTeam == ETeam.TeamA) {
                sb.Append(string.Format(formatString, pd.ID, teamA));
            }
            else {
                sb.Append(string.Format(formatString, pd.ID, teamB));
            }
            sb.Append('\n');
        }
        infoText.text = sb.ToString();
    }

    public void JoinTeamA()
    {

    }
    public void JoinTeamB() {

    }
    public void Ready_Yes_No() {

    }
    
}
