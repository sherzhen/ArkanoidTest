using UnityEngine;

namespace MiniIT.ArkanoidTest
{
    public class BallController : MonoBehaviour, IMovable
    {
        [SerializeField]
        private new Rigidbody2D rigidbody2D = null;
        [SerializeField]
        private AudioSource audioSource = null;

        void FixedUpdate()
        {
            if (isMoving)
            {
                Move();
            }
            else
            {
                WaitForFirstTouch();
            }
        }

        private void WaitForFirstTouch()
        {
            if (Input.GetMouseButton(0))
            {
                StartMoving();
            }
        }

        #region Movement

        private bool isMoving = false;
        private Vector2 currentDirection;

        /// <summary>Allows the ball to start moving</summary>
        public void StartMoving()
        {
            isMoving = true;
            currentDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1);
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

        private void Move()
        {
            // Prevents endless impacts on side walls
            if (Mathf.Abs(currentDirection.y) < 0.1f)
            {
                currentDirection.y = currentDirection.y > 0 ? 0.1f : -0.1f;
            }

            // Prevents endless impacts on the ceiling and platform
            if (Mathf.Approximately(rigidbody2D.velocity.x, 0) && Mathf.Abs(currentDirection.x) > 0)
            {
                currentDirection.x *= -1;
            }

            rigidbody2D.velocity = currentDirection * moveSpeed;
        }
        #endregion

        #region Collision
        private enum LayersName
        {
            Wall = 8,
            Block = 9,
            Platform = 10,
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            audioSource.PlayOneShot(audioSource.clip);

            switch (collision.gameObject.layer)
            {
                case (int)LayersName.Wall:

                    currentDirection = Vector2.Reflect(currentDirection, collision.contacts[0].normal).normalized;

                    break;

                case (int)LayersName.Block:

                    collision.gameObject.GetComponent<BlockController>().GetDamage();

                    if (collision.gameObject.GetComponent<BlockController>().Health > 0)
                    {
                        currentDirection = (transform.position - collision.transform.position).normalized;
                    }

                    break;

                case (int)LayersName.Platform:

                    currentDirection = (transform.position - collision.transform.position).normalized;

                    break;
            }
        }

        public bool IsBallDestroyed { get; private set; } = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Death Trigger")
            {
                IsBallDestroyed = true;
            }
        }
        #endregion

        private float startPositionY = -5.65f;

        /// <summary>Returns all ball values ​​to starting point</summary>
        public void ResetBall()
        {
            rigidbody2D.velocity = Vector2.zero;
            transform.position = new Vector2(0, startPositionY);
            isMoving = false;
            IsBallDestroyed = false;
            gameObject.SetActive(false);
        }
    }
}