using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Attach to player object in scene
    /// Coyote Time: Time after player has left the ground but still is allowed to jump
    /// Apex Threshold: The Y-Axis threshold that is considering the jumps apex
    /// Jump Cut: When player releases jump button before reaching apex
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    internal class PlayerController2D : MonoBehaviour
    {
        #region Fields and Properties

        //This script assumes you have a child object of the player called "PlayerVisuals"
        //which is used to flip the player sprite
        private const string PLAYERVISUALS_NAME = "PlayerVisuals";
        private const string X_AXIS = "Horizontal";
        private const string Y_AXIS = "Vertical";

        private Rigidbody2D _rigidbody;
        private Collider2D _playerCollider;
        private Vector2 _moveInput;
        private GameObject _playerVisuals;

        private bool _isFacingRight;
        private bool _isJumping;
        private bool _isJumpFalling;
        private bool _jumpWasCut;
        private bool _isFalling;

        private float _coyoteTimeCooldown;
        private float _jumpInputBufferCooldown;

        [SerializeField] private LayerMask _groundLayer;

        //Fields contains exemplary seetings, fine tune to fit your game
        [Header("Gravity")]
        [SerializeField] private float _maxFallSpeed = 30;
        [SerializeField] private float _maxFastFallSpeed = 45;
        [SerializeField] private float _fallGravityMultiplier = 5;
        [SerializeField] private float _fastFallGravityMultiplier = 8;
        [SerializeField] private float _jumpCutGravityMultiplier = 5;
        [SerializeField] private float _jumpApexGravityMultiplier = 0.4f;

        [Header("Run")]
        [SerializeField] private float _runMaxSpeed = 15;
        [SerializeField] private float _runAccelerationSpeed = 3;
        [SerializeField] private float _runDeccelerationSpeed;
        [SerializeField] private float _accelarationInAir = 1;
        [SerializeField] private float _deccelerationInAir = 1;

        [Header("Jump")]
        [SerializeField] private float _jumpHeight = 20;
        [SerializeField] private float _jumpTimeToApex = 1;
        [SerializeField] private float _jumpApexDefiningThreshold = 0.1f;
        [SerializeField] private float _jumpApexAccelerationMultiplier = 1;
        [SerializeField] private float _jumpApexMaxSpeedMultiplier = 1;
        [SerializeField] private float _coyoteTime = 0.15f;
        [SerializeField] private float _jumpInputBufferTime = 0.1f;

        //Automatically calculated Movement Fields           
        private float _gravityScale;
        private float _runAccelerationAmount;
        private float _runDeccelerationAmount;
        private float _jumpForce;
        private Vector2 _groundCheckSize;
        private Vector2 _groundCheckPoint;

        #endregion

        #region Functions

        #region Set-Up

        private void Start()
        {
            SetReferences();
            SetMovementFieldValues();
            SetMiscellanousVariables();
        }

        private void SetReferences()
        {
            _playerCollider = GetComponentInChildren<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerVisuals = GameObject.Find(PLAYERVISUALS_NAME);
        }

        private void SetMovementFieldValues()
        {
            var gravityStrength = -(2 * _jumpHeight) / (_jumpTimeToApex * _jumpTimeToApex);
            _gravityScale = gravityStrength / Physics2D.gravity.y;
            _runAccelerationAmount = (50 * _runAccelerationSpeed) / _runMaxSpeed;
            _runDeccelerationAmount = (50 * _runDeccelerationSpeed) / _runMaxSpeed;
            _jumpForce = Mathf.Abs(gravityStrength) * _jumpTimeToApex;
            _runAccelerationSpeed = Mathf.Clamp(_runAccelerationSpeed, 0.01f, _runMaxSpeed);
            _runDeccelerationSpeed = Mathf.Clamp(_runDeccelerationSpeed, 0.01f, _runMaxSpeed);
            SetGravityScale(_gravityScale);
        }

        private void SetMiscellanousVariables()
        {
            _isFacingRight = true;
            _groundCheckSize = new(_playerCollider.bounds.size.x - 0.05f, _playerCollider.bounds.size.y + 0.05f);
        }

        private void SetGravityScale(float gravityScale)
        {
            _rigidbody.gravityScale = gravityScale;
        }

        #endregion

        #region Updates

        private void Update()
        {
            SetTimers();
            HandleInputs();
            SetNewGroundCheckPoint();
            SetPlayerIsGroundedConditions();
            SetPlayerStartsFallingConditions();

            if (CanPlayerJump())
            {
                _jumpWasCut = false;
                _isJumpFalling = false;

                if (_jumpInputBufferCooldown > 0)
                {
                    Jump();
                }
            }

            if (CheckForFastFallGravityChange())
            {
                SetGravityScale(_gravityScale * _fastFallGravityMultiplier);
                _rigidbody.velocity = new(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, -_maxFastFallSpeed));
            }
            else if (CheckForJumpCutGravityChange())
            {
                SetGravityScale(_gravityScale * _jumpCutGravityMultiplier);
                _rigidbody.velocity = new(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, -_maxFastFallSpeed));
            }
            else if (CheckForJumpApexGravityChange())
            {
                SetGravityScale(_gravityScale * _jumpApexGravityMultiplier);
            }
            else if (CheckForFallGravityChange())
            {
                SetGravityScale(_gravityScale * _fallGravityMultiplier);
                _rigidbody.velocity = new(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, -_maxFallSpeed));
            }
            else
            {
                SetGravityScale(_gravityScale);
            }
        }

        #region Update Helper Functions

        private void SetTimers()
        {
            _jumpInputBufferCooldown -= Time.deltaTime;
            _coyoteTimeCooldown -= Time.deltaTime;
        }

        private void HandleInputs()
        {
            _moveInput.x = Input.GetAxisRaw(X_AXIS);
            _moveInput.y = Input.GetAxisRaw(Y_AXIS);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J))
            {
                //note: good place to set the player animation to jumping
                _jumpInputBufferCooldown = _jumpInputBufferTime;
            }

            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.J)) &&
                (_isJumping && _rigidbody.velocity.y > 0))
            {
                _jumpWasCut = true;
            }
        }

        private void SetNewGroundCheckPoint()
        {
            _groundCheckPoint = new(_playerVisuals.transform.position.x, _playerVisuals.transform.position.y - 0.2f);
        }

        private void SetPlayerIsGroundedConditions()
        {
            var isGrounded = CheckIfGrounded();
            if (isGrounded && !_isJumping)
            {
                _coyoteTimeCooldown = _coyoteTime;

                //if player was falling before, now he isn't anymore
                if (_isFalling)
                {
                    _isFalling = false;
                }
            }
        }

        private bool CheckIfGrounded()
        {
            return Physics2D.OverlapBox(_groundCheckPoint, _groundCheckSize, 0f, _groundLayer);
        }

        private void SetPlayerStartsFallingConditions()
        {
            if (_isJumping && _rigidbody.velocity.y < 0)
            {
                //note: good place to set the player animator to falling
                _isFalling = true;
                _isJumping = false;
                _isJumpFalling = true;
            }
        }

        private bool CanPlayerJump()
        {
            return (_coyoteTimeCooldown > 0 && !_isJumping);
        }

        private void Jump()
        {
            //note: good place to play jump sound effect
            _isJumping = true;
            _jumpInputBufferCooldown = 0;
            _coyoteTimeCooldown = 0;

            float force = _jumpForce;
            if (_rigidbody.velocity.y < 0)
            {
                force -= _rigidbody.velocity.y;
            }

            _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        private bool CheckForFastFallGravityChange()
        {
            return (_rigidbody.velocity.y < 0 && _moveInput.y < 0);
        }

        private bool CheckForJumpCutGravityChange()
        {
            return (_jumpWasCut);
        }

        private bool CheckForJumpApexGravityChange()
        {
            return ((_isJumping || _isJumpFalling) && (Mathf.Abs(_rigidbody.velocity.y) < _jumpApexDefiningThreshold));
        }

        private bool CheckForFallGravityChange()
        {
            return (_rigidbody.velocity.y < 0);
        }

        #endregion

        private void FixedUpdate()
        {
            if (_moveInput.x != 0 && !CheckIfFacingRightDirection())
            {
                Turn();
            }
            MoveHorizontal();
        }

        private bool CheckIfFacingRightDirection()
        {
            return ((_moveInput.x < 0) != _isFacingRight);
        }

        private void Turn()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            _isFacingRight = !_isFacingRight;
        }

        private void MoveHorizontal()
        {
            float targetSpeed = _moveInput.x * _runMaxSpeed;
            targetSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed, 1);

            //Calculate Acceleration Rate
            float accelerationRate;
            if (_coyoteTimeCooldown > 0)
            {
                accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _runAccelerationAmount : _runDeccelerationAmount;
            }
            else
            {
                accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? (_runAccelerationAmount * _accelarationInAir) : (_runDeccelerationAmount * _deccelerationInAir);
            }

            //Change Acceleration during Jump Apex
            if (_isJumping || _isJumpFalling)
            {
                accelerationRate *= _jumpApexAccelerationMultiplier;
                targetSpeed *= _jumpApexMaxSpeedMultiplier;
            }

            //Conserve Momentum
            if ((Mathf.Abs(_rigidbody.velocity.x) > Mathf.Abs(targetSpeed))
                && (Mathf.Sign(_rigidbody.velocity.x) == Mathf.Sign(targetSpeed))
                && (Mathf.Abs(targetSpeed) > 0.01f) && (_coyoteTimeCooldown < 0))
            {
                accelerationRate = 0;
            }

            float speedDifference = targetSpeed - _rigidbody.velocity.x;
            float movement = speedDifference * accelerationRate;

            _rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_groundCheckPoint, _groundCheckSize);
        }

        #endregion
    }
}
