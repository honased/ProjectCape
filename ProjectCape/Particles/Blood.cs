using HonasGame.Helper;
using HonasGame.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Particles
{
    public class Blood : ParticleSystem
    {
        public Blood() : base(50) { }

        private const int STARTING_ALPHA = 255;
        private Color _color;

        protected override void InitializeConstants()
        {
            textureFilename = "square";

            minNumParticles = 10;
            maxNumParticles = 20;

            blendState = BlendState.AlphaBlend;
            DrawOrder = AlphaBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDir(0, MathHelper.Pi) * new Vector2(RandomHelper.NextFloat(-15, -50), RandomHelper.NextFloat(-100, -200));

            var lifetime = RandomHelper.NextFloat(0.5f, 1.0f);

            var acceleration = new Vector2(0.0f, 500.0f);

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            p.Initialize(where, velocity, acceleration, _color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: .1f);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            particle.Scale = .1f + .2f * normalizedLifetime;
            particle.Color = Color.FromNonPremultiplied(particle.OriginalColor.R, particle.OriginalColor.G, particle.OriginalColor.B, STARTING_ALPHA - (int)(normalizedLifetime * STARTING_ALPHA));
        }

        public void PlaceBlood(Vector2 where, Color color)
        {
            _color = color;
            AddParticles(where);
        }
    }
}
