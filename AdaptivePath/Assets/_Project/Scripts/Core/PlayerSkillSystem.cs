using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class PlayerSkillSystem : MonoBehaviour
{

    
    public List<PlayerSkill> playerSkills = new List<PlayerSkill>();
    private List<PlayerSkill> milestoneCandidates = new List<PlayerSkill>();

    private bool milestoneWindowActive = false;
    private float milestoneTimer = 0f;

    private bool dailyWindowActive = false;
    private float dailyTimer = 0f;

    int penaltyTier = 0;

    private int currentMilestone = 0;

    [SerializeField] private SkillsData swordSkill;
    [SerializeField] private SkillsData fireSkill;

    void Start()
    {
        SkillsManager manager = FindFirstObjectByType<SkillsManager>();

        if (manager == null)
        {

            Debug.LogError("SkillsMnager não encontrado na cena!");
            return;
        
        }

        foreach (SkillsData skill in manager.allSkills)
        {

            playerSkills.Add(new PlayerSkill(skill, this));

        }

        Debug.Log("PlayerSkillSystem inicializado com " + playerSkills.Count + "skills");
        
    }

    public void GainSkillXP(SkillsData skillName, int baseXP, TrainingType type)
    {

        PlayerSkill skill = playerSkills.Find(s => s.data == skillName);

        if (skill == null) return;
        {

            float trainingMultiplier = GetTrainingMultiplier(type);
            float specializationPenalty = GetSpecializationPenalty(skill.data.group);

            int finalXP = Mathf.RoundToInt(baseXP * trainingMultiplier * specializationPenalty);

            skill.AddXP(finalXP);

            Debug.Log("Tentando gahar XP em " + skillName);

            if (skill == null)
            {

                Debug.Log("Skill não encontrada: " + skillName);
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
    }

        void StartDailyWindow()
        {

            dailyWindowActive = true;

            dailyTimer = 600f;

        }

        void EndDailyWindow()
        {

            dailyWindowActive = false;

            foreach (PlayerSkill skill in playerSkills)
            {
                if (skill.state == SkillState.Neutral && skill.level >= currentMilestone)
                {

                    skill.state = SkillState.Penalized;
                    skill.penaltyStacks = penaltyTier;

                    Debug.Log(skill.data.skillName + "foi penalizada no fim do dia");

            }
            }

        }
    

    public void NotifyReached25(PlayerSkill skill)
    {

        if (!milestoneWindowActive)
        {

            Debug.Log("Janela de especialização iniciada");
            milestoneWindowActive = true;
            milestoneTimer = 60f;
            milestoneCandidates.Clear();

        }

        if (!milestoneCandidates.Contains(skill))
        { 
            
            milestoneCandidates.Add(skill);
            Debug.Log(skill.data.skillName + "entrou como candidata.");

        }

    }

    void Update()
    {
        if (milestoneWindowActive)
         {
             milestoneTimer -= Time.deltaTime;

             if (milestoneTimer <= 0f)
             {
                milestoneTimer = 0f;
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

        Debug.Log("Timer milestone" + Mathf.Ceil(milestoneTimer));
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

        if (!milestoneCandidates.Contains(skill))
        {
            Debug.Log(skill.data.skillName + "entrou como candidata.");
            milestoneCandidates.Add(skill);
        }
    }

    void EndMilestoneWindow()
    {
        milestoneWindowActive = false;
        milestoneTimer = 0f;

        Debug.Log("Janela encerrada. Total candidatos" + milestoneCandidates.Count);

        
        foreach (var skill in milestoneCandidates)
        {

          skill.state = SkillState.Specialized;
          skill.specializationTier = 1;

          Debug.Log(skill.data.skillName + "Virou Specialized!");
        }

        foreach(var skill in playerSkills)
        {

            if (skill.level >= 25 && skill.state != SkillState.Specialized)
            {

              skill.state = SkillState.Penalized;
              Debug.Log(skill.data.skillName + "travada em 25");

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
    
        GainSkillXP(swordSkill, 999999, TrainingType.Combat);

    }

    [ContextMenu("Test Fire XP")]
    void TestFireXP()
    {

        GainSkillXP(fireSkill, 100, TrainingType.Combat);

    }

}