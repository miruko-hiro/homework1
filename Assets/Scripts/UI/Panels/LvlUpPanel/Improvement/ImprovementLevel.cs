using System;
using UnityEngine;

namespace UI.Panels.LvlUpPanel.Improvement
{
    public class ImprovementLevel
    {
        private string _textTypeOne = "Lvl {0}: {1} dmg";
        private string _upTextTypeOne = "Next {0}: +{1} dmg";
        
        private string _textTypeFour = "Lvl {0}: {1} dmg {2}s cd";
        private string _upTextTypeFour  = "Next {0}: +{1} dmg -{2}s cd";
        
        public ImprovementData GetImprovementData(ImprovementType type, int level) => type switch
        {
            ImprovementType.One => GetImprovementDataTypeOne(level),
            ImprovementType.Two => GetImprovementDataTypeOne(level),
            ImprovementType.Three => GetImprovementDataTypeOne(level),
            ImprovementType.Four => GetImprovementDataTypeFour(level),
            _ => GetImprovementDataTypeOne(level)
        };

        private ImprovementData GetImprovementDataTypeOne(int level)
        {
            int money = 5;
            int spentMoney = 5;
            int nextLevel = level + 1;
            int damage = 1;
            int damageText = 1;
            int nextDamageText = 1;

            if (level == 0)
            {
                damageText = 0;
            }
            else if (level < 9)
            {
                damageText = level;
            } 
            else if (level == 9)
            {
                damage = 2;
                damageText = level;
                nextDamageText = 2;
                money *= 2;
            }
            else if (level < 19)
            {
                damage = 2;
                damageText = 9 + (level * 2);
                nextDamageText = 2;
                money *= 2;
                spentMoney *= 3;
            } else if (level == 19)
            {
                damage = 3;
                damageText = 9 + (level * 2);
                nextDamageText = 3;
                money *= 3;
                spentMoney *= 3;
            }
            
            return new ImprovementData(
                GetText(_textTypeOne, level, damageText), 
                GetUpText(_upTextTypeOne, nextLevel, nextDamageText), 
                money,
                spentMoney,
                damage,
                0);
        }

        private ImprovementData GetImprovementDataTypeFour(int level)
        {
            int money = 10;
            int spentMoney = 10;
            int nextLevel = level + 1;
            int damage = 10;
            int damageText = 10;
            int nextDamageText = 10;
            int cooldown = 5;
            int nextCooldown = 1;

            switch (level)
            {
                case 0:
                    damageText = 0;
                    cooldown = 0;
                    break;
                case 1:
                    money *= 2;
                    break;
                case 2:
                    damageText = 20;
                    cooldown = 4;
                    money *= 3;
                    spentMoney *= 2;
                    break;
                case 3:
                    damageText = 30;
                    cooldown = 3;
                    money *= 4;
                    spentMoney *= 3;
                    break;
                case 4:
                    damageText = 40;
                    cooldown = 3;
                    money *= 5;
                    spentMoney *= 4;
                    break;
                case 5:
                    damageText = 50;
                    cooldown = 2;
                    money *= 6;
                    spentMoney *= 5;
                    break;
            }
            
            return new ImprovementData(
                GetText(_textTypeFour, level, damageText, cooldown), 
                GetUpText(_upTextTypeFour, nextLevel, nextDamageText, nextCooldown), 
                money,
                spentMoney,
                damage,
                cooldown);
        }
        
        private string GetText(string mainText, params object[] objects)
        {
            return string.Format(mainText, objects);
        }

        private string GetUpText(string mainText, params object[] objects)
        {
            return string.Format(mainText, objects);
        }
    }
}