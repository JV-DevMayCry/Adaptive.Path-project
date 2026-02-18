using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class PlayerSkillSystem : MonoBehaviour
{

    public List<SkillsData> allSkills;
    public List<PlayerSkill> playerSkills = new List<PlayerSkill>();
    private List<PlayerSkill> milestoneCandidates = new List<PlayerSkill>();

    private bool milestoneWindowActive = false;
    private float milestoneTimer = 0f;

    private bool dailyWindowActive = false;
    private float dailyTimer = 0f;

    private int currentMilestone = 0;

    void Start()
    {

        foreach (SkillsData skill in allSkills)
        {

            playerSkills.Add(new PlayerSkill(skill, this));

        }

    }

    public void GainSkillXP(string skillName, int baseXP, TrainingType type)
    {

        PlayerSkill skill = playerSkills.Find(s => s.data.skillName == skillName);

        if (skill == null) return;
        {

            float trainingMultiplier = GetTrainingMultiplier(type);
            float specializationPenalty = GetSpecializationPenalty(skill.data.group);

            int finalXP = Mathf.RoundToInt(baseXP * trainingMultiplier * specializationPenalty);

            skill.AddXP(finalXP);


        }

        float GetSpecializationPenalty(SkillGroup targetGroup)
        {
            if (targetGroup == SkillGroup.Crafting)
                return 1f;

            SkillGroup oppositeGroup = targetGroup == SkillGroup.Weapons ? SkillGroup.Magic : SkillGroup.Weapons;

            int highestOppositeLevel = 0;

            foreach (PlayerSkill skill in playerSkills)
            {
                if (skill.data.group == oppositeGroup)
                {

                    if (skill.level > highestOppositeLevel)
                        highestOppositeLevel = skill.level;

                }
            }

            float penalty = 1f / (1f + highestOppositeLevel * 0.01f);


            return Mathf.Clamp(penalty, 0.2f, 1f);
        }

        float GetTrainingMultiplier(TrainingType type)
        {

            switch (type)
            {
                case TrainingType.Solo:
                    return 1f;

                case TrainingType.Combat:
                    return 1.5f;

                case TrainingType.Instructor:
                    return 2f;

                default:
                    return 0f;
            }
            ;


        }



    }

    void StartDailyWindow()
    {

        dailyWindowActive = true;

        dailyTimer = 600f;
    
    }

    void EndDailyWindow()
    {

        dailyWindowActive = false;

        foreach(PlayerSkill skill in playerSkills)
        {
            if(skill.state == SkillState.Neutral)
            {

                skill.state = SkillState.Penalized;
                skill.penaltyStacks++;

            }
        }

    }
    public void NotifyMilestone(PlayerSkill skill)
    {

        Debug.Log("Milestone atingido por:" + skill.data.skillName + "nivel" + skill.level);

        currentMilestone = skill.level;

        StartMilestoneWindow(skill);
        StartDailyWindow();

    }

    void Update()
    {
         {
             milestoneTimer -= Time.deltaTime;

             if (milestoneTimer <= 0f)
             {
                 EndMilestoneWindow();
             }
         }
         if (dailyWindowActive)
         {
             dailyTimer -= Time.deltaTime;
             if (dailyTimer <= 0f)
             {
                 EndDailyWindow();
             }
         }
    }

    void StartMilestoneWindow(PlayerSkill skill)
    {
        if (!milestoneWindowActive)
        {
            Debug.Log("Janela de especialização iniciada!");
            milestoneWindowActive = true;
            milestoneTimer = 60f;
            milestoneCandidates.Clear();
            currentMilestone = skill.level;
        }

        if (skill.level == currentMilestone)
        {
            Debug.Log(skill.data.skillName + "entrou como candidata.");
            milestoneCandidates.Add(skill);
        }
    }

    void EndMilestoneWindow()
    {
        milestoneWindowActive = false;

        Debug.Log("Janela encerrada. Total candidatos" + milestoneCandidates.Count);

        if (milestoneCandidates.Count > 1)
        {
            foreach (var skill in milestoneCandidates)
            {

                skill.state = SkillState.Specialized;
                skill.specializationTier++;

                Debug.Log(skill.data.skillName + "Virou Specialized!");
            }
        }
    }

    public void CancelMilestoneWindow()
    {
        milestoneWindowActive = false;

    }

    [ContextMenu("Test Sword XP")]
    void TestSwordXP()
    {
    
        GainSkillXP("Swordsmanship", 1000, TrainingType.Combat);

    }

    [ContextMenu("Test Fire XP")]
    void TestFireXP()
    {

        GainSkillXP("Fire Magic", 1000, TrainingType.Combat);

    }

}