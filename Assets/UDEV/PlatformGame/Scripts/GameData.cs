using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    [System.Serializable]
    public class LevelData
    {
        public bool isUnlocked;
        public bool isPassed;
        public Vector3 checkPoint;
        public float playTime;
        public float completeTime;
    }

    public class GameData : Singleton<GameData>
    {
        public int coin;
        public int curLevelId;
        public float musicVol;
        public float soundVol;
        public int hp;
        public int live;
        public int bullet;
        public int key;
        public List<LevelData> levels;

        protected override void Awake()
        {
            base.Awake();
            levels = new List<LevelData>();
        }

        public void SaveData()
        {
            Pref.GameData = JsonUtility.ToJson(this);
        }

        public void LoadData()
        {
            string data = Pref.GameData;
            if (string.IsNullOrEmpty(data)) return;

            JsonUtility.FromJsonOverwrite(data, this);
        }

        private T GetValue<T>(List<T> dataList, int idx)
        {
            if (dataList == null || dataList.Count <= 0 || idx < 0 || idx >= dataList.Count) return default;

            return dataList[idx];
        }

        private void UpdateValue<T> (ref List<T> dataList, int idx, T value)
        {
            if (dataList == null || idx < 0) return;

            if(dataList.Count <= 0 || (dataList.Count > 0 && idx >= dataList.Count))// 2 2
            {
                dataList.Add(value);
            }else
            {
                dataList[idx] = value;
            }
        }

        #region LEVEL
        public LevelData GetLevelData(int levelId)
        {
            return GetValue<LevelData>(levels ,levelId);
        }

        public void UpdateLevelData(LevelData level, int levelId)
        {
            UpdateValue<LevelData>(ref levels, levelId, level);
        }

        public void UpdateLevelUnlocked(int levelId, bool isUnlocked)
        {
            LevelData level = GetLevelData(levelId);
            level.isUnlocked = isUnlocked;
            UpdateValue<LevelData>(ref levels, levelId, level);
        }

        public void UpdateLevelPassed(int levelId, bool isPassed)
        {
            LevelData level = GetLevelData(levelId);
            level.isPassed = isPassed;
            UpdateValue<LevelData>(ref levels, levelId, level);
        }

        public float GetLevelScore(int levelId)
        {
            LevelData level = GetLevelData(levelId);
            return level.completeTime;
        }

        public void UpdateLevelScore(int levelId, float completeTime)
        {
            LevelData level = GetLevelData(levelId);
            float oldCompleteTime = level.completeTime;

            if(completeTime < oldCompleteTime)
            {
                level.completeTime = completeTime;
                UpdateValue<LevelData>(ref levels, levelId, level);
            }
        }

        public void UpdateLevelScoreNoneCheck(int levelId, float completeTime)
        {
            LevelData level = GetLevelData(levelId);
            level.completeTime = completeTime;
            UpdateValue<LevelData>(ref levels, levelId, level);
        }

        public Vector3 GetCheckPoint(int levelId)
        {
            LevelData level = GetLevelData(levelId);
            return level.checkPoint;
        }

        public void UpdateCheckPoint(int levelId, Vector3 checkPoint)
        {
            LevelData level = GetLevelData(levelId);
            level.checkPoint = checkPoint;
            UpdateValue<LevelData>(ref levels, levelId, level);
        }

        public float GetPlayTime(int levelId)
        {
            LevelData level = GetLevelData(levelId);
            return level.playTime;
        }

        public void UpdatePlayTime(int levelId, float playTime)
        {
            LevelData level = GetLevelData(levelId);
            level.playTime = playTime;
            UpdateValue<LevelData>(ref levels, levelId, level);
        }

        public bool IsLevelUnlocked(int levelId)
        {
            LevelData level = GetLevelData(levelId);
            return level.isUnlocked;
        }

        public bool IsLevelPassed(int levelId)
        {
            LevelData level = GetLevelData(levelId);
            return level.isPassed;
        }
        #endregion
    }
}
