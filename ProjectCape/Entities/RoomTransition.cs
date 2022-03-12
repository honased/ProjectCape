using HonasGame.ECS;
using HonasGame.ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities
{
    public class RoomTransition : Entity
    {
        private Transform2D _transform;

        public RoomTransition()
        {
            _transform = new Transform2D(this);
            Persistent = true;
        }

        protected override void Cleanup()
        {

        }
    }
}
