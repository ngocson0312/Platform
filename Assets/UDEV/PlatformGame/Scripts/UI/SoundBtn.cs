using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class SoundBtn : MonoBehaviour
    {
        private Button m_btn;

        private void Awake()
        {
            m_btn = GetComponent<Button>();
        }

        private void Start()
        {
            if (m_btn)
            {
                m_btn.onClick.RemoveAllListeners();
                m_btn.onClick.AddListener(() => PlaySound());
            }
        }

        private void PlaySound()
        {
            AudioController.Ins.PlaySound(AudioController.Ins.btnClick);
        }
    }
}
