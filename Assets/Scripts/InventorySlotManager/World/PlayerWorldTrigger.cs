﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace World
{
    public class PlayerWorldTrigger : MonoBehaviour
    {
        public UnityEvent OnPlayerTrigger;

        [SerializeField] private Collider2D _Trigger;
        [SerializeField] private Collider2D[] _CollisionBlocks;
        [SerializeField] private bool _BlockPassAfterTrigger;
        [SerializeField] private LayerMask _WhatIsPlayer;

        private bool _HasPlayerPassed;
        
        private void Awake()
        {
            if (OnPlayerTrigger == null)
                OnPlayerTrigger = new UnityEvent();
        }

        private void Update()
        {
            CheckForPlayer();
        }

        private void CheckForPlayer()
        {
            if (!_HasPlayerPassed)
            {
                Collider2D col = Physics2D.OverlapBox(_Trigger.bounds.min, _Trigger.bounds.size, 0f, _WhatIsPlayer);
                if (col != null && col.transform.CompareTag("Player"))
                {
                    OnPlayerTrigger.Invoke();

                    if (_BlockPassAfterTrigger)
                    {
                        foreach (var collisionBlock in _CollisionBlocks)
                        {
                            collisionBlock.enabled = true;
                        }
                    }

                    _HasPlayerPassed = true;
                }
            }
        }

        public void UnblockColliders()
        {
            
            foreach (var collisionBlock in _CollisionBlocks)
            {
                collisionBlock.enabled = false;
            }
        }
    }
}