using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasVictory : UICanvas
{
    public Text resultText;

    public void SetResult(bool val)
    {
        if(val)
        {
            resultText.text = "VICTORY";
        }
        else
        {
            resultText.text = "LOSE";
        }
    }

    public void NextButton()
    {
        UIManager.Ins.OpenUI(UICanvasID.GamePlay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        FrameManager.Ins.ChangeState(GameState.GamePlay);

        Close();
    }

    public void RestartButton()
    {
        UIManager.Ins.OpenUI(UICanvasID.GamePlay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Close();
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);

        Close();
    }

    protected override void OnOpenCanvas()
    {
        base.OnOpenCanvas();
        UIManager.Ins.CloseUI(UICanvasID.GamePlay);
    }

    protected override void OnCloseCanvas()
    {
        base.OnCloseCanvas();
        // GameManager.Ins.
    }
}
