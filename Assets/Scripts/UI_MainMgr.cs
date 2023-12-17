using System.Collections;
using System.Collections.Generic;
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
        emotion.sprite = _apeMgr.apes.Count switch
        {
            > 100 => Resources.Load<Sprite>("emo5"),
            > 80 => Resources.Load<Sprite>("emo4"),
            > 50 => Resources.Load<Sprite>("emo3"),
            > 20 => Resources.Load<Sprite>("emo2"),
            _ => Resources.Load<Sprite>("emo1")
        };
        genePoints.value = _apeMgr.CalAllGenePoints();
    }

    public void OnPointerEnter(GameObject o)
    {
        o.transform.GetChild(0).gameObject.SetActive(true);
        var UIPos = o.GetComponent<RectTransform>();
        var pos = UIPos.anchoredPosition;
        UIPos.anchoredPosition = new Vector2(pos.x, pos.y + 100);
    }
    
    public void OnPointerExit(GameObject o)
    {
        o.transform.GetChild(0).gameObject.SetActive(false);
        var UIPos = o.GetComponent<RectTransform>();
        var pos = UIPos.anchoredPosition;
        UIPos.anchoredPosition = new Vector2(pos.x, pos.y - 100);
    }
}
