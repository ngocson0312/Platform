using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    [CreateAssetMenu(fileName = "GameplaySetting", menuName = "UDEV/Gameplay Setting")]
    public class GameplaySetting : ScriptableObject
    {
        public bool isOnMobile;
        public int startingLive;
        public int startingBullet;
    }
}
