using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UpGames.ProceduralGeneration
{
    public static class NameGenerator
    {
        static float firstLetterConsonancy = 0.8f;
        
        public static string vowelList = "aeiouy";
        public static string consonantList = "bcdfghjklmnpqrstvwxz";
        public static string GetRandomName(string generationStyle, int wordCount = 1, int maxLettersPerWord = 7)
        {
            string randomName = "";
            for (int i = 0; i < wordCount; i++)
            {
                randomName += GetRandomString(Random.Range(3, maxLettersPerWord)) + ((i < wordCount -1) ? " ": "");
            }
            return randomName;
        }
        static string GetRandomString(int letterCount)
        {
            letterCount--;
            string randomString = "";
            bool consonant = Check();
            int countC = 0;
            int countV = 0;
            if (consonant)
            {
                countC++;
            }
            else
            {
                countV++;
            }
            randomString += GetChar(consonant);
            for(int i = 0; i < letterCount; i++)
            {
                if (countC > 0)
                {
                    consonant = Check(0.2f);
                }
                else if (countV > 0)
                {
                    consonant = Check(0.8f);
                }
                randomString += GetChar(consonant);
                if (consonant)
                {
                    countC++;
                    countV = 0;
                }
                else
                {
                    countV++;
                    countC = 0;
                }            
            }
            return randomString;
            
        }
        static char GetChar(bool consonant)
        {
            if (consonant)
            {
                return consonantList[Random.Range(0, consonantList.Length)];
            }
            else
            {
                return vowelList[Random.Range(0, vowelList.Length)];
            }
        }
        static bool Check(float chance = 0.8f)
        {
            float r = Random.Range(0f, 1f);
            if (r < chance)
            {
                return true;
            }
            return false;
        }
        
    }
}


