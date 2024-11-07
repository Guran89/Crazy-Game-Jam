using System;
using _Project.Player.Data;
using _Project.Player.Scripts.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Player.Scripts.Managers
{
    public class ShootManager : MonoBehaviour
    {
        // Get references
        private PlayerController _playerController;
        [SerializeField] private Rigidbody _projectilePrefab;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private ParticleSystem _elementParticlePrefab;
        
        // Properties for shot timer
        private float _timeBetweenShots;
        private float _timeSinceLastShot;

        public int CurrentAmmo;
        [SerializeField] private float _projectileSpeed = 20f;
        [SerializeField] private Vector3 _yAim = new Vector3(0f, 0f, 0f);
        private ParticleSystem.EmissionModule _particleEmission;

        private void Start()
        {
            _playerController = GetComponentInParent<PlayerController>();
            _playerController.OnShootInput += ShootBullet;
            _timeBetweenShots = 1f / _playerData.AttackSpeed;
            CurrentAmmo = 5;
            _particleEmission = _elementParticlePrefab.emission;
        }

        private void Update()
        {
            _particleEmission.rateOverTime = CurrentAmmo;
        }

        private void ShootBullet()
        {
            // Shoot only if timer has run out and has ammo
            if (Time.time - _timeSinceLastShot >= _timeBetweenShots && CurrentAmmo > 0)
            {
                Rigidbody projectileClone = Instantiate(_projectilePrefab, transform.position, transform.rotation);
                projectileClone.linearVelocity = transform.TransformDirection((Vector3.forward + _yAim) * _projectileSpeed);

                _timeSinceLastShot = Time.time;
                CurrentAmmo--;
            }
        }

        private void OnDisable()
        {
            _playerController.OnShootInput -= ShootBullet;
        }
    }
}
