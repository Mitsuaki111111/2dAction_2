using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plyayer : MonoBehaviour
{
    [Header("移動速度")] public float speed;           //速度
    [Header("重力")] public float gravity;         //重力
    [Header("ジャンプ速度")] public float jumpSpeed;       //ジャンプ速度
    [Header("ジャンプ速度")] public float jumpHeight;      //ジャンプ速度
    [Header("ジャンプ速度")] public float jumpLimitTime;   //ジャンプ速度
    [Header("ジャンプ速度")] public Ground_Check ground;   //ジャンプ速度
    [Header("ジャンプ速度")] public Ground_Check Head;     //ジャンプ速度

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

            //上ボタンを押されている。かつ、現在の高さがジャンプした位置から自分の決めた位置より下ならジャンプを継続する
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
