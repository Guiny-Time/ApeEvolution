using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

/// <summary>
/// ape 总的管理类
/// 
/// </summary>
public class ApeMgr : BaseMgr<ApeMgr>
{
    // 管理所有ape
    public List<Ape> apes = new List<Ape>();
    // 发情期的ape
    public List<Ape> f_oes_apes = new List<Ape>();
    public List<Ape> m_oes_apes = new List<Ape>();

    // 阶段性基因，指的是当前阶段猩猩个体携带的基因数，为4，8，16，32（遵循2^(n+1)，32时为人类，到这一阶段就胜利了，所以不会显示）
    // todo: 在评分达到下一阶段时更新该数值
    public int geneStage = 4;

    public void AddApe(Ape a)
    {
        // 加入总列表
        apes.Add(a);

        // 处于发情期则加入求偶列表
        if (a.InOestrus())
        {
            AddOesApe(a);
        }
    }

    /// <summary>
    /// 猩猩进入发情期时添加
    /// </summary>
    /// <param name="a"></param>
    /// <param name="mark"></param>
    // todo: 世界时钟建立后，记得在ape成年时将其加入求偶列表
    // todo: 另外，在母猩猩生完之后也可以再加入求偶列表
    public void AddOesApe(Ape a)
    {
        if (a.gender == 0)
        {
            f_oes_apes.Add(a);
            f_oes_apes.Sort((x, y) =>  y.overall_attractiveness_point.CompareTo(x.overall_attractiveness_point));
        }
        else
        {
            m_oes_apes.Add(a);
            m_oes_apes.Sort((x, y) =>  y.overall_attractiveness_point.CompareTo(x.overall_attractiveness_point));
        }
    }

    /// <summary>
    /// 猩猩死亡时调用
    /// </summary>
    /// <param name="a"></param>
    public void RemoveApe(Ape a)
    {
        apes.Remove(a);
        if (a.gender == 0)
        {
            if(f_oes_apes.Contains(a))
                f_oes_apes.Remove(a);
        }
        else
        {
            if(m_oes_apes.Contains(a))
                m_oes_apes.Remove(a);
        }
    }

    /// <summary>
    /// 母猩猩怀孕时，需要从发情列表中移除
    /// </summary>
    /// <param name="a"></param>
    public void RemoveOesApe(Ape a)
    {
        if (a.gender == 0)
        {
            f_oes_apes.Remove(a);
        }
        else
        {
            m_oes_apes.Remove(a);
        }
    }

    /// <summary>
    /// 寻找最受欢迎的猩猩
    /// </summary>
    /// <param name="a"></param>
    public Vector3 FindApeMost(Ape a)
    {
        if (a.gender == 0)
        {
            if (m_oes_apes == null)
            {
                return m_oes_apes[0].gameObject.transform.position;
            }
            else
            {
                return new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);
            }
        }
        else
        {
            return (f_oes_apes == null) 
                ? f_oes_apes[0].gameObject.transform.position 
                : new Vector3(Random.Range(-18, 18), Random.Range(-8, 8), 0);;
        }
    }
}
