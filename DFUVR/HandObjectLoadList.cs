using BepInEx;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility.AssetInjection;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DFUVR
{
    public class HandObjectLoadList : MonoBehaviour
    {
        public enum AssetType { DaggerfallResource, AssetBundle }

        public AssetType assetType { get; private set; }

        // used when loading from asset bundles
        public string assetBundlePath { get; private set; }
        // used when using asset ids from Daggerfall's resources
        public uint assetId { get; private set; }

        public WeaponTypes[] weaponTypes { get; private set; }
        public string assetName { get; private set; }
        public Vector3 unsheatedPositionOffset { get; private set; }
        public Quaternion unsheatedRotationOffset { get; private set; }
        public Vector3 sheatedPositionOffset { get; private set; }
        public Quaternion sheatedRotationOffset { get; private set; }
        public bool renderSheated { get; private set; }
        public bool resetPosition { get; private set; }
        public bool isActive { get; private set; }
        public Type collisionType { get; private set; }
        public Type colliderType { get; private set; }
        public Action<GameObject> postAction { get; private set; }
        private HandObjectLoadList(uint assetId, Action<GameObject> postAction, Type collisionType, Type colliderType, WeaponTypes[] weaponTypes, string assetName, bool renderSheated = false, bool resetPosition = false, bool isActive = false)
        {
            this.assetId = assetId;
            this.assetBundlePath = null;
            this.assetType = AssetType.DaggerfallResource;

            this.collisionType = collisionType;
            this.colliderType = colliderType;
            this.weaponTypes = weaponTypes;
            this.assetName = assetName;
            this.unsheatedPositionOffset = Vector3.zero;
            this.unsheatedRotationOffset = Quaternion.identity;
            this.sheatedPositionOffset = Vector3.zero;
            this.sheatedRotationOffset = Quaternion.identity;
            this.renderSheated = renderSheated;
            this.resetPosition = resetPosition;
            this.isActive = isActive;
            this.postAction = postAction;
        }

        private HandObjectLoadList(uint assetId, Action<GameObject> postAction, Type collisionType, Type colliderType, WeaponTypes[] weaponTypes, string assetName, Vector3 unsheatedPositionOffset, Quaternion unsheatedRotationOffset, Vector3 sheatedPositionOffset, Quaternion sheatedRotationOffset, bool renderSheated = false, bool resetPosition = false, bool isActive = false)
        {
            this.assetId = assetId;
            this.assetBundlePath = null;
            this.assetType = AssetType.DaggerfallResource;

            this.collisionType = collisionType;
            this.colliderType = colliderType;
            this.weaponTypes = weaponTypes;
            this.assetName = assetName;
            this.unsheatedPositionOffset = unsheatedPositionOffset;
            this.unsheatedRotationOffset = unsheatedRotationOffset;
            this.sheatedPositionOffset = sheatedPositionOffset;
            this.sheatedRotationOffset = sheatedRotationOffset;
            this.renderSheated = renderSheated;
            this.resetPosition = resetPosition;
            this.isActive = isActive;
            this.postAction = postAction;
        }

        private HandObjectLoadList(string assetBundlePath, Action<GameObject> postAction, Type collisionType, Type colliderType, WeaponTypes[] weaponTypes, string assetName, Vector3 unsheatedPositionOffset, Quaternion unsheatedRotationOffset, Vector3 sheatedPositionOffset, Quaternion sheatedRotationOffset, bool renderSheated = false, bool resetPosition = false, bool isActive = false)
        {
            this.assetBundlePath = assetBundlePath;
            this.assetId = 0;
            this.assetType = AssetType.AssetBundle;

            this.collisionType = collisionType;
            this.colliderType = colliderType;
            this.weaponTypes = weaponTypes;
            this.assetName = assetName;
            this.unsheatedPositionOffset = unsheatedPositionOffset;
            this.unsheatedRotationOffset = unsheatedRotationOffset;
            this.sheatedPositionOffset = sheatedPositionOffset;
            this.sheatedRotationOffset = sheatedRotationOffset;
            this.renderSheated = renderSheated;
            this.resetPosition = resetPosition;
            this.isActive = isActive;
            this.postAction = postAction;
        }

        public static readonly List<HandObjectLoadList> handObjectLoadList = new List<HandObjectLoadList>()
        {
            #region WISA models
            new HandObjectLoadList(128001, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Dagger"),
            new HandObjectLoadList(128002, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Tanto"),
            new HandObjectLoadList(128003, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Shortsword"),
            new HandObjectLoadList(128004, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Wakazashi"),
            new HandObjectLoadList(128005, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Longsword"),
            new HandObjectLoadList(128006, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Broadsword"),
            new HandObjectLoadList(128007, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Claymore"),
            new HandObjectLoadList(128008, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Saber"),
            new HandObjectLoadList(128009, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Katana"),
            new HandObjectLoadList(128010, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Dai Katana"),
            new HandObjectLoadList(128011, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Mace"),
            new HandObjectLoadList(128012, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Staff"),
            new HandObjectLoadList(128013, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Flail"),
            new HandObjectLoadList(128014, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Warhammer"),
            new HandObjectLoadList(128015, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Battle Axe"),
            new HandObjectLoadList(128016, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron War Axe"),
            new HandObjectLoadList(128017, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Short Bow"),
            new HandObjectLoadList(128018, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Long Bow"),
            new HandObjectLoadList(128021, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Crossbow"),
            new HandObjectLoadList(128022, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Archers Axe"),
            new HandObjectLoadList(128023, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Light Flail"),
            new HandObjectLoadList(128024, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Long Spear"),
            //new HandObjectLoadList(128028, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Dart"),
            new HandObjectLoadList(128029, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Crossbow Bolt"),
            new HandObjectLoadList(128030, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Arrow Broadhead"),
            new HandObjectLoadList(128101, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Dagger"),
            new HandObjectLoadList(128102, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Tanto"),
            new HandObjectLoadList(128103, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Shortsword"),
            new HandObjectLoadList(128104, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Wakazashi"),
            new HandObjectLoadList(128105, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Longsword"),
            new HandObjectLoadList(128106, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Broadsword"),
            new HandObjectLoadList(128107, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Claymore"),
            new HandObjectLoadList(128108, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Saber"),
            new HandObjectLoadList(128109, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Katana"),
            new HandObjectLoadList(128110, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Dai Katana"),
            new HandObjectLoadList(128111, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Mace"),
            new HandObjectLoadList(128112, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Staff"),
            new HandObjectLoadList(128113, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Flail"),
            new HandObjectLoadList(128114, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Warhammer"),
            new HandObjectLoadList(128115, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Battle Axe"),
            new HandObjectLoadList(128116, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel War Axe"),
            new HandObjectLoadList(128117, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Short Bow"),
            new HandObjectLoadList(128118, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Long Bow"),
            new HandObjectLoadList(128121, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Crossbow"),
            new HandObjectLoadList(128122, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Archers Axe"),
            new HandObjectLoadList(128123, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Light Flail"),
            new HandObjectLoadList(128124, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Long Spear"),
            //new HandObjectLoadList(128128, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Dart"),
            new HandObjectLoadList(128129, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Crossbow Bolt"),
            new HandObjectLoadList(128130, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Arrow Broadhead"),
            new HandObjectLoadList(128201, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Dagger"),
            new HandObjectLoadList(128202, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Tanto"),
            new HandObjectLoadList(128203, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Shortsword"),
            new HandObjectLoadList(128204, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Wakazashi"),
            new HandObjectLoadList(128205, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Longsword"),
            new HandObjectLoadList(128206, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Broadsword"),
            new HandObjectLoadList(128207, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Claymore"),
            new HandObjectLoadList(128208, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Saber"),
            new HandObjectLoadList(128209, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Katana"),
            new HandObjectLoadList(128210, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Dai Katana"),
            new HandObjectLoadList(128211, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Mace"),
            new HandObjectLoadList(128212, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Staff"),
            new HandObjectLoadList(128213, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Flail"),
            new HandObjectLoadList(128214, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Warhammer"),
            new HandObjectLoadList(128215, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Battle Axe"),
            new HandObjectLoadList(128216, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver War Axe"),
            new HandObjectLoadList(128217, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Short Bow"),
            new HandObjectLoadList(128218, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Long Bow"),
            new HandObjectLoadList(128221, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Crossbow"),
            new HandObjectLoadList(128222, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Archers Axe"),
            new HandObjectLoadList(128223, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Light Flail"),
            new HandObjectLoadList(128224, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Long Spear"),
            //new HandObjectLoadList(128228, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Dart"),
            new HandObjectLoadList(128229, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Crossbow Bolt"),
            new HandObjectLoadList(128230, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Arrow Broadhead"),
            new HandObjectLoadList(128301, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Dagger"),
            new HandObjectLoadList(128302, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Tanto"),
            new HandObjectLoadList(128303, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Shortsword"),
            new HandObjectLoadList(128304, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Wakazashi"),
            new HandObjectLoadList(128305, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Longsword"),
            new HandObjectLoadList(128306, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Broadsword"),
            new HandObjectLoadList(128307, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Claymore"),
            new HandObjectLoadList(128308, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Saber"),
            new HandObjectLoadList(128309, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Katana"),
            new HandObjectLoadList(128310, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Dai Katana"),
            new HandObjectLoadList(128311, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Mace"),
            new HandObjectLoadList(128312, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Staff"),
            new HandObjectLoadList(128313, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Flail"),
            new HandObjectLoadList(128314, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Warhammer"),
            new HandObjectLoadList(128315, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Battle Axe"),
            new HandObjectLoadList(128316, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven War Axe"),
            new HandObjectLoadList(128317, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Short Bow"),
            new HandObjectLoadList(128318, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Long Bow"),
            new HandObjectLoadList(128321, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Crossbow"),
            new HandObjectLoadList(128322, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Archers Axe"),
            new HandObjectLoadList(128323, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Light Flail"),
            new HandObjectLoadList(128324, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Long Spear"),
            //new HandObjectLoadList(128328, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Dart"),
            new HandObjectLoadList(128329, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Crossbow Bolt"),
            new HandObjectLoadList(128330, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Arrow Broadhead"),
            new HandObjectLoadList(128401, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Dagger"),
            new HandObjectLoadList(128402, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Tanto"),
            new HandObjectLoadList(128403, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Shortsword"),
            new HandObjectLoadList(128404, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Wakazashi"),
            new HandObjectLoadList(128405, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Longsword"),
            new HandObjectLoadList(128406, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Broadsword"),
            new HandObjectLoadList(128407, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Claymore"),
            new HandObjectLoadList(128408, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Saber"),
            new HandObjectLoadList(128409, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Katana"),
            new HandObjectLoadList(128410, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Dai Katana"),
            new HandObjectLoadList(128411, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Mace"),
            new HandObjectLoadList(128412, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Staff"),
            new HandObjectLoadList(128413, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Flail"),
            new HandObjectLoadList(128414, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Warhammer"),
            new HandObjectLoadList(128415, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Battle Axe"),
            new HandObjectLoadList(128416, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven War Axe"),
            new HandObjectLoadList(128417, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Short Bow"),
            new HandObjectLoadList(128418, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Long Bow"),
            new HandObjectLoadList(128421, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Crossbow"),
            new HandObjectLoadList(128422, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Archers Axe"),
            new HandObjectLoadList(128423, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Light Flail"),
            new HandObjectLoadList(128424, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Long Spear"),
            //new HandObjectLoadList(128428, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Dart"),
            new HandObjectLoadList(128429, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Crossbow Bolt"),
            new HandObjectLoadList(128430, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Arrow Broadhead"),
            new HandObjectLoadList(128501, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Dagger"),
            new HandObjectLoadList(128502, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Tanto"),
            new HandObjectLoadList(128503, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Shortsword"),
            new HandObjectLoadList(128504, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Wakazashi"),
            new HandObjectLoadList(128505, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Longsword"),
            new HandObjectLoadList(128506, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Broadsword"),
            new HandObjectLoadList(128507, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Claymore"),
            new HandObjectLoadList(128508, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Saber"),
            new HandObjectLoadList(128509, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Katana"),
            new HandObjectLoadList(128510, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Dai Katana"),
            new HandObjectLoadList(128511, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Mace"),
            new HandObjectLoadList(128512, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Staff"),
            new HandObjectLoadList(128513, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Flail"),
            new HandObjectLoadList(128514, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Warhammer"),
            new HandObjectLoadList(128515, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Battle Axe"),
            new HandObjectLoadList(128516, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril War Axe"),
            new HandObjectLoadList(128517, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Short Bow"),
            new HandObjectLoadList(128518, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Long Bow"),
            new HandObjectLoadList(128521, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Crossbow"),
            new HandObjectLoadList(128522, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Archers Axe"),
            new HandObjectLoadList(128523, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Light Flail"),
            new HandObjectLoadList(128524, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Long Spear"),
            //new HandObjectLoadList(128528, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Dart"),
            new HandObjectLoadList(128529, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Crossbow Bolt"),
            new HandObjectLoadList(128530, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Arrow Broadhead"),
            new HandObjectLoadList(128601, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Dagger"),
            new HandObjectLoadList(128602, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Tanto"),
            new HandObjectLoadList(128603, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Shortsword"),
            new HandObjectLoadList(128604, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Wakazashi"),
            new HandObjectLoadList(128605, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Longsword"),
            new HandObjectLoadList(128606, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Broadsword"),
            new HandObjectLoadList(128607, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Claymore"),
            new HandObjectLoadList(128608, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Saber"),
            new HandObjectLoadList(128609, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Katana"),
            new HandObjectLoadList(128610, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Dai Katana"),
            new HandObjectLoadList(128611, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Mace"),
            new HandObjectLoadList(128612, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Staff"),
            new HandObjectLoadList(128613, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Flail"),
            new HandObjectLoadList(128614, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Warhammer"),
            new HandObjectLoadList(128615, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Battle Axe"),
            new HandObjectLoadList(128616, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium War Axe"),
            new HandObjectLoadList(128617, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Short Bow"),
            new HandObjectLoadList(128618, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Long Bow"),
            new HandObjectLoadList(128621, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Crossbow"),
            new HandObjectLoadList(128622, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Archers Axe"),
            new HandObjectLoadList(128623, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Light Flail"),
            new HandObjectLoadList(128624, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Long Spear"),
            //new HandObjectLoadList(128628, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Dart"),
            new HandObjectLoadList(128629, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Crossbow Bolt"),
            new HandObjectLoadList(128630, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Arrow Broadhead"),
            new HandObjectLoadList(128701, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Dagger"),
            new HandObjectLoadList(128702, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Tanto"),
            new HandObjectLoadList(128703, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Shortsword"),
            new HandObjectLoadList(128704, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Wakazashi"),
            new HandObjectLoadList(128705, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Longsword"),
            new HandObjectLoadList(128706, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Broadsword"),
            new HandObjectLoadList(128707, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Claymore"),
            new HandObjectLoadList(128708, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Saber"),
            new HandObjectLoadList(128709, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Katana"),
            new HandObjectLoadList(128710, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Dai Katana"),
            new HandObjectLoadList(128711, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Mace"),
            new HandObjectLoadList(128712, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Staff"),
            new HandObjectLoadList(128713, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Flail"),
            new HandObjectLoadList(128714, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Warhammer"),
            new HandObjectLoadList(128715, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Battle Axe"),
            new HandObjectLoadList(128716, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony War Axe"),
            new HandObjectLoadList(128717, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Short Bow"),
            new HandObjectLoadList(128718, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Long Bow"),
            new HandObjectLoadList(128721, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Crossbow"),
            new HandObjectLoadList(128722, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Archers Axe"),
            new HandObjectLoadList(128723, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Light Flail"),
            new HandObjectLoadList(128724, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Long Spear"),
            //new HandObjectLoadList(128728, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Dart"),
            new HandObjectLoadList(128729, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Crossbow Bolt"),
            new HandObjectLoadList(128730, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Arrow Broadhead"),
            new HandObjectLoadList(128801, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Dagger"),
            new HandObjectLoadList(128802, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Tanto"),
            new HandObjectLoadList(128803, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Shortsword"),
            new HandObjectLoadList(128804, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Wakazashi"),
            new HandObjectLoadList(128805, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Longsword"),
            new HandObjectLoadList(128806, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Broadsword"),
            new HandObjectLoadList(128807, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Claymore"),
            new HandObjectLoadList(128808, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Saber"),
            new HandObjectLoadList(128809, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Katana"),
            new HandObjectLoadList(128810, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Dai Katana"),
            new HandObjectLoadList(128811, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Mace"),
            new HandObjectLoadList(128812, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Staff"),
            new HandObjectLoadList(128813, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Flail"),
            new HandObjectLoadList(128814, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Warhammer"),
            new HandObjectLoadList(128815, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Battle Axe"),
            new HandObjectLoadList(128816, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish War Axe"),
            new HandObjectLoadList(128817, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Short Bow"),
            new HandObjectLoadList(128818, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Long Bow"),
            new HandObjectLoadList(128821, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Crossbow"),
            new HandObjectLoadList(128822, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Archers Axe"),
            new HandObjectLoadList(128823, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Light Flail"),
            new HandObjectLoadList(128824, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Long Spear"),
            //new HandObjectLoadList(128828, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Dart"),
            new HandObjectLoadList(128829, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Crossbow Bolt"),
            new HandObjectLoadList(128830, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Arrow Broadhead"),
            new HandObjectLoadList(128901, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Dagger"),
            new HandObjectLoadList(128902, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Tanto"),
            new HandObjectLoadList(128903, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Shortsword"),
            new HandObjectLoadList(128904, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Wakazashi"),
            new HandObjectLoadList(128905, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Longsword"),
            new HandObjectLoadList(128906, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Broadsword"),
            new HandObjectLoadList(128907, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Claymore"),
            new HandObjectLoadList(128908, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Saber"),
            new HandObjectLoadList(128909, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Katana"),
            new HandObjectLoadList(128910, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Dai Katana"),
            new HandObjectLoadList(128911, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Mace"),
            new HandObjectLoadList(128912, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Staff"),
            new HandObjectLoadList(128913, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Flail"),
            new HandObjectLoadList(128914, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Warhammer"),
            new HandObjectLoadList(128915, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Battle Axe"),
            new HandObjectLoadList(128916, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric War Axe"),
            new HandObjectLoadList(128917, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Short Bow"),
            new HandObjectLoadList(128918, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Long Bow"),
            new HandObjectLoadList(128921, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Crossbow"),
            new HandObjectLoadList(128922, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Archers Axe"),
            new HandObjectLoadList(128923, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Light Flail"),
            new HandObjectLoadList(128924, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Long Spear"),
            //new HandObjectLoadList(128928, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Dart"),
            new HandObjectLoadList(128929, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Crossbow Bolt"),
            new HandObjectLoadList(128930, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Arrow Broadhead"),
            new HandObjectLoadList(129001, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Auriels Bow"),
            new HandObjectLoadList(129002, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Auriels Shield"),
            new HandObjectLoadList(129003, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Chrysamere"),
            new HandObjectLoadList(129004, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Blade"),
            new HandObjectLoadList(129005, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Hercine Ring"),
            new HandObjectLoadList(129006, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mace Of Molag Bal"),
            new HandObjectLoadList(129007, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mehrunes Razor"),
            new HandObjectLoadList(129008, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Spell Breaker Shield"),
            new HandObjectLoadList(129009, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Staff Of Magnus"),
            new HandObjectLoadList(129010, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Volendrung"),
            new HandObjectLoadList(129015, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dragon Dagger"),
            new HandObjectLoadList(129040, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Torch Wood"),
            new HandObjectLoadList(129041, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Torch Iron"),
            //new HandObjectLoadList(129042, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Candle Plate"),
            new HandObjectLoadList(129043, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Candlestick"),
            new HandObjectLoadList(129044, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Candelabra"),
            new HandObjectLoadList(129045, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Lantern Iron"),
            new HandObjectLoadList(129501, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dagger Enchanted"),
            new HandObjectLoadList(129502, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Tanto Enchanted"),
            new HandObjectLoadList(129503, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Shortsword Enchanted"),
            new HandObjectLoadList(129504, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Wakazashi Enchanted"),
            new HandObjectLoadList(129505, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Longsword Enchanted"),
            new HandObjectLoadList(129506, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Broadsword Enchanted"),
            new HandObjectLoadList(129507, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Claymore Enchanted"),
            new HandObjectLoadList(129508, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Saber Enchanted"),
            new HandObjectLoadList(129509, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Katana Enchanted"),
            new HandObjectLoadList(129510, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dai Katana Enchanted"),
            new HandObjectLoadList(129511, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mace Enchanted"),
            new HandObjectLoadList(129512, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Staff Enchanted"),
            new HandObjectLoadList(129513, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Flail Enchanted"),
            new HandObjectLoadList(129514, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Warhammer Enchanted"),
            new HandObjectLoadList(129515, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Battle Axe Enchanted"),
            new HandObjectLoadList(129516, null, typeof(WeaponCollision), typeof(MeshCollider), null, "War Axe Enchanted"),
            new HandObjectLoadList(129517, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Short Bow Enchanted"),
            new HandObjectLoadList(129518, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Long Bow Enchanted"),
            new HandObjectLoadList(129521, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Crossbow Enchanted"),
            new HandObjectLoadList(129522, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Archers Axe Enchanted"),
            new HandObjectLoadList(129523, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Light Flail Enchanted"),
            new HandObjectLoadList(129524, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Long Spear Enchanted"),
            //new HandObjectLoadList(129528, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dart Enchanted"),
            new HandObjectLoadList(129529, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Crossbow Bolt Enchanted"),
            new HandObjectLoadList(129530, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Arrow Broadhead Enchanted"),
            new HandObjectLoadList(132090, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Buckler Shield"),
            new HandObjectLoadList(132091, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Round Shield"),
            new HandObjectLoadList(132092, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Kite Shield"),
            new HandObjectLoadList(132093, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Iron Tower Shield"),
            new HandObjectLoadList(132190, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Buckler Shield"),
            new HandObjectLoadList(132191, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Round Shield"),
            new HandObjectLoadList(132192, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Kite Shield"),
            new HandObjectLoadList(132193, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Steel Tower Shield"),
            new HandObjectLoadList(132290, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Buckler Shield"),
            new HandObjectLoadList(132291, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Round Shield"),
            new HandObjectLoadList(132292, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Kite Shield"),
            new HandObjectLoadList(132293, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Silver Tower Shield"),
            new HandObjectLoadList(132390, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Buckler Shield"),
            new HandObjectLoadList(132391, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Round Shield"),
            new HandObjectLoadList(132392, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Kite Shield"),
            new HandObjectLoadList(132393, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Elven Tower Shield"),
            new HandObjectLoadList(132490, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Buckler Shield"),
            new HandObjectLoadList(132491, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Round Shield"),
            new HandObjectLoadList(132492, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Kite Shield"),
            new HandObjectLoadList(132493, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Dwarven Tower Shield"),
            new HandObjectLoadList(132590, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Buckler Shield"),
            new HandObjectLoadList(132591, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Round Shield"),
            new HandObjectLoadList(132592, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Kite Shield"),
            new HandObjectLoadList(132593, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Mithril Tower Shield"),
            new HandObjectLoadList(132690, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Buckler Shield"),
            new HandObjectLoadList(132691, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Round Shield"),
            new HandObjectLoadList(132692, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Kite Shield"),
            new HandObjectLoadList(132693, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Adamantium Tower Shield"),
            new HandObjectLoadList(132790, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Buckler Shield"),
            new HandObjectLoadList(132791, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Round Shield"),
            new HandObjectLoadList(132792, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Kite Shield"),
            new HandObjectLoadList(132793, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Ebony Tower Shield"),
            new HandObjectLoadList(132890, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Buckler Shield"),
            new HandObjectLoadList(132891, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Round Shield"),
            new HandObjectLoadList(132892, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Kite Shield"),
            new HandObjectLoadList(132893, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Orcish Tower Shield"),
            new HandObjectLoadList(132990, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Buckler Shield"),
            new HandObjectLoadList(132991, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Round Shield"),
            new HandObjectLoadList(132992, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Kite Shield"),
            new HandObjectLoadList(132993, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Daedric Tower Shield"),
            new HandObjectLoadList(133090, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Leather Buckler Shield"),
            new HandObjectLoadList(133091, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Leather Round Shield"),
            new HandObjectLoadList(133092, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Leather Kite Shield"),
            new HandObjectLoadList(133093, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Leather Tower Shield"),
            new HandObjectLoadList(133190, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Fur Buckler Shield"),
            new HandObjectLoadList(133191, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Fur Round Shield"),
            new HandObjectLoadList(133192, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Fur Kite Shield"),
            new HandObjectLoadList(133193, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Fur Tower Shield"),
            new HandObjectLoadList(133290, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Chain Buckler Shield"),
            new HandObjectLoadList(133291, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Chain Round Shield"),
            new HandObjectLoadList(133292, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Chain Kite Shield"),
            new HandObjectLoadList(133293, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Chain Tower Shield"),
            new HandObjectLoadList(133390, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Enchanted Buckler Shield"),
            new HandObjectLoadList(133391, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Enchanted Round Shield"),
            new HandObjectLoadList(133392, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Enchanted Kite Shield"),
            new HandObjectLoadList(133393, null, typeof(WeaponCollision), typeof(MeshCollider), null, "Enchanted Tower Shield"),
            #endregion
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.LongBlade, WeaponTypes.LongBlade_Magic }, "Sword", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity, true, true, true),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Dagger, WeaponTypes.Dagger_Magic }, "Steel_Dagger_512", Vector3.zero, Quaternion.Euler(0, 90, 90), Vector3.zero, Quaternion.Euler(0, 90, 90), true),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Battleaxe, WeaponTypes.Battleaxe_Magic }, "MM_Axe_01_01_lod2", Vector3.zero, Quaternion.Euler(0, -90, 90), Vector3.zero, Quaternion.Euler(0, 180, 180)),
            new HandObjectLoadList("AssetBundles/weapons", null, null, null, new[] { WeaponTypes.None }, "Sheath", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity, true),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Mace, WeaponTypes.Mace_Magic }, "mace", new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0), new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0)),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Flail, WeaponTypes.Flail_Magic }, "mace", new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0), new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0)),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(BoxCollider), new[] { WeaponTypes.Warhammer, WeaponTypes.Warhammer_Magic }, "Warhammer_1", Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.zero, Quaternion.identity),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(BoxCollider), new[] { WeaponTypes.Staff, WeaponTypes.Staff_Magic }, "Staff_1", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity),
            new HandObjectLoadList("AssetBundles/weapons", BowPostAction, null, typeof(SphereCollider), new[] { WeaponTypes.Bow }, "Crossbow", Vector3.zero, Quaternion.Euler(270, 90, 0), Vector3.zero, Quaternion.Euler(270, 90, 0)),
            new HandObjectLoadList("AssetBundles/hands", HandPostAction, typeof(WeaponCollision), typeof(SphereCollider), new[] { WeaponTypes.Melee, WeaponTypes.Werecreature }, "rHandClosed", new Vector3(0, 0, -0.05f), Quaternion.Euler(20, 10, 270), Vector3.zero, Quaternion.identity)
        };

        /// <summary>
        /// Load all objects that can be held in hand.
        /// The objects are loaded from the list above.
        /// </summary>
        public static void LoadAllHandObjects(Dictionary<WeaponTypes, HandObject> handObjectDictionary, Dictionary<string, HandObject> handObjectDictionaryByName)
        {
            AssetBundle currentBundle = null;
            string currentBundlePath = string.Empty;

            foreach (var handObjectLoad in handObjectLoadList)
            {
                Plugin.LoggerInstance.LogInfo("Loading HandObject: " + handObjectLoad.assetName);

                GameObject gameObject = null;
                // if it's a daggerfall resource, it can come from mods (like the WISA mod), so we have to load it differently
                if (handObjectLoad.assetType == AssetType.DaggerfallResource)
                {
                    gameObject = MeshReplacement.ImportCustomGameobject(handObjectLoad.assetId, null, Matrix4x4.identity);
                    if (gameObject == null)
                    {
                        Plugin.LoggerInstance.LogError($"Failed to load asset '{handObjectLoad.assetName}' from DaggerfallResources.");
                        continue;
                    }
                }
                // this is for when it's from the bundled assets
                else
                {
                    if (handObjectLoad.assetBundlePath != currentBundlePath)
                    {
                        currentBundlePath = handObjectLoad.assetBundlePath;
                        currentBundle?.Unload(false);

                        string assetBundlePath = Path.Combine(Paths.PluginPath, currentBundlePath);
                        currentBundle = AssetBundle.LoadFromFile(assetBundlePath);
                        if (currentBundle == null)
                        {
                            Plugin.LoggerInstance.LogError($"Failed to load AssetBundle from path '{handObjectLoad.assetBundlePath}'.");
                            continue;
                        }
                    }

                    gameObject = Instantiate(currentBundle.LoadAsset<GameObject>(handObjectLoad.assetName));
                    if (gameObject == null)
                        Plugin.LoggerInstance.LogError($"Failed to load asset '{handObjectLoad.assetName}' from AssetBundle.");
                }

                // if it has collisionType, it means it has both Collision and Collider
                if (handObjectLoad.collisionType != null)
                {
                    var collider = gameObject.GetComponent(handObjectLoad.colliderType) as Collider;
                    if (collider == null)
                    {
                        if (handObjectLoad.colliderType == typeof(MeshCollider))
                        {
                            var meshFilter = gameObject.GetComponentInChildren<MeshFilter>();
                            if (meshFilter == null)
                            {
                                Plugin.LoggerInstance.LogError($"Failed to find mesh from '{handObjectLoad.assetName}'. Components:");
                                foreach (var component in gameObject.GetComponentsInChildren<Component>())
                                    Plugin.LoggerInstance.LogError($"{component.GetType().Name}");
                            }
                            else
                            {
                                if (!meshFilter.sharedMesh.isReadable)
                                {
                                    Plugin.LoggerInstance.LogWarning($"Mesh for '{handObjectLoad.assetName}' is not readable, collision may not work properly. Adding BoxCollider as replacement.");

                                    BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                                    boxCollider.size = meshFilter.mesh.bounds.size;
                                    boxCollider.center = meshFilter.mesh.bounds.center;
                                    collider = boxCollider;
                                }
                                else
                                {
                                    var newCollider = gameObject.AddComponent(handObjectLoad.colliderType) as MeshCollider;
                                    newCollider.convex = true;
                                    newCollider.sharedMesh = meshFilter.sharedMesh;
                                    collider = newCollider;
                                }
                            }
                        }
                        else
                            collider = gameObject.AddComponent(handObjectLoad.colliderType) as Collider;
                    }

                    if (collider == null)
                        Plugin.LoggerInstance.LogError($"Failed to add collider of type '{handObjectLoad.colliderType}' to '{handObjectLoad.assetName}'.");
                    else
                        collider.isTrigger = true;

                    gameObject.AddComponent(handObjectLoad.collisionType);
                }
                // only collider
                else if (handObjectLoad.colliderType != null)
                {
                    var collider = gameObject.AddComponent(handObjectLoad.colliderType) as Collider;
                    if (collider == null)
                        Plugin.LoggerInstance.LogError($"Failed to add collider of type '{handObjectLoad.colliderType}' to '{handObjectLoad.assetName}'.");
                    else
                        collider.isTrigger = true;
                }

                gameObject.SetActive(handObjectLoad.isActive);

                if (handObjectLoad.resetPosition)
                {
                    gameObject.transform.position = Vector3.zero;
                    gameObject.transform.rotation = Quaternion.identity;
                }

                // these are for defaults per weapon type
                if (handObjectLoad.weaponTypes != null)
                {
                    foreach (var type in handObjectLoad.weaponTypes)
                    {
                        if (!handObjectDictionary.ContainsKey(type))
                            handObjectDictionary.Add(type, new HandObject(handObjectLoad, gameObject));
                    }
                }
                // this is for accessing by name later
                else if (!string.IsNullOrEmpty(handObjectLoad.assetName))
                {
                    handObjectDictionaryByName.Add(handObjectLoad.assetName, new HandObject(handObjectLoad, gameObject));
                }

                handObjectLoad.postAction?.Invoke(gameObject);

                Plugin.LoggerInstance.LogInfo($"Loaded ${handObjectLoad.assetName}.");
            }

            //handObjectDictionary[WeaponTypes.LongBlade].gameObject.AddComponent<DebugColliders>();

            currentBundle?.Unload(false);
        }

        private static void BowPostAction(GameObject bow)
        {
            bow.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

        private static void HandPostAction(GameObject meleeHandR)
        {
            try
            {
                var sword = Var.handObjects[WeaponTypes.LongBlade].gameObject;
                meleeHandR.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = sword.GetComponent<MeshRenderer>().material;
                sword.SetActive(false);
            }
            catch (Exception e)
            {
                Plugin.LoggerInstance.LogError(e);
            }
        }

        public override string ToString()
        {
            return $"HandObjectLoadList(weaponTypes={string.Join(",", weaponTypes)}, assetName={assetName}, sheatedPositionOffset={unsheatedPositionOffset}, sheatedRotationOffset={unsheatedRotationOffset}, unsheatedPositionOffset={sheatedPositionOffset}, unsheatedRotationOffset={sheatedRotationOffset}, renderUnsheated={renderSheated}, resetPosition={resetPosition}, isActive={isActive}, collisionType={collisionType}, colliderType={colliderType}, postAction={(postAction != null ? postAction.Method.Name : "null")})";
        }
    }
}
