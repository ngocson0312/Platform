using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    [RequireComponent(typeof(LineMoving))]
    public class LineMovingEnemy : Enemy
    {
        private LineMoving m_lineMoving;

        protected override void Awake()
        {
            base.Awake();
            m_lineMoving = GetComponent<LineMoving>();
            FSMInit(this);
        }

        public override void Start()
        {
            base.Start();
            movingDist = m_lineMoving.movingDist;
        }

        public override void Move()
        {
            if (m_isKnockBack) return;

            m_lineMoving.Move();
            Flip(m_lineMoving.moveDir);
        }

        #region FSM
        protected override void Moving_Update()
        {
            base.Moving_Update();
            m_targetDir = m_lineMoving.BackDir;
            m_lineMoving.speed = m_curSpeed;
            m_lineMoving.SwitchDirChecking();
        }

        protected override void Chasing_Enter()
        {
            base.Chasing_Enter();
            GetTargetDir();
            m_lineMoving.SwitchDir(m_targetDir);
        }

        protected override void Chasing_Update()
        {
            base.Chasing_Update();
            GetTargetDir();
            m_lineMoving.speed = m_curSpeed;
        }

        protected override void Chasing_Exit()
        {
            base.Chasing_Exit();
            m_lineMoving.SwitchDirChecking();
        }

        protected override void GotHit_Update()
        {
            base.GotHit_Update();
            m_lineMoving.SwitchDirChecking();
            GetTargetDir();
            if (m_isKnockBack)
            {
                KnockBackMove(0.55f);
            }else
            {
                m_fsm.ChangeState(EnemyAnimState.Moving);
            }
        }
        #endregion
    }
}
