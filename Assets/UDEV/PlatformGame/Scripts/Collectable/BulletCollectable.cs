using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class BulletCollectable : Collectable
    {
        protected override void TriggerHandle()
        {
            GameManager.Ins.CurBullet += m_bonus;
            GameData.Ins.bullet = GameManager.Ins.CurBullet;
            GUIManager.Ins.UpdateBullet(GameData.Ins.bullet);
        }
    }
}
