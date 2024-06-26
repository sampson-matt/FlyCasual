﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrade;
using Ship;
using System.Linq;
using SubPhases;
using Bombs;
using SubPhases.FirstEdition;

namespace UpgradesList.FirstEdition
{
    public class ClusterMines : GenericContactMineFE
    {
        public ClusterMines() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Cluster Mines",
                UpgradeType.Device,
                cost: 4
            );

            bombPrefabPath = "Prefabs/Bombs/ClusterMinesCentral";

            bombSidePrefabPath = "Prefabs/Bombs/ClusterMinesSide";
            bombSideDistanceX = 4.05f;
            bombSideDistanceZ = 0.1264f;

            IsDiscardedAfterDropped = true;
        }

        public override void ExplosionEffect(GenericShip ship, Action callBack)
        {
            Selection.ActiveShip = ship;
            Phases.StartTemporarySubPhaseOld(
                "Damage from " + UpgradeInfo.Name,
                typeof(ClusterMinesCheckSubPhase),
                delegate {
                    Phases.FinishSubPhase(typeof(ClusterMinesCheckSubPhase));
                    callBack();
                });
        }

        public override void PlayDetonationAnimSound(GenericDeviceGameObject bombObject, Action callBack)
        {
            int random = UnityEngine.Random.Range(1, 8);
            Sounds.PlayBombSound(bombObject, "Explosion-" + random);
            bombObject.transform.Find("Explosion/Explosion").GetComponent<ParticleSystem>().Play();
            bombObject.transform.Find("Explosion/Ring").GetComponent<ParticleSystem>().Play();

            GameManagerScript.Wait(1, delegate { callBack(); });
        }

    }

}

namespace SubPhases.FirstEdition
{

    public class ClusterMinesCheckSubPhase : DiceRollCheckSubPhase
    {

        public override void Prepare()
        {
            DiceKind = DiceKind.Attack;
            DiceCount = 2;

            AfterRoll = FinishAction;
        }

        protected override void FinishAction()
        {
            HideDiceResultMenu();

            CurrentDiceRoll.RemoveAllFailures();
            if (!CurrentDiceRoll.IsEmpty)
            {
                foreach (Die die in CurrentDiceRoll.DiceList)
                {
                    if (die.Side == DieSide.Crit) die.TrySetSide(DieSide.Success);
                }

                SufferDamage();
            }
            else
            {
                NoDamage();
            }

        }

        private void SufferDamage()
        {
            Messages.ShowInfo("Cluster Mines: The attacked ship suffered damage");

            DamageSourceEventArgs clustermineDamage = new DamageSourceEventArgs()
            {
                Source = "Cluster Mines",
                DamageType = DamageTypes.BombDetonation
            };

            Selection.ActiveShip.Damage.TryResolveDamage(CurrentDiceRoll.DiceList, clustermineDamage, CallBack);
        }

        private void NoDamage()
        {
            Messages.ShowInfoToHuman("The attacked ship suffered no damage");
            CallBack();
        }
    }

}