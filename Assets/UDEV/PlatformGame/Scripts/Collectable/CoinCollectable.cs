using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class CoinCollectable : Collectable
    {
        protected override void TriggerHandle()
        {
            GameManager.Ins.AddCoins(m_bonus);
        }
    }
}
