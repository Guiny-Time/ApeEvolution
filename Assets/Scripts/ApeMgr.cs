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


    public void AddApe(Ape a, int mark)
    {
        // 加入总列表
        apes.Add(a);

        // 处于发情期则加入求偶列表
        if (a.InOestrus())
        {
            AddOesApe(a,mark);
        }
    }

    /// <summary>
    /// 猩猩进入发情期时添加
    /// </summary>
    /// <param name="a"></param>
    /// <param name="mark"></param>
    // todo: 世界时钟建立后，记得在ape成年时将其加入求偶列表
    // todo: 另外，在母猩猩生完之后也可以再加入求偶列表
    public void AddOesApe(Ape a, int mark)
    {
        if (a.gender == 0)
        {
            f_oes_apes.Add(a);
            f_oes_apes.Sort((x, y) =>  y.overall_attractiveness_point.CompareTo(x.overall_attractiveness_point));
            print(f_oes_apes[0].overall_attractiveness_point);
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
    public GameObject FindApeMost(Ape a)
    {
        if (a.gender == 0)
        {
            return m_oes_apes[0].gameObject;
        }
        else
        {
            return f_oes_apes[0].gameObject;
        }
    }
}
