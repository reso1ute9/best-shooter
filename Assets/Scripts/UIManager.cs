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
    
    private void Awake() {
        Instance = this;
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

    public void UpdateScore(int score) {
        scoreText.text = score.ToString();
    }
}
