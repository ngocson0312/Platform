using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class Door : MonoBehaviour
    {
        public int keyRequired;
        public Sprite openSp;
        public Sprite closeSp;

        private SpriteRenderer m_sp;
        private bool m_isOpened;

        public bool IsOpened { get => m_isOpened;}

        private void Awake()
        {
            m_sp= GetComponent<SpriteRenderer>();
        }


        private void Start()
        {
            DoorChecking();
        }

        private void DoorChecking()
        {
            m_isOpened = GameData.Ins.IsLevelUnlocked(LevelManager.Ins.CurLevelId + 1);
            m_sp.sprite = m_isOpened ? openSp : closeSp;
        }

        public void OpenDoor()
        {
            if (m_isOpened)
            {
                GameManager.Ins.CurKey = 0;
                GameManager.Ins.LevelClear();
                GUIManager.Ins.UpdateKey(GameManager.Ins.CurKey);
                return;
            }

            if(GameManager.Ins.CurKey >= keyRequired)
            {
                GameManager.Ins.CurKey = 0;
                GameData.Ins.key = 0;
                GameData.Ins.UpdateLevelUnlocked(LevelManager.Ins.CurLevelId + 1, true);
                GameData.Ins.UpdateLevelPassed(LevelManager.Ins.CurLevelId, true);
                GameManager.Ins.LevelClear();
                DoorChecking();

                GUIManager.Ins.UpdateKey(GameManager.Ins.CurKey);
            }
        }
    }
}
