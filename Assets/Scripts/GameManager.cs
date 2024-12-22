using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text incomeText;
    [SerializeField] StoreUpgrade[] storeUpgrades;
    [SerializeField] int updatesPerSecond = 5;
    [SerializeField] AudioSource clickSound;

    [HideInInspector] public float count = 0;
    float nextTimeCheck = 1;
    float lastIncomeValue = 0;

    public void ResetProgression() {
        count = 0;
        foreach (var storeUpgrade in storeUpgrades)
        {
            storeUpgrade.level = 0;
            storeUpgrade.UpdateUI();
        }
        UpdateUI();
    }

    void IdleCalculate() {
        float sum = 0;
        foreach (var storeUpgrade in storeUpgrades)
        {
            sum += storeUpgrade.CalculateIncomePerSecond();
            storeUpgrade.UpdateUI();
        }
        lastIncomeValue = sum;
        count += sum / updatesPerSecond;
        UpdateUI();
    }

    public void ClickAction() {
        count++;
        clickSound.Play();
        UpdateUI();
    }

    void UpdateUI() {
        countText.text = Mathf.RoundToInt(count).ToString();
        incomeText.text = lastIncomeValue.ToString() + "/s";
    }

    public bool PurchaseAction(int cost) {
        if (count >= cost) {
            count -= cost;
            UpdateUI();
            return true;
        }
        return false;
    }

    void Start() {
        UpdateUI();
    }

    void Update() {
        if (nextTimeCheck < Time.timeSinceLevelLoad) {
            IdleCalculate();
            nextTimeCheck = Time.timeSinceLevelLoad + 1f / updatesPerSecond;
        }
    }
}
