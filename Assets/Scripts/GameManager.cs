using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int health = 100;
    public int mana = 100;
    public int damage = 3;
    public float fireRate = 2;
    
    public static GameManager gameManager;

    void Awake() 
    {
        if(gameManager == null) {
            gameManager = this;
        }
        else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
