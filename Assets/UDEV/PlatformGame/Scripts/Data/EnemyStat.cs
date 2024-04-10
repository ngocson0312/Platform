using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    [CreateAssetMenu(fileName = "Enemy Stat", menuName = "UDEV/Enemy Stat")]
    public class EnemyStat : ActorStat
    {
        public float chasingSpeed;
    }
}
