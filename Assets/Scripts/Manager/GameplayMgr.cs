using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMgr
{
    private static GameplayMgr _instance;
    public static GameplayMgr Instance
    {
        get
        {
            if (_instance == null) _instance = new GameplayMgr();
            return _instance;
        }
    }

    public void Init()
    {
        // TODO
    }
}
