using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// base class of ape
/// plz don't modify anything inside
/// you can add new func
/// </summary>
public class Ape : MonoBehaviour
{
    
    //
    // --- BASIC PARAMS ---//
    //

    
    /// <summary>
    /// 年龄Age of an ape<br></br>
    /// in first stage, max age would be 30<br></br>
    /// in second stage, max age would be 45<br></br>
    /// in third stage, max age would be 55<br></br>
    /// in fourth stage, max age would be 70
    /// </summary>
    [Header("Basic Parameters")]
    [SerializeField]private int age;

    /// <summary>
    /// 性别 gender<br></br>
    /// 1(true): male<br></br>
    /// 0(false): female
    /// </summary>
    public int gender;

    /// <summary>
    /// 健康指数health point<br></br>
    /// begin at 100, down to 0 (dead)
    /// </summary>
    [SerializeField]private float health;

    /// <summary>
    /// 是否生病health state<br></br>
    /// stage1: -30 pre day, will dead soon if ill<br></br>
    /// stage2: -14.3 pre day, will dead after a week<br></br>
    /// stage3: -8.5 pre day, maybe heal after times (related to immunity)<br></br>
    /// stage4: -2.5 pre day, maybe heal
    /// </summary>
    [SerializeField]private bool ill;

    /// <summary>
    /// 是否怀孕pregnancy state<br></br>
    /// while pregnant, after 8 month there will be a new born ape<br></br>
    /// male ape will never pregnant<br></br>
    /// female can be pregnant after 16 years old
    /// </summary>
    [SerializeField] private bool pregnant;

    /// <summary>
    /// 是否性成熟 whether sexual maturity<br></br>
    /// ape can create baby after 16 years old (sexual maturity)
    /// </summary>
    [SerializeField] private bool sexualMaturity;

    /// <summary>
    /// 综合吸引指数 overall attractiveness point<br></br>
    /// 通过魅力(C)、免疫力(I)、健康指数(H)、年龄(A)综合计算得到<br></br>
    /// formula:(H+I)*C/A
    /// </summary>
    public float overall_attractiveness_point;


    //
    // --- GENE PARAMS --- //
    //

    
    /// <summary>
    /// 免疫力P in illness, and P to be healed after ill
    /// </summary>
    [Header("Gene Parameters")]
    [SerializeField]private float immunity;

    /// <summary>
    /// 智力related to it's gene<br></br>
    /// high intelligence can live better in terrible environment
    /// </summary>
    [SerializeField]private float intelligence;

    /// <summary>
    /// 变异率P in gene mutation
    /// mutation can be good or bad
    /// </summary>
    [SerializeField]private float mutation;

    /// <summary>
    /// 魅力Charisma, it means, beautiful/handsome or not.<br></br>
    /// low: ugly; high: cool<br></br>
    /// P in met with different gender and make baby (what
    /// </summary>
    [SerializeField]private float charisma;

    /// <summary>
    /// 猩猩个体持有的基因列表<br></br>
    /// gene list that this ape has.
    /// </summary>
    protected List<Gene> genes = new List<Gene>();
    
    
    //
    // --- OBJECT PARAMS --- //
    //

    /// <summary>
    /// 贴图颜色与猩猩性别相关<br></br>
    /// male: FFA700<br></br>
    /// female: F8495E
    /// </summary>
    [Header("Object Parameters")]
    public SpriteRenderer genderColor;
    
    /// <summary>
    /// 猩猩大小与年龄相关<br></br>
    /// 越老越大
    /// </summary>
    public Transform ageSize;

    private bool hasMatted; // 是否已经交配

    private void Start()
    {
        hasMatted = false;
    }

    private void Update()
    {
        if (sexualMaturity && !pregnant)
        {
            Move_Oestrus();
        }
        else
        {
            Move_Normal();
        }
    }

