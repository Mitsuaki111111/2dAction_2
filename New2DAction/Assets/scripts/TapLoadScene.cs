using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Unity�G���W���̃V�[���Ǘ��v���O�����𗘗p����

public class TapLoadScene : MonoBehaviour //change�Ƃ������O�ɂ��܂�
{
    public string scenename = "GameTitle";
    public void change_button() //change_button�Ƃ������O�ɂ��܂�
    {
        SceneManager.LoadScene(scenename);//second���Ăяo���܂�
    }
}
