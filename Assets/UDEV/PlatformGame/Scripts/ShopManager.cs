using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.PlatformGame
{
    public class ShopManager : Singleton<ShopManager>
    {
        public ShopItem[] items;

        public void BuyItem(ShopItem shopItem, int shopItemIdx, UnityAction BuyingSuccess = null)
        {
            if (shopItem == null) return;

            if (GameData.Ins.coin >= shopItem.price)
            {
                GameData.Ins.coin -= shopItem.price;

                switch (shopItem.itemType)
                {
                    case CollectableType.Hp:
                        GameData.Ins.hp++;
                        break;
                    case CollectableType.Live:
                        GameData.Ins.live++;
                        break;
                    case CollectableType.Bullet:
                        GameData.Ins.bullet++;
                        break;
                    case CollectableType.Key:
                        GameData.Ins.key++;
                        break;
                }

                GameData.Ins.SaveData();
                BuyingSuccess?.Invoke();

                AudioController.Ins.PlaySound(AudioController.Ins.buy);
            }
            else
            {
                Debug.Log("You don't have enough coin !!!");
            }
        }
    }

}