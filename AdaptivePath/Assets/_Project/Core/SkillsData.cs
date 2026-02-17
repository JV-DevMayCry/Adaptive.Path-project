using UnityEngine;


[CreateAssetMenu(fileName = "NewSkill", menuName = "RPG/Skill")]


public class SkillsData : ScriptableObject
{

    public string skillName;
    public SkillGroup group;
    public string description;

    public int baseXPToNextLevel = 250;
    public float growthMultiplier = 1.4f;

}
