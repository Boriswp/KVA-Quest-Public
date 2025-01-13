using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorsController : MonoBehaviour
{

    private float currHealth = 100;

    public float maxHealth = 100;

    private float currMana = 100;

    public float maxMana = 100;

    public int needMinusStartHealthAndMana = 0;

    public bool isInvulnerable = false;

    public Image healthSlider;

    public TMP_Text healthText;

    public Image manaSlider;

    public TMP_Text manaText;

    public delegate void OnStaminaChanged(int changeCount);
    public static OnStaminaChanged onStaminaChanged;

    public delegate void OnStaminaFull(bool isfull);
    public static OnStaminaFull onStaminaFull;

    public delegate void OnHealthChanged(int changeCount);
    public static OnHealthChanged onHealthChanged;


    void Awake()
    {
        onHealthChanged += healthChange;
        currHealth = maxHealth - needMinusStartHealthAndMana;
        healthSlider.fillAmount = currHealth / maxHealth;
        healthText.text = currHealth.ToString();
        onStaminaChanged += staminaChange;
        currMana = maxMana - needMinusStartHealthAndMana;
        manaSlider.fillAmount = currMana / maxMana;
        manaText.text = currMana.ToString();
    }

    private void OnDisable()
    {
        onHealthChanged -= healthChange;
        onStaminaChanged -= staminaChange;
    }

    public void UpdateHealthMaxValue(float newValue)
    {
        maxHealth = newValue;
        currHealth = newValue;
        healthSlider.fillAmount = currHealth / maxHealth;
        healthText.text = currHealth.ToString();
    }

    public void UpdateManaMaxValue(float newValue)
    {
        maxMana = newValue;
        currMana = newValue;
        manaSlider.fillAmount = currMana / maxMana;
        manaText.text = currMana.ToString();
    }

    public int getStamina()
    {
        return (int)currMana;
    }

    public void healthChange(int changeCount)
    {

        currHealth += changeCount;

        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

        if (currHealth <= 0)
        {
            currHealth = 0;
            GetComponent<PlayerController>().StartGameEnd();
        }
        healthText.text = currHealth.ToString();
        healthSlider.fillAmount = currHealth / maxHealth;
    }


    public void staminaChange(int changeCount)
    {
        currMana += changeCount;

        if (currMana > maxMana)
        {
            currMana = maxMana;
            onStaminaFull?.Invoke(true);
        }
        else
        {
            onStaminaFull?.Invoke(false);
        }

        if (currMana <= 0)
        {
            currMana = 0;
        }

        manaText.text = currMana.ToString();
        manaSlider.fillAmount = currMana / maxMana;
    }
}
