using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    public Slider manaBar;
    
    void Awake()
    {
        UpdateBars();
    }

    public void UpdateHealthUI(int health)
    {
        // Updating the player's life bar.
        healthBar.value = health;
    }

    public void UpdateManaUI(float mana)
    {
        // Updating the player's mana bar.
        manaBar.value = mana;
    }

    public void UpdateBars()
    {
        healthBar.maxValue = GameManager.gameManager.health;
        manaBar.maxValue = GameManager.gameManager.mana;
    }
}
