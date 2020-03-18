using UnityEngine;
using UnityEngine.Events;

namespace MiniIT.ArkanoidTest
{
    public class BlockController : MonoBehaviour
    {
        [SerializeField]
        protected ParticleSystem explodeFX = null;
        [SerializeField]
        private BoxCollider2D boxCollider2D = null;
        [SerializeField]
        private SpriteRenderer spriteRenderer = null;
        [SerializeField]
        private AudioSource audioSource = null;

        public int Health { get; protected set; } = 0;

        /// <summary>Called when this block is destroyed</summary>
        public UnityAction OnExplodeAction { get; set; } = null;

        public void GetDamage()
        {
            audioSource.Play();

            Health--;

            if (Health <= 0)
                Explode();
        }

        protected virtual void Explode()
        {
            explodeFX.Play();
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            OnExplodeAction.Invoke();
        }
    }
}