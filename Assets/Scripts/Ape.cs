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
    [Header("- Basic Parameters -")]
    [SerializeField]private float age;

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
    [Header("- Gene Parameters -")]
    public float immunity;

    /// <summary>
    /// 智力related to it's gene<br></br>
    /// high intelligence can live better in terrible environment
    /// </summary>
    public float intelligence;

    /// <summary>
    /// 变异率P in gene mutation
    /// mutation can be good or bad
    /// </summary>
    public float mutation;

    /// <summary>
    /// 魅力Charisma, it means, beautiful/handsome or not.<br></br>
    /// low: ugly; high: cool<br></br>
    /// P in met with different gender and make baby (what
    /// </summary>
    public float charisma;

    /// <summary>
    /// 猩猩个体持有的基因列表<br></br>
    /// gene list that this ape has.
    /// </summary>
    public List<Gene> genes = new List<Gene>();
    
    
    //
    // --- OBJECT PARAMS --- //
    //

    /// <summary>
    /// 贴图颜色与猩猩性别相关<br></br>
    /// male: FFA700<br></br>
    /// female: F8495E
    /// </summary>
    [Header("- Object Parameters -")]
    public SpriteRenderer genderColor;
    
    /// <summary>
    /// 猩猩大小与年龄相关<br></br>
    /// 越老越大
    /// </summary>
    public Transform ageSize;

    
    /* --- EVENT PARAMS --- */
    
    private bool hasMatted; // 是否已经交配
    private Ape lover;
    private float p_startTime;      // 怀孕周期计时器
    private float g_startTime;      // 成长周期计时器
    private Vector3 target;         // normal move 目标

    private ApeMgr _apeMgr;
    private GeneMgr _geneMgr;

    private void Awake()
    {
        hasMatted = false;
        lover = null;
        target = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
        _apeMgr = ApeMgr.GetInstance();
        _geneMgr = GeneMgr.GetInstance();

        SexColor();
    }

    private void Update()
    {
        ageSize.localScale = new Vector3(0.5f + (age-1) * 0.005f,0.5f + (age-1)*0.005f,0.5f + (age-1)*0.005f);  // 年龄特征（大小）
        GrowthCycle();
        if (sexualMaturity && !pregnant && !_apeMgr.ExistentialPressure())
        {
            Move_Oestrus();
        }
        else
        {
            Move_Normal();
        }

        if (pregnant)
        {
            PregnancyCycle();
        }
    }

    /// <summary>
    /// 游戏开始时初始化随机个体
    /// </summary>
    public void InitApe()
    {
        age = Random.Range(5, 25);      // 随机生成5~24岁的猩猩
        g_startTime = Time.time - (age * 3.6f);
        sexualMaturity = (age >= 16);   // 判断是否性成熟
        gender = Random.Range(0, 2);    // 随机性别
        health = 100.0f;                // 初始健康100
        ill = false;                    // 初始未生病
        pregnant = false;               // 初始未怀孕

        SexColor();
        ageSize.localScale = new Vector3(0.5f + (age-1) * 0.005f,0.5f + (age-1)*0.005f,0.5f + (age-1)*0.005f);  // 年龄特征（大小）

        genes = GeneMgr.GetInstance().GenerateGeneList(_apeMgr.geneStage);  // 初始基因组
        CalculateGeneParams();         // 计算综合得分

        _apeMgr.AddApe(this);  // 加入猩猩管理器
    }
    
    /// <summary>
    /// 指定性别的初始化
    /// </summary>
    public void InitApe(int g)
    {
        age = Random.Range(5, 25);      // 随机生成5~24岁的猩猩
        g_startTime = Time.time - (age * 3.6f);
        sexualMaturity = (age >= 16);   // 判断是否性成熟
        gender = g;                     // 指定性别
        health = 100.0f;                // 初始健康100
        ill = false;                    // 初始未生病
        pregnant = false;               // 初始未怀孕

        SexColor();
        ageSize.localScale = new Vector3(0.5f + (age-1) * 0.005f,0.5f + (age-1)*0.005f,0.5f + (age-1)*0.005f);  // 年龄特征（大小）

        genes = GeneMgr.GetInstance().GenerateGeneList(_apeMgr.geneStage);  // 初始基因组
        CalculateGeneParams();         // 计算综合得分

        _apeMgr.AddApe(this);  // 加入猩猩管理器
    }

    /// <summary>
    /// 初始化新生儿数据
    /// </summary>
    public void InitBabyData()
    {
        age = 0;
        g_startTime = Time.time;
        gender = Random.Range(0, 2);
        health = 100.0f;
        ill = false;
        pregnant = false;

        SexColor();
        ageSize.localScale = new Vector3(0.5f,0.5f,0.5f);

        _apeMgr.AddApe(this);
    }

    /// <summary>
    /// 子代基因分配 new born baby func<br></br>
    /// Obtaining a gene from parents<br></br>
    /// randomly selecting a gene to mutate based on the mutation rate of the parents
    /// </summary>
    public void InstanceNewBabyApe()
    {
        PoolMgr.GetInstance().GetObj("Ape",  o =>
        {
            var position = transform.position;
            o.transform.position = new Vector3(position.x + Random.Range(-2, 2), position.y + Random.Range(-2, 2), 0);
            o.transform.parent = MainController.GetInstance().apeContainer.transform;
            o.GetComponent<Ape>().InitBabyData();
            o.GetComponent<Ape>().genes = _geneMgr.GenerateGeneListFromParent(this, lover, _apeMgr.geneStage);// 新生儿基因组
            o.GetComponent<Ape>().CalculateGeneParams();
        });
    }

    /// <summary>
    /// 计算个体基因积分 calculate the gene parameters based on gene
    /// </summary>
    public void CalculateGeneParams()
    {
        foreach (var g in genes)
        {
            immunity += g.immunity;
            intelligence += g.intelligence;
            charisma += g.charisma;
            mutation += g.mutation;
            //todo: 在后续阶段，智力也会作为影响综合吸引指数的一个因素
            overall_attractiveness_point += ((g.immunity + health + g.charisma) / age);
        }
    }
    

    /// <summary>
    /// 闲逛。孕妇、儿童、生病的人不会进入发情状态
    /// </summary>
    public void Move_Normal()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 0.005f);
        // 漫无目的地乱动
        if (transform.position == target)
        {
            target = new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
        }
        
    }
    
    /// <summary>
    /// 求爱路线
    /// </summary>
    public void Move_Oestrus()
    {
        var position = transform.position;
        StopAllCoroutines();
        //朝最佳配偶移动
        transform.position = Vector3.MoveTowards(position, _apeMgr.FindApeMost(this), 0.05f);
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
    /// 生长周期
    /// 
    /// </summary>
    public void GrowthCycle()
    {
        age = (Time.time - g_startTime) / 3.6f;
        if (Time.time - g_startTime >= 58.44f && !sexualMaturity)
        {
            // 猩猩性成熟
            sexualMaturity = true;
            _apeMgr.AddOesApe(this);
        }
        if (Time.time - g_startTime >= _apeMgr.maxAge)
        {
            // 猩猩死亡
            Death();
        }
    }

    /// <summary>
    /// 怀孕周期，2.43秒后产生新的猩猩
    /// </summary>
    public void PregnancyCycle()
    {
        if (Time.time - p_startTime >= 2.43f && !_apeMgr.ExistentialPressure())
        {
            if (!_apeMgr.ExistentialPressure()) //在生存压力大时，新生儿无法生存，或发生流产
            {
                // 诞下子代
                InstanceNewBabyApe();
            }
           
            _apeMgr.AddOesApe(this);
            pregnant = false;
            hasMatted = false;
            SexColor();
        }
    }

    /// <summary>
    /// 个体颜色
    /// </summary>
    public void SexColor()
    {
        ColorUtility.TryParseHtmlString( "#FFA700" , out Color maleColor );
        ColorUtility.TryParseHtmlString( "#F8495E" , out Color femaleColor );
        genderColor.color = (gender == 0) ? maleColor : femaleColor;    // 性别特征（颜色）
    }
    
    /// <summary>
    /// 猩猩死亡，重置一切变量，移除出管理器
    /// </summary>
    public void Death()
    {
        _apeMgr.RemoveApe(this);
        age = 0;
        gender = Random.Range(0, 2);
        health = 100.0f;
        ill = false;
        pregnant = false;
        sexualMaturity = false;
        genes.Clear();
        immunity = 0;
        intelligence = 0;
        charisma = 0;
        mutation = 0;
        overall_attractiveness_point = 0;
        p_startTime = 0;
        g_startTime = 0;
        hasMatted = false;
        lover = null;
        PoolMgr.GetInstance().PushObj("Ape", this.gameObject); // 回到对象池
    }
    
    /// <summary>
    /// 当异性apes接触且处于发情期时，生育后代
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        Ape ape = other.gameObject.GetComponent<Ape>();
        if ((ape.gender != this.gender) && ape.InOestrus() && !hasMatted && !_apeMgr.ExistentialPressure()) //对方是异性、处于发情期且当前暂无生存压力
        {
            if (gender == 1 && InOestrus())    // 如果该个体为母猩猩且性成熟，则进入怀孕周期
            {
                _apeMgr.RemoveOesApe(this);
                lover = other.gameObject.GetComponent<Ape>();
                p_startTime = Time.time;
                genderColor.color = Color.cyan;
                pregnant = true;
                hasMatted = true;
            }
        }
    }

}
