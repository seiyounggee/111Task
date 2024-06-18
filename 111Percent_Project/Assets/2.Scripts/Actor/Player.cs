using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Agent
{
    public float speed = 10f;
    [ReadOnly] public bool isGrounded = false;

    private Vector3 jumpUpVelocity = Vector3.zero;
    private Vector3 fallDownVelocity = Vector3.zero;

    public void Jump()
    {
        jumpUpVelocity = Vector3.up * 25f;
        fallDownVelocity = Vector3.zero;
    }

    public void Attack()
    { 
    
    }

    public void Defense()
    { 
    
    }

    private void FixedUpdate()
    {
        if (jumpUpVelocity.y > 0)
        {
            this.transform.position += Vector3.up * Time.deltaTime * jumpUpVelocity.y;

            jumpUpVelocity -= Vector3.up * Time.deltaTime * speed;
            if (jumpUpVelocity.y <= 0)
            { 
                jumpUpVelocity.y = 0;
            }
        }

        if (isGrounded == false && jumpUpVelocity.y <= 0)
        {
            this.transform.position += Vector3.up * Time.deltaTime * fallDownVelocity.y;
            fallDownVelocity -= Vector3.up * Time.deltaTime * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Floor))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Floor))
        {
            isGrounded = false;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Space))
        {
            Attack();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (isGrounded && Mathf.Approximately(jumpUpVelocity.y, 0f))
                Jump();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Defense();
        }
    }
#endif
}
