using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour {

    public Rigidbody2D rb;

    public float horizontal_moveSpeed;
    public float horizontal_acceleration;
    public float jump_height_max;
    public float jump_height_min;
    public float initial_distance_to_peak;
    public float final_distance_to_peak;

    public float gravity;
    public bool grounded; 

    public Vector2 _velocity;
    public BoxCollider2D _collider;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        SetGrounded();

        gravity = CalculateGravity(jump_height_max, initial_distance_to_peak, horizontal_moveSpeed);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            grounded = false;

            _velocity.y = CalculateJumpVelocity(jump_height_max, initial_distance_to_peak, horizontal_moveSpeed);
        }

        if(Input.GetKeyUp (KeyCode.UpArrow) && _velocity.y > 0)
        {
            _velocity.y = CalculateJumpVelocity(jump_height_min, initial_distance_to_peak, horizontal_moveSpeed);
        }
        _velocity.x = Input.GetAxis("Horizontal") * horizontal_moveSpeed;
        _velocity.x = Mathf.Max(Mathf.Min(_velocity.x, horizontal_moveSpeed), -horizontal_moveSpeed);

        
    }

    private void FixedUpdate()
    {
   
        if (!grounded)
        {
            _velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            _velocity.y = 0;
        }

        Debug.Log(_velocity.x * Time.deltaTime + horizontal_acceleration / 2 * Time.deltaTime * Time.deltaTime);
        rb.MovePosition((Vector2)transform.position + 
            new Vector2(_velocity.x * Time.deltaTime + horizontal_acceleration/2 *Time.deltaTime * Time.deltaTime,
                         _velocity.y * Time.deltaTime + gravity/2 * Time.deltaTime * Time.deltaTime));

        Debug.Log(_velocity.x * Time.deltaTime + horizontal_moveSpeed / 2 * Time.deltaTime);
        Debug.Log(CalculateGravity(jump_height_max, initial_distance_to_peak, horizontal_moveSpeed) * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        //bottomright
        Gizmos.DrawSphere(transform.TransformPoint(_collider.offset + new Vector2(_collider.size.x / 2, -_collider.size.y / 2)), .1f);
        //topright
        Gizmos.DrawSphere(transform.TransformPoint(_collider.offset + new Vector2(_collider.size.x / 2, _collider.size.y / 2)), .1f);
        //bottomleft
        Gizmos.DrawSphere(transform.TransformPoint(_collider.offset + new Vector2(-_collider.size.x / 2, -_collider.size.y / 2)), .1f);
        //topleft
        Gizmos.DrawSphere(transform.TransformPoint(_collider.offset + new Vector2(-_collider.size.x / 2, _collider.size.y / 2)), .1f);

        //Gizmos.DrawSphere(transform.TransformPoint(_collider.offset - (_collider.size / 2) + new Vector2(0, 0)), .1f);
        //Gizmos.DrawSphere(transform.TransformPoint(_collider.offset - (_collider.size / 2) + new Vector2(0, 0)), .1f);
    }

    void SetGrounded()
    {
        Vector2 pt1 = transform.TransformPoint(_collider.offset + new Vector2(_collider.size.x / 2, -_collider.size.y / 2));//(box.size / 2));
        Vector2 pt2 = transform.TransformPoint(_collider.offset - (_collider.size / 2) + new Vector2(0,0));

        grounded = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Platform")) != null;
    }

    public float CalculateGravity(float jump_height, float lateral_distance, float x_velocity)
    {
        float gravity;

        gravity = -2 * jump_height * (x_velocity * x_velocity) / ((lateral_distance / 2) * (lateral_distance / 2));

        return gravity;

    }

    public float CalculateJumpVelocity(float jump_height, float lateral_distance, float x_velocity)
    {
        float y_velocity;
   
        y_velocity = (2 * jump_height * x_velocity) / (lateral_distance / 2);

        return y_velocity;
    }
}
