using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class ConfigManager : MonoBehaviour {
    public static ConfigManager Instance;
    
    public List<DuckLayerConfig> duckLayerConfigs = new List<DuckLayerConfig>();
    public Vector2 menuDuckPosition = new Vector2(0, -5.52f);
    public Vector2 duckMoveXRnage = new Vector2(-30, 30);
    public float duckMoveYRange = 8;
    public float duckMoveSpeed = 8;
    public List<DuckConfig> duckConfigs = new List<DuckConfig>();

    public float superModeDuckInterval = 0.2f;
    public float normalModeDuckInterval = 1.0f;
    public Vector3 gunOffset = new Vector3(0, 6.4f, 0);
    public float shootCd = 1.0f;
    public float superModeShootCd = 0.5f;
    public float readyGoAnimationTime = 2.0f;
    public float maxGameTime = 60;
    [Range(0f, 1.0f)]
    public float targetDuckProbability = 0.5f;      // 生成带有标记鸭子的概率
    public AnimationCurve generateDuckCurve;        // 生成游戏内鸭子的动画曲线
    
    private void Awake() {
        Instance = this;
    }
    
    // 随机获取一个鸭子配置
    public DuckConfig GetRandomDuckConfig() {
        return duckConfigs[Random.Range(0, duckConfigs.Count)];
    }

    public DuckLayerConfig GetRandomDuckLayerConfig() {
        return duckLayerConfigs[Random.Range(0, duckLayerConfigs.Count)];
    }

    public bool GetRandomTargetDuck() {
        return UnityEngine.Random.Range(0, 1f) < targetDuckProbability;
    }

    public float GetRandomMovePointX() {
        return UnityEngine.Random.Range(duckMoveXRnage.x, duckMoveXRnage.y);
    }
    
    public float GetRandomMovePointY(bool isUp) {
        if (isUp) {
            return UnityEngine.Random.Range(duckMoveYRange / 3, duckMoveYRange);
        }
        else {
            return UnityEngine.Random.Range(0, duckMoveYRange / 3);
        }
    }
    
    // 随机获取一个鸭子的生成信息
    public DuckGenerateInfo GetRandomDuckGenerateInfo() {
        DuckConfig config = GetRandomDuckConfig();
        DuckLayerConfig layer = GetRandomDuckLayerConfig();
        // 注意: Y值需要放到每一层对应的高度上
        Vector2 point = new Vector2(GetRandomMovePointX(), layer.poxY);     
        bool isTargetDuck = GetRandomTargetDuck();
        return new DuckGenerateInfo(config, layer, point, isTargetDuck);
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

// 保存当前生成鸭子的相关信息
public struct DuckGenerateInfo {
    public DuckConfig config;
    public DuckLayerConfig layer;
    public Vector2 point;
    public bool isTargetDuck;

    public DuckGenerateInfo(DuckConfig config, DuckLayerConfig layer, Vector2 point, bool isTargetDuck) {
        this.config = config;
        this.layer = layer;
        this.point = point;
        this.isTargetDuck = isTargetDuck;
    }
}