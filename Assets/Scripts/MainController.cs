using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

public class MainController : BaseMgr<MainController>
{
    public GameObject apeContainer;

    void Start()
    {
        GenerateNewGame();
        Application.targetFrameRate = 150;
    }

    /// <summary>
    /// 新游戏
    /// 我不是很想给demo做存档系统
    /// </summary>
    private void GenerateNewGame()
    {
        // 防止死局
        PoolMgr.GetInstance().GetObj("Ape",  o =>
        {
            o.transform.position = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
            o.transform.parent = apeContainer.transform;
            o.GetComponent<Ape>().InitApe(0);
        });
        PoolMgr.GetInstance().GetObj("Ape",  o =>
        {
            o.transform.position = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
            o.transform.parent = apeContainer.transform;
            o.GetComponent<Ape>().InitApe(1);
        });
        
        // 初始族群的成员数量
        var apeNumber = Random.Range(4, 10);
        for (int i = 0; i < apeNumber; i++)
        {
            PoolMgr.GetInstance().GetObj("Ape",  o =>
            {
                o.transform.position = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
                o.transform.parent = apeContainer.transform;
                o.GetComponent<Ape>().InitApe();
            });
        }
    }

}
