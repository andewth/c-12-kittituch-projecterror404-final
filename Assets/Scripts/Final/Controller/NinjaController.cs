using System.Collections;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    public Rigidbody2D Rb2d;
    public Animator Animator;

    Vector2 move;
    float moveX;
    float speed;
    float jumpSpeed;

    bool isGrounded;


    // จุดเริ่มต้นเมื่อเริ่มเกม
    void Start()
    {
        speed = 350f;
        jumpSpeed = 14f;
        Animator = GetComponent<Animator>();
    }


    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        // การเคลื่อนที่ในแนวนอน
        Rb2d.velocity = new Vector2(moveX * speed * Time.fixedDeltaTime, Rb2d.velocity.y);

        // อัพเดทสถานะการวิ่ง
        Animator.SetFloat("Speed", Mathf.Abs(moveX * speed));
        if (moveX != 0)
        {
            transform.localScale = new Vector3(moveX > 0 ? 0.5f : -0.5f, 0.5f, 1);
        }

        // เช็คการกระโดด
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Rb2d.velocity = new Vector2(Rb2d.velocity.x, jumpSpeed);
        }
    }

    // เช็คการสัมผัสกับพื้น
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Animator.SetBool("IsJumping", false);
            isGrounded = true;
        }
    }

    // ถ้าห่างจากพื้น
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Animator.SetBool("IsJumping", true);
            isGrounded = false;
        }
    }
}
