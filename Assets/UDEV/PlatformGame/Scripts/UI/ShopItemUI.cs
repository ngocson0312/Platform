using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private Text m_priceTxt;
        [SerializeField] private Text m_amountTxt;
        [SerializeField] private Image m_preview;
        public Button btnComp;

        public void UpdateUI(ShopItem shopItem, int itemIdx)
        {
            if (shopItem == null) return;

            if (m_preview)
            {
                m_preview.sprite = shopItem.preview;
            }

            switch (shopItem.itemType)
            {
                case CollectableType.Hp:
                    UpdateAmountTxt(GameData.Ins.hp);
                    break;
                case CollectableType.Live:
                    UpdateAmountTxt(GameData.Ins.live);
                    break;
                case CollectableType.Bullet:
                    UpdateAmountTxt(GameData.Ins.bullet);
                    break;
                case CollectableType.Key:
                    UpdateAmountTxt(GameData.Ins.key);
                    break;
            }

            if(m_priceTxt)
            {
                m_priceTxt.text = shopItem.price.ToString();
            }
        }

        private void UpdateAmountTxt(int amount)
        {
            if (m_amountTxt)
            {
                m_amountTxt.text = amount.ToString();
            }
        }
    }
}
