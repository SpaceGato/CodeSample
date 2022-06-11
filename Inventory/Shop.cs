using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject buyButton;
    TextMeshProUGUI buyText;

    public Transform[] shopSlots;
    public Image[] slotImage;
    public GameObject[] weapon;
    public GameObject[] hull;
    public GameObject[] crew;

    Upgrade selectedUpgrade;

    Sprite[] upgradeSprites;
    GameManager gameManager;
    MoneyManager moneyManager;
    MenuManager menu;
    Inventory inventory;
    AudioSource shuffleSFX;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        moneyManager = gameManager.money;
        menu = gameManager.GetComponent<MenuManager>();
        inventory = menu.inventoryMenu.GetComponent<Inventory>();
        buyText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
        shuffleSFX = GetComponent<AudioSource>();
        upgradeSprites = Resources.LoadAll<Sprite>("UI/UpgradeIcons/");

        foreach (Image slot in slotImage)
            slot.GetComponent<Image>().sprite = upgradeSprites[0];
    }
    private void OnEnable()
    {
        menu.shopPanel.SetActive(false);
        StartCoroutine(Roll());
    }

    public void Select(Upgrade selected) 
    {
        if (selected == selectedUpgrade || selected == null)
        {
            selectedUpgrade = null;
            buyButton.SetActive(false);
        }
        else
        {
            selectedUpgrade = selected;
            selected.UpdatePrice();
            buyButton.SetActive(true);
            UpdateBuyText(selected.updatedPrice);        
        }
    }

    public void UpdateBuyText(int price)
    {
        buyText.text = "Buy <sprite=0>" + price;
    }
    public void Buy()
    {
        bool buyAllowed = false;
        if (moneyManager.CurrentCredits >= selectedUpgrade.updatedPrice && selectedUpgrade != null)
        {
            buyAllowed = true;
            gameManager.UpdateCredits(-selectedUpgrade.updatedPrice);
            selectedUpgrade.Equip();
            Select(null);
        }

        if (buyAllowed) 
            gameManager.audioManager.Play("BuyFX");
        else
            gameManager.audioManager.Play("CollisionFX");
    }

    public IEnumerator Roll()
    {
        Select(null);
        foreach (Transform slot in shopSlots)
        {
            if (slot.GetComponentInChildren<Upgrade>())
            {
                Upgrade slotUpgrade = slot.GetComponentInChildren<Upgrade>();
                slotUpgrade.gameObject.SetActive(false);
            }
        }
        for (int i = 0; i <= 20; i++)
        {
            shuffleSFX.Play();
            slotImage[0].sprite = upgradeSprites[Random.Range(21, upgradeSprites.Length)];
            slotImage[1].sprite = upgradeSprites[Random.Range(21, upgradeSprites.Length)];
            slotImage[2].sprite = upgradeSprites[Random.Range(11, 20)];
            slotImage[3].sprite = upgradeSprites[Random.Range(11, 20)];
            slotImage[4].sprite = upgradeSprites[Random.Range(1, 10)];
            slotImage[5].sprite = upgradeSprites[Random.Range(1, 10)];
            yield return new WaitForSecondsRealtime(0.05f);
        }
        foreach (Image image in slotImage)
            image.sprite = upgradeSprites[0];

        GenerateNewUpgrades();
    }

    private void GenerateNewUpgrades()
    {
        List<string> weaponIDs = new List<string>();
        List<string> hullIDs = new List<string>();
        List<string> crewIDs = new List<string>();

        //check inventory and save ids
        for (int i = 0; i < inventory.weaponSlots.Length; i++)
        {
            if (inventory.weaponSlots[i].gameObject.tag == "FilledSlot")
                weaponIDs.Add(inventory.weaponSlots[i].GetComponentInChildren<Upgrade>().ID);
        }
        for (int i = 0; i < inventory.hullSlots.Length; i++)
        {
            if (inventory.hullSlots[i].gameObject.tag == "FilledSlot")
                hullIDs.Add(inventory.hullSlots[i].GetComponentInChildren<Upgrade>().ID);
        }
        for (int i = 0; i < inventory.crewSlots.Length; i++)
        {
            if (inventory.crewSlots[i].gameObject.tag == "FilledSlot")
                crewIDs.Add(inventory.crewSlots[i].GetComponentInChildren<Upgrade>().ID);
        }

        //check shop and save ids, then clear shop items
        foreach (Transform slot in shopSlots)
        {
            if (slot.GetComponentInChildren<Upgrade>())
            {
                Upgrade slotUpgrade = slot.GetComponentInChildren<Upgrade>();
                switch (slotUpgrade.upgradeType)
                {
                    case Upgrade.UpgradeType.Weapon:
                        weaponIDs.Add(slotUpgrade.ID);
                        break;
                    case Upgrade.UpgradeType.Hull:
                        hullIDs.Add(slotUpgrade.ID);
                        break;
                    case Upgrade.UpgradeType.Crew:
                        crewIDs.Add(slotUpgrade.ID);
                        break;
                    default:
                        break;
                }
                Destroy(slotUpgrade.gameObject);
            }
        }

        //create new items in correct slots, saving ids to prevent duplicates
        GameObject[] array;
        List<string> list;
        for (int i = 0; i < shopSlots.Length; i++)
        {
            int searchAttempts = 0;
            switch (i)
            {
                case 0:
                    array = weapon;
                    list = weaponIDs;
                    break;
                case 1:
                    array = weapon;
                    list = weaponIDs;
                    break;
                case 2:
                    array = hull;
                    list = hullIDs;
                    break;
                case 3:
                    array = hull;
                    list = hullIDs;
                    break;
                case 4:
                    array = crew;
                    list = crewIDs;
                    break;
                default:
                    array = crew;
                    list = crewIDs;
                    break;
            }
            while (searchAttempts < 100)
            {
                GameObject newUpgrade = array[Random.Range(0, array.Length)];
                string upgradeID = newUpgrade.GetComponent<Upgrade>().ID;
                if (!list.Contains(upgradeID))
                {
                    GameObject clone = Instantiate(newUpgrade, shopSlots[i]);
                    list.Add(upgradeID);
                    break;
                }
                searchAttempts++;
            }
        }
    }
}