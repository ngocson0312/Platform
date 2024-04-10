using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class FreeMovingEnemy : Enemy
    {
        public bool canRotate;
        private float m_movePosXR;
        private float m_movePosXL;
        private float m_movePosYT;
        private float m_movePosYD;

        private bool m_haveMovingPos;
        private Vector2 m_movingPos;

        protected override void Awake()
        {
            base.Awake();
            FSMInit(this);
        }


        protected override void Update()
        {
            base.Update();
            GetTargetDir();
        }
        public void FindMaxMovePos()
        {
            m_movePosXR = m_startingPos.x + movingDist;
            m_movePosXL = m_startingPos.x - movingDist;
            m_movePosYT = m_startingPos.y + movingDist; 
            m_movePosYD = m_startingPos.y - movingDist;
        }

        public override void Move()
        {
            if (m_isKnockBack) return;

            if (!m_haveMovingPos)
            {
                float randPosX = Random.Range(m_movePosXL, m_movePosXR);
                float randPosY = Random.Range(m_movePosYD, m_movePosYT);
                m_movingPos = new Vector2(randPosX, randPosY);
                m_movingDir = m_movingPos - (Vector2)transform.position;
                m_movingDir.Normalize();
                m_movingDirBackup = m_movingDir;
                m_haveMovingPos = true;
            }

            float angle = 0f;

            if (canRotate)
            {
                angle = Mathf.Atan2(m_movingDir.y, m_movingDir.x) * Mathf.Rad2Deg;
            }

            if(m_movingDir.x > 0)
            {
                if (canRotate)
                {
                    angle = Mathf.Clamp(angle, -41, 41);
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
                Flip(Direction.Right);
            }else if(m_movingDir.x < 0)
            {
                if (canRotate)
                {
                    float newAngle = angle + 180f;
                    newAngle = Mathf.Clamp(newAngle, 25, 325);
                    transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
                }
                Flip(Direction.Left);
            }

            DestReachedChecking();
        }

        private void DestReachedChecking()
        {
            if(Vector2.Distance(transform.position, m_movingPos) <= 0.5f)
            {
                m_haveMovingPos = false;
            }else
            {
                m_rb.velocity = m_movingDir * m_curSpeed;
            }
        }

        #region FSM
        protected override void Moving_Enter()
        {
            base.Moving_Enter();
            m_haveMovingPos = false;
            FindMaxMovePos();
        }

        protected override void Chasing_Update()
        {
            base.Chasing_Update();
            m_movingDir = m_targetDir;
        }

        protected override void GotHit_Update()
        {
            if (m_isKnockBack)
            {
                KnockBackMove(m_targetDir.y);
            }else
            {
                m_fsm.ChangeState(EnemyAnimState.Moving);
            }
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, new Vector3(
                transform.position.x + movingDist, transform.position.y, transform.position.z
                ));

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, new Vector3(
                transform.position.x - movingDist, transform.position.y, transform.position.z
                ));

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, new Vector3(
                transform.position.x, transform.position.y + movingDist, transform.position.z
                ));

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, new Vector3(
                transform.position.x, transform.position.y - movingDist, transform.position.z
                ));
        }
    }
}
