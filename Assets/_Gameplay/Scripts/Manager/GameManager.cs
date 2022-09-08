// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using UnityEngine.SceneManagement;

// public class GameManager : Singleton<GameManager>
// {

//     public static event Action<GameState> OnGameStateChanged;
//     public bool isAutoChangePlayState;

//     private void Start()
//     {
//         UIManager.Ins.OpenUI(UICanvasID.MainMenu);
//         StartCoroutine(DelayChangeGameState(GameState.Loading, 0.15f));
//     }
//     public void ChangeGameState(GameState state)
//     {
//         switch (state)
//         {
//             case GameState.NewScene:
//                 OnGameStateNewScene();
//                 break;
//             case GameState.Loading:
//                 OnGameStateLoading();
//                 break;
//             case GameState.LoadLevel:
//                 OnGameStateLoadLevel();
//                 break;
//             case GameState.Playing:
//                 OnGameStatePlaying();
//                 break;
//             case GameState.EndLevel:
//                 OnGameStateEndLevel();
//                 break;
//             case GameState.ResultPhase:
//                 OnGameStateResultPhase();
//                 break;
//             // case GameState.MainMenu:
//             //     OnGameStateMainMenu();
//             //     break;
//             default:
//                 break;
//         }

//         OnGameStateChanged?.Invoke(state);
//     }

//     private void OnGameStateNewScene()
//     {
//         Debug.Log("GameStateNewScene");
//         StartCoroutine(DelayChangeGameState(GameState.LoadLevel, 0.15f));
//     }
//     private void OnGameStateLoading()
//     {
//         Debug.Log("GameStateLoading");
//         StartCoroutine(DelayChangeGameState(GameState.LoadLevel, 0.15f));
//         LevelManager.Ins.LoadGame();
//     }
//     private void OnGameStateLoadLevel()
//     {
//         Debug.Log("GameStateLoadLevel");
//         LevelManager.Instance.LoadLevel();

//         if (isAutoChangePlayState)
//         {
//             StartCoroutine(DelayChangeGameState(GameState.Playing, 0.15f));
//         }
//     }
//     private void OnGameStatePlaying()
//     {
//         Debug.Log("GameStatePlaying");
//     }
//     private void OnGameStateEndLevel()
//     {
//         Debug.Log("GameStateEndLevel");
//         StartCoroutine(DelayChangeGameState(GameState.ResultPhase, 5f));
//         UIManager.Instance.CloseUI(UICanvasID.GamePlay);
//     }
//     private void OnGameStateResultPhase()
//     {
//         Debug.Log("GameStateResultPhase");
//     }
//     // private void OnGameStateMainMenu()
//     // {
//     //     Debug.Log("GameStateMainMenu");
//     // }


//     public enum GameState
//     {
//         NewScene,
//         Loading,
//         LoadLevel,
//         Playing,
//         EndLevel,
//         ResultPhase,
//         // MainMenu
//     }

//     IEnumerator DelayChangeGameState(GameState state, float time)
//     {
//         yield return new WaitForSeconds(time);
//         ChangeGameState(state);
//     }
// }
