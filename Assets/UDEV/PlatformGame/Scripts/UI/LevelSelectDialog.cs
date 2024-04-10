using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class LevelSelectDialog : Dialog
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private LevelItemUI m_itemUIPrefab;
        [SerializeField] private Text m_coinCountingTxt;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_coinCountingTxt)
            {
                m_coinCountingTxt.text = GameData.Ins.coin.ToString();
            }

            var levels = LevelManager.Ins.levels;

            if (levels == null || !m_gridRoot || !m_itemUIPrefab) return;

            Helper.ClearChilds(m_gridRoot);

            for (int i = 0; i < levels.Length; i++)
            {
                int levelIdx = i;
                var level = levels[levelIdx];
                if (level == null) continue;

                var itemUIClone = Instantiate(m_itemUIPrefab, Vector3.zero, Quaternion.identity);
                itemUIClone.transform.SetParent(m_gridRoot);
                itemUIClone.transform.localScale = Vector3.one;
                itemUIClone.transform.localPosition = Vector3.zero;
                itemUIClone.UpdateUI(level, levelIdx);
                if (itemUIClone.btnComp)
                {
                    itemUIClone.btnComp.onClick.RemoveAllListeners();
                    itemUIClone.btnComp.onClick.AddListener(() => SelectLevelBtnEvent(level, levelIdx));
                }
            }
        }

        private void SelectLevelBtnEvent(LevelItem levelItem, int levelIdx)
        {
            LevelManager.Ins?.UnlockLevel(levelItem, levelIdx);
        }
    }
}
