using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Animator anim;


    Vector2 movement;
    private void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        anim.SetFloat("horizontal", movement.x);
        anim.SetFloat("vertical", movement.y);
        anim.SetFloat("speed", movement.sqrMagnitude);

        if (movement != Vector2.zero) {
            UpdateDirection(movement);
            anim.SetFloat("lastHorizontal", movement.x);
            anim.SetFloat("lastVertical", movement.y);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
    void UpdateDirection(Vector2 movement)
    {
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            // Horizontal movement
            anim.SetInteger("Direction", movement.x > 0 ? 1 : 3); // Right = 1, Left = 3
        }
        else
        {
            // Vertical movement
            anim.SetInteger("Direction", movement.y > 0 ? 0 : 2); // Up = 0, Down = 2
        }
    }
}
