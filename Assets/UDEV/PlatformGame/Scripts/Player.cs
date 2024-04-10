using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System;

namespace UDEV.PlatformGame
{
    public class Player : Actor
    {
        private StateMachine<PlayerAnimState> m_fsm;
        [Header("Smooth Jumping Setting:")]
        [Range(0f, 5f)]
        public float jumpingFallingMultipiler = 2.5f;
        [Range(0f, 5f)]
        public float lowJumpingMultipiler = 2.5f;

        [Header("References:")]
        public SpriteRenderer sp;
        public ObstacleChecker obstacleChker;
        public CapsuleCollider2D defaultCol;
        public CapsuleCollider2D flyingCol;
        public CapsuleCollider2D inWaterCol;

        private PlayerStat m_curStat;
        private PlayerAnimState m_prevState;
        private float m_waterFallingTime = 1f;
        private float m_attackTime;
        private bool m_isAttacked;

        private bool IsDead
        {
            get => m_fsm.State == PlayerAnimState.Dead || m_prevState == PlayerAnimState.Dead;
        }

        private bool IsJumping
        {
            get => m_fsm.State == PlayerAnimState.Jump ||
                m_fsm.State == PlayerAnimState.OnAir ||
                m_fsm.State == PlayerAnimState.Land;
        }

        private bool IsFlying
        {
            get => m_fsm.State == PlayerAnimState.OnAir ||
                m_fsm.State == PlayerAnimState.Fly ||
                m_fsm.State == PlayerAnimState.FlyOnAir;
        }

        private bool IsAttacking
        {
            get => m_fsm.State == PlayerAnimState.HammerAttack ||
                m_fsm.State == PlayerAnimState.FireBullet;
        }

        protected override void Awake()
        {
            base.Awake();
            m_fsm = StateMachine<PlayerAnimState>.Initialize(this);
            m_fsm.ChangeState(PlayerAnimState.Idle);
        }

        protected override void Init()
        {
            base.Init();
            if(stat != null)
            {
                m_curStat = (PlayerStat)stat;
            }
        }

        private void Update()
        {
            if (sp)
            {
                if (obstacleChker.IsOnWater)
                {
                    sp.sortingOrder = (int)SpriteOrder.InWater;
                }else
                {
                    sp.sortingOrder = (int)SpriteOrder.Normal;
                }
            }

            if (IsDead)
            {
                GameManager.Ins.SetMapSpeed(0f);
            }

            ActionHandle();
        }

        private void FixedUpdate()
        {
            SmoothJump();
        }

        private void ActionHandle()
        {
            ReduceActionRate(ref m_isAttacked, ref m_attackTime, m_curStat.attackRate);

            if (IsAttacking || m_isKnockBack) return;


            if (GamepadController.Ins.IsStatic)
            {
                GameManager.Ins.SetMapSpeed(0f);
                m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
            }else
            {
                
            }
            if (obstacleChker.IsOnLadder && m_fsm.State != PlayerAnimState.LadderIdle
                && m_fsm.State != PlayerAnimState.OnLadder
                )
            {
                ChangeState(PlayerAnimState.LadderIdle);
            }


            if (!obstacleChker.IsOnWater)
            {
                AttackChecking(); 
            }
        }

        protected override void Dead()
        {
            if (IsDead) return;
            
            if(GameManager.Ins.CurLive > 0)
            {
                ChangeState(PlayerAnimState.Idle);
                GameManager.Ins.Revive();
            }else
            {
                base.Dead();
                ChangeState(PlayerAnimState.Dead);
            }
        }

