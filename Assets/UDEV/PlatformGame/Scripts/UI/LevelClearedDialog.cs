using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame {
    public class LevelClearedDialog : Dialog
    {
        [SerializeField] private Image[] m_starImgs;
        [SerializeField] private Sprite m_activeStar;
        [SerializeField] private Sprite m_deactiveStar;

        [SerializeField] private Text m_liveCountingTxt;
        [SerializeField] private Text m_hpCountingText;
        [SerializeField] private Text m_timeCountingTxt;
        [SerializeField] private Text m_coinCountingTxt;

        public override void Show(bool isShow)
        {
            base.Show(isShow);

            if(m_starImgs != null && m_starImgs.Length > 0)
            {
                for (int i = 0; i < m_starImgs.Length; i++)
                {
                    var star = m_starImgs[i];
                    if (star)
                    {
                        star.sprite = m_deactiveStar;
                    }
                }

                for (int i = 0; i < GameManager.Ins.GoalStar; i++)
                {
                    var star = m_starImgs[i];
                    if (star)
                    {
                        star.sprite = m_activeStar;
                    }
                }
            }

            if (m_liveCountingTxt)
            {
                m_liveCountingTxt.text = $"x {GameManager.Ins.CurLive}";
            }

            if (m_hpCountingText)
            {
                m_hpCountingText.text = $"x {GameManager.Ins.player.CurHp}";
            }

            if (m_timeCountingTxt)
            {
                m_timeCountingTxt.text = $"x {Helper.TimeConvert(GameManager.Ins.GameplayTime)}";
            }

            if (m_coinCountingTxt)
            {
                m_coinCountingTxt.text = $"x {GameManager.Ins.CurCoin}";
            }
        }

        public void Replay()
        {
            Close();
            GameManager.Ins.Replay();
        }

        public void NextLevel()
        {
            Close();
            GameManager.Ins.NextLevel();
        }
    }
}
