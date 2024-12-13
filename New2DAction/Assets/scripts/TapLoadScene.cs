using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Unityエンジンのシーン管理プログラムを利用する

public class TapLoadScene : MonoBehaviour //changeという名前にします
{
    public string scenename = "GameTitle";
    public void change_button() //change_buttonという名前にします
    {
        SceneManager.LoadScene(scenename);//secondを呼び出します
    }
}
