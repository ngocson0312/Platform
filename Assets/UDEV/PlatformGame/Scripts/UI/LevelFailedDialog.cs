using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class LevelFailedDialog : Dialog
    {
        [SerializeField] private Text m_timeCountingTxt;
        [SerializeField] private Text m_coinCountingTxt;

        public override void Show(bool isShow)
        {
            base.Show(isShow);

            if (m_timeCountingTxt)
            {
                m_timeCountingTxt.text = $"{Helper.TimeConvert(GameManager.Ins.GameplayTime)}";
            }

            if(m_coinCountingTxt)
            {
                m_coinCountingTxt.text = $"{GameManager.Ins.CurCoin}";
            }
        }

        public void Replay()
        {
            Close();
            GameManager.Ins.Replay();
        }

        public void BackToMenu()
        {
            SceneController.Ins.LoadScene(GameScene.MainMenu.ToString());
        }
    }
}
