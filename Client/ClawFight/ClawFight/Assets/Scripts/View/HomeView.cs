using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeView : ViewBase
{
    public GameObject startConnectButton;
    public GameObject waitConnect;
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
    }
    public void StartConnect() {
        ConnectManager.instance.Connect();
        startConnectButton.SetActive(false);
        waitConnect.SetActive(true);
    }
}
