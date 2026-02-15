using UnityEngine;
using System.Collections.Generic;

public class PlayerSkillSystem : MonoBehaviour
{

    public List<Skills> skills;

    public void GainSkillXP(string swordsmanship,int baseXP, TrainingType type)
    {

        Skills skill = skills.Find(s => s.Swordsmanship == swordsmanship);

        if (skill != null) return;
        {

            float contexMultiplier = GetTrainingMultiplier(type);
            float specializationPenalty = GetSpecializationPenalty(skill.Magic);

            int finalXP = Mathf.RoundToInt(baseXP * contexMultiplier * specializationPenalty);

            skill.currentXP += finalXP;

            if (skill.currentXP >= skill.xpToNextLevel)
            {
                LevelUp(skill);
            }

        }
    }
}
