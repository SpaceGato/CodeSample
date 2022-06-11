using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade: MonoBehaviour
{
    public UpgradeType upgradeType;
    public enum UpgradeType
    {
        Weapon, Hull, Crew
    }

    public string ID, title, description;
    [NonSerialized] public string componentName;
    [SerializeField] private int basePrice;
    [NonSerialized] public int updatedPrice;

    Inventory inventory;

    GameObject player;
    GameManager gameManager;
    Shop shop;
    Image image;
    ToolTip toolTip;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        shop = gameManager.GetComponent<MenuManager>().shopMenu.GetComponent<Shop>();
        inventory = gameManager.inventory;
        player = GameObject.Find("Player");
        toolTip = GetComponent<ToolTip>();
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        string text = title;
        componentName = text.Replace(" ", "");

        toolTip.title = title;
        toolTip.description = description;
        image.sprite = Resources.Load<Sprite>("UI/UpgradeIcons/" + ID);

        UpdatePrice();
    }

    public void UpdatePrice()
    {
        int multiplier;
        switch (upgradeType)
        {
            case UpgradeType.Hull:
                multiplier = gameManager.saveData.priceMultiplier1;
                break;
            case UpgradeType.Crew:
                multiplier = gameManager.saveData.priceMultiplier2;
                break;
            default:
                multiplier = gameManager.saveData.priceMultiplier0;
                break;
        }
        updatedPrice = basePrice + (multiplier * 100);
    }

    private void SelectUpgrade() 
    {
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        shop.Select(this);        
    }

    public void Equip()
    {
        gameManager.saveData.upgrades.Add(ID, this);
        switch (upgradeType)
        {
            case UpgradeType.Hull:
                gameManager.saveData.priceMultiplier1++;
                break;
            case UpgradeType.Crew:
                gameManager.saveData.priceMultiplier2++;
                break;
            default:
                gameManager.saveData.priceMultiplier0++;
                break;
        }
        
        player.AddComponent(Type.GetType(componentName));
        inventory.Refresh();

        Destroy(this.gameObject);        
    }

    public void UnEquip()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        componentName = title.Replace(" ", "");

        gameManager.saveData.upgrades.Remove(ID);
        Destroy(player.GetComponent(Type.GetType(componentName)));
    }
}
