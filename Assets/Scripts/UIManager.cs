using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    public Animation readyGoAnimation;
    
    private void Awake() {
        Instance = this;
        EnterMenu();
    }

    public void EnterMenu() {
        readyGoAnimation.gameObject.SetActive(false);
    }

    public void EnterReadyGo() {
        readyGoAnimation.gameObject.SetActive(true);
        readyGoAnimation.Play("ReadyGo");
    }
}
