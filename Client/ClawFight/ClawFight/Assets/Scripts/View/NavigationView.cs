using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationView : ViewBase
{
    public Text IDText;
    public Text pingText;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        EventManager.instance.RegistEvent(HallEvents.HALLEVENT_SYNCINFO, OnSyncInfo);
    }
    private void OnDisable()
    {
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_SYNCINFO, OnSyncInfo);
    }
    // Update is called once per frame
    void Update()
    {
        pingText.text = GameManager.instance.pingTime.ToString();
    }
    void OnSyncInfo() {
        IDText.text = PlayerManager.instance.mainPlayer.playerData.ID.ToString();
    }
}
