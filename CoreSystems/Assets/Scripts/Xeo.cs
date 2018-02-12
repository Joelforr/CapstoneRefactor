
using UnityEngine;

namespace Xeo
{
    public class Collisions
    {
        public static bool IsGrounded(Transform _transform, BoxCollider2D _physicsCollider, LayerMask _collisionMask)
        {
            //Bottom Left
            Vector2 bl = _transform.TransformPoint(_physicsCollider.offset + new Vector2(-_physicsCollider.size.x / 2, -_physicsCollider.size.y / 2));
            //Bottom Right
            Vector2 br = _transform.TransformPoint(_physicsCollider.offset + new Vector2(_physicsCollider.size.x / 2, -_physicsCollider.size.y / 2));

            //Ground Check
            return Physics2D.OverlapArea(bl, br, _collisionMask) != null;

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

    public class Utility
    {
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

