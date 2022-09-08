using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGameplay : UICanvas
{
    public void SettingButton()
    {
        UIManager.Ins.OpenUI(UICanvasID.Setting);

        Close();
    }
}
