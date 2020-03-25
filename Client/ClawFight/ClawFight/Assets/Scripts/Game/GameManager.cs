using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using message;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string ipAddress = "127.0.0.1";
    public Camera viewCam;
    public bool isQuickEnter = false;
    public float rotateXSense = 1, rotateYSense = 1;
    public GameObject canvas;
    public List<GameObject> donotDestroy = new List<GameObject>();
    public List<string> mapPathList = new List<string>();
    public static GameManager instance;
    public MatchProxy matchProxy;
    [HideInInspector]
    public float gameTime = -1;
    //public List<IEnumerator> tasks = new List<IEnumerator>();
    //public List<IEnumerator> removeTasks = new List<IEnumerator>();
    //public List<IEnumerator> candidateTasks = new List<IEnumerator>();
    public LinkedList<IEnumerator> tasks = new LinkedList<IEnumerator>();
    private List<IManager> managerList = new List<IManager>();
    private string playerPrefabPath = @"Player/Player";
    private string redMaterialPath = @"PlayerMat/Red";
    private string blueMaterialPath = @"PlayerMat/Blue";
    private object obj_lock = new object();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        matchProxy = new MatchProxy();
        PlayerManager.instance.Init();

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
        EventManager.instance.RegistProto(EMessageType.SCP_BeginPlay, OnBeginPlay);
    }
    private void OnDisable()
    {
        EventManager.instance.UnRegistProto(EMessageType.SCP_EnterPlay, OnEnterPlay);
        EventManager.instance.UnRegistProto(EMessageType.SCP_BeginPlay, OnBeginPlay);
    }

    private void OnBeginPlay(IMessage obj)
    {
        viewCam.clearFlags = CameraClearFlags.Depth;

        ViewManager.instance.HideView(EViewType.LoadingView);

        EventManager.instance.SendEvent(BattleEvents.BATTLEEVENT_BATTLE_START);

        Cursor.visible = false;
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
        CSP_BeginPlay msg = new CSP_BeginPlay();
        msg.PlayerID = PlayerManager.instance.mainPlayerInBattle.playerData.ID;
        ConnectManager.instance.connectProxy.SendMessage(EMessageType.CSP_BeginPlay, msg);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTask();
        TimeManager.instance.Update();
    }
    
    void UpdateTask() {
        LinkedListNode<IEnumerator> node = tasks.First;
        while (node != null)
        {
            IEnumerator v = node.Value;
            if (!v.MoveNext())
            {
                var pre = node;
                node = node.Next;
                tasks.Remove(pre);
            }
            else
            {
                node = node.Next;
            }
        }
    }
    public void AddManager(IManager m) {
        if (!managerList.Contains(m)) {
            managerList.Add(m);
        }
    }
}
