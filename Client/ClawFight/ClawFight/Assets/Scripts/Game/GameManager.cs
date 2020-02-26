using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MatchProxy matchProxy;
    public HallPlayer hallPlayer;
    // Start is called before the first frame update
    void Start()
    {
        TcpConnect tcpConnect = new TcpConnect();
        tcpConnect.Start();
        instance = this;

        matchProxy = new MatchProxy();
        hallPlayer = new HallPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
