using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    [Header("�X�e�[�W�̃��C���[")] public LayerMask StageLayer;
    [Header("�A�j���[�^�[")] public Animator animator;
    [Header("�A�C�h����ԃX�v���C�g")] public SpriteRenderer idlesprite;
    [Header("����X�v���C�g")] public SpriteRenderer runsprite;
    [Header("�W�����v�X�v���C�g")] public SpriteRenderer jumpsprite;

    private bool jumpingstandby;
    private string enemyTag = "Enemy";

    // �����Ԃ��`���� 
    private enum MOVE_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
        JUMP,
    }
    MOVE_TYPE move = MOVE_TYPE.STOP; // ������Ԃ�STOP 
    Rigidbody2D rbody2D;             // Rigidbody2D���`
    float speed;                     // �ړ����x���i�[����ϐ�

    private void Start()
    {
        // �����Ԃ̉摜������
        runsprite.enabled = false;
        jumpsprite.enabled = false;
        jumpingstandby = false;
        // Rigidbody2D�̃R���|�[�l���g���擾
        rbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizonkey = Input.GetAxis("Horizontal");      // ���������̃L�[�擾
        //Debug.Log(horizonkey);
        // �擾�������������̃L�[�����ɕ���
        //if (horizonkey == 0)
        //{
        //    // �L�[���͂Ȃ��̏ꍇ�͒�~
        //    move = MOVE_TYPE.STOP;
        //}
        if (horizonkey >= 0.1)
        {
            // �L�[���͂����̏ꍇ�͉E�ֈړ�����
            move = MOVE_TYPE.RIGHT;
            animator.SetBool("run", true);
            //// ������Ԃ̉摜������
            //idlesprite.enabled = false;
            //// �����Ԃ̉摜���o��
            //runsprite.enabled = true;
        }
        else if (horizonkey <= -0.1)
        {
            // �L�[���͂����̏ꍇ�͍��ֈړ�����
            move = MOVE_TYPE.LEFT;
            animator.SetBool("run", true);
            //// ������Ԃ̉摜������
            //idlesprite.enabled = false;
            //// �����Ԃ̉摜���o��
            //runsprite.enabled = true;
        }
        else
        {
            move = MOVE_TYPE.STOP;
            animator.SetBool("run", false);
            //// ������Ԃ̉摜���o��
            //idlesprite.enabled = true;
            //// �����Ԃ̉摜������
            //runsprite.enabled = false;
        }

        // �W�����v���ɒn�㗎������jumpingstandby�̃t���O���܂�Ă���Ȃ�idle�ɖ߂�
        if (GroundChk() && animator.GetCurrentAnimatorStateInfo(0).IsName("jump") && !jumpingstandby)
        {
            animator.SetBool("jump", false);
        }

        // space����������W�����v�֐���
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
        // jumpingstandy��true���󒆂ɂ���Ȃ�jumpingstandby�̃t���O��܂�
        else if(!GroundChk() && jumpingstandby)
        {
            jumpingstandby = false;
        }
        
    }

    private void LateUpdate()
    {
        // Player�̕��������߂邽�߂ɃX�P�[���̎��o��
        Vector3 scale = transform.localScale;
        if (move == MOVE_TYPE.STOP)
        {
            speed = 0;
        }
        else if (move == MOVE_TYPE.RIGHT)
        {
            scale.x = 1; // �E����
            speed = 1.0f;
        }
        else if (move == MOVE_TYPE.LEFT)
        {
            scale.x = -1; // ������
            speed = -1.0f;
        }
        transform.localScale = scale; // scale����
                                      // rigidbody2D��velocity(���x)�֎擾����speed������By�����͓����Ȃ��̂ł��̂܂܂ɂ���
        rbody2D.velocity = new Vector2(speed, rbody2D.velocity.y);

        
    }

    // �������Z(rigidbody)��FixedUpdate�ŏ�������
    private void FixedUpdate()
    {
        
    }

    void Jump()
    {
        // ��ɗ͂�������
        rbody2D.AddForce(Vector2.up * 200);
        // �W�����v�{�^���������Ă������܂łɃ��O������̂ŏ���������
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    bool GroundChk()
    {
        Vector3 startposition = transform.position;                     // Player�̒��S���n�_�Ƃ���
        Vector3 endposition = transform.position - transform.up * 0.3f; // Player�̑������I�_�Ƃ���

        // Debug�p�Ɏn�_�ƏI�_��\������
        //Debug.DrawLine(startposition, endposition, Color.red);

        // Physics2D.Linecast���g���A�x�N�g����StageLayer���ڐG���Ă�����True��Ԃ�
        return Physics2D.Linecast(startposition, endposition, StageLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == enemyTag)
        {
            Debug.Log("�G�ƐڐG");
        }
    }
}