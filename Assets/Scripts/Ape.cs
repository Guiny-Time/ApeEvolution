using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
    public int overall_attractiveness_point;


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

    // protected List<Gene> gene_text;
    
    
    //
    // --- EVENT PARAMS --- //
    //
    
    
    /// <summary>
    /// 是否处于发情期。母猩猩性成熟且未怀孕时处于发情期；公猩猩性成熟后一直处于发情期
    /// </summary>
    [Header("Event Parameters")]
    [SerializeField] private bool inOestrus;


    private void Start()
    {
        // inOestrus = (sexualMaturity && !pregnant);
        // ApeMgr.GetInstance().AddApe(this, overall_attractiveness_point);
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
        age = Random.Range(5, 25);  // 随机生成5~24岁的猩猩
        sexualMaturity = (age >= 16);
        gender = Random.Range(0, 2);
        health = 100.0f;
        ill = false;
        pregnant = false;
        overall_attractiveness_point = Random.Range(0, 100);
        ApeMgr.GetInstance().AddApe(this, overall_attractiveness_point);
        // 基因
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
    /// 第一代apes的基因分配 (新游戏时new的)
    /// </summary>
    public void NewGene()
    {
        print("This is new gene func");
    }

    /// <summary>
    /// 计算个体基因积分 calculate the gene parameters based on gene
    /// </summary>
    public void CalculateGeneParams()
    {
        
    }

    public void Move_Normal()
    {
        //漫无目的的乱动
        StartCoroutine(MoveLerp(1,transform.position,0.5f));
            
    }
    
    public void Move_Oestrus()
    {
        /*if (MainController.GetInstance().f_apes.Peek().gameObject)
        {
            print("Oestrus move");
            //朝最佳配偶移动
            StartCoroutine(MoveLerpOestrus(1,transform.position, MainController.GetInstance().f_apes.Peek().gameObject.transform.position));

        }*/
          
    }

    /// <summary>
    /// 判断是否处于发情期
    /// </summary>
    /// <returns></returns>
    public bool InOestrus()
    {
        return (sexualMaturity && !pregnant);
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

    IEnumerator MoveLerpOestrus(float speed, Vector3 startPos, Vector3 endPos)
    {
        float i = 0.0f;
        float time = Vector3.Distance(startPos, endPos)/speed;
        float rate = 1.0f/time;

        while (i < 1.0f) {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }

        StartCoroutine(MoveLerpOestrus(1, transform.position, endPos));
    }

}


