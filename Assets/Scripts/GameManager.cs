using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public Animation gunAnimation;

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
                    UIManager.Instance.EnterMenu();
                    break;
                case GameState.ReadyGo:
                    UIManager.Instance.EnterReadyGo();
                    AudioManager.Instance.PlayOneShot(AudioManager.Instance.readyGoClip);
                    break;
                case GameState.Game:
                    break;
                case GameState.GameOver:
                    break;
            }
        }
    }

    public void Update() {
        switch (gameState) {
            case GameState.Menu:
                DuckController menuDuck = RayCastDuck();
                if (menuDuck != null && menuDuck.isDead == false) {
                    GunShoot(menuDuck);
                    menuDuck.Dead();
                    GameState = GameState.ReadyGo;
                }
                break;
            case GameState.ReadyGo:
                break;
            case GameState.Game:
                break;
            case GameState.GameOver:
                break;
        }
    }

    // 改为协程利于进入游戏时等待游戏界面加载完毕
    IEnumerator Start() {
        // 播放搭建场景音效
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.showSceneClip);
        yield return new WaitForSeconds(1.5f);
        GameState = GameState.Menu;
    }

    private DuckController RayCastDuck() {
        if (Input.touchCount > 0) {
            UnityEngine.Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity);
                if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out DuckController duckController)) {
                    return duckController;
                }
            }
        }
        return null;
    }
    
    // 枪打一个点
    private void GunShoot(Vector2 targetPosition) {
        gunAnimation.transform.position = targetPosition;
        // 当枪移动到左侧时需要进行反转
        gunAnimation.transform.localScale = new Vector3(targetPosition.x >= 0 ? 1 : -1, 1, 1);
        gunAnimation.Play("Gun");
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.shotGunClip);
    }
    
    // 枪打鸭子
    private void GunShoot(DuckController duckController) {
        GunShoot(duckController.transform.position + ConfigManager.Instance.gunOffset);
        AudioManager.Instance.PlayHitDuckClip();
    }
}
