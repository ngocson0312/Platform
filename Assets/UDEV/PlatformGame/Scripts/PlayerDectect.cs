using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class PlayerDectect : MonoBehaviour
    {
        [SerializeField] private bool m_disable;
        [SerializeField] private DetectMethod m_dectectMethod;
        [SerializeField] private LayerMask m_targetLayer;
        [SerializeField] private float m_detectDist;

        private Player m_target;
        private Vector2 m_dirToTarget;
        private bool m_isDetected;

        public Player Target { get => m_target;}
        public Vector2 DirToTarget { get => m_dirToTarget;}
        public bool IsDetected { get => m_isDetected;}

        private void Start()
        {
            m_target = GameManager.Ins.player;
        }

        private void FixedUpdate()
        {
            DetectPlayer();
        }

        private void DetectPlayer()
        {
            if (!m_target || m_disable) return;

            if (m_dectectMethod == DetectMethod.RayCast)
            {
                m_dirToTarget = m_target.transform.position - transform.position;
                m_dirToTarget.Normalize();

                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(m_dirToTarget.x, 0), m_detectDist, m_targetLayer);

                m_isDetected = hit.collider != null;

            }
            else if (m_dectectMethod == DetectMethod.CircleOverlap)
            {
                Collider2D col = Physics2D.OverlapCircle(transform.position, m_detectDist, m_targetLayer);
                m_isDetected = col != null;
            }

            if (m_isDetected)
            {
                Debug.Log("Player was detected!.");
            }
            else
            {
                Debug.Log("Player not detected!.");
            }
        }

        private void OnDrawGizmos()
        {
            if(m_dectectMethod == DetectMethod.RayCast)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position,
                    new Vector3(
                        transform.position.x + m_detectDist,
                        transform.position.y, transform.position.z
                    ));
            }else if(m_dectectMethod == DetectMethod.CircleOverlap)
            {
                Gizmos.color = Helper.ChangAlpha(Color.green, 0.2f);
                Gizmos.DrawSphere(transform.position, m_detectDist);
            }
        }
    }
}
