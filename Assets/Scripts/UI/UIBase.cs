using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected UIArgs uiArgs;
    virtual
    public void Init(UIArgs uiArgs)
    {
        this.uiArgs = uiArgs;
    }

    virtual
    public void Destory()
    {
    }
}
