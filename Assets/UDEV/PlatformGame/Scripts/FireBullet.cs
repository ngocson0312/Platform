using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.PlatformGame
{
    public class FireBullet : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private Transform m_firePoint;
        [SerializeField] private Bullet m_bulletPb;

        private float m_curSpeed;

        public void Fire()
        {
            if (!m_bulletPb || !m_player || !m_firePoint || GameManager.Ins.CurBullet <= 0) return;

            m_curSpeed = m_player.IsFacingLeft ? -m_bulletPb.speed : m_bulletPb.speed;
            var bulletClone = Instantiate(m_bulletPb, m_firePoint.position, Quaternion.identity);
            bulletClone.speed = m_curSpeed;
            bulletClone.owner = m_player;
            GameManager.Ins.ReduceBullet();

            AudioController.Ins.PlaySound(AudioController.Ins.fireBullet);
        }
    }
}
