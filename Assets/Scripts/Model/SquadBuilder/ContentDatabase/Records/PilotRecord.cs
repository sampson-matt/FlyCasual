using Editions;
using Ship;
using System;
using UnityEngine;
using System.Collections.Generic;
using Content;


namespace SquadBuilderNS
{
    public class PilotRecord
    {
        public GenericShip Instance { get; }
        public string PilotName => Instance.PilotInfo.PilotName;
        public string PilotTypeName => Instance.GetType().ToString();
        public string PilotNameCanonical => Instance.PilotNameCanonical;
        public Faction PilotFaction => Instance.Faction;
        public int PilotSkill => Instance.PilotInfo.Initiative;
        public List<Tags> Tags => Instance.PilotInfo.Tags;
        public ShipRecord Ship { get; }
        public bool IsAllowedForSquadBuilder => Instance.PilotInfo != null && Instance.IsAllowedForSquadBuilder();

        public PilotRecord(ShipRecord ship, Type type)
        {
            Ship = ship;

            Instance = (GenericShip) System.Activator.CreateInstance(type);
            Edition.Current.AdaptPilotToRules(Instance);
        }
    }
}
