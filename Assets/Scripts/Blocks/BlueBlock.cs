using UnityEngine;

namespace MiniIT.ArkanoidTest
{
    public class BlueBlock : BlockController
    {
        private void Start()
        {
            Health = 2;
        }

        protected override void Explode()
        {
            // Set explosion FX color
            ParticleSystem.MainModule settings = explodeFX.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(Color.blue);

            base.Explode();
        }
    }
}
