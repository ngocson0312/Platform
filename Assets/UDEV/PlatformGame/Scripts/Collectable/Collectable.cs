using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private CollectableType m_type;
        [SerializeField] private int m_minBonus;
        [SerializeField] private int m_maxBonus;
        [SerializeField] private AudioClip m_collisionSfx;
        [SerializeField] private GameObject m_destroyVfxPb;

        protected int m_bonus;
        protected Player m_player;

        private void Start()
        {
            m_player = GameManager.Ins.player;

            if (!m_player) return;

            m_bonus = Random.Range(m_minBonus, m_maxBonus);

            Init();
        }

        public virtual void Init()
        {
            DestroyWhenLevelPassed();
        }

        protected virtual void TriggerHandle()
        {

        }

        protected void DestroyWhenLevelPassed()
        {
            if (GameData.Ins.IsLevelPassed(LevelManager.Ins.CurLevelId))
            {
                Destroy(gameObject);
            }
        }

        public void Trigger()
        {
            TriggerHandle();

            if (m_destroyVfxPb)
            {
                Instantiate(m_destroyVfxPb, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);

            AudioController.Ins.PlaySound(m_collisionSfx);
        }
    }
}