    public void InitApe()
    {
        age = Random.Range(5, 25);      // 随机生成5~24岁的猩猩
        sexualMaturity = (age >= 16);   // 判断是否性成熟
        gender = Random.Range(0, 2);    // 随机性别
        health = 100.0f;                // 初始健康100
        ill = false;                    // 初始未生病
        pregnant = false;               // 初始未怀孕
        
        ColorUtility.TryParseHtmlString( "#FFA700" , out Color maleColor );
        ColorUtility.TryParseHtmlString( "#F8495E" , out Color femaleColor );
        genderColor.color = (gender == 0) ? maleColor : femaleColor;    // 性别特征（颜色）
        ageSize.localScale = new Vector3(0.5f + (age-1) * 0.005f,0.5f + (age-1)*0.005f,0.5f + (age-1)*0.005f);  // 年龄特征（大小）

        CalculateGeneParams(4);         // 计算综合得分

        ApeMgr.GetInstance().AddApe(this);  // 加入猩猩管理器
        
    }

    /// <summary>
    /// 子代基因分配 new born baby func<br></br>
    /// Obtaining a gene from parents<br></br>
    /// randomly selecting a gene to mutate based on the mutation rate of the parents
    /// </summary>
    public void NewGene(Ape father, Ape mother)
    {
        
    }

    /// <summary>
    /// 计算个体基因积分 calculate the gene parameters based on gene
    /// </summary>
    public void CalculateGeneParams(int geneNum)
    {
        genes = GeneMgr.GetInstance().GenerateGeneList(geneNum);  // 初始基因组
        foreach (var g in genes)
        {
            overall_attractiveness_point += ((g.immunity + health + g.charisma) / age);
        }
    }

    /// <summary>
    /// 闲逛。孕妇、儿童、生病的人不会进入发情状态
    /// </summary>
    public void Move_Normal()
    {
        var position = transform.position;
        //漫无目的的乱动
        StartCoroutine(MoveLerp(1,position,0.5f));
            
    }
    
    /// <summary>
    /// 求爱路线
    /// </summary>
    public void Move_Oestrus()
    {
        var position = transform.position;
        StopAllCoroutines();
        //朝最佳配偶移动
        transform.position = Vector3.MoveTowards(position, ApeMgr.GetInstance().FindApeMost(this), 0.1f);
    }

    /// <summary>
    /// 判断是否处于发情期
    /// </summary>
    /// <returns></returns>
    public bool InOestrus()
    {
        return (sexualMaturity && !pregnant);
    }

    /// <summary>
    /// 生育后代
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        Ape ape = other.gameObject.GetComponent<Ape>();
        if ((ape.gender != this.gender) && ape.InOestrus() && !hasMatted) //对方是异性，并且处于发情期
        {
            if (gender == 1)    // 如果该个体为母猩猩，则进入怀孕周期
            {
                pregnant = true;
                hasMatted = true;
                // todo: 在怀孕周期结束、子代出生时，记得把trigger重置为false
            }
        }
    }

    IEnumerator MoveLerp(float speed, Vector3 startPos, float drunkenness)
    {
        float i = 0.0f;
        Vector3 endPos = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
        float time = Vector3.Distance(startPos, endPos)/speed;
        float rate = 1.0f/time;
        Vector3 drunkenStep = endPos + new Vector3(Random.Range(-drunkenness, drunkenness), Random.Range(-drunkenness, drunkenness), 0);

        float timer = 0.0f;
        while (i < 1.0f) {
            i += Time.deltaTime * rate;
            timer += Time.deltaTime; 
            if (timer >= 0.5f) {
                drunkenStep = endPos + new Vector3(Random.Range(-drunkenness, drunkenness), Random.Range(-drunkenness, drunkenness), 0);
                timer = 0.0f;
            }
            transform.position = Vector3.Lerp(startPos, drunkenStep, i);
            yield return null;
        }

        StartCoroutine(MoveLerp(1, transform.position, 0.5f));
    }


}


