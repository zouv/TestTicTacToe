using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    private Transform _tsHelpPanel;
    private Transform _tsSettingsPanel;
    private Toggle _togFirstMove;

    void Start()
    {
        _tsHelpPanel = this.transform.Find("Panels/HelpPanel");
        _tsSettingsPanel = this.transform.Find("Panels/SettingsPanel");
        _togFirstMove = this.transform.Find("Buttons/TogFirstMove").GetComponent<Toggle>();
    }
  
    public void ShowHelpPane(bool value)
    {
        _tsHelpPanel.gameObject.SetActive(value);
    }

    public void ShowSettingPane(bool value)
    {
        _tsSettingsPanel.gameObject.SetActive(value);
    }

    public void StartGame1(string uiName)
    {
        //Debug.Log($"StartGame1___ {_togFirstMove.isOn}, {uiName}");
        UIManager.Instance.ChangeUI(uiName);
    }

    public void StartGame2(string uiName)
    {
        UIManager.Instance.ChangeUI(uiName);
    }
}
