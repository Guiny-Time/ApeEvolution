using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    private ApeMgr _apeMgr;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _apeMgr = ApeMgr.GetInstance();
        genePoints.value = 0;
        emotion.sprite = Resources.Load<Sprite>("emo1");
    }

    // Update is called once per frame
    void Update()
    {
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
        var UIPos = o.GetComponent<RectTransform>();
        var pos = UIPos.anchoredPosition;
        UIPos.anchoredPosition = new Vector2(pos.x, pos.y - 100);
    }

    /// <summary>
    /// 瘟疫
    /// 筛选免疫力较低个体，降低幸存个体的颜值
    /// </summary>
    public void OnPlagueDown()
    {
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
        _apeMgr.maxAge = (_apeMgr.maxAge * 100 / 365) * 0.95f * 365 / 100;
        foreach (var ape in _apeMgr.apes.ToList())
        {
            ape.mutation *= 2;
        }
    }
}
