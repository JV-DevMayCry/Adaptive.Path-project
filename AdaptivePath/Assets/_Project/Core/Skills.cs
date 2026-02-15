using UnityEngine;

[System.Serializable]
public class Skills
{

    public string Swordsmanship;
    public SkillGroup Weapons;
    public int level = 0;
    public int currentXP = 0;
    public int xpToNextLevel = 250;


    public void GainXP(int basAmount, TrainingType type)
    {
        float multiplier = GetMultiplier(type);

        int finalXP = Mathf.RoundToInt(basAmount * multiplier);
        currentXP += finalXP;

        if (currentXP >= xpToNextLevel)
        {

            LevelUp();

        }
    }

    float GetMultiplier(TrainingType type)
    {
         switch(type)
        {
            case TrainingType.Solo:
                return 1f;

            case TrainingType.Combat:
                return 1.5f;

            case TrainingType.Instructor:
                return 2f;

            default:
                return 0f;
        };
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);

        Debug.Log(Swordsmanship + "is now, level" + level);
    }
}