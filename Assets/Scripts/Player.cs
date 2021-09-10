using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{   
    [Header("Health")]
    private int health;
    private int maxHealth;

    [Header("Mana")]
    [HideInInspector]
    public float mana;
    private float maxMana;

    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public bool tookDamage = false;

    public GameObject gameOver;

    [Header("Fall System")]
    private float lastPositionY;
    private float fallDistance;

    [Range(1,15)]
    public float fallHeight = 4;
    [Range(1,15)]
    public float damageMeter = 5;

    private GameObject currentPlayer;
    private CharacterController controller;

    public static Player player;

    GameManager gameManager;

    void Start()
    {
        player = this;
        gameManager = GameManager.gameManager;

        SetPlayerStatus();
        health = maxHealth;
        mana = maxMana;
        UpdateHealthUI();
        UpdateManaUI();

        controller = GetComponent<CharacterController>();
        currentPlayer = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        FallSystem();
        ManaSystem();
    }

    public void SetPlayerStatus()
    {
        maxHealth = gameManager.health;
        maxMana = gameManager.mana;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator TookDamage(int damage)
    {
        tookDamage = true;
        health -= damage;
        UpdateHealthUI();
        if(health <= 0) {
            isDead = true;
            GameOver();
        }
        else {
            // Ignores collision with the enemy layer for 2 seconds.
            Physics.IgnoreLayerCollision(9, 10);
            yield return new WaitForSeconds(0.2f);

            // Collide again.
            Physics.IgnoreLayerCollision(9, 10, false);
            tookDamage = false;
        }
    }

    void UpdateHealthUI()
    {
        FindObjectOfType<UIManager>().UpdateHealthUI(health);
    }

    public void UpdateManaUI()
    {
        FindObjectOfType<UIManager>().UpdateManaUI(mana);
    }

    void FallSystem()
    {
        if(lastPositionY > currentPlayer.transform.position.y && controller.velocity.y < 0) {
            fallDistance += lastPositionY - currentPlayer.transform.position.y;
        }

        lastPositionY = currentPlayer.transform.position.y;

        if(fallDistance >= fallHeight && controller.isGrounded) {
            // Every 1 meter he loses 5 life.
            health = health - (int)(damageMeter * fallDistance);
            UpdateHealthUI();
            if(health <= 0) {
                isDead = true;
                GameOver();
            }
            fallDistance = 0;
            lastPositionY = 0;
        }

        if(fallDistance < fallHeight && controller.isGrounded) {
            fallDistance = 0;
            lastPositionY = 0;
        }
    }

    void ManaSystem()
    {
        // Updates mana over time.
        if(mana > maxMana) {
            mana = maxMana;
        }else {
            mana += Time.deltaTime * 0.5f;
            UpdateManaUI();
        }
        if(mana < 0) {
            mana = 0;
        }
    }

    void GameOver()
    {
        if(isDead) {
            gameOver.SetActive(true);
            Invoke("ReloadScene", 10f);
        }
    }

    void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.CompareTag("HealthPotion")) {
            Destroy(collision.gameObject);
            if(health < maxHealth) {
                health += 10;
                UpdateHealthUI();
            }
        }else if(collision.gameObject.CompareTag("ManaPotion")) {
            Destroy(collision.gameObject);
            if(mana < maxMana) {
                mana += 10;
                UpdateManaUI();
            }
        }
    }
}
