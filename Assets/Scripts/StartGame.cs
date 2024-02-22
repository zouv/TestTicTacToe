using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    void Start()
    {
        var uiMgr = GetComponent<UIManager>();
        UIManager.Instance = uiMgr;
        UIManager.Instance.Init();
    }
}
