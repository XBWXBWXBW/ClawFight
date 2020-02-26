using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MatchProxy matchProxy;
    public HallPlayer hallPlayer;
    public ConnectProxy connectProxy;
    public List<IEnumerator> tasks = new List<IEnumerator>();
    public List<IEnumerator> removeTasks = new List<IEnumerator>();
    // Start is called before the first frame update
    void Start()
    {
        
        instance = this;

        matchProxy = new MatchProxy();
        hallPlayer = new HallPlayer();
        connectProxy = new ConnectProxy();
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
}
