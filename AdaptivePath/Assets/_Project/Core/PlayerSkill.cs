using System.Runtime.CompilerServices;
using UnityEngine;



[System.Serializable]


public class PlayerSkill
{

    public SkillsData data;

    public int level = 1;
    public int currentXP = 0;
    public int XPToNextLevel;

    public SkillState state = SkillState.Neutral;

    public int penaltyStacks = 0;
    public int specializationTier = 0;

    public int lastMilestoneReached = 0;

    private PlayerSkillSystem skillSystem;

    public PlayerSkill(SkillsData skillsData, PlayerSkillSystem system)
    {

        data = skillsData;
        XPToNextLevel = data.baseXPToNextLevel;

    }

    public void AddXP(int amount)
    {

       currentXP += amount;

       while (currentXP >= XPToNextLevel)
        {

            LevelUp();

        }

        
    }

    void LevelUp()
    {

        level++;
        currentXP -= XPToNextLevel;
        XPToNextLevel = Mathf.RoundToInt(XPToNextLevel * data.growthMultiplier);

        Debug.Log(data.skillName + "subiu para nìvel" + level);

        skillSystem.NotifyMilestone(this);

    }
}