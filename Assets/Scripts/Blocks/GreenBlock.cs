using UnityEngine;

namespace MiniIT.ArkanoidTest
{
    public class GreenBlock : BlockController
    {
        private void Start()
        {
            Health = 1;
        }

        protected override void Explode()
        {
            // Set explosion FX color
            ParticleSystem.MainModule settings = explodeFX.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(Color.green);

            base.Explode();
        }
    }
}