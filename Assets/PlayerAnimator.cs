using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    public WeaponsAnimations[] weaponsAnimations;
    Dictionary<Equipment, AnimationClip[]> weaponsAnimationsDict;
    protected override void Start()
    {
        base.Start();
        EquimentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        weaponsAnimationsDict = new Dictionary<Equipment, AnimationClip[]>();
        foreach (WeaponsAnimations a in weaponsAnimations)
        {
            weaponsAnimationsDict.Add(a.weapon, a.clips);
        }
    }
    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null && newItem.equipmentSlot == EquipmentSlot.Weapon)
        {
            animator.SetLayerWeight(1, 1);
            if(weaponsAnimationsDict.ContainsKey(newItem))
            {
                currentAttackAnimSet = weaponsAnimationsDict[newItem];
            }
        }
        else if (newItem == null && oldItem != null && oldItem.equipmentSlot == EquipmentSlot.Weapon)
        {
            animator.SetLayerWeight(1, 0);
            currentAttackAnimSet = defaultAttackAnimSet;
        }
        if (newItem != null && newItem.equipmentSlot == EquipmentSlot.Shield)
        {
            animator.SetLayerWeight(2, 1);
        }
        else if (newItem == null && oldItem != null && oldItem.equipmentSlot == EquipmentSlot.Shield)
        {
            animator.SetLayerWeight(2, 0);
        }
    }
    [System.Serializable]
    public struct WeaponsAnimations
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}
