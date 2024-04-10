using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

    public enum GameState
    {
        Starting,
        Playing,
        LevelClear,
        LevelFail
    }

    public enum GamePref
    {
        GameData,
        IsFirstTime
    }

    public enum GameTag
    {
        Player,
        Enemy,
        MovingPlatform,
        Thorn,
        Collectable,
        CheckPoint,
        Door,
        DeadZone
    }

    public enum GameScene
    {
        MainMenu,
        Gameplay,
        Level_
    }

    public enum SpriteOrder
    {
        Normal = 5,
        InWater = 1
    }

    public enum PlayerAnimState
    {
        SayHello,
        Walk,
        Jump,
        OnAir,
        Land,
        Swim,
        FireBullet,
        Fly,
        FlyOnAir,
        SwimOnDeep,
        OnLadder,
        Dead,
        Idle,
        LadderIdle,
        HammerAttack,
        GotHit
    }

    public enum EnemyAnimState
    {
        Moving,
        Chasing,
        GotHit,
        Dead
    }

    public enum DetectMethod
    {
        RayCast,
        CircleOverlap
    }

    public enum PlayerCollider
    {
        Default,
        Flying,
        InWater,
        None
    }

    public enum CollectableType
    {
        Hp,
        Live,
        Bullet,
        Key,
        None
    }

    [System.Serializable]
    public class LevelItem
    {
        public int price;
        public Sprite preview;
        public Goal goal;
    }

    [System.Serializable]
    public class ShopItem {
        public CollectableType itemType;
        public int price;
        public Sprite preview;
    }

    [System.Serializable]
    public class Goal
    {
        public int timeOneStar;
        public int timeTwoStar;
        public int timeThreeStar;

        public int GetStar(int time)
        {
            if(time < timeThreeStar)
            {
                return 3;
            }else if(time < timeTwoStar) {
                return 2;
            }else
            {
                return 1;
            }
        }
    }
}
