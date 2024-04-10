using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class Bullet : MonoBehaviour
    {
        public float speed;
        public LayerMask targetLayer;
        private Vector3 m_prevPos;

        [HideInInspector]
        public Actor owner;

        private void Awake()
        {
            m_prevPos = transform.position;
        }

        private void Update()
        {
            transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            Vector2 dir = (Vector2)(transform.position - m_prevPos);
            float dist = dir.magnitude;
            RaycastHit2D hit = Physics2D.Raycast(m_prevPos, dir, dist, targetLayer);
            if(hit && hit.collider)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.TakeDamage(owner.stat.damage, owner);
                }
                gameObject.SetActive(false);
            }

            m_prevPos = transform.position;
        }
    }
}
