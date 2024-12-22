using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreUpgrade : MonoBehaviour
{
    [Header("Components")]
    public TMP_Text priceText;
    public TMP_Text incomeInfoText;
    public Button button;
    public Image characterImage;
    public TMP_Text upgradeNameText;
    [SerializeField] private AudioSource buySound;

    [Header("Generator Values")]
    public string upgradeName;
    public int startPrice = 15;
    public float upgradePriceMultiplier;
    public float cookiesPerUpgrade = 0.1f;

    [Header("Managers")]
    public GameManager gameManager;

    [HideInInspector] public int level = 0;

    private void Start() {
        UpdateUI();
    }

    public void ClickAction() {
        int price = CalculatePrice();
        bool purchaseSuccess = gameManager.PurchaseAction(price);
        if (purchaseSuccess) {
            level++;
            buySound.Play();
            UpdateUI();
        }
    }

    public void UpdateUI() {
        priceText.text = CalculatePrice().ToString();
        incomeInfoText.text = level.ToString() + " x " + cookiesPerUpgrade + "/s";
        // 5 x 0.5/s
        bool canAfford = gameManager.count >= CalculatePrice();
        button.interactable = canAfford;

        //Secret Upgrades
        bool isPurchased = level > 0;
        characterImage.color = isPurchased ? Color.white : Color.black;
        upgradeNameText.text = isPurchased ? upgradeName : "???";
    }

    int CalculatePrice() {
        int price = Mathf.RoundToInt(startPrice * Mathf.Pow(upgradePriceMultiplier, level));
        return price;
    }

    public float CalculateIncomePerSecond() {
        return cookiesPerUpgrade * level;
    }
}
