using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeneMgr : BaseMgr<GeneMgr>
{
    public TextAsset geneFile;
    // Start is called before the first frame update
    void Start()
    {
        GeneDataBase geneDBJson = JsonUtility.FromJson<GeneDataBase>(geneFile.text);

        foreach (Gene employee in geneDBJson.geneDatabase)
        {
            Debug.Log("Found employee: " + employee.name + " " + employee.id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
