﻿using ActionsList;
using Arcs;
using Movement;
using Ship;
using System;
using System.Collections.Generic;
using Tokens;
using UnityEngine;
using Upgrade;
using Editions.RuleSets;
using Obstacles;

namespace Editions
{
    public abstract class Edition
    {
        public static Edition Current { get; set; }

        public abstract string Name { get; }
        public abstract string NameShort { get; }
        public RuleSet RuleSet { get; set; }
        public abstract int MaxPoints { get; }
        public abstract int MaxShipsCount { get; }
        public abstract int MinShipsCount { get; }
        public abstract string CombatPhaseName { get; }
        public abstract Color MovementEasyColor { get; }
        public abstract bool CanAttackBumpedTarget { get; }
        public abstract MovementComplexity IonManeuverComplexity { get; }
        public abstract Dictionary<Type, int> DamageDeckContent { get; }
        public abstract Dictionary<BaseSize, int> NegativeTokensToAffectShip { get; }
        public abstract Dictionary<string, string> PreGeneratedAiSquadrons { get; }
        public abstract string PathToSavedSquadrons { get; }
        public abstract string PathToCampaignSquadrons { get; }
        public abstract string PathToCampaignSetup { get; }
        public abstract string PathToElitePilotUpgrades { get; }
        public abstract string RootUrlForImages { get; }
        public abstract Vector2 UpgradeCardSize { get; }
        public abstract Vector2 UpgradeCardCompactOffset { get; }
        public abstract Vector2 UpgradeCardCompactSize { get; }

        public virtual bool IsSquadBuilderLocked { get { return false; } }

        public abstract int MinShipCost(Faction faction);

        public virtual void ActionIsFailed(GenericShip ship, GenericAction action, bool overWrittenInstead = false, bool hasSecondChance = false)
        {
            if (!overWrittenInstead && !hasSecondChance)
            {
                ship.RemoveAlreadyExecutedAction(action);
                ActionsHolder.CurrentAction = null;
            }
            action.RevertActionOnFail(hasSecondChance);
        }

        public Edition()
        {
            Current = this;
        }

        public abstract void EvadeDiceModification(DiceRoll diceRoll);
        public abstract bool IsWeaponHaveRangeBonus(IShipWeapon weapon);
        public abstract void SetShipBaseImage(GenericShip ship);
        public abstract void BarrelRollTemplatePlanning();
        public abstract void DecloakTemplatePlanning();
        public abstract bool DefenderIsReinforcedAgainstAttacker(ArcFacing facing, GenericShip defender, GenericShip attacker);
        public abstract bool ReinforceEffectCanBeUsed(ArcFacing facing);
        public abstract bool ReinforcePostCombatEffectCanBeUsed(ArcFacing facing);
        public abstract void TimedBombActivationTime(GenericShip ship);
        public abstract void SquadBuilderIsOpened();
        public abstract bool IsTokenCanBeDiscardedByJam(GenericToken token);
        public abstract string GetPilotImageUrl(GenericShip ship, string filename);
        public abstract string GetUpgradeImageUrl(GenericUpgrade upgrade, string filename = null);

        public virtual void AdaptShipToRules(GenericShip ship) { }
        public virtual void AdaptPilotToRules(GenericShip ship) { }
        public virtual void AdaptUpgradeToRules(GenericUpgrade upgrade) { }
        public virtual void AdaptArcsToRules(GenericShip ship) { }
        public virtual void RotateMobileFiringArc(GenericArc arc, ArcFacing facing) { }
        public virtual void RotateMobileFiringArcAlt(GenericArc arc, ArcFacing facing) { }
        public virtual void SubScribeToGenericShipEvents(GenericShip ship) { }
        public virtual void WhenIonized(GenericShip ship) { }

        public abstract string FactionToXws(Faction faction);
        public abstract Faction XwsToFaction(string factionXWS);

        public abstract string UpgradeTypeToXws(UpgradeType faction);
        public abstract UpgradeType XwsToUpgradeType(string upgradeXWS);
    }
}