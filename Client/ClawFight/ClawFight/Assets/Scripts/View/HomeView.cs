using message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeView : ViewBase
{
    public GameObject startConnectButton;
    public GameObject waitConnect;
    public GameObject joinRoomButton;
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
        EventManager.instance.RegistEvent(HallEvents.HALLEVENT_CONNECTED, OnConnected);
    }
    private void OnDisable()
    {
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_CONNECTED, OnConnected);
    }
    void OnConnected() {
        waitConnect.SetActive(false);
        joinRoomButton.SetActive(true);
    }
    public void StartConnect() {
        GameManager.instance.connectProxy.Connect();
        startConnectButton.SetActive(false);
        waitConnect.SetActive(true);
    }
    public void JoinRoom()
    {
        CSP_JoinRoom msg = new CSP_JoinRoom();
        msg.PlayerID = PlayerManager.instance.mainPlayer.playerData.ID;
        GameManager.instance.connectProxy.SendMessage(EMessageType.CSP_JoinRoom, msg);

        joinRoomButton.SetActive(false);

        RoomView roomView = ViewManager.instance.GetView(EViewType.RoomView) as RoomView;
        roomView.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
