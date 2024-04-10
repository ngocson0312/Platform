using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.SceneManagement;

namespace UDEV.PlatformGame
{
    public class GameManager : Singleton<GameManager>
    {
        public GameplaySetting setting;
        public Player player;
        public FreeParallax map;

        private StateMachine<GameState> m_fsm;

        private int m_curLive;
        private int m_curCoin;
        private int m_curKey;
        private int m_curBullet;
        private float m_gameplayTime;
        private int m_goalStar;

        public int CurLive { get => m_curLive; set => m_curLive = value; }
        public int CurCoin { get => m_curCoin; set => m_curCoin = value; }
        public int CurKey { get => m_curKey; set => m_curKey = value; }
        public int CurBullet { get => m_curBullet; set => m_curBullet = value; }
        public float GameplayTime { get => m_gameplayTime;}
        public int GoalStar { get => m_goalStar;}
        public StateMachine<GameState> Fsm { get => m_fsm;}

        protected override void Awake()
        {
            MakeSingleton(false);
            m_fsm = StateMachine<GameState>.Initialize(this);
            m_fsm.ChangeState(GameState.Playing);
        }

        private void Start()
        {
            LoadData();
            StartCoroutine(CamFollowDelay());
            if (setting.isOnMobile)
            {
                GUIManager.Ins.ShowMobileGamepad(true);
            }else
            {
                GUIManager.Ins.ShowMobileGamepad(false);
            }

            AudioController.Ins.PlayBackgroundMusic();

#if UNITY_EDITOR
            StartCoroutine(SaveData());
#endif
        }

        private void LoadData()
        {
            m_curLive = setting.startingLive;
            m_curBullet = setting.startingBullet;

            if(GameData.Ins.live != 0)
            {
                m_curLive = GameData.Ins.live;
            }

            if(GameData.Ins.bullet != 0)
            {
                m_curBullet = GameData.Ins.bullet;
            }

            if(GameData.Ins.key != 0)
            {
                m_curKey = GameData.Ins.key;
            }

            if(GameData.Ins.hp != 0)
            {
                player.CurHp = GameData.Ins.hp;
            }

            Vector3 checkPoint = GameData.Ins.GetCheckPoint(LevelManager.Ins.CurLevelId);

            if(checkPoint != Vector3.zero)
            {
                player.transform.position = checkPoint;
            }

            float gameplayTime = GameData.Ins.GetPlayTime(LevelManager.Ins.CurLevelId);
            if(gameplayTime > 0)
            {
                m_gameplayTime = gameplayTime;
            }

            GUIManager.Ins.UpdateLive(m_curLive);
            GUIManager.Ins.UpdateHp(player.CurHp);
            GUIManager.Ins.UpdateCoin(m_curCoin);
            GUIManager.Ins.UpdatePlayTime(Helper.TimeConvert(m_gameplayTime));
            GUIManager.Ins.UpdateBullet(m_curBullet);
            GUIManager.Ins.UpdateKey(m_curKey);
        }

        public void BackToCheckPoint()
        {
            player.transform.position = GameData.Ins.GetCheckPoint(LevelManager.Ins.CurLevelId);
        }

        public void Revive()
        {
            m_curLive--;
            player.CurHp = player.stat.hp;
            GameData.Ins.hp = player.CurHp;
            GameData.Ins.live = m_curLive;
            BackToCheckPoint();
            GUIManager.Ins.UpdateLive(m_curLive);
        }

        public void AddCoins(int coins)
        {
            m_curCoin += coins;
            GameData.Ins.coin += coins;

            GUIManager.Ins.UpdateCoin(GameData.Ins.coin);
        }

        public void ReduceBullet()
        {
            m_curBullet--;
            GameData.Ins.bullet = m_curBullet;
            
            GUIManager.Ins.UpdateBullet(m_curBullet);
        }

        public void Replay()
        {
            SceneController.Ins.LoadLevelScene(LevelManager.Ins.CurLevelId);
        }

        public void NextLevel()
        {
            LevelManager.Ins.CurLevelId++;
            if(LevelManager.Ins.CurLevelId >= SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneController.Ins.LoadScene(GameScene.MainMenu.ToString());
            }else
            {
                SceneController.Ins.LoadLevelScene(LevelManager.Ins.CurLevelId);
            }
        }

        public void SaveCheckPoint()
        {
            GameData.Ins.UpdatePlayTime(LevelManager.Ins.CurLevelId, m_gameplayTime);
            GameData.Ins.UpdateCheckPoint(LevelManager.Ins.CurLevelId,
                new Vector3(
                    player.transform.position.x - 0.5f,
                    player.transform.position.y + 1.5f,
                    player.transform.position.z
                    ));
        }

        public void LevelFailed()
        {
            m_fsm.ChangeState(GameState.LevelFail);
            GameData.Ins.UpdateLevelScore(LevelManager.Ins.CurLevelId,
                Mathf.RoundToInt(m_gameplayTime));

            StartCoroutine(ShowLvFailedDialogDelay()); 
        }

        private IEnumerator ShowLvFailedDialogDelay()
        {
            yield return new WaitForSeconds(1);

            if (GUIManager.Ins.lvFailedDialog)
            {
                GUIManager.Ins.lvFailedDialog.Show(true);
            }

            AudioController.Ins.PlaySound(AudioController.Ins.fail);
        }

        public void LevelClear()
        {
            m_fsm.ChangeState(GameState.LevelClear);
            GameData.Ins.UpdateLevelScore(LevelManager.Ins.CurLevelId,
                Mathf.RoundToInt(m_gameplayTime));
            m_goalStar = LevelManager.Ins.CurLevel.goal.GetStar(Mathf.RoundToInt(m_gameplayTime));

            StartCoroutine(ShowLvClearedDialogDelay());
        }

        private IEnumerator ShowLvClearedDialogDelay()
        {
            yield return new WaitForSeconds(1);

            if (GUIManager.Ins.lvClearedDialog)
            {
                GUIManager.Ins.lvClearedDialog.Show(true);
            }

            AudioController.Ins.PlaySound(AudioController.Ins.missionComplete);
        }

        private IEnumerator CamFollowDelay()
        {
            yield return new WaitForSeconds(0.3f);
            CameraFollow.Ins.target = player.transform;
        }

        public void SetMapSpeed(float speed)
        {
            if (map)
            {
                map.Speed = speed;
            }
        }

        #region FSM
        protected void Starting_Enter() { }
        protected void Starting_Update() { }
        protected void Starting_Exit() { }
        protected void Playing_Enter() { }
        protected void Playing_Update() {
            if (GameData.Ins.IsLevelPassed(LevelManager.Ins.CurLevelId)) return;

            m_gameplayTime += Time.deltaTime;

            GUIManager.Ins.UpdatePlayTime(Helper.TimeConvert(m_gameplayTime));
        }
        protected void Playing_Exit() { }
        protected void LevelClear_Enter() { }
        protected void LevelClear_Update() { }
        protected void LevelClear_Exit() { }
        protected void LevelFail_Enter() { }
        protected void LevelFail_Update() {
            
        }
        protected void LevelFail_Exit() { }

        #endregion

        private void OnApplicationQuit()
        {
            GameData.Ins.SaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            GameData.Ins.SaveData();
        }

        private IEnumerator SaveData()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                GameData.Ins.SaveData();
            }
        }
    }
}
