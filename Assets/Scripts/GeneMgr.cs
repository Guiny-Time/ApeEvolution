using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeneMgr : BaseMgr<GeneMgr>
{
    public TextAsset geneFile;

    public GeneDataBase geneDBJson;
    // Start is called before the first frame update
    void Start()
    {
        geneDBJson = JsonUtility.FromJson<GeneDataBase>(geneFile.text);

        /*foreach (Gene employee in geneDBJson.geneDatabase)
        {
            Debug.Log("Found gene: " + employee.name + " " + employee.id);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
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
        for (int i = 0; i < requireNum; i++)
        {
            int index = Random.Range(0, geneDBJson.geneDatabase.Length-1);
            result.Add(geneDBJson.geneDatabase[index]);
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
