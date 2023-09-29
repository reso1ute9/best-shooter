using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckManager : MonoBehaviour {
    public static DuckManager Instance;
    
    public GameObject duckPrefab;
    private Stack<DuckController> duckPool = new Stack<DuckController>();
    private List<DuckController> currentDuckList = new List<DuckController>();
    private float currentTime = 0;
    
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
    
    // 回收鸭子资源
    public void RecycleDuck(DuckController duckController) {
        duckController.gameObject.SetActive(false);
        duckPool.Push(duckController);
        currentDuckList.Remove(duckController);
    }

    public void CleanAllDuck() {
        for (int i = 0; i < currentDuckList.Count; i++) {
            RecycleDuck(currentDuckList[i]);
        }
    }

    public void EnterGame() {
        StartCoroutine(GenerateGameDuckEveryInterval());
        StartCoroutine(GenerateGameDuckAvoidZero());
    }

    private IEnumerator GenerateGameDuckAvoidZero() {
        while (true) {
            yield return null;
            // 超级模式下场上一定要有三只鸭子以上
            if (GameManager.Instance.superMode && currentDuckList.Count <= 3) {
                GenerateGameDuck();
            } else if (currentDuckList.Count == 0) {
                GenerateGameDuck();
            }
        }
    }
    
    private IEnumerator GenerateGameDuckEveryInterval() {
        float generateDuckInterval = GameManager.Instance.superMode
            ? ConfigManager.Instance.superModeDuckInterval
            : ConfigManager.Instance.normalModeDuckInterval;
        WaitForSeconds wiWaitForSeconds = new WaitForSeconds(generateDuckInterval);
        while (true) {
            yield return wiWaitForSeconds;
            currentTime += generateDuckInterval;
            if (currentDuckList.Count < 15) {
                float randomValue = UnityEngine.Random.Range(0, 1f);
                if (randomValue <
                    ConfigManager.Instance.generateDuckCurve.Evaluate(currentTime /
                                                                      ConfigManager.Instance.maxGameTime)) {
                    GenerateGameDuck();
                }
            }
        }
    }
    
    // 生成鸭子
    private void GenerateGameDuck() {
        DuckController duckController = GetDuck();
        DuckGenerateInfo duckGenerateInfo = ConfigManager.Instance.GetRandomDuckGenerateInfo();
        duckController.InitGameDuck(duckGenerateInfo);
        currentDuckList.Add(duckController);
    }
}
