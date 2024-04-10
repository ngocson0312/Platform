using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class SettingDialog : Dialog
    {
        [SerializeField] private Slider m_musicSlider;
        [SerializeField] private Slider m_soundSlider;

        public override void Show(bool isShow)
        {
            base.Show(isShow);

            if (m_musicSlider)
            {
                m_musicSlider.value = GameData.Ins.musicVol;
                AudioController.Ins.SetMusicVolume(GameData.Ins.musicVol);
            }

            if (m_soundSlider)
            {
                m_soundSlider.value = GameData.Ins.soundVol;
                AudioController.Ins.SetSoundVolume(GameData.Ins.soundVol);
            }
        }

        public void OnMusicChange(float value)
        {
            AudioController.Ins.SetMusicVolume(value);
        }

        public void OnSoundChange(float value)
        {
            AudioController.Ins.SetSoundVolume(value);
        }

        public void Save()
        {
            GameData.Ins.musicVol = AudioController.Ins.musicVolume;
            GameData.Ins.soundVol = AudioController.Ins.sfxVolume;
            GameData.Ins.SaveData();
            Close();
        }

        public override void Close()
        {
            base.Close();
            Time.timeScale = 1f;
        }
    }
}
