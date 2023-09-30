using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    public Animation readyGoAnimation;
    public Text scoreText;
    public Text timeText;
    public Image gunCdMask;
    public Button superModeButton;

    public Transform gameOverPanel;
    public Button gameOverShareButton;
    public Button gameOverCloseButton;
    public Text gameOverScore;
    
    
    
    private void Awake() {
        Instance = this;
        superModeButton.onClick.AddListener(SuperModeButtonClick);
        gameOverShareButton.onClick.AddListener(GameOverShareButtonClick);
        gameOverCloseButton.onClick.AddListener(GameOverCloseButtonClick);
        EnterMenu();
    }

    public void EnterMenu() {
        readyGoAnimation.gameObject.SetActive(false);
        // 隐藏分数
        scoreText.transform.parent.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        gunCdMask.transform.parent.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
    }

    public void EnterReadyGo() {
        superModeButton.gameObject.SetActive(false);
        readyGoAnimation.gameObject.SetActive(true);
        readyGoAnimation.Play("ReadyGo");
    }

    public void EnterGame() {
        // 开启UI
        scoreText.transform.parent.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        gunCdMask.transform.parent.gameObject.SetActive(true);
    }
    
    // 更新时间
    public void UpdateTime(int time) {
        timeText.text = time.ToString();
    }
    
    // 更新分数
    public void UpdateScore(int score) {
        scoreText.text = score.ToString();
    }
    
    // 更新武器CD
    public void UpdateGunCd(float gunCd) {
        gunCdMask.fillAmount = gunCd;
    }
    
    // 开启超级模式
    private void SuperModeButtonClick() {
        // TODO: 添加广告SDK
    }

    public void GameOver() {
        gameOverScore.text = scoreText.text;
        gameOverPanel.gameObject.SetActive(true);
    }
    
    // 游戏结束分享按钮
    private void GameOverShareButtonClick() {
        // TODO: 需要调用平台SDK
    }
    
    // 游戏结束关闭按钮
    private void GameOverCloseButtonClick() {
        GameManager.Instance.GameState = GameState.Menu;
    }
}
