using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Items;
using UnityEngine;

namespace DFUVR
{
    public class Weapon : MonoBehaviour
    {
        public static GameObject GetWeaponObjectForHandObject(DaggerfallUnityItem weaponItem, out HandObject currentHandObject)
        {
            currentHandObject = GetHandObjectByType(weaponItem);
            if (currentHandObject == null)
            {
                Plugin.LoggerInstance.LogError("Current hand object is null");
                return null;
            }

            var weaponObject = Instantiate(currentHandObject.gameObject);
            if (weaponObject == null)
            {
                Plugin.LoggerInstance.LogError("Weapon object is null after instantiation");
                return null;
            }

            Collider collider = weaponObject.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = true;

            WeaponCollision weaponCollision = weaponObject.GetComponent<WeaponCollision>();
            if (weaponCollision != null)
                weaponCollision.item = weaponItem;

            return weaponObject;
        }

        public static string GetWeaponName(DaggerfallUnityItem weaponItem)
        {
            if (weaponItem == null)
                return "rHandClosed";
            else
                return weaponItem.LongName;
        }

        private static HandObject GetHandObjectByType(DaggerfallUnityItem weaponItem)
        {
            string weaponName = GetWeaponName(weaponItem);
            var weaponType = weaponItem?.GetWeaponType() ?? WeaponTypes.Melee;

            HandObject currentHandObject;
            if (weaponType != WeaponTypes.Bow && Var.handObjectsByName.ContainsKey(weaponName))
                currentHandObject = Var.handObjectsByName[weaponName];
            else
                currentHandObject = Var.handObjects[weaponType];

            return currentHandObject;
        }
    }
}
