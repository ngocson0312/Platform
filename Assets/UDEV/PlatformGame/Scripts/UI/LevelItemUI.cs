using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class LevelItemUI : MonoBehaviour
    {
        [SerializeField] private Image m_preview;
        [SerializeField] private GameObject m_lockedArea;
        [SerializeField] private GameObject m_checkMark;
        [SerializeField] private Text m_priceTxt;
        public Button btnComp;

        public void UpdateUI(LevelItem levelItem, int levelIdx)
        {
            if (levelItem == null) return;

            bool isUnlocked = GameData.Ins.IsLevelUnlocked(levelIdx);

            if (m_preview)
            {
                m_preview.sprite = levelItem.preview;
            }

            if (m_priceTxt)
            {
                m_priceTxt.text = levelItem.price.ToString();
            }

            if (m_lockedArea)
            {
                m_lockedArea.SetActive(!isUnlocked);
            }

            if (isUnlocked)
            {
                if (m_checkMark)
                {
                    m_checkMark.SetActive(GameData.Ins.curLevelId == levelIdx);
                }
            }else if (m_checkMark)
            {
                m_checkMark.SetActive(false);
            }
        }
    }
}
