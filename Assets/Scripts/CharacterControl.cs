using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class CharacterControl : MonoBehaviour
    {
        public float speed;

        public float speedJoystick;

        private Animator animator;
        float horizontalMove = 0;
        float verticallMove = 0;
        private Rigidbody2D rigidBody;
        public Joystick joystick;
      



        private void Start()
        {
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody2D>();
        }


        private void Update()
        {
            Vector2 dir = Vector2.zero;

            
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                dir.x = -1;
                animator.SetInteger("Direction", 3);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                dir.x = 1;
                animator.SetInteger("Direction", 2);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                dir.y = 1;
                animator.SetInteger("Direction", 1);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                dir.y = -1;
                animator.SetInteger("Direction", 0);
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            if (dir!=Vector2.zero) {
                GetComponent<Rigidbody2D>().velocity = speed * dir;
            }
            else {
                horizontalMove = joystick.Horizontal * speed;
                verticallMove = joystick.Vertical * speed;

                if (horizontalMove<0)
                {
                    
                    animator.SetInteger("Direction", 3);
                }
                else if (horizontalMove > 0)
                {
                    
                    animator.SetInteger("Direction", 2);
                }

                if (verticallMove > 0 && Mathf.Abs(verticallMove)> Mathf.Abs(horizontalMove))
                {
                    
                    animator.SetInteger("Direction", 1);
                }
                else if (verticallMove < 0 && Mathf.Abs(verticallMove) > Mathf.Abs(horizontalMove))
                {
                    
                    animator.SetInteger("Direction", 0);
                }

                GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalMove, verticallMove).normalized * speedJoystick;
            }

            




        }

       


        
    }
}
