using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using message;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public List<GameObject> donotDestroy = new List<GameObject>();
    public List<string> mapPathList = new List<string>();
    public static GameManager instance;
    public MatchProxy matchProxy;
    public ConnectProxy connectProxy;
    public List<IEnumerator> tasks = new List<IEnumerator>();
    public List<IEnumerator> removeTasks = new List<IEnumerator>();
    private List<IManager> managerList = new List<IManager>();
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        matchProxy = new MatchProxy();
        connectProxy = new ConnectProxy();
        PlayerManager.instance.Init();
        ViewManager.instance.canvas = canvas;
        ViewManager.instance.Init();

        for (int i = 0; i < donotDestroy.Count; i++) {
            GameManager.DontDestroyOnLoad(donotDestroy[i]);
        }
    }
    private void OnEnable()
    {
        EventManager.instance.RegistProto(EMessageType.SCP_EnterPlay, OnEnterPlay);
    }
    private void OnDisable()
    {
        EventManager.instance.UnRegistProto(EMessageType.SCP_EnterPlay, OnEnterPlay);
    }

    private void OnEnterPlay(IMessage obj)
    {
        SCP_EnterPlay msg = obj as SCP_EnterPlay;
        int mapID = msg.MapID;

        EventManager.instance.SendEvent(HallEvents.HALLEVENT_ENTER_PLAY);

        string mapPath = mapPathList[mapID - 1];
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(mapPath);
        StartCoroutine(LoadSceneAsync(sceneAsync));
    }
    IEnumerator LoadSceneAsync(AsyncOperation sceneAsync) {
        while (!sceneAsync.isDone) {
            yield return null;
        }
        Debug.LogError("XBW~~  sceen load done");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var t in tasks) {
            if (!t.MoveNext()) {
                removeTasks.Add(t);
            }
        }
        foreach (var t in removeTasks) {
            tasks.Remove(t);
        }
        removeTasks.Clear();
    }
    public void AddManager(IManager m) {
        if (!managerList.Contains(m)) {
            managerList.Add(m);
        }
    }
}
