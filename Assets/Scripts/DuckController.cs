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

    private bool isGameDuck;
    private bool isTargetDuck;
    private List<Vector2> physicsShape = new List<Vector2>();
    
    public bool isDead = false;
    
    private void Init(DuckConfig duckConfig) {
        isDead = false;
        duckSpriteRenderer.sprite = duckConfig.sprite;
        duckSpriteRenderer.transform.localRotation = Quaternion.identity;
        SetColliderShape(duckSpriteRenderer.sprite);
    }
    
    // 设置碰撞体形状
    private void SetColliderShape(Sprite sprite) {
        sprite.GetPhysicsShape(0, physicsShape);
        polygonCollider.SetPath(0, physicsShape);
    }

    #region Menu
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
    
    // 菜单鸭子死亡
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
    #endregion
}
