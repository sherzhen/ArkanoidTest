using UnityEngine;

namespace MiniIT.ArkanoidTest
{
    public class RedBlock : BlockController
    {
        private void Start()
        {
            Health = 2;
        }

        protected override void Explode()
        {
            // Set explosion FX color
            ParticleSystem.MainModule settings = explodeFX.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(Color.red);

            base.Explode();
        }
    }
}