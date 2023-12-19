using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMgr : MonoBehaviour
{
    /// <summary>
    /// 基因得分
    /// </summary>
    public Slider genePoints;

    /// <summary>
    /// 生存压力显示
    /// </summary>
    public Image emotion;

    /// <summary>
    /// 游戏结束画面
    /// </summary>
    public GameObject gameOverWindow;

    /// <summary>
    /// 游戏胜利画面
    /// </summary>
    public GameObject gameCompletedWindow;
    private ApeMgr _apeMgr;
    private AudioMgr _audioMgr;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _apeMgr = ApeMgr.GetInstance();
        _audioMgr = AudioMgr.GetInstance();
        genePoints.value = 0;
        emotion.sprite = Resources.Load<Sprite>("emo1");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_apeMgr.DeadRoll())
        {
            // _audioMgr.PlaySound("Music/gameOver");
            ShowGameOver();
        }

        if (Math.Abs(genePoints.value - genePoints.maxValue) < 5)
        {
            _audioMgr.PlaySound("Music/Victory");
            ShowGameCompleted();
        }
        
        if (_apeMgr.apes.Count > 100 * _apeMgr.pressureFactor)
            emotion.sprite = Resources.Load<Sprite>("emo5");
        else if (_apeMgr.apes.Count > 80 * _apeMgr.pressureFactor)
            emotion.sprite = Resources.Load<Sprite>("emo4");
        else if (_apeMgr.apes.Count > 50 * _apeMgr.pressureFactor)
            emotion.sprite = Resources.Load<Sprite>("emo3");
        else if (_apeMgr.apes.Count > 20 * _apeMgr.pressureFactor)
            emotion.sprite = Resources.Load<Sprite>("emo2");
        else
            emotion.sprite = Resources.Load<Sprite>("emo1");

        genePoints.value = _apeMgr.CalAllGenePoints();
    }

    /// <summary>
    /// 卡牌弹出效果
    /// </summary>
    /// <param name="o"></param>
    public void OnPointerEnter(GameObject o)
    {
        _audioMgr.PlaySound("Music/arrow");
        o.transform.GetChild(0).gameObject.SetActive(true);
        var UIPos = o.GetComponent<RectTransform>();
        var pos = UIPos.anchoredPosition;
        UIPos.anchoredPosition = new Vector2(pos.x, pos.y + 100);
    }
    
    /// <summary>
    /// 卡牌收回效果
    /// </summary>
    /// <param name="o"></param>
    public void OnPointerExit(GameObject o)
    {
        o.transform.GetChild(0).gameObject.SetActive(false);
        var uiPos = o.GetComponent<RectTransform>();
        var pos = uiPos.anchoredPosition;
        uiPos.anchoredPosition = new Vector2(pos.x, pos.y - 100);
    }

    /// <summary>
    /// 瘟疫
    /// 筛选免疫力较低个体，降低幸存个体的颜值
    /// </summary>
    public void OnPlagueDown()
    {
        _audioMgr.PlaySound("Music/plague");
        foreach (var ape in _apeMgr.apes.ToList())
        {
            if (ape.immunity < 280)
            {
                ape.Death();
            }
            else
            {
                ape.charisma -= ape.charisma * 0.6f;
            }
        }
    }

    /// <summary>
    /// 冰河时代
    /// 筛选免疫力与智力较低的个体
    /// </summary>
    public void OnIceDown()
    {
        _audioMgr.PlaySound("Music/ice");
        foreach (var ape in _apeMgr.apes.ToList())
        {
            if (ape.immunity < 200 || ape.intelligence < 150)
            {
                ape.Death();
            }
        }
    }

    /// <summary>
    /// 火山爆发
    /// 筛选智力不合格的个体
    /// 提高生存压力（上限下降5%）
    /// 大幅降低存活个体的突变率
    /// </summary>
    public void OnVolcanoDown()
    {
        _audioMgr.PlaySound("Music/volcano");
        _apeMgr.pressureFactor *= 0.95f;
        foreach (var ape in _apeMgr.apes.ToList())
        {
            if (ape.intelligence < 200)
            {
                ape.Death();
            }
            else
            {
                ape.mutation *= 0.05f;
            }
        }
    }

    /// <summary>
    /// 太阳风
    /// 提高在场个体的突变率（翻倍）
    /// 降低总体寿命（上限下降5%），不可逆
    /// </summary>
    public void OnSolarDown()
    {
        _audioMgr.PlaySound("Music/solar");
        _apeMgr.maxAge = (_apeMgr.maxAge * 100 / 365) * 0.95f * 365 / 100;
        foreach (var ape in _apeMgr.apes.ToList())
        {
            ape.mutation *= 2;
        }
    }

    /// <summary>
    /// 显示游戏失败弹窗
    /// </summary>
    public void ShowGameOver()
    {
        gameOverWindow.SetActive(true);
    }

    /// <summary>
    /// 显示游戏胜利弹窗
    /// </summary>
    public void ShowGameCompleted()
    {
        gameCompletedWindow.SetActive(true);
    }
    
    /// <summary>
    /// 游戏失败
    /// 场上只剩下同性猩猩
    /// 或无幸存者
    /// </summary>
    public void GameOver()
    {
        SceneManager.LoadScene("0-Title");
    }
}
