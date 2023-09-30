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

    private int currentScore;           // 当前分数
    private float gameTime;             // 游戏时间
    private float shootTime;            // 射击冷却时间
    
    public Animation gunAnimation;
    public bool superMode = false;
    
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
                    superMode = false;
                    DuckManager.Instance.CreatMenuDuck();
                    UIManager.Instance.EnterMenu();
                    break;
                case GameState.ReadyGo:
                    UIManager.Instance.EnterReadyGo();
                    AudioManager.Instance.PlayOneShot(AudioManager.Instance.readyGoClip);
                    // 等动画播放完毕进入游戏
                    Invoke(nameof(StartGame), ConfigManager.Instance.readyGoAnimationTime);
                    break;
                case GameState.Game:
                    currentScore = 0;
                    gameTime = ConfigManager.Instance.maxGameTime;
                    UIManager.Instance.EnterGame();
                    // 更新默认分数
                    UIManager.Instance.UpdateScore(0);
                    // 进入游戏
                    DuckManager.Instance.EnterGame();
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
                if (gameTime <= 0) {
                    GameState = GameState.GameOver;
                    return;
                }
                // 更新游戏时间
                gameTime -= Time.deltaTime;
                UIManager.Instance.UpdateTime((int)gameTime);
                // 更新射击cd
                shootTime -= Time.deltaTime;
                float shootCd = superMode
                    ? (shootTime / ConfigManager.Instance.superModeShootCd)
                    : (shootTime / ConfigManager.Instance.shootCd);
                UIManager.Instance.UpdateGunCd(shootCd);
                // 检查当前是否进行射击
                if (shootCd <= 0 && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                    shootTime = shootCd;
                    DuckController duckController = RayCastDuck();
                    if (duckController != null && duckController.isDead == false) {
                        // 更新游戏分数
                        currentScore += duckController.isTargetDuck ? 5 : 1;
                        duckController.Dead();
                        UIManager.Instance.UpdateScore(currentScore);
                        // TODO: 屏幕在不同模式下/不同平台上震动
                    } else {
                        if (superMode == false) {
                            AudioManager.Instance.PlayOneShot(AudioManager.Instance.unHitDuckClip);
                        }
                        GunShoot(Input.GetTouch(0).position);
                    }
                }
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

    private void StartGame() {
        GameState = GameState.Game;
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
