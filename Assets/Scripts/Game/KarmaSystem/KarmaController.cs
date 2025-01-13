using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KarmaController : MonoBehaviour
{
    private static KarmaController _instance;
    public static KarmaController Instance => _instance;

    public GameObject StatueUI;

    public GameObject NeedMoreMoney;
    public GameObject Success;
    public GameObject Fail;

    private float currentSpeedKarma;
    private float currentHpKarma;
    private float currentManaKarma;
    private float currentStrengthKarma;

    private PlayerController playerController;
    private IndicatorsController indicatorsController;

    public TMP_Text money;

    public TMP_Text crystals;

    public TMP_Text timer;

    public TMP_Text strength;

    public TMP_Text hp;

    public TMP_Text mana;

    public TMP_Text SuccessSpec;

    public TMP_Text FailSpec;


    public int totalMoney = 0;

    public float defaultTimerDuration = 60f;
    public float timerDuration = 0f;
    private bool timerIsRunning = false;


    public void Awake()
    {
        currentHpKarma = Constants.defaultHpProbKarma;
        currentSpeedKarma = Constants.defaultSpeedProbKarma;
        currentStrengthKarma = Constants.defaultStrengthProbKarma;
        currentManaKarma = Constants.addManaProbKarma;

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        indicatorsController = player.GetComponent<IndicatorsController>();

        hp.text = $"Здоровье: {indicatorsController.maxHealth}";
        mana.text = $"Мана: {indicatorsController.maxMana}";
        strength.text = $"Сила: {playerController.damageWeapon}";
    }

    public void OnDisable()
    {
        _instance = null;
    }

    public void setCrystals(int max, int current)
    {
        crystals.text = $"Кристаллы: {current}/{max}";
    }

    public void setNewText(string text)
    {
        crystals.text = text;
    }

    public void OpenStatue()
    {
        StatueUI.SetActive(true);
    }


    public void AddMoney(int count)
    {
        totalMoney += count;
        money.text = $"Монеты: {totalMoney}";
        if (Helpers.IsKarma(currentSpeedKarma))
        {
            if (timerIsRunning)
            {
                timerDuration += defaultTimerDuration;
            }
            else
            {
                timerDuration = defaultTimerDuration;
                StartCoroutine(StartTimer());
            }
        }
        if (currentSpeedKarma <= 0.8f)
        {
            currentSpeedKarma += Constants.addSpeedProbKarma;
        }
    }

    public bool SpendMoney(int count)
    {
        if (totalMoney < count)
        {
            NeedMoreMoney.SetActive(true);
            return false;
        }
        totalMoney -= count;
        money.text = $"Монеты: {totalMoney}";
        timerDuration = 0f;
        currentSpeedKarma = Constants.defaultSpeedProbKarma;
        return true;
    }

    private IEnumerator StartTimer()
    {
        timer.enabled = true;
        timerIsRunning = true;
        playerController.moveSpeed = Constants.karmaPlayerSpeed;
        while (timerDuration > 0)
        {
            timer.text = $"Замедление: {Helpers.FormatTime(timerDuration)}";
            yield return new WaitForSeconds(1f);
            timerDuration--;
        }
        playerController.moveSpeed = Constants.defaultPlayerSpeed;
        timerIsRunning = false;
        timer.enabled = false;
    }

    public void BuyHp(int count, int addHP)
    {
        if (!SpendMoney(count)) return;
        if (Helpers.IsKarma(currentHpKarma))
        {
            Fail.SetActive(true);
            FailSpec.text = "Здоровье";
            currentHpKarma = Constants.defaultHpProbKarma;
            indicatorsController.UpdateHealthMaxValue(Constants.defaultMaxHealthValue);
        }
        else
        {
            Success.SetActive(true);
            SuccessSpec.text = "Здоровье";
            indicatorsController.UpdateHealthMaxValue(indicatorsController.maxHealth + addHP);
            currentHpKarma += Constants.addHpProbKarma;
        }

        hp.text = $"Здоровье: {indicatorsController.maxHealth}";
    }

    public void BuyMana(int count, int addMana)
    {
        if (!SpendMoney(count)) return;
        if (Helpers.IsKarma(currentManaKarma))
        {
            Fail.SetActive(true);
            FailSpec.text = "Мана";
            currentManaKarma = Constants.defaultManaProbKarma;
            indicatorsController.UpdateManaMaxValue(Constants.defaultMaxManaValue);
        }
        else
        {
            Success.SetActive(true);
            SuccessSpec.text = "Мана";
            indicatorsController.UpdateManaMaxValue(indicatorsController.maxMana + addMana);
            currentManaKarma += Constants.addManaProbKarma;
        }

        mana.text = $"Мана: {indicatorsController.maxMana}";
    }

    public void BuyStrength(int count, int addStrength)
    {
        if (!SpendMoney(count)) return;
        if (Helpers.IsKarma(currentStrengthKarma))
        {
            Fail.SetActive(true);
            FailSpec.text = "Сила";
            playerController.damageWeapon = Constants.defaultPlayerDamage;
            currentStrengthKarma = Constants.defaultStrengthProbKarma;
        }
        else
        {
            Success.SetActive(true);
            SuccessSpec.text = "Сила";
            playerController.damageWeapon += addStrength;
            currentStrengthKarma += Constants.addStrengthProbKarma;
        }

        strength.text = $"Сила: {playerController.damageWeapon}";
    }


    public void DeactivateObjects()
    {
        crystals.enabled = false;
        money.enabled = false;
    }

    public void ActivateObjects()
    {
        crystals.enabled = true;
        money.enabled = true;
    }

}
