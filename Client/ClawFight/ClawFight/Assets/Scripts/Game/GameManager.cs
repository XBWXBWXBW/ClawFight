using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using message;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isQuickEnter = false;
    public GameObject canvas;
    public List<GameObject> donotDestroy = new List<GameObject>();
    public List<string> mapPathList = new List<string>();
    public static GameManager instance;
    public MatchProxy matchProxy;
    public ConnectProxy connectProxy;
    public List<IEnumerator> tasks = new List<IEnumerator>();
    public List<IEnumerator> removeTasks = new List<IEnumerator>();
    private List<IManager> managerList = new List<IManager>();
    private string playerPrefabPath = @"Player/Player";
    private string redMaterialPath = @"PlayerMat/Red";
    private string blueMaterialPath = @"PlayerMat/Blue";
    
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

        if (isQuickEnter)
        {
            SceneManager.LoadScene(mapPathList[0]);
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
        ViewManager.instance.HideHallView();
        ViewManager.instance.ShowView(EViewType.LoadingView);
        StartCoroutine(LoadSceneAsync(sceneAsync));
    }
    IEnumerator LoadSceneAsync(AsyncOperation sceneAsync) {
        while (!sceneAsync.isDone) {
            yield return null;
        }
        StartCoroutine(LoadPlayer());
    }
    IEnumerator LoadPlayer() {
        var playerInRoom = PlayerManager.instance.playerInRoom_Dict;
        ResourceRequest rr = Resources.LoadAsync<GameObject>(playerPrefabPath);
        while (!rr.isDone) {
            yield return null;
        }
        GameObject pGO = GameObject.Instantiate(rr.asset as GameObject);
        foreach (var e in playerInRoom) {
            Player p = e.Value;
            PlayerData pd = p.playerData;
            if (pd.isMainPlayer) continue;
            GameObject otherPlayer = GameObject.Instantiate(pGO);
            NetPlayer np = otherPlayer.AddComponent<NetPlayer>();
            np.playerData = pd;
            PlayerManager.instance.AddPlayerInBattle(np, pd);
        }
        LocalPlayer lp = pGO.AddComponent<LocalPlayer>();
        lp.playerData = PlayerManager.instance.mainPlayer.playerData;
        PlayerManager.instance.AddPlayerInBattle(lp, lp.playerData);

        StartCoroutine(LoadMaterial());
    }
    IEnumerator LoadMaterial() {
        ResourceRequest redRR = Resources.LoadAsync<Material>(redMaterialPath);
        ResourceRequest blueRR = Resources.LoadAsync<Material>(blueMaterialPath);
        while (!(redRR.isDone && blueRR.isDone)) {
            yield return null;
        }
        Material redMat = redRR.asset as Material;
        Material blueMat = blueRR.asset as Material;
        foreach (var e in PlayerManager.instance.playerInBattle_Dict) {
            if (e.Value.playerData.eTeam == ETeam.TeamA)
            {
                e.Value.SetMaterial(redMat);
            }
            else {
                e.Value.SetMaterial(blueMat);
            }
            e.Value.StartBorn();
            e.Value.gameObject.SetActive(true);
        }

        SceneLoadDone();
    }
    private void SceneLoadDone() {
        ViewManager.instance.HideView(EViewType.LoadingView);
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
