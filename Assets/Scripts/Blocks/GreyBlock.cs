using UnityEngine;

namespace MiniIT.ArkanoidTest
{
    public class GreyBlock : BlockController
    {
        private void Start()
        {
            Health = 3;
        }

        protected override void Explode()
        {
            // Set explosion FX color
            ParticleSystem.MainModule settings = explodeFX.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(Color.grey);

            base.Explode();
        }
    }
}