using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName = " ";
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);//移動先のシーンの名前を必ずafterにしてください！
        }
    }

}