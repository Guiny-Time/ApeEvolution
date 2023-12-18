using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Utils;
using Random = UnityEngine.Random;

public class MainController : BaseMgr<MainController>
{
    public GameObject _apeContainer;

    // Start is called before the first frame update
    void Start()
    {
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
        int ape_number = Random.Range(4, 10);
        for (int i = 0; i < ape_number; i++)
        {
            PoolMgr.GetInstance().GetObj("Ape",  o =>
            {
                o.transform.position = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
                o.transform.parent = _apeContainer.transform;
                o.GetComponent<Ape>().InitApe();
            });
        }
    }

    /// <summary>
    /// 游戏失败
    /// 场上只剩下同性猩猩
    /// 或无幸存者
    /// </summary>
    private void GameOver()
    {
        
    }

    /// <summary>
    /// 评分达标
    /// </summary>
    private void GameCompleted()
    {
        
    }
    
}
