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
    
    private void Awake() {
        Instance = this;
        superModeButton.onClick.AddListener(SuperModeButtonClick);
        EnterMenu();
    }

    public void EnterMenu() {
        readyGoAnimation.gameObject.SetActive(false);
        // 隐藏分数
        scoreText.transform.parent.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        gunCdMask.transform.parent.gameObject.SetActive(false);
    }

    public void EnterReadyGo() {
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
    public void UpdateGunCd(float cd) {
        gunCdMask.fillAmount = cd;
    }
    
    // 开启超级模式
    private void SuperModeButtonClick() {
        // TODO: 添加广告SDK
    }
}
