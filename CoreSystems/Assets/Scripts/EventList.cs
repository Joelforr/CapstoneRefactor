using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

namespace EventList
{

    public class AnimationCompleteEvent: GameEvent
    {
        public XAnimator sender;

        public AnimationCompleteEvent(XAnimator animator)
        {
            this.sender = animator;
        }
    }

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


    public class HitEvent : GameEvent
    {
        public Player player_hit;
        public CollisionBoxData properties;
        public float launch_dir;

        public HitEvent(Player player_hit, CollisionBoxData properties, float launch_direction)
        {
            this.player_hit = player_hit;
            this.properties = properties;
            this.launch_dir = launch_direction;
        }
    }

    public class WallInteractionEvent : GameEvent
    {
        public Player sender;
        public Player.CollidedSurface wall;

        public WallInteractionEvent(Player player, Player.CollidedSurface cs)
        {
            this.sender = player;
            this.wall = cs;
        }
    }
}

