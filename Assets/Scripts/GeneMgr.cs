using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;

public class GeneMgr : BaseMgr<GeneMgr>
{
    public TextAsset geneFile;

    public GeneDataBase geneDBJson;
    // Start is called before the first frame update
    void Start()
    {
        geneDBJson = JsonUtility.FromJson<GeneDataBase>(geneFile.text);
    }

    /// <summary>
    /// 根据需要的数量生成基因词条列表
    /// </summary>
    /// <param name="requireNum"></param>
    /// <returns></returns>
    public List<Gene> GenerateGeneList(int requireNum)
    {
        List<Gene> result = new List<Gene>();
        geneDBJson = JsonUtility.FromJson<GeneDataBase>(geneFile.text);
        while (result.Count < requireNum)
        {
            int index = Random.Range(0, geneDBJson.geneDatabase.Length-1);
            var newGene = geneDBJson.geneDatabase[index];
            if (!result.Contains(newGene))
            {
                result.Add(newGene);
            }
        }

        return result;
    }

    /// <summary>
    /// 子代出生时生成的基因列表
    /// </summary>
    /// <param name="father">父亲</param>
    /// <param name="mother">母亲</param>
    /// <param name="requireNum">需要的基因个数</param>
    /// <returns></returns>
    public List<Gene> GenerateGeneListFromParent(Ape father, Ape mother, int requireNum)
    {
        var result = new List<Gene>();
        geneDBJson = JsonUtility.FromJson<GeneDataBase>(geneFile.text);
        var mutation = (father.mutation + mother.mutation) / 2;

        while (result.Count < requireNum)
        {
            float isMutation = Random.Range(0, requireNum * 100 + 1);
            Gene nextGene;
            if (isMutation < mutation)  // 发生1个突变
            {
                var index = Random.Range(0, geneDBJson.geneDatabase.Length-1);
                nextGene = geneDBJson.geneDatabase[index];
            }
            else
            {
                if (mother.genes.Count!=0)
                {
                    var index = Random.Range(0, requireNum);
                    var fom = Random.Range(0, 2);

                    nextGene = (fom == 0) ? father.genes[index] : mother.genes[index];
                }
                else
                {
                    var index = Random.Range(0, requireNum);
                    nextGene = father.genes[index];
                }
            }
            if (!result.Contains(nextGene))
            {
                result.Add(nextGene);
            }
        }
        return result;
    }


}

[System.Serializable]
public class Gene
{
    public string name;
    public int id;
    public int immunity;
    public int intelligence;
    public int charisma;
    public int mutation;
}

[System.Serializable]
public class GeneDataBase
{
    public Gene[] geneDatabase;
    // public Gene[] ExistGene;
    // public Gene[] LockedGene;
}
