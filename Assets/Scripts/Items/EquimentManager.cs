using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquimentManager : MonoBehaviour
{
    #region Singleton
    public static EquimentManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    public Equipment[] defaultItems;
    Equipment[] currentEquiment;
    SkinnedMeshRenderer[] currentMeshes;
    public SkinnedMeshRenderer targetmesh;
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.instance;
        int numOfSlots=System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquiment = new Equipment[numOfSlots];
        currentMeshes = new SkinnedMeshRenderer[numOfSlots];
        EquipDefaultItems();

    }
    public void Equip(Equipment newItem)
    {
        int slotIndex=(int)newItem.equipmentSlot;
        Equipment oldItem = Unequip(slotIndex);
        if(onEquipmentChanged!=null)
        {
            onEquipmentChanged.Invoke(newItem,oldItem);
        }
        SetEquipmentBlendShapes(newItem, 100);
        currentEquiment[slotIndex] = newItem;
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetmesh.transform;
        newMesh.bones = targetmesh.bones;
        newMesh.rootBone = targetmesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }
    public Equipment Unequip(int slotIndex)
    {
        if(currentEquiment[slotIndex]!=null)
        {
            if(currentMeshes[slotIndex]!=null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            Equipment oldItem = currentEquiment[slotIndex];
            SetEquipmentBlendShapes(oldItem, 0);
            inventory.Add(oldItem);
            currentEquiment[slotIndex] = null;
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }
    public void UnequipAll()
    {
        for (int i = 0; i < currentEquiment.Length; i++)
        {
                Unequip(i);
        }
        EquipDefaultItems();
    }
    void SetEquipmentBlendShapes(Equipment item, int weight)
    {
        foreach(EquipmentMeshRegion blendShape in item.coveredMeshRegions)
        {
            targetmesh.SetBlendShapeWeight((int)blendShape, weight);
        }
    }
    void EquipDefaultItems()
    {
        foreach (Equipment item in defaultItems)
        {
            Equip(item);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

}
