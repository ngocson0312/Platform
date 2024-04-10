using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

namespace UDEV.PlatformGame
{
    public class Enemy : Actor
    {
        [Header("Moving: ")]
        public float movingDist;

        protected PlayerDectect m_playerDetect;
        protected EnemyStat m_curStat;
        protected Vector2 m_movingDir;
        protected Vector2 m_movingDirBackup;
        protected Vector2 m_startingPos;
        protected Vector2 m_targetDir;
        protected StateMachine<EnemyAnimState> m_fsm;

        public bool IsDead
        {
            get => m_fsm.State == EnemyAnimState.Dead;
        }

        protected override void Awake()
        {
            base.Awake();
            m_playerDetect = GetComponent<PlayerDectect>();
            m_startingPos = transform.position;
        }

        protected void FSMInit(MonoBehaviour behav)
        {
            m_fsm = StateMachine<EnemyAnimState>.Initialize(behav);
            m_fsm.ChangeState(EnemyAnimState.Moving);
        }

        protected override void Init()
        {
            if(stat != null)
            {
                m_curStat = (EnemyStat)stat;
            }
        }

        protected virtual void Update()
        {
            ActionHandle();
        }

        private void ActionHandle()
        {
            if (IsDead)
            {
                m_fsm.ChangeState(EnemyAnimState.Dead);
            }

            if (m_isKnockBack || IsDead) return;

            if (m_playerDetect.IsDetected)
            {
                m_fsm.ChangeState(EnemyAnimState.Chasing);
            }
            else
            {
                m_fsm.ChangeState(EnemyAnimState.Moving);
            }

            if (m_rb.velocity.y <= -50)
            {
                Dead();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (m_isKnockBack || IsDead) return;

            Move();
        }

        protected override void Dead()
        {
            base.Dead();
            m_fsm.ChangeState(EnemyAnimState.Dead);
        }

        protected void GetTargetDir()
        {
            m_targetDir = m_playerDetect.Target.transform.position - transform.position;
            m_targetDir.Normalize();
        }

        public virtual void Move()
        {

        }

        public override void TakeDamage(int dmg, Actor whoHit = null)
        {
            if (IsDead) return;
            base.TakeDamage(dmg, whoHit);
            if(m_curHp > 0 && !m_isInvincible)
            {
                m_fsm.ChangeState(EnemyAnimState.GotHit);
            }
        }

        #region FSM
        protected virtual void Moving_Enter() { }
        protected virtual void Moving_Update() {
            m_curSpeed = m_curStat.moveSpeed;
            Helper.PlayAnim(m_anim, EnemyAnimState.Moving.ToString());
        }
        protected virtual void Moving_Exit() { }
        protected virtual void Chasing_Enter() { }
        protected virtual void Chasing_Update() {
            m_curSpeed = m_curStat.chasingSpeed;
            Helper.PlayAnim(m_anim, EnemyAnimState.Chasing.ToString());
        }
        protected virtual void Chasing_Exit() { }
        protected virtual void GotHit_Enter() { }
        protected virtual void GotHit_Update() { }
        protected virtual void GotHit_Exit() { }
        protected virtual void Dead_Enter() {
            if (deadVfxPb)
            {
                Instantiate(deadVfxPb, transform.position, Quaternion.identity);
            }
            gameObject.SetActive(false);
            AudioController.Ins.PlaySound(AudioController.Ins.enemyDead);
        }
        protected virtual void Dead_Update() { }
        protected virtual void Dead_Exit() { }
        #endregion
    }
}
