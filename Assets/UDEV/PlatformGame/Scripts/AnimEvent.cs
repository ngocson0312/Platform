using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class AnimEvent : MonoBehaviour
    {
        public void HammerAttack()
        {
            CamShake.Ins.ShakeTrigger(0.3f, 0.1f, 1);
            AudioController.Ins.PlaySound(AudioController.Ins.attack);
        }

        public void PlayFootStepSound()
        {
            AudioController.Ins.PlaySound(AudioController.Ins.footStep);
        }
    }
}
