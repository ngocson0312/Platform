using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class LevelManager : Singleton<LevelManager>
    {
        public LevelItem[] levels;
        private int m_curLevelId;

        public int CurLevelId { get => m_curLevelId; set => m_curLevelId = value; }

        public LevelItem CurLevel
        {
            get => levels[m_curLevelId];// curLevelId = 2 chung se lay LevelItem thu 2 o trong mang levels
        }

        public void Init()
        {
            if (levels == null || levels.Length <= 0) return;

            for (int i = 0; i < levels.Length; i++)
            {
                var level = levels[i];

                if (level == null) continue;

                LevelData levelData = GameData.Ins.GetLevelData(i);

                if (levelData != null) continue;

                levelData = new LevelData();

                if (i == 0)
                {
                    levelData.isUnlocked = true;
                }
                else
                {
                    levelData.isUnlocked = false;
                }

                GameData.Ins.UpdateLevelData(levelData, i);               
            }

            GameData.Ins.SaveData();
        }

        public void UnlockLevel(LevelItem levelItem, int levelIdx)
        {
            if (levelItem == null) return;

            bool isUnlocked = GameData.Ins.IsLevelUnlocked(levelIdx);

            if (isUnlocked)
            {
                GameData.Ins.curLevelId = levelIdx;
                GameData.Ins.SaveData();

                CurLevelId = levelIdx;
                SceneController.Ins.LoadLevelScene(levelIdx);
            }
            else
            {
                if (GameData.Ins.coin >= levelItem.price)
                {
                    GameData.Ins.coin -= levelItem.price;
                    GameData.Ins.curLevelId = levelIdx;
                    GameData.Ins.UpdateLevelUnlocked(levelIdx, true);
                    GameData.Ins.SaveData();

                    CurLevelId = levelIdx;
                    SceneController.Ins.LoadLevelScene(levelIdx);

                    AudioController.Ins.PlaySound(AudioController.Ins.unlock);
                }
                else
                {
                    Debug.Log("You don't have enough coins!!!");
                }
            }
        }
    }
}
