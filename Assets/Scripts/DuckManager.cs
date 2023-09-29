using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckManager : MonoBehaviour {
    public static DuckManager Instance;
    
    public GameObject duckPrefab;
    private Stack<DuckController> duckPool = new Stack<DuckController>();
    private List<DuckController> currentDuckList = new List<DuckController>();
    
    private void Awake() {
        Instance = this;
    }
    
    // 创建菜单鸭子
    public void CreatMenuDuck() {
        DuckController duckController = GetDuck();
        DuckConfig duckConfig = ConfigManager.Instance.GetRandomDuckConfig();
        duckController.InitMenuDuck(duckConfig, ConfigManager.Instance.menuDuckPosition);
        
        currentDuckList.Add(duckController);
    }
    
    // 获取鸭子实例
    private DuckController GetDuck() {
        DuckController duckController;
        if (duckPool.Count == 0) {
            duckController = GameObject.Instantiate(duckPrefab).GetComponent<DuckController>();
            duckPool.Push(duckController);
        }
        duckController = duckPool.Pop();
        duckController.gameObject.SetActive(true);
        return duckController;
    }
}
