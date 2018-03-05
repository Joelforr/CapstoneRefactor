using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

namespace EventList
{
    public class CollisionEvent : GameEvent
    {
        public Player sender;
        public Collision2D collision2D;

        public CollisionEvent(Player player, Collision2D collision2D)
        {
            this.sender = player;
            this.collision2D = collision2D;
        }
    }

    public class WallInteractionEvent : GameEvent
    {
        public Player sender;
        public Player.CollidedSurface wall;

        public WallInteractionEvent()
        {
            
        }
    }
}