        private void Move(Direction dir)
        {
            if (m_isKnockBack) return;

            m_rb.isKinematic = false;

            if(dir == Direction.Left || dir == Direction.Right)
            {
                Flip(dir);

                m_hozDir = dir == Direction.Left ? -1 : 1;

                if (GameManager.Ins.setting.isOnMobile)
                {
                    m_rb.velocity = new Vector2(GamepadController.Ins.joystick.xValue * m_curSpeed, m_rb.velocity.y);
                }
                else
                {
                    m_rb.velocity = new Vector2(m_hozDir * m_curSpeed, m_rb.velocity.y);
                }

                if (CameraFollow.Ins.IsHozStuck)
                {
                    GameManager.Ins.SetMapSpeed(0f);
                }else
                {
                    GameManager.Ins.SetMapSpeed(-m_hozDir * m_curSpeed);
                }
            }else if(dir == Direction.Up || dir == Direction.Down)
            {
                m_vertDir = dir == Direction.Down ? -1 : 1;

                if(GameManager.Ins.setting.isOnMobile)
                {
                    m_rb.velocity = new Vector2(m_rb.velocity.x, GamepadController.Ins.joystick.yValue * m_curSpeed);
                }
                else
                {
                    m_rb.velocity = new Vector2(m_rb.velocity.x, m_vertDir * m_curSpeed);
                }
            }
        }

        private void Jump()
        {
            //GamepadController.Ins.CanJump = false;
            m_rb.velocity = new Vector2(m_rb.velocity.x, 0f);
            m_rb.isKinematic = false;
            m_rb.gravityScale = m_startingGrav;
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_curStat.jumpForce);
        }

