/*
 * UI 管理
 *  ! 注：此处为了简化逻辑，未添加界面动态加载、也没做界面层级管理
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // define all ui names
    public static class UINames
    {
        public const string Main = "UIMain";
        public const string Game3T = "UIGame3T";
    }

    public Canvas uiCanvas;
    public string initialUI;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    private Dictionary<string, UIBase> _dicUI;
    private string _currUI;

    public void Init()
    {
        if (uiCanvas == null)
        {
            Debug.LogError("canvas not set!");
            return;
        }

        _dicUI = new Dictionary<string, UIBase>();
        foreach (Transform ts in uiCanvas.transform)
        {
            var ui = ts.GetComponent<UIBase>();
            _dicUI.Add(ts.name, ui);
        }

        ChangeUI(String.IsNullOrWhiteSpace(initialUI) ? UINames.Main : initialUI);
    }

    public void ChangeUI(string uiName, UIArgs uiArgs = null)
    {
        // close old ui
        if (!String.IsNullOrEmpty(_currUI))
        {
            var old = _dicUI[_currUI];
            old.gameObject.SetActive(false);
            old.Destory();
        }

        // open new ui
        var ins = _dicUI[uiName];
        ins.Init(uiArgs);
        ins.gameObject.SetActive(true);
        _currUI = uiName;
    }
}
