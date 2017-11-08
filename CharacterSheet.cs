using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet : MonoBehaviour {

    
        public int strengthScore;
        public int constitutionScore;
        public int dexterityScore;
        public int intelligenceScore;
        public int wisdomScore;
        public int charismaScore;
        public string ownerUsername;
        public string characterClass;
        public bool classIsAssigned = false;
        public string gender;

        public void AssignValues() {
            if (characterClass == "warrior")
            {
                strengthScore = 16;
                constitutionScore = 14;
                dexterityScore = 13;
                intelligenceScore = 11;
                wisdomScore = 10;
                charismaScore = 12;
            }

            else if (characterClass == "rogue")
            {
                strengthScore = 13;
                constitutionScore = 14;
                dexterityScore = 16;
                intelligenceScore = 12;
                charismaScore = 11;
                wisdomScore = 10;
            }
            else if (characterClass == "wizard")
        {
            strengthScore = 10;
            constitutionScore = 13;
            dexterityScore = 12;
            intelligenceScore = 16;
            charismaScore = 11;
            wisdomScore = 14;

        }
            else {
                strengthScore = 14;
                constitutionScore = 16;
                dexterityScore = 13;
                intelligenceScore = 12;
                charismaScore = 10;
                wisdomScore = 11;
            }
        }

    public string CharacterStats() {
        string charStats = "Owner/Name: " + ownerUsername + " | Gender: " + gender + " | Class: " + characterClass + " | Strength: " + strengthScore + " | Dexterity: " + dexterityScore + " | Constitution: " + constitutionScore + " | Intelligence: " + intelligenceScore + " | Wisdom: " + wisdomScore + " | Charisma: " + charismaScore;
        return charStats;
        }

     
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public int GetAbilityScoreModifier(int abilityScore) {
        if (abilityScore < 2)
        {
            return -5;
        }
        else if (abilityScore < 4)
        {
            return -4;
        }
        else if (abilityScore < 6)
        {
            return -3;
        }
        else if (abilityScore < 8)
        {
            return -2;
        }
        else if (abilityScore < 10)
        {
            return -1;
        }
        else if (abilityScore < 12)
        {
            return 0;
        }
        else if (abilityScore < 14)
        {
            return 1;
        }
        else if (abilityScore < 16)
        {
            return 2;
        }
        else if (abilityScore < 18)
        {
            return 3;
        }
        else if (abilityScore < 20)
        {
            return 4;
        }
        else if (abilityScore < 22)
        {
            return 5;
        }
        else if (abilityScore < 24)
        {
            return 6;
        }
        else if (abilityScore < 26)
        {
            return 7;
        }
        else if (abilityScore < 28)
        {
            return 8;
        }
        else if (abilityScore < 30)
        {
            return 9;
        }
        else if (abilityScore < 32)
        {
            return 10;
        }
        else if (abilityScore < 34)
        {
            return 11;
        }
        else {
            return 12;
        }
    }
}
    

