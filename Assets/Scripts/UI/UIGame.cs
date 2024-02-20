using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : UIBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ExitGame()
    {
        UIManager.Instance.ChangeUI(UIManager.UINames.Main);
    }
}
