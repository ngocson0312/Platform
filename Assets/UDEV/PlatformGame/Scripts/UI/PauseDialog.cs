using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class PauseDialog : Dialog
    {
        public override void Show(bool isShow)
        {
            base.Show(isShow);

            Time.timeScale = 0f;
        }

        public void Replay()
        {
            Close();
            SceneController.Ins.LoadLevelScene(LevelManager.Ins.CurLevelId);
        }

        public void OpenSetting()
        {
            Close();
            Time.timeScale = 0f;
            if (GUIManager.Ins.settingDialog)
            {
                GUIManager.Ins.settingDialog.Show(true);
            }
        }

        public void Exit()
        {
            Close();
            SceneController.Ins.LoadScene(GameScene.MainMenu.ToString());
        }

        public override void Close()
        {
            Time.timeScale = 1f;
            base.Close();
        }
    }
}
