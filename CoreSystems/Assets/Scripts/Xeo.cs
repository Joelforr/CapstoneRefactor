using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xeo
{
    public class Collisions
    {
        public static bool IsGroundedOverlap(Transform _transform, BoxCollider2D _physicsCollider, LayerMask _collisionMask)
        {
            //Bottom Left
            Vector2 bl = _transform.TransformPoint(_physicsCollider.offset + new Vector2(-_physicsCollider.size.x / 2, -_physicsCollider.size.y / 2));
            //Bottom Right
            Vector2 br = _transform.TransformPoint(_physicsCollider.offset + new Vector2(_physicsCollider.size.x / 2, -_physicsCollider.size.y / 2));

            //Ground Check
            return Physics2D.OverlapArea(bl, br, _collisionMask) != null;

        }

        public static bool IsGrounded(BoxCollider2D _physicsCollider, LayerMask _collisionMask, float checkDistance)
        {
            RaycastHit2D closestHit = Raycasting.GetClosestHit(_physicsCollider.bounds.center, Vector3.down, _physicsCollider, checkDistance, _collisionMask);
            return closestHit.collider != null;
        }

        public static bool IsAgainstRightWall(BoxCollider2D _physicsCollider, LayerMask _collisionMask, float checkDistance)
        {
            RaycastHit2D  closestHit = Raycasting.GetClosestHit(_physicsCollider.bounds.center, Vector3.right, _physicsCollider, checkDistance, _collisionMask);
            return closestHit.collider != null;
        }

        public static bool IsAgainstLeftWall( BoxCollider2D _physicsCollider, LayerMask _collisionMask, float checkDistance)
        {
            RaycastHit2D closestHit = Raycasting.GetClosestHit(_physicsCollider.bounds.center, Vector3.left, _physicsCollider, checkDistance, _collisionMask);
            return closestHit.collider != null;
        }
    }
   
    public class EventManager
    {
        public delegate void EventDelegate<T>(T e) where T : GameEvent;
        private delegate void EventDelegate(GameEvent e);

        private readonly Dictionary<Type, EventDelegate> _delegates = new Dictionary<Type, EventDelegate>();
        private readonly Dictionary<Delegate, EventDelegate> _delegateLookup = new Dictionary<Delegate, EventDelegate>();
        private readonly List<GameEvent> _queuedEvents = new List<GameEvent>();
        private readonly object _queueLock = new object();

        private static readonly EventManager _instance = new EventManager();
        public static EventManager GlobalInstance
        {
            get
            {
                return _instance;
            }
        }

        public EventManager() { }

        public void AddHandler<T>(EventDelegate<T> del) where T : GameEvent
        {
            if (_delegateLookup.ContainsKey(del))
            {
                return;
            }

            EventDelegate internalDelegate = (e) => del((T)e);
            _delegateLookup[del] = internalDelegate;

            EventDelegate tempDel;
            if (_delegates.TryGetValue(typeof(T), out tempDel))
            {
                _delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                _delegates[typeof(T)] = internalDelegate;
            }
        }

        public void RemoveHandler<T>(EventDelegate<T> del) where T : GameEvent
        {
            EventDelegate internalDelegate;
            if (_delegateLookup.TryGetValue(del, out internalDelegate))
            {
                EventDelegate tempDel;
                if (_delegates.TryGetValue(typeof(T), out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        _delegates.Remove(typeof(T));
                    }
                    else
                    {
                        _delegates[typeof(T)] = tempDel;
                    }
                }
                _delegateLookup.Remove(del);
            }
        }

        public void Clear()
        {
            lock (_queueLock)
            {
                if (_delegates != null) _delegates.Clear();
                if (_delegateLookup != null) _delegateLookup.Clear();
                if (_queuedEvents != null) _queuedEvents.Clear();
            }
        }

        public void Fire(GameEvent e)
        {
            EventDelegate del;
            if (_delegates.TryGetValue(e.GetType(), out del))
            {
                del.Invoke(e);
            }
        }

        public void ProcessQueuedEvents()
        {
            List<GameEvent> events;
            lock (_queueLock)
            {
                if (_queuedEvents.Count > 0)
                {
                    events = new List<GameEvent>(_queuedEvents);
                    _queuedEvents.Clear();
                }
                else
                {
                    return;
                }
            }

            foreach (var e in events)
            {
                Fire(e);
            }
        }

        public void Queue(GameEvent e)
        {
            lock (_queueLock)
            {
                _queuedEvents.Add(e);
            }
        }

    }


    /* public class Input
     {

         public const string HORIZONTAL = "Horizontal";
         public const string VERTICAL = "Vertical";
         public const string JUMP = "Jump";
         public const string DASH = "Fire1";
         public const string SHIFT = "Shift";
     }
     */

    public class GameEvent { }

    public class Globals
    {
        // Input threshold in order to take effect. Arbitarily set.
        public const float INPUT_THRESHOLD = 0.5f;
        public const float FAST_FALL_THRESHOLD = 0.5f;

        public const int ENV_MASK = 0x100;

        public const string PACKAGE_NAME = "Xeo";

        public const float MINIMUM_DISTANCE_CHECK = 0.01f;

        public static int GetFrameCount(float time)
        {
            float frames = time / Time.fixedDeltaTime;
            int roundedFrames = Mathf.RoundToInt(frames);

            if (Mathf.Approximately(frames, roundedFrames))
            {
                return roundedFrames;
            }

            return Mathf.CeilToInt(frames);

        }
    }

    public class PhysX
    {
        public static float CalculateGravity(float jump_height, float lateral_distance, float x_velocity)
        {
            float gravity;

            gravity = -2 * jump_height * (x_velocity * x_velocity) / ((lateral_distance / 2) * (lateral_distance / 2));

            return gravity;

        }

        public static float CalculateJumpVelocity(float jump_height, float lateral_distance, float x_velocity)
        {
            float y_velocity;

            y_velocity = (2 * jump_height * x_velocity) / (lateral_distance / 2);

            return y_velocity;
        }
    }

    public class Raycasting
    {
        public static RaycastHit2D GetClosestHit(Vector2 origin, Vector3 direction, BoxCollider2D _physicsCollider, float distance, LayerMask _collisionMask, bool useBox = true)
        {
            if (useBox)
            {
                return Physics2D.BoxCast(
                    origin,
                    _physicsCollider.bounds.size,
                    0f,
                    direction,
                    distance,
                    _collisionMask);
            }

            return Physics2D.Raycast(origin, direction, distance, _collisionMask);
        }
    }

    public class Utility
    {
        public static GameObject CreateChildObj(string name, Transform parent)
        {
            GameObject obj = new GameObject(name);
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }

        public static Texture2D textureFromSprite(Sprite sprite)
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
    }
}

