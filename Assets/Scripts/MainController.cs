using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Utils;
using Random = UnityEngine.Random;

public class MainController : BaseMgr<MainController>
{
    private GameObject _apeContainer;
    private ApeMgr _apeMgr;

    private List<GameObject> testList;

    // Start is called before the first frame update
    void Start()
    {
        _apeContainer = GameObject.Find("Apes");
        _apeMgr = ApeMgr.GetInstance();
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
            PoolMgr.GetInstance().GetObj("Ape",  o =>
            {
                o.transform.position = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
                o.transform.parent = _apeContainer.transform;
                o.GetComponent<Ape>().InitApe();
                o.GetComponent<Ape>().NewGene();
                // _apeMgr.AddApe(o, 100);
                // testList.Add(o.gameObject);
                
                
            });
        }
        
    }
}