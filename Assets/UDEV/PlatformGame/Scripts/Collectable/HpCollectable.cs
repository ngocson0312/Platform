using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class HpCollectable : Collectable
    {
        protected override void TriggerHandle()
        {
            m_player.CurHp += m_bonus;
            GameData.Ins.hp = m_player.CurHp;
            GUIManager.Ins.UpdateHp(GameData.Ins.hp);
        }
    }
}
