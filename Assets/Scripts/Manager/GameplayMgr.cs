/*
 * 玩法管理器：
 *      开始、结束、运行
 */

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

    private AIRunner _ai;
    public AIRunner AI
    {
        get { return _ai; }
    }

    public void Init()
    {
    }

    public void StartGame(uint size, bool playerFirst, int flag)
    {
        _ai = new G3TAIRunner(size); // more ai options
        if (!playerFirst) _ai.SetAITurn();
    }

    public void StopGame()
    {
        _ai.Destory();
        _ai = null;

        UIManager.Instance.ChangeUI(UIManager.UINames.Main);
    }

    public void Update()
    {
        if (_ai != null) _ai.Update();
    }
}
