using UnityEngine;

namespace MiniIT.ArkanoidTest
{
    public class PlatformController : MonoBehaviour, IMovable
    {
        private bool isUnderControl = false;

        private void Update()
        {
            if (isUnderControl)
                Move();
        }

        /// <summary>The platform goes under the control of the player</summary>
        public void StartMoving()
        {
            isUnderControl = true;
        }

        [SerializeField]
        private float moveSpeed = 0;
        public float MoveSpeed
        {
            get
            {
                return moveSpeed;
            }

            set
            {
                moveSpeed = value;
            }
        }

        private float startTouchPos = 0;
        private float boundsX = 2;
        private float deltaPos = 0;

        private void Move()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPos = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                deltaPos = Input.mousePosition.x - startTouchPos;

                transform.position = new Vector2(Mathf.Lerp(transform.position.x, transform.position.x + deltaPos, Time.deltaTime * moveSpeed), transform.position.y);
                transform.position = new Vector2(Mathf.Clamp(transform.position.x, -boundsX, boundsX), transform.position.y);
            }

            startTouchPos = Input.mousePosition.x;
        }

        /// <summary>Returns all platform values ​​to starting points</summary>
        public void ResetPlatform()
        {
            startTouchPos = 0;
            transform.position = new Vector2(0, transform.position.y);
            isUnderControl = false;
        }
    }
}