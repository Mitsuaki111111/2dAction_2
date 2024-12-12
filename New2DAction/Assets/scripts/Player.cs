using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plyayer : MonoBehaviour
{
    [Header("�ړ����x")] public float speed;           //���x
    [Header("�d��")] public float gravity;         //�d��
    [Header("�W�����v���x")] public float jumpSpeed;       //�W�����v���x
    [Header("�W�����v���x")] public float jumpHeight;      //�W�����v���x
    [Header("�W�����v���x")] public float jumpLimitTime;   //�W�����v���x
    [Header("�W�����v���x")] public Ground_Check ground;   //�W�����v���x
    [Header("�W�����v���x")] public Ground_Check Head;     //�W�����v���x

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isHead = false;
    private bool isJump = false;
    private bool isDown = false;
    private float jumpPos = 0.0f;
    private float jumpTime = 0.0f;
    private string enemyTag = "Enemy";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGround = ground.IsGround();
        isHead = Head.IsGround();

        SetAnimation();

        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;
        float ySpeed = -gravity;
        float verticalKey = Input.GetAxis("Vertical");

        if (isGround)
        {
            if(verticalKey > 0)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y;
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        else if (isJump) 
        {
            bool pushUpKey = verticalKey > 0;
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            bool canTime = jumpLimitTime > jumpTime;

            //��{�^����������Ă���B���A���݂̍������W�����v�����ʒu���玩���̌��߂��ʒu��艺�Ȃ�W�����v���p������
            if (pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }
        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = speed;
        }
        else if (horizontalKey < 0) 
        {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = -speed;
        }
        else
        {
            anim.SetBool("run", false);
            xSpeed = 0.0f;
        }


        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);
        rb.velocity = new Vector2(xSpeed, ySpeed);

    }

    private void SetAnimation()
    {
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == enemyTag)
        {
            anim.Play("player_down");
            isDown = true;
        }

    }
}
