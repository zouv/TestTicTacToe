using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    private Transform _tsPanelMask;
    private Transform _tsHelpPanel;
    private Transform _tsSettingsPanel;
    private Toggle _togFirstMove;

    void Awake()
    {
        _tsPanelMask = this.transform.Find("Panels/Mask");
        _tsHelpPanel = this.transform.Find("Panels/HelpPanel");
        _tsSettingsPanel = this.transform.Find("Panels/SettingsPanel");
        _togFirstMove = this.transform.Find("Buttons/TogFirstMove").GetComponent<Toggle>();
    }
  
    public void ShowHelpPane(bool value)
    {
        _tsHelpPanel.gameObject.SetActive(value);
        _tsPanelMask.gameObject.SetActive(value);
    }

    public void ShowSettingPane(bool value)
    {
        _tsSettingsPanel.gameObject.SetActive(value);
        _tsPanelMask.gameObject.SetActive(value);
    }

    public void StartGameClassics()
    {
        var args = new UIAGame3T { playerFirst = _togFirstMove.isOn, flag = Game3TFlag.Classics };
        UIManager.Instance.ChangeUI(UIManager.UINames.Game3T, args);
    }

    public void StartGameFightToDeath()
    {
        var args = new UIAGame3T { playerFirst = _togFirstMove.isOn, flag = Game3TFlag.FightToDeath };
        UIManager.Instance.ChangeUI(UIManager.UINames.Game3T, args);
    }
}
