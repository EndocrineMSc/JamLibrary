using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.Player.Movement
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float _speed;
        private SpriteRenderer _spriteRenderer;

        #endregion

        #region Private Functions

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector3 moveDelta = new(x, y, 0);

            //Swap sprite direction, whether you're going left or right
            if (x > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (x < 0)
            {
                _spriteRenderer.flipX= true;
            }

            //movement
            transform.Translate(_speed * Time.deltaTime * moveDelta);
        }

        #endregion
    }
}
