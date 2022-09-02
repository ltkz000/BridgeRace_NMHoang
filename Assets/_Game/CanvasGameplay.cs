using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGameplay : UICanvas
{
    public void SettingButton()
    {
        UIManager.Ins.OpenUI(UIID.UICSetting);
    }

    public void FailButton()
    {
        UIManager.Ins.OpenUI(UIID.UICFail);

        Close();
    }

    public void VictoryButton()
    {
        UIManager.Ins.OpenUI<CanvasVictory>(UIID.UICVictory).OnInitData(Random.Range(0, 100));

        Close();
    }
}
