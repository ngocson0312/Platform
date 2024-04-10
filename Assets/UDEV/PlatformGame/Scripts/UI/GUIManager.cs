using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class GUIManager : Singleton<GUIManager>
    {
        [SerializeField] private Text m_liveCountingTxt;
        [SerializeField] private Text m_hpCountingTxt;
        [SerializeField] private Text m_coinCountingTxt;
        [SerializeField] private Text m_timeCountingTxt;
        [SerializeField] private Text m_bulletCountingTxt;
        [SerializeField] private Text m_keyCountingTxt;
        [SerializeField] private GameObject m_mobileGamepad;

        public Dialog settingDialog;
        public Dialog pauseDialog;
        public Dialog lvClearedDialog;
        public Dialog lvFailedDialog;

        protected override void Awake()
        {
            MakeSingleton(false);
        }

        public void UpdateTxt(Text txt, string content)
        {
            if (txt)
            {
                txt.text = content;
            }
        }

        public void UpdateLive(int live)
        {
            UpdateTxt(m_liveCountingTxt, "x" + live.ToString());
        }

        public void UpdateHp(int hp)
        {
            UpdateTxt(m_hpCountingTxt, "x" + hp.ToString());
        }

        public void UpdateCoin(int coin)
        {
            UpdateTxt(m_coinCountingTxt, "x" + coin.ToString());
        }

        public void UpdatePlayTime(string time)
        {
            UpdateTxt(m_timeCountingTxt, time.ToString());
        }

        public void UpdateBullet(int bullet)
        {
            UpdateTxt(m_bulletCountingTxt, "x" + bullet.ToString());
        }

        public void UpdateKey(int key)
        {
            UpdateTxt(m_keyCountingTxt, "x" + key.ToString());
        }

        public void ShowMobileGamepad(bool isShow)
        {
            if (m_mobileGamepad)
            {
                m_mobileGamepad.SetActive(isShow);
            }
        }
    }
}
