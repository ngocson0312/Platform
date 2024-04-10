using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class LineMoving : MonoBehaviour
    {
        public Direction moveDir;
        public float movingDist;
        public float speed;
        public bool isOnlyUp;
        public bool isAuto;

        private Vector2 m_dest;
        private Vector3 m_backDir;
        private Vector3 m_startingPos;
        private Rigidbody2D m_rb;
        private bool m_isGizmHaveStartPos;

        public Vector2 Dest { get => m_dest;}
        public Vector3 BackDir { get => m_backDir;}

        private void Awake()
        {
            m_rb= GetComponent<Rigidbody2D>();
            m_startingPos = transform.position;
        }

        private void Start()
        {
            GetMovingDest();
        }

        private void Update()
        {
            m_backDir = m_startingPos - transform.position;
            m_backDir.Normalize();
        }

        private void FixedUpdate()
        {
            if (!isAuto) return;

            Move();
            SwitchDirChecking();
        }

        public void GetMovingDest()
        {
            switch (moveDir)
            {
                case Direction.Left:
                    m_dest = new Vector2(m_startingPos.x - movingDist, transform.position.y);
                    break;
                case Direction.Right:
                    m_dest = new Vector2(m_startingPos.x + movingDist, transform.position.y);
                    break;
                case Direction.Up:
                    m_dest = new Vector2(transform.position.x, m_startingPos.y + movingDist);
                    break;
                case Direction.Down:
                    m_dest = new Vector2(transform.position.x, m_startingPos.y - movingDist);
                    break;
            }
        }

        public bool IsReached()
        {
            float dist1 = Vector2.Distance(m_startingPos, transform.position);
            float dist2 = Vector2.Distance(m_startingPos, m_dest);
            
            return dist1 > dist2;
        }

        public void SwitchDir(Vector2 dir)
        {
            if(moveDir == Direction.Left || moveDir == Direction.Right)
            {
                moveDir = dir.x < 0 ? Direction.Left : Direction.Right;
            }else if(moveDir == Direction.Up || moveDir == Direction.Down)
            {
                moveDir = dir.y < 0 ? Direction.Down : Direction.Up;
            }
        }

        public void SwitchDirChecking()
        {
            if (IsReached())
            {
                SwitchDir(m_backDir);

                GetMovingDest();
            }
        }

        public void Move()
        {
            switch (moveDir)
            {
                case Direction.Left:
                    m_rb.velocity = new Vector2(-speed, m_rb.velocity.y);
                    //transform.position = new Vector2(transform.position.x, m_startingPos.y);
                    break;
                case Direction.Right:
                    m_rb.velocity = new Vector2(speed, m_rb.velocity.y);
                    //transform.position = new Vector2(transform.position.x, m_startingPos.y);
                    break;
                case Direction.Up:
                    m_rb.velocity = new Vector2(m_rb.velocity.x, speed);
                    transform.position = new Vector2(m_startingPos.x, transform.position.y);
                    break;
                case Direction.Down:
                    transform.position = new Vector2(m_startingPos.x, transform.position.y);

                    if (isOnlyUp) return;
                    m_rb.velocity = new Vector2(m_rb.velocity.x, -speed);
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            if (!m_isGizmHaveStartPos)
            {
                m_startingPos = transform.position;
                GetMovingDest();
                m_isGizmHaveStartPos = true;
            }
            Gizmos.DrawLine(transform.position, m_dest);
        }
    }
}
