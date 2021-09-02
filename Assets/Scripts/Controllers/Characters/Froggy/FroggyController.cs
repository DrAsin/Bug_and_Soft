using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Damage;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Froggy
{
    public class FroggyController : BaseController
    {
        public GameObject _froggyTongue;

        [Header("Froggy Specific Values")] [SerializeField]
        private AttackStateData _attackStateData;

        [SerializeField] private DeadStateData _deadStateData;
        [SerializeField] private IdleStateData _idleStateData;
        [SerializeField] private JumpStateData _jumpStateData;
        [SerializeField] private PrepareAttackStateData _prepareAttackStateData;
        [SerializeField] private DamageStateData _damageStateData;
        [SerializeField] private GameObject _farty;
        public GameObject _superFroggyNuke;
        private bool canFroggyDie = false;
        private FroggyTongueController instatiatedTongue;

        public Froggy_IdleState _idleState { get; private set; }
        public JumpState _jumpState { get; private set; }
        public Froggy_PrepareAttackState _prepareAttackState { get; private set; }
        public Froggy_AttackState _attackState { get; private set; }
        public Froggy_DeadState _deadState { get; private set; }
        public Froggy_DamageState _damageState { get; private set; }
        public Froggy_NearAttackState _nearAttackState { get; private set; }

        // Super Froggy
        public bool transforming;
        public int currentPhase = 1;
        public bool transformed;
        [SerializeField] private SuperFroggy_SecondPhaseStateData _superFroggySecondPhaseStateData;
        
        public SuperFroggy_SecondPhase _superFroggySecondPhase { get; private set; }
        public int hallPosition = 0;

        protected override void Start()
        {
            base.Start();

            _jumpState = new JumpState(this, StateMachine, "Jumping", _jumpStateData, this);
            _idleState = new Froggy_IdleState(this, StateMachine, "Idle", _idleStateData, this);
            _prepareAttackState =
                new Froggy_PrepareAttackState(this, StateMachine, "PreparingAttack", _prepareAttackStateData, this);
            _attackState = new Froggy_AttackState(this, StateMachine, "Attacking", _attackStateData, this);
            _deadState = new Froggy_DeadState(this, StateMachine, "Dead", _deadStateData, this);
            _damageState = new Froggy_DamageState(this, StateMachine, "Dead", _damageStateData, this);
            _nearAttackState = new Froggy_NearAttackState(this, StateMachine, "Idle", _jumpStateData, this);
            _superFroggySecondPhase = new SuperFroggy_SecondPhase(this, StateMachine, "Idle", _superFroggySecondPhaseStateData, this);
            
            StateMachine.Initialize(_idleState);

            InvokeRepeating("MoveCejas", 0f, 7f);
        }
        
        public void Anim_OnAttackingAnimStarted()
        {
            _attackState.attackStarted = true;
        }

        /// <summary>
        /// Ataque especial de Froggy => Lengua suculenta
        /// </summary>
        public void SpecialAttack()
        {
            instatiatedTongue = Instantiate(_froggyTongue, transform.position, Quaternion.Inverse(transform.rotation))
                .GetComponent<FroggyTongueController>();
            instatiatedTongue.SetProps(this, _attackState);
        }

        /// <summary>
        ///     Metodo para empezar la animacion de mover cejas del PJ Njord
        /// </summary>
        private void MoveCejas()
        {
            GetAnimator().SetBool("EyebrowsMovement", true);
        }

        /// <summary>
        ///     Metodo para terminar la animacion de mover cejas del PJ Njord
        /// </summary>
        /// <remarks>
        ///     Es llamado al final de la animacion cejas.
        /// </remarks>
        private void AnimCejasEnded()
        {
            GetAnimator().SetBool("EyebrowsMovement", false);
        }

        public override void Die()
        {
            base.Die();
            StateMachine.ChangeState(_deadState);
        }

        // SuperFroggy : MiniBoss => SecondPhase
        public void EnterPhaseTwo()
        {
            if (controllerKind == EControllerKind.Boss && currentPhase == 1)
            {
                transforming = true;
                currentPhase++;
                StateMachine.ChangeState(_superFroggySecondPhase);
            }
        }

        public void Explode()
        {
            Instantiate(_farty, transform.position, Quaternion.Euler(0f, 0f, 0f));
            Destroy(gameObject);
        }

        public void PlaceBomb()
        {
            Instantiate(_superFroggySecondPhaseStateData.bombs, transform.position + new Vector3(0f, 2f), Quaternion.Euler(0f, 0f, 0f));
        }
    }
}