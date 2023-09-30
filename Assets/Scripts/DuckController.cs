using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class DuckController : MonoBehaviour { 
    public new Animation animation;
    public PolygonCollider2D polygonCollider;
    public SortingGroup sortingGroup;
    public Transform duckTargetIcon;
    public SpriteRenderer duckSpriteRenderer;

    public bool isGameDuck;
    public bool isTargetDuck;
    private List<Vector2> physicsShape = new List<Vector2>();
    private Vector2 generatePoint;

    private bool isLeft;
    private bool isUp;
    
    private float targetX;
    private float targetY;

    public float TargetX {
        get => targetX;
        set {
            targetX = value;
            isLeft = transform.position.x > targetX;
            transform.localScale = new Vector3(isLeft ? -1 : 1, 1, 1);
        }
    }

    public float TargetY {
        get => targetY;
        set {
            targetY = value;
            isUp = transform.position.y < targetY;
        }
    }
    
    public bool isDead = false;
    
    private void Init(DuckConfig duckConfig) {
        isDead = false;
        duckSpriteRenderer.sprite = duckConfig.sprite;
        duckSpriteRenderer.transform.localRotation = Quaternion.identity;
        SetColliderShape(duckSpriteRenderer.sprite);
    }

    private void Update() {
        if (isGameDuck == false || isDead == true) {
            return;
        }
        Move();
    }
    
    private void Move() {
        // 更新鸭子目标坐标X
        if (isLeft == true && transform.position.x <= TargetX) {
            TargetX = ConfigManager.Instance.GetRandomMovePointX();
        } else if (isLeft == false && transform.position.x >= TargetX) {
            TargetX = ConfigManager.Instance.GetRandomMovePointX();
        }
        // 更新鸭子目标坐标Y
        if (isUp == true && transform.position.y >= targetY) {
            TargetY = ConfigManager.Instance.GetRandomMovePointY(isUp) + generatePoint.y;
        } else if (isUp == false && transform.position.x <= TargetY) {
            TargetY = ConfigManager.Instance.GetRandomMovePointY(isUp) + generatePoint.y;
        }
        Vector2 direction = Vector2.zero;
        direction.x = isLeft ? -1 : 1;
        direction.y = isUp ? 1 : -1;
        transform.Translate(direction.normalized * ConfigManager.Instance.duckMoveSpeed * Time.deltaTime);
    }

    // 设置碰撞体形状
    private void SetColliderShape(Sprite sprite) {
        sprite.GetPhysicsShape(0, physicsShape);
        polygonCollider.SetPath(0, physicsShape);
    }
    
    #region MenuDuck
    // 初始化菜单鸭子
    public void InitMenuDuck(DuckConfig duckConfig, Vector2 targetPosition) {
        isTargetDuck = false;
        // 播放菜单鸭子音效
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.menuDuckClip);
        // 初始化菜单鸭子
        Init(duckConfig);
        isGameDuck = false;
        // 注意: 一开始需要将鸭子降低到地下
        transform.position = targetPosition + new Vector2(0, -5);
        duckTargetIcon.gameObject.SetActive(true);
        sortingGroup.sortingLayerName = "Duck2";
        StartCoroutine(MoveToMenuPosition(targetPosition.y));
    }
    
    // 使用协程将菜单鸭子移动到指定地点
    private IEnumerator MoveToMenuPosition(float targetPositionY) {
        while (transform.position.y < targetPositionY) {
            transform.position += new Vector3(0, Time.deltaTime * ConfigManager.Instance.duckMoveSpeed, 0);
            yield return null;
        }
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.menuDuckReadyClip);
        transform.position = new Vector3(transform.position.x, targetPositionY, 0);
    }
    #endregion

    #region GameDuck
    // 初始化游戏内的鸭子
    public void InitGameDuck(DuckGenerateInfo duckGenerateInfo) {
        // 如果生成的鸭子是特殊鸭子则可以播放特殊音效
        if (duckGenerateInfo.isTargetDuck == true) {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.menuDuckClip);
        }
        Init(duckGenerateInfo.config);
        isGameDuck = true;
        isTargetDuck = duckGenerateInfo.isTargetDuck;
        generatePoint = duckGenerateInfo.point;
        // 注意: 鸭子身上的靶子由鸭子类型决定
        duckTargetIcon.gameObject.SetActive(isTargetDuck);
        sortingGroup.sortingLayerName = duckGenerateInfo.layer.sortingLayer;
        transform.position = duckGenerateInfo.point;
        // 移动游戏内的鸭子
        TargetX = ConfigManager.Instance.GetRandomMovePointX();
        TargetY = ConfigManager.Instance.GetRandomMovePointY(true) + generatePoint.y;
    }
    #endregion
    
    // 鸭子死亡
    public void Dead() {
        isDead = true;
        // 播放死亡动画
        animation.Play("Hit");
        // 播放菜单鸭子退出效果
        StopAllCoroutines();
        StartCoroutine(DoExit());
    }

    private IEnumerator DoExit() {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.duckGoBakClip);
        float targetPosY = transform.position.y - 7;
        while (transform.position.y > targetPosY) {
            yield return null;
            transform.position -= new Vector3(0, Time.deltaTime * ConfigManager.Instance.duckMoveSpeed, 0);
        }
        // 回收鸭子资源
        DuckManager.Instance.RecycleDuck(this);
    }
}
