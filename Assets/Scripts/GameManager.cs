using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Menu,
    ReadyGo,
    Game,
    GameOver
}

// 游戏管理器
public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    private void Awake() {
        Instance = this;
    }

    private GameState gameState;

    public GameState GameState {
        get => gameState;
        set {
            gameState = value;
            switch (gameState) {
                case GameState.Menu:
                    DuckManager.Instance.CreatMenuDuck();
                    break;
                case GameState.ReadyGo:
                    break;
                case GameState.Game:
                    break;
                case GameState.GameOver:
                    break;
            }
        }
    }
    
    // 改为协程利于进入游戏时等待游戏界面加载完毕
    IEnumerator Start() {
        // 播放搭建场景音效
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.showSceneClip);
        yield return new WaitForSeconds(1.5f);
        GameState = GameState.Menu;
    }
}
