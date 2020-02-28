using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
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
