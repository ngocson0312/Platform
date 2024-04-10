using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class LifeCollectable : Collectable
    {
        protected override void TriggerHandle()
        {
            GameManager.Ins.CurLive += m_bonus;
            GameData.Ins.live = GameManager.Ins.CurLive;
            GUIManager.Ins.UpdateLive(GameData.Ins.live);
        }
    }
}
