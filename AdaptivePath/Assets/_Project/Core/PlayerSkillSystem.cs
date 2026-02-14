using UnityEngine;
using System.Collections.Generic;

public class PlayerSkillSystem : MonoBehaviour
{

    public List<Skills> skills = new List<Skills>();

    public void UseSkill(string swordsmanship, TrainingType type)
    {

        Skills skill = skills.Find(s => s.Swordsmanship == swordsmanship);

        if (skill != null)
        {

            skill.GainXP(10, type);

        }
    }
}
