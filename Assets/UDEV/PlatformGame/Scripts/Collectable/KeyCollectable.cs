using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class KeyCollectable : Collectable
    {
        protected override void TriggerHandle()
        {
            GameManager.Ins.CurKey += m_bonus;
            GameData.Ins.key = GameManager.Ins.CurKey;
            GUIManager.Ins.UpdateKey(GameData.Ins.key);
        }
    }
}
