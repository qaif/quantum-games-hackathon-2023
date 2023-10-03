using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Equipment", menuName ="Inventory/Equipment")]
public class Equipment : Item
{
    public int armorModifier;
    public int damageModifier;
    public EquipmentSlot equipmentSlot;
    public SkinnedMeshRenderer mesh;
    public EquipmentMeshRegion[] coveredMeshRegions;
    public override void Use()
    {
        base.Use();
        EquimentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}
public enum EquipmentSlot {Head, Chest, Legs, Weapon, Shield, Feet}
public enum EquipmentMeshRegion { Legs,Arms,Torso} //corresponds to body blend shapes