using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SkillsManager : MonoBehaviour
{

    public static SkillsManager Instance;
        
    public List<SkillsData> allSkills = new List<SkillsData>();

    void Awake()
    { 
        
        Instance = this;
        LoadAllSkills();
    
    }

    void LoadAllSkills()
    {
        SkillsData[] loadedSkills = Resources.LoadAll<SkillsData>("Skills");

        allSkills.Clear();
        allSkills.AddRange(loadedSkills);

        Debug.Log($"Skills carregadas: {allSkills.Count}");

    }
}
