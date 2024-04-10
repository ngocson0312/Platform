using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class ObstacleChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private LayerMask m_waterLayer;
        [SerializeField] private LayerMask m_ladderLayer;
        [SerializeField] private float m_deepWaterChkDist;
        [SerializeField] private float m_checkingRadius;
        [SerializeField] private Vector3 m_offset;
        [SerializeField] private Vector3 m_deepWaterOffset;
        private bool m_isOnGround;
        private bool m_isOnWater;
        private bool m_isOnLadder;
        private bool m_isOnDeepWater;

        public bool IsOnGround { get => m_isOnGround;}
        public bool IsOnWater { get => m_isOnWater;}
        public bool IsOnLadder { get => m_isOnLadder;}
        public bool IsOnDeepWater { get => m_isOnDeepWater;}

        private void FixedUpdate()
        {
            ObstacleDetect();
        }

        public void ObstacleDetect()
        {
            m_isOnGround = OverlapChecking(m_groundLayer);
            m_isOnWater = OverlapChecking(m_waterLayer);
            m_isOnLadder = OverlapChecking(m_ladderLayer);

            RaycastHit2D waterHit = Physics2D.Raycast(transform.position + m_deepWaterOffset,
                Vector2.up, m_deepWaterChkDist, m_waterLayer);

            m_isOnDeepWater = waterHit;

            //Debug.Log($"Ground : {m_isOnGround} _ Water: {m_isOnWater} _ Ladder : {m_isOnLadder}");
        }

        private bool OverlapChecking(LayerMask layerToCheck)
        {
            Collider2D col = Physics2D.OverlapCircle(
                transform.position + m_offset,
                m_checkingRadius, layerToCheck
                );

            return col != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Helper.ChangAlpha(Color.red, 0.4f);
            Gizmos.DrawSphere(transform.position + m_offset, m_checkingRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + m_deepWaterOffset,
                new Vector3(
                    transform.position.x + m_deepWaterOffset.x,
                    transform.position.y + m_deepWaterOffset.y + m_deepWaterChkDist,
                    transform.position.z
                ));
        }
    }
}
