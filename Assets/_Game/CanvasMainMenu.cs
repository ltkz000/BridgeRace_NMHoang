using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    public void PlayGameButton()
    {
        UIManager.Ins.OpenUI(UIID.UICGamePlay);

        Close();
    }
}