        private void SmoothJump()
        {
            if (obstacleChker.IsOnGround || (obstacleChker.IsOnWater && IsJumping)) return;

            if (m_rb.velocity.y < 0)
            {
                m_rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpingFallingMultipiler - 1) * Time.deltaTime;
            }
            else if (m_rb.velocity.y > 0 && !GamepadController.Ins.IsJumpHolding)
            {
                m_rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpingMultipiler - 1) * Time.deltaTime;
            }
        }

        private void JumpChecking()
        {
            if (GamepadController.Ins.CanJump)
            {
                Jump();
                ChangeState(PlayerAnimState.Jump);
            }
        }

        private void HozMoveChecking()
        {
            if (GamepadController.Ins.CanMoveLeft)
            {
                Move(Direction.Left);
            }

            if (GamepadController.Ins.CanMoveRight)
            {
                Move(Direction.Right);
            }
        }

        private void VertMoveChecking()
        {
            if (IsJumping) return;

            if (GamepadController.Ins.CanMoveUp)
            {
                Move(Direction.Up);
            }else if (GamepadController.Ins.CanMoveDown)
            {
                Move(Direction.Down);
            }

            GamepadController.Ins.CanFly = false;
        }

        private void WaterChecking()
        {
            if (obstacleChker.IsOnLadder) return;

            if (obstacleChker.IsOnDeepWater)
            {
                m_rb.gravityScale = 0f;
                m_rb.velocity = new Vector2(m_rb.velocity.x, 0f);
                ChangeState(PlayerAnimState.SwimOnDeep);
            }else if(obstacleChker.IsOnWater && !IsJumping)
            {
                m_waterFallingTime -= Time.deltaTime;
                if(m_waterFallingTime <= 0)
                {
                    m_rb.gravityScale = 0f;
                    m_rb.velocity = Vector2.zero;
                }
                GamepadController.Ins.CanMoveUp = false;
                ChangeState(PlayerAnimState.Swim);
            }
        }

        private void AttackChecking()
        {
            if (GamepadController.Ins.CanAttack)
            {
                if (m_isAttacked) return;
                    
                ChangeState(PlayerAnimState.HammerAttack);
            }
            else if (GamepadController.Ins.CanFire && GameManager.Ins.CurBullet > 0)
            {
                // kiem tra xem co con du dan hay khong
                // thi moi chuyen trang thai
                ChangeState(PlayerAnimState.FireBullet);
            }
        }

        public override void TakeDamage(int dmg, Actor whoHit = null)
        {
            if (IsDead) return;
            base.TakeDamage(dmg, whoHit);
            GameData.Ins.hp = m_curHp;
            if(m_curHp > 0 && !m_isInvincible)
            {
                ChangeState(PlayerAnimState.GotHit);
            }
            GUIManager.Ins.UpdateHp(m_curHp);
        }

        public void ChangeState(PlayerAnimState state)
        {
            m_prevState = m_fsm.State;
            m_fsm.ChangeState(state);
        }

        private IEnumerator ChangeStateDelayCo(PlayerAnimState newState, float timeExtra = 0)
        {
            var animClip = Helper.GetClip(m_anim, m_fsm.State.ToString());
            if(animClip != null)
            {
                yield return new WaitForSeconds(animClip.length + timeExtra);
                ChangeState(newState);
            }
            yield return null;
        }

        private void ChangeStateDelay(PlayerAnimState newState, float timeExtra = 0)
        {
            StartCoroutine(ChangeStateDelayCo(newState, timeExtra));
        }

        private void ActiveCol(PlayerCollider collider)
        {
            if (defaultCol)
                defaultCol.enabled = collider == PlayerCollider.Default;

            if(flyingCol)
                flyingCol.enabled = collider == PlayerCollider.Flying;

            if(inWaterCol)
                inWaterCol.enabled = collider == PlayerCollider.InWater;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag(GameTag.Enemy.ToString())){
                Enemy enemy = col.gameObject.GetComponent<Enemy>();

                if (enemy)
                {
                    TakeDamage(enemy.stat.damage, enemy);
                }
            }

            if (col.gameObject.CompareTag(GameTag.MovingPlatform.ToString()))
            {
                m_rb.isKinematic = true;
                transform.SetParent(col.gameObject.transform);
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.CompareTag(GameTag.MovingPlatform.ToString()))
            {
                if(obstacleChker.IsOnGround && m_fsm.State == PlayerAnimState.Idle)
                {
                    m_rb.isKinematic = true;
                    transform.SetParent(col.gameObject.transform);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject.CompareTag(GameTag.MovingPlatform.ToString()))
            {
                if (!obstacleChker.IsOnGround)
                {
                    m_rb.isKinematic = false;
                    transform.SetParent(null);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(GameTag.Thorn.ToString())) {
                TakeDamage(1);
            }

            if (col.CompareTag(GameTag.CheckPoint.ToString())){
                GameManager.Ins.SaveCheckPoint();
            }

            if (col.CompareTag(GameTag.Collectable.ToString()))
            {
                Collectable collectable = col.GetComponent<Collectable>();
                if (collectable)
                {
                    collectable.Trigger();
                }
            }

            if (col.CompareTag(GameTag.Door.ToString()))
            {
                Door door = col.GetComponent<Door>();
                if (door)
                {
                    door.OpenDoor();

                    if (door.IsOpened)
                    {
                        ChangeState(PlayerAnimState.SayHello);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag(GameTag.DeadZone.ToString())) {
                Dead();
            }
        }

        #region FSM

        #region SAY_HELLO_STATE
        private void SayHello_Enter() { }
        private void SayHello_Update() {
            gameObject.layer = invincibleLayer;
            m_rb.velocity = Vector2.zero;
            Helper.PlayAnim(m_anim, PlayerAnimState.SayHello.ToString());
        }
        private void SayHello_Exit() { }
        #endregion

        #region SAY_WALK_STATE
        private void Walk_Enter() {
            ActiveCol(PlayerCollider.Default);
            m_curSpeed = stat.moveSpeed;
        }
        private void Walk_Update() {
            JumpChecking();

            if(!GamepadController.Ins.CanMoveLeft && !GamepadController.Ins.CanMoveRight)
            {
                ChangeState(PlayerAnimState.Idle);
            }

            if (!obstacleChker.IsOnGround)
            {
                ChangeState(PlayerAnimState.OnAir);
            }

            HozMoveChecking();

            Helper.PlayAnim(m_anim, PlayerAnimState.Walk.ToString());
        }
        private void Walk_Exit() { }
        #endregion

        #region JUMP_STATE
        private void Jump_Enter() {
            ActiveCol(PlayerCollider.Default);
            AudioController.Ins.PlaySound(AudioController.Ins.jump);
        }
        private void Jump_Update() {
            m_rb.isKinematic = false;
            if((m_rb.velocity.y < 0 && !obstacleChker.IsOnGround))
            {
                ChangeState(PlayerAnimState.OnAir);
            }

            if (obstacleChker.IsOnGround && m_rb.velocity.y == 0)
            {
                ChangeState(PlayerAnimState.Land);
            }

            HozMoveChecking();
            Helper.PlayAnim(m_anim, PlayerAnimState.Jump.ToString());
        }
        private void Jump_Exit() { }
        #endregion

        #region ON_AIR_STATE
        private void OnAir_Enter() {

            ActiveCol(PlayerCollider.Default);
        }
        private void OnAir_Update() {
            m_rb.gravityScale = m_startingGrav;

            if (obstacleChker.IsOnGround)
            {
                ChangeState(PlayerAnimState.Land);
            }

            if (GamepadController.Ins.CanFly)
            {
                ChangeState(PlayerAnimState.Fly);
            }

            if (obstacleChker.IsOnWater)
            {
                m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
                WaterChecking();
            }

            Helper.PlayAnim(m_anim, PlayerAnimState.OnAir.ToString());
        }
        private void OnAir_Exit() { }
        #endregion

        #region lAND_STATE
        private void Land_Enter() {
            ActiveCol(PlayerCollider.Default);
            ChangeStateDelay(PlayerAnimState.Idle);
            AudioController.Ins.PlaySound(AudioController.Ins.land);
        }
        private void Land_Update() {
            m_rb.velocity = Vector2.zero;
            Helper.PlayAnim(m_anim, PlayerAnimState.Land.ToString());
        }
        private void Land_Exit() { }
        #endregion

        #region FLY_STATE
        private void Fly_Enter() {
            ActiveCol(PlayerCollider.Flying);
            ChangeStateDelay(PlayerAnimState.FlyOnAir);
        }
        private void Fly_Update() {
            if (obstacleChker.IsOnWater)
            {
                m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
                WaterChecking();
            }
            HozMoveChecking();
            m_rb.velocity = new Vector2(m_rb.velocity.x, -m_curStat.flyingSpeed);
            Helper.PlayAnim(m_anim, PlayerAnimState.Fly.ToString());
        }
        private void Fly_Exit() { }
        #endregion

        #region FLY_ON_AIR_STATE
        private void FlyOnAir_Enter() {
            ActiveCol(PlayerCollider.Flying);
        }
        private void FlyOnAir_Update() {
            if (obstacleChker.IsOnWater)
            {
                m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
                WaterChecking();
            }
            HozMoveChecking();
            m_rb.velocity = new Vector2(m_rb.velocity.x, -m_curStat.flyingSpeed);
            if (obstacleChker.IsOnGround)
            {
                ChangeState(PlayerAnimState.Land);
            }

            if (!GamepadController.Ins.CanFly)
            {
                ChangeState(PlayerAnimState.OnAir);
            }

            Helper.PlayAnim(m_anim, PlayerAnimState.FlyOnAir.ToString());
        }
        private void FlyOnAir_Exit() { }
        #endregion

        #region SWIM_STATE
        private void Swim_Enter()
        {
            m_curSpeed = m_curStat.swimSpeed;
            ActiveCol(PlayerCollider.InWater);
        }
        private void Swim_Update()
        {
            JumpChecking();
            WaterChecking();
            HozMoveChecking();
            VertMoveChecking();

            Helper.PlayAnim(m_anim, PlayerAnimState.Swim.ToString());
        }
        private void Swim_Exit()
        {
            m_waterFallingTime = 1f;
        }
        #endregion

        #region SWIM_ON_DEEP_STATE
        private void SwimOnDeep_Enter()
        {
            ActiveCol(PlayerCollider.InWater);
            m_curSpeed = m_curStat.swimSpeed;
            m_rb.velocity = Vector2.zero;
        }
        private void SwimOnDeep_Update()
        {
            WaterChecking();
            HozMoveChecking();
            VertMoveChecking();
            Helper.PlayAnim(m_anim, PlayerAnimState.SwimOnDeep.ToString());
        }
        private void SwimOnDeep_Exit()
        {
            m_rb.velocity = Vector2.zero;
            GamepadController.Ins.CanMoveUp = false;
        }
        #endregion

        #region LADDER_IDLE_STATE
        private void LadderIdle_Enter()
        {
            m_rb.velocity = Vector2.zero;
            ActiveCol(PlayerCollider.Default);
            m_curSpeed = m_curStat.ladderSpeed;
        }
        private void LadderIdle_Update()
        {
            if (GamepadController.Ins.CanMoveUp || GamepadController.Ins.CanMoveDown)
            {
                ChangeState(PlayerAnimState.OnLadder);
            }

            if (!obstacleChker.IsOnLadder)
            {
                ChangeState(PlayerAnimState.OnAir);
            }
            GamepadController.Ins.CanFly = false;
            m_rb.gravityScale = 0;
            HozMoveChecking();

            Helper.PlayAnim(m_anim, PlayerAnimState.LadderIdle.ToString());
        }
        private void LadderIdle_Exit() { }
        #endregion

        #region ON_LADDER_STATE
        private void OnLadder_Enter() {
            m_rb.velocity = Vector2.zero;
            ActiveCol(PlayerCollider.Default);
        }
        private void OnLadder_Update() {
            VertMoveChecking();
            HozMoveChecking();

            if(!GamepadController.Ins.CanMoveUp && !GamepadController.Ins.CanMoveDown)
            {
                m_rb.velocity = new Vector2(m_rb.velocity.x, 0f);
                ChangeState(PlayerAnimState.LadderIdle);
            }

            if(!obstacleChker.IsOnLadder)
            {
                ChangeState(PlayerAnimState.OnAir);
            }

            GamepadController.Ins.CanFly = false;
            m_rb.gravityScale = 0f;

            Helper.PlayAnim(m_anim, PlayerAnimState.OnLadder.ToString());
        }
        private void OnLadder_Exit() { }
        private void Dead_Enter() {
            CamShake.Ins.ShakeTrigger(0.7f, 0.1f);
            GameManager.Ins.LevelFailed();
            AudioController.Ins.PlaySound(AudioController.Ins.dead);
        }
        #endregion

        #region DEAD_STATE
        private void Dead_Update() {
            gameObject.layer = deadLayer;
            GUIManager.Ins.UpdateHp(m_curHp);
            Helper.PlayAnim(m_anim, PlayerAnimState.Dead.ToString());
        }
        private void Dead_Exit() { }
        private void Idle_Enter() {
            ActiveCol(PlayerCollider.Default);
        }
        private void Idle_Update() {

            JumpChecking();

            if(GamepadController.Ins.CanMoveLeft || GamepadController.Ins.CanMoveRight)
            {
                ChangeState(PlayerAnimState.Walk);
            }

            Helper.PlayAnim(m_anim, PlayerAnimState.Idle.ToString());
        }
        private void Idle_Exit() { }
        #endregion    

        #region FIREBULLET_STATE
        private void FireBullet_Enter()
        {
            ChangeStateDelay(PlayerAnimState.Idle);
        }
        private void FireBullet_Update()
        {
            Helper.PlayAnim(m_anim, PlayerAnimState.FireBullet.ToString());
        }
        private void FireBullet_Exit() { }
        #endregion

        #region HAMMER_ATTACK_STATE
        private void HammerAttack_Enter() {
            m_isAttacked = true;
            ChangeStateDelay(PlayerAnimState.Idle);
        }
        private void HammerAttack_Update() {
            m_rb.velocity = Vector2.zero;
            Helper.PlayAnim(m_anim, PlayerAnimState.HammerAttack.ToString());
        }
        private void HammerAttack_Exit() { }
        #endregion

        #region GOT_HIT_STATE
        private void GotHit_Enter() {
            AudioController.Ins.PlaySound(AudioController.Ins.getHit);
        }
        private void GotHit_Update() {
            if (m_isKnockBack)
            {
                KnockBackMove(0.25f);
            }else if (obstacleChker.IsOnWater)
            {
                if (obstacleChker.IsOnDeepWater)
                {
                    ChangeState(PlayerAnimState.SwimOnDeep);
                }
                else
                {
                    ChangeState(PlayerAnimState.Swim);
                }
            }else
            {
                ChangeState(PlayerAnimState.Idle);
            }

            GUIManager.Ins.UpdateHp(m_curHp);
        }
        private void GotHit_Exit() { }
        #endregion

        #endregion
    }
}
