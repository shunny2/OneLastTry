using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string scene;

    private void OnTriggerEnter(Collider collider) 
    {
        if(collider.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene(scene);
        }
    }
}
