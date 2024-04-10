using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class Hammer : MonoBehaviour
    {
        [SerializeField] private LayerMask m_enemyLayer;
        [SerializeField] private float m_atkRadius;
        [SerializeField] private Vector3 m_offset;
        [SerializeField]
        private Player m_player;

        public void Attack()
        {
            if (m_player == null) return;

            Collider2D col = Physics2D.OverlapCircle(transform.position + m_offset, m_atkRadius, m_enemyLayer);

            if (!col) return;

            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(m_player.stat.damage, m_player);
            }
        }

        private void Update()
        {
            FlipOffset();
        }

        private void FlipOffset()
        {
            if (m_player == null) return;

            if (m_player.transform.localScale.x > 0)
            {
                if (m_offset.x < 0)
                {
                    m_offset = new Vector3(m_offset.x * -1, m_offset.y, m_offset.z);
                }
            }
            else if (m_player.transform.localScale.x < 0)
            {
                if (m_offset.x > 0)
                {
                    m_offset = new Vector3(m_offset.x * -1, m_offset.y, m_offset.z);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Helper.ChangAlpha(Color.yellow, 0.4f);
            Gizmos.DrawSphere(transform.position + m_offset, m_atkRadius);
        }
    }
}
