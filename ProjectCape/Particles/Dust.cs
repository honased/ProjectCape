using HonasGame.Helper;
using HonasGame.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Particles
{
    public class Dust : ParticleSystem
    {
        public Dust(int maxDust) : base(maxDust * 25) { }

        protected override void InitializeConstants()
        {
            textureFilename = "circle";

            minNumParticles = 20;
            maxNumParticles = 25;

            blendState = BlendState.AlphaBlend;
            DrawOrder = AlphaBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDir() * RandomHelper.NextFloat(5, 20);

            var lifetime = RandomHelper.NextFloat(0.5f, 1.0f);

            var acceleration = -velocity / lifetime;

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            p.Initialize(where, velocity, acceleration, Color.Brown, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: .1f);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            particle.Scale = .1f + .2f * normalizedLifetime;
        }

        public void PlaceFirework(Vector2 where)
        {
            AddParticles(where);
        }
    }
}
