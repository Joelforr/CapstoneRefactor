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
        public BaseCharacter sender;
        public Collision2D collision2D;

        public CollisionEvent(BaseCharacter player, Collision2D collision2D)
        {
            this.sender = player;
            this.collision2D = collision2D;
        }
    }

    public class GameModeChangeEvent : GameEvent
    {
        public GameManager.Mode newMode;

        public GameModeChangeEvent(GameManager.Mode newMode)
        {
            this.newMode = newMode;
        }
    }

    public class HitEvent : GameEvent
    {
        public BaseCharacter player_hit;
        public CollisionBoxData properties;
        public float launch_dir;

        public HitEvent(BaseCharacter player_hit, CollisionBoxData properties, float launch_direction)
        {
            this.player_hit = player_hit;
            this.properties = properties;
            this.launch_dir = launch_direction;
        }
    }

    public class WallInteractionEvent : GameEvent
    {
        public BaseCharacter sender;
        public BaseCharacter.CollidedSurface wall;

        public WallInteractionEvent(BaseCharacter player, BaseCharacter.CollidedSurface cs)
        {
            this.sender = player;
            this.wall = cs;
        }
    }
}

