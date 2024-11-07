using System;
using _Project.Player.Scripts.Components;
using UnityEngine;

namespace _Project.Player.Scripts.Managers
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerController _playerController;
        private Transform _playerTransform;

        private void Start()
        {
            _playerController = GetComponentInParent<PlayerController>();
            _rigidbody = GetComponentInParent<Rigidbody>();
            _playerTransform = GetComponent<Transform>();
        }
    }
}