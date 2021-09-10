using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public string scene;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(new Vector3(180f,90f,90f) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider) 
    {
        if(collider.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene(scene);
            Destroy(gameObject);
        }
    }
}
