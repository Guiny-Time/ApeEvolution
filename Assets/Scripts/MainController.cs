using System.Collections;
using UnityEngine;
using System;
using Utils;
using Random = UnityEngine.Random;

public class MainController : MonoBehaviour
{
    private GameObject _apeContainer;
    
    // ape优先队列，权重为魅力+年龄+健康的综合考虑（配偶吸引程度）
    private PriorityQueue<Ape, int> f_apes;
    private PriorityQueue<Ape, int> m_apes;

    // Start is called before the first frame update
    void Start()
    {
        _apeContainer = GameObject.Find("Apes");
        GenerateNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 新游戏
    /// 我不是很想给demo做存档系统
    /// </summary>
    private void GenerateNewGame()
    {
        // 初始族群的成员数量
        int ape_number = Random.Range(2, 10);
        for (int i = 0; i < ape_number; i++)
        {
            PoolMgr.GetInstance().GetObj("Ape", (GameObject o) =>
            {
                o.transform.SetParent(_apeContainer.transform);
                o.GetComponent<Ape>().InitApe();
                o.GetComponent<Ape>().NewGene();
                o.transform.position = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
            });
        }
        
    }
}
