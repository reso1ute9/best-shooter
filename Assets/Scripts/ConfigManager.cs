using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConfigManager : MonoBehaviour {
    public static ConfigManager Instance;
    
    public List<DuckLayerConfig> duckLayerConfigs = new List<DuckLayerConfig>();
    public Vector2 menuDuckPosition = new Vector2(0, -5.52f);
    public Vector2 duckMoveHRnage = new Vector2(-25, 25);
    public float duckMoveSpeed = 8;
    public List<DuckConfig> duckConfigs = new List<DuckConfig>();
    public Vector3 gunOffset = new Vector3(0, 6.4f, 0);
    
    private void Awake() {
        Instance = this;
    }
    
    // 随机获取一个鸭子配置
    public DuckConfig GetRandomDuckConfig() {
        return duckConfigs[Random.Range(0, duckConfigs.Count)];
    }
}

[Serializable]
public class DuckConfig {
    public Sprite sprite;                   // 鸭子精灵图片
    
}

[Serializable]
public class DuckLayerConfig {
    public string sortingLayer;             // 鸭子在场景中的层级
    public float poxY;                      // 鸭子在场景中的起始点
}