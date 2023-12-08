using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base class of ape
/// plz don't modify anything inside
/// you can add new func
/// </summary>
public abstract class Ape
{
    //
    // --- BASIC PARAMS ---//
    //
    
    /// <summary>
    /// Age of an ape<br></br>
    /// in first stage, max age would be 30<br></br>
    /// in second stage, max age would be 45<br></br>
    /// in third stage, max age would be 55<br></br>
    /// in fourth stage, max age would be 70
    /// </summary>
    protected int age;

    /// <summary>
    /// health point<br></br>
    /// begin at 100, down to 0 (dead)
    /// </summary>
    protected float health;

    /// <summary>
    /// health state<br></br>
    /// stage1: -30 pre day, will dead soon if ill<br></br>
    /// stage2: -14.3 pre day, will dead after a week<br></br>
    /// stage3: -8.5 pre day, maybe heal after times (related to immunity)<br></br>
    /// stage4: -2.5 pre day, maybe heal
    /// </summary>
    protected bool ill;

    /// <summary>
    /// 1(true): male<br></br>
    /// 0(false): female
    /// </summary>
    protected bool gender;
    
    //
    // --- GENE PARAMS --- //
    //

    /// <summary>
    /// P in illness, and P to be healed after ill
    /// </summary>
    protected float immunity;

    /// <summary>
    /// related to it's gene<br></br>
    /// high intelligence can live better in terrible environment
    /// </summary>
    protected float intelligence;

    /// <summary>
    /// P in gene mutation
    /// mutation can be good or bad
    /// </summary>
    protected float mutation;

    /// <summary>
    /// Charisma, it means, beautiful/handsome or not.<br></br>
    /// low: ugly; high: cool<br></br>
    /// P in met with different gender and make baby (what
    /// </summary>
    protected float charisma;

    // protected List<Gene> gene_text;

    /// <summary>
    /// new born baby func<br></br>
    /// Obtaining a gene from parents<br></br>
    /// randomly selecting a gene to mutate based on the mutation rate of the parents
    /// </summary>
    public void NewGene(Ape father, Ape mother)
    {
        
    }

    /// <summary>
    /// calculate the gene parameters based on gene
    /// </summary>
    public void CalculateGeneParams()
    {
        
    }
}

public class ApeStage1 : Ape
{
    
}


