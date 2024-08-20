using Arcs;
using BoardTools;
using Players;
using Ship;
using System;
using System.Collections.Generic;
using UnityEngine;
using Upgrade;
using System.IO;

namespace Remote
{
    public abstract class GenericRemote : GenericShip
    {
        public override bool HasCombatActivation { get { return false; } }
        public RemoteInfo RemoteInfo { get; protected set; }
        public new RemoteTokensHolder Tokens { get; protected set; } // Assign only Red TLs
        public abstract Dictionary<string, Vector3> BaseEdges { get; }
        public new BoardObjectType BoardObjectType => BoardObjectType.Remote;

        public GenericRemote(GenericPlayer owner)
        {
            Owner = owner;
        }

        public void SpawnModel(int shipId, Vector3 position, Quaternion rotation)
        {
            ShipId = shipId;

            GeneratePilotInfo();
            GenerateModel(position, rotation);
            
            GeneratePseudoBase();
            GeneratePseudoShip();
            InitializeRosterPanel();

            ActivatePilotAbilities();

            Board.RegisterRemote(this);

            Roster.AddShipToLists(this);

            //string filePath = "C:/Users/Matt/Desktop/test.txt";

            //Transform RemoteCollider = ShipAllParts.Find("ShipBase/model/RemoteCollider").transform;
            //Vector3[] list = RemoteCollider.GetComponent<MeshFilter>().mesh.vertices;

            //String text = "";
            //int iter = 0;
            //foreach (Vector3 vertexActual in list)
            //{
            //    Vector3 temp = RemoteCollider.transform.TransformPoint(vertexActual);
            //    Vector3 vertexLocal = Model.transform.InverseTransformPoint(temp);
            //    if (vertexLocal.y == 0)
            //    {
            //        text += "{ \"R" + iter + "\", new Vector3(" + vertexLocal.x + "f, " + vertexLocal.y + ", " + vertexLocal.z + "f) },\r\n";
            //        iter++;
            //    }

            //}

            //File.WriteAllText(filePath, text);


            //Dictionary<string, Vector3> test = ShipBase.GetBaseEdges();

            //for (int i = 0; i < 39; i++)
            //{

            //    Debug.DrawLine(test.GetValueOrDefault("R" + i) + new Vector3(0, 0.01f, 0), GetCenter() + new Vector3(0, 0.01f, 0), Color.red, 30f);
            //}
            //Debug.DrawLine(test.GetValueOrDefault("R0") + new Vector3(0, 0.01f, 0), test.GetValueOrDefault("R39") + new Vector3(0, 0.01f, 0), Color.red, 30f);
        }

        private void GeneratePseudoBase()
        {
            ShipBase = new RemoteShipBase(this);
        }

        private void GeneratePilotInfo()
        {
            ShipInfo = new ShipCardInfo(
                "Remote",
                BaseSize.None,
                Faction.None,
                RemoteInfo.ArcInfo,
                RemoteInfo.Agility,
                RemoteInfo.Hull,
                0,
                new ShipActionsInfo(),
                new ShipUpgradesInfo()
            );

            SoundInfo = RemoteInfo.SoundInfo;

            PilotInfo = new PilotCardInfo(
                RemoteInfo.Name,
                RemoteInfo.Initiative,
                0,
                abilityType: RemoteInfo.AbilityType,
                charges: RemoteInfo.Charges,
                regensCharges: RemoteInfo.RegensCharges
            );

            ImageUrl = RemoteInfo.ImageUrl;
        }

        private void GenerateModel(Vector3 position, Quaternion rotation)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Remotes/" + RemoteInfo.Name);
            Model = MonoBehaviour.Instantiate(prefab, position, rotation, BoardTools.Board.GetBoard());
            ShipAllParts = Model.transform.Find("RotationHelper/RotationHelper2/ShipAllParts").transform;

            ModelInfo = new ShipModelInfo(RemoteInfo.Name, null);
            modelCenter = ShipAllParts.Find("ShipModels/" + (SpecialModel ?? FixTypeName(ModelInfo.ModelName)) + "/ModelCenter").transform;

            SetTagOfChildrenRecursive(Model.transform, "ShipId:" + ShipId.ToString());
            SetRaycastTarget(true);
            SetSpotlightMask();
            SetShipIdText(Model);
            SetPlayerCustomization();

            // InitializeShipBase();
        }

        protected virtual void SetPlayerCustomization()
        {
            // Customize to show different view of remote for Player1 and Player2
        }

        public Vector3 GetJointAngles(int jointIndex)
        {
            return ShipAllParts.Find("ShipBase/ManeuverJoints").Find("ManeuverJoint" + jointIndex).Find("Rotation").eulerAngles;
        }

        public Vector3 GetJointPosition(int jointIndex)
        {
            return ShipAllParts.Find("ShipBase/ManeuverJoints").Find("ManeuverJoint" + jointIndex).Find("Rotation").position;
        }

        private void GeneratePseudoShip()
        {
            Damage = new Damage(this);
            ActionBar.Initialize();
            InitializeState();            
            InitializeShipBaseArc();
            InitializeSectors();
            foreach (ShipArcInfo arcInfo in ShipInfo.ArcInfo.Arcs)
            {
                PrimaryWeaponClass weapon = new PrimaryWeaponClass(this, arcInfo);
                weapon.WeaponInfo = new SpecialWeaponInfo(2, 1, 2, noRangeBonus: true);

                if (arcInfo.Firepower != -1) PrimaryWeapons.Add(weapon);
            }
        }

        public void ToggleJointArrow(int jointIndex, bool isVisible)
        {
            ShipAllParts.Find("ShipBase/ManeuverJoints").Find("ManeuverJoint" + jointIndex).gameObject.SetActive(isVisible);
        }

        public override Vector3 GetCenter()
        {
            return Model.transform.TransformPoint(0, 0, -ShipBase.HALF_OF_SHIPSTAND_SIZE);
        }

        public override Transform GetModelTransform()
        {
            return ShipAllParts.Find("ShipModels/" + (SpecialModel ?? FixTypeName(ModelInfo.ModelName)) + "/ModelCenter").transform;
        }

        public override Vector3 GetModelCenter()
        {
            return GetModelTransform().position;
        }

        public class RemoteShipBase : GenericShipBase
        {
            public RemoteShipBase(GenericShip host) : base(host)
            {
                baseEdges = new Dictionary<string, Vector3>((host as GenericRemote).BaseEdges);

                HALF_OF_FIRINGARC_SIZE = 0.425f;
                HALF_OF_SHIPSTAND_SIZE = 0.5f;
                SHIPSTAND_SIZE = 1f;
                SHIPSTAND_SIZE_CM = 4f;
            }

            public override List<ManeuverTemplate> BoostTemplatesAvailable => throw new NotImplementedException();
            public override List<ManeuverTemplate> BarrelRollTemplatesAvailable => throw new NotImplementedException();
            public override List<ManeuverTemplate> DecloakBoostTemplatesAvailable => throw new NotImplementedException();
            public override List<ManeuverTemplate> DecloakBarrelRollTemplatesAvailable => throw new NotImplementedException();
        }
    }
}
