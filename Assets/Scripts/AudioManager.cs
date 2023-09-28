using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    public AudioClip[] hitDuckClips;                // 鸭子受击音效
    public AudioClip showSceneClip;                 // 场景搭建音效
    public AudioClip readyGoClip;                   // 准备音效
    public AudioClip shotGunClip;                   // 射击音效
    public AudioClip menuDuckClip;                  // 菜单鸭子音效
    public AudioClip menuDuckReadyClip;             // 菜单鸭子准备音效
    public AudioClip duckGoBakClip;                 // 鸭子返回音效
    public AudioClip unHitDuckClip;                 // 未击中时鸭子嘲笑音效
    
    private void Awake() {
        Instance = this;
    }

    public void PlayOneShot(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayHitDuckClip() {
        PlayOneShot(hitDuckClips[Random.Range(0, hitDuckClips.Length)]);
    }
}
