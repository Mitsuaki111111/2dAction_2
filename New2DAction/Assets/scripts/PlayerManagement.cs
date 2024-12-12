using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    [Header("ステージのレイヤー")] public LayerMask StageLayer;
    [Header("アニメーター")] public Animator animator;
    [Header("アイドル状態スプライト")] public SpriteRenderer idlesprite;
    [Header("走りスプライト")] public SpriteRenderer runsprite;
    [Header("ジャンプスプライト")] public SpriteRenderer jumpsprite;

    private bool jumpingstandby;
    private string enemyTag = "Enemy";

    // 動作状態を定義する 
    private enum MOVE_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
        JUMP,
    }
    MOVE_TYPE move = MOVE_TYPE.STOP; // 初期状態はSTOP 
    Rigidbody2D rbody2D;             // Rigidbody2Dを定義
    float speed;                     // 移動速度を格納する変数

    private void Start()
    {
        // 走り状態の画像を消す
        runsprite.enabled = false;
        jumpsprite.enabled = false;
        jumpingstandby = false;
        // Rigidbody2Dのコンポーネントを取得
        rbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizonkey = Input.GetAxis("Horizontal");      // 水平方向のキー取得
        //Debug.Log(horizonkey);
        // 取得した水平方向のキーを元に分岐
        //if (horizonkey == 0)
        //{
        //    // キー入力なしの場合は停止
        //    move = MOVE_TYPE.STOP;
        //}
        if (horizonkey >= 0.1)
        {
            // キー入力が正の場合は右へ移動する
            move = MOVE_TYPE.RIGHT;
            animator.SetBool("run", true);
            //// 立ち状態の画像を消す
            //idlesprite.enabled = false;
            //// 走り状態の画像を出す
            //runsprite.enabled = true;
        }
        else if (horizonkey <= -0.1)
        {
            // キー入力が負の場合は左へ移動する
            move = MOVE_TYPE.LEFT;
            animator.SetBool("run", true);
            //// 立ち状態の画像を消す
            //idlesprite.enabled = false;
            //// 走り状態の画像を出す
            //runsprite.enabled = true;
        }
        else
        {
            move = MOVE_TYPE.STOP;
            animator.SetBool("run", false);
            //// 立ち状態の画像を出す
            //idlesprite.enabled = true;
            //// 走り状態の画像を消す
            //runsprite.enabled = false;
        }

        // ジャンプ中に地上落下かつjumpingstandbyのフラグが折れているならidleに戻す
        if (GroundChk() && animator.GetCurrentAnimatorStateInfo(0).IsName("jump") && !jumpingstandby)
        {
            animator.SetBool("jump", false);
        }

        // spaceを押したらジャンプ関数へ
        if (GroundChk() && !jumpingstandby)
        {
            if (Input.GetKeyDown("space"))
            {
                //UnityEditor.EditorApplication.isPaused = true;
                animator.SetBool("jump", true);
                jumpingstandby = true;
                Jump();
            }    
        }
        // jumpingstandyがtrueかつ空中にいるならjumpingstandbyのフラグを折る
        else if(!GroundChk() && jumpingstandby)
        {
            jumpingstandby = false;
        }
        
    }

    private void LateUpdate()
    {
        // Playerの方向を決めるためにスケールの取り出し
        Vector3 scale = transform.localScale;
        if (move == MOVE_TYPE.STOP)
        {
            speed = 0;
        }
        else if (move == MOVE_TYPE.RIGHT)
        {
            scale.x = 1; // 右向き
            speed = 1.0f;
        }
        else if (move == MOVE_TYPE.LEFT)
        {
            scale.x = -1; // 左向き
            speed = -1.0f;
        }
        transform.localScale = scale; // scaleを代入
                                      // rigidbody2Dのvelocity(速度)へ取得したspeedを入れる。y方向は動かないのでそのままにする
        rbody2D.velocity = new Vector2(speed, rbody2D.velocity.y);

        
    }

    // 物理演算(rigidbody)はFixedUpdateで処理する
    private void FixedUpdate()
    {
        
    }

    void Jump()
    {
        // 上に力を加える
        rbody2D.AddForce(Vector2.up * 200);
        // ジャンプボタンを押しても浮くまでにラグがあるので少し浮かす
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    bool GroundChk()
    {
        Vector3 startposition = transform.position;                     // Playerの中心を始点とする
        Vector3 endposition = transform.position - transform.up * 0.3f; // Playerの足元を終点とする

        // Debug用に始点と終点を表示する
        //Debug.DrawLine(startposition, endposition, Color.red);

        // Physics2D.Linecastを使い、ベクトルとStageLayerが接触していたらTrueを返す
        return Physics2D.Linecast(startposition, endposition, StageLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == enemyTag)
        {
            Debug.Log("敵と接触");
        }
    }
}