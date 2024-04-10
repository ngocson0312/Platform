using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class ShopDialog : Dialog
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private ShopItemUI m_itemUIPrefab;
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

            var shopItems = ShopManager.Ins.items;

            if (shopItems == null || shopItems.Length <= 0) return;

            Helper.ClearChilds(m_gridRoot);

            for (int i = 0; i < shopItems.Length; i++)
            {
                int shopItemIdx = i;

                var shopItem = shopItems[i];

                if (shopItem != null)
                {
                    var itemUIClone = Instantiate(m_itemUIPrefab, Vector3.zero, Quaternion.identity);

                    itemUIClone.transform.SetParent(m_gridRoot);

                    itemUIClone.transform.localPosition = Vector3.zero;

                    itemUIClone.transform.localScale = Vector3.one;

                    itemUIClone.UpdateUI(shopItem, shopItemIdx);

                    if (itemUIClone.btnComp)
                    {
                        itemUIClone.btnComp.onClick.RemoveAllListeners();
                        itemUIClone.btnComp.onClick.AddListener(() => BuyingItemBtnEvent(shopItem, shopItemIdx));
                    }
                }
            }
        }

        private void BuyingItemBtnEvent(ShopItem shopItem, int shopItemIdx)
        {
            ShopManager.Ins?.BuyItem(shopItem, shopItemIdx, UpdateUI);
        }
    }
}
