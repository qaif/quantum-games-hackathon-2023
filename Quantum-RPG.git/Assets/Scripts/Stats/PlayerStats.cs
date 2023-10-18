using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        EquimentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    // Update is called once per frame
    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            armour.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
            max.AddModifier(newItem.maxModifier);
            heal.AddModifier(newItem.healModifier);

        }
        if (oldItem != null)
        {
            armour.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            max.RemoveModifier(oldItem.maxModifier);
            heal.RemoveModifier(oldItem.healModifier);

        }
    }
    public override void Die()
    {
        base.Die();
        //Kill the player, play animation, respaws or game over screen
        PlayerManager.instance.KillPlayer();
    }
}
