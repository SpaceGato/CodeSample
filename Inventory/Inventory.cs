using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{    
    public SaveData saveData;

    public Transform[] weaponSlots, hullSlots, crewSlots;
    List<Transform> allSlots = new List<Transform>();
    MenuManager menu;

    public void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            allSlots.Add(weaponSlots[i]);
            allSlots.Add(hullSlots[i]);
            allSlots.Add(crewSlots[i]);
        }
        gameObject.SetActive(true);
    }

    public void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform slot in allSlots)
        {
            slot.tag = "EmptySlot";
            slot.GetComponent<Image>().color = new Color32(255, 255, 255, 70);

            Upgrade emptyUpgrade = slot.GetChild(0).GetComponent<Upgrade>();
            emptyUpgrade.ID = "0";
            emptyUpgrade.title = "";
            emptyUpgrade.description = "";

            emptyUpgrade.gameObject.SetActive(false);
        }

        foreach (KeyValuePair<string, Upgrade> upgrade in saveData.upgrades)
        {
            Transform[] slots;
            switch (upgrade.Value.upgradeType)
            {
                case Upgrade.UpgradeType.Hull:
                    slots = hullSlots;
                    break;
                case Upgrade.UpgradeType.Crew:
                    slots = crewSlots;
                    break;
                default:
                    slots = weaponSlots;
                    break;
            }
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].gameObject.tag == "EmptySlot")
                {
                    slots[i].gameObject.tag = "FilledSlot";
                    slots[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);

                    Upgrade emptyUpgrade = slots[i].GetChild(0).GetComponent<Upgrade>();
                    emptyUpgrade.ID = upgrade.Value.ID;
                    emptyUpgrade.title = upgrade.Value.title;
                    emptyUpgrade.description = upgrade.Value.description;

                    emptyUpgrade.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void RemoveUpgrade(string id)
    {
        foreach (KeyValuePair<string, Upgrade> upgrade in saveData.upgrades)
        {
            if (upgrade.Key == id)
            {
                upgrade.Value.UnEquip();
                break;
            }
        }
        Refresh();
    }
}
