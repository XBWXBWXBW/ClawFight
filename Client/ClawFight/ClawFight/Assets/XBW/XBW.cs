using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XBW : MonoBehaviour
{
    public string playerPrefabPath = @"Player/Player";
    // Start is called before the first frame update
    void Start()
    {
        GameObject go = Resources.Load(playerPrefabPath) as GameObject;
        GameObject.Instantiate(go);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("ClearAllMesh")]
    public void ClearAllMesh() {
        List<MeshRenderer> all = new List<MeshRenderer>();
        transform.GetComponentsInChildren<MeshRenderer>(true, all);
        foreach (var a in all) {
            a.enabled = false;
        }
    }
}
