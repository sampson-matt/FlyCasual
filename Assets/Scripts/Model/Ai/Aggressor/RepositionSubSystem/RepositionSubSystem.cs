using Actions;
using ActionsList;
using BoardTools;
using Movement;
using Obstacles;
using Players;
using Ship;
using SubPhases;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Aggressor
{
    // A boost or barrel roll the AI could perform, and how good the position it ends in is
    public class RepositionOption
    {
        public string BoostTemplateName { get; private set; }

        public ManeuverTemplate BarrelRollTemplate { get; private set; }
        public Direction BarrelRollDirection { get; private set; }
        public Direction BarrelRollShift { get; private set; }

        public ShipPositionInfo FinalPosition { get; private set; }

        public bool HasShot { get; set; }
        public int EnemiesInShotRange { get; set; }
        public int EnemiesTargetingThisShip { get; set; }
        public bool IsOffTheBoard { get; set; }
        // The template or the final position overlaps something that would make the action fail
        public bool IsObstructed { get; set; }

        public int Utility
        {
            get { return RepositionSubSystem.CalculateUtility(HasShot, EnemiesInShotRange, EnemiesTargetingThisShip); }
        }

        public static RepositionOption ForBoost(string templateName, ShipPositionInfo finalPosition)
        {
            return new RepositionOption()
            {
                BoostTemplateName = templateName,
                FinalPosition = finalPosition
            };
        }

        public static RepositionOption ForBarrelRoll(ManeuverTemplate template, Direction direction, Direction shift, ShipPositionInfo finalPosition)
        {
            return new RepositionOption()
            {
                BarrelRollTemplate = template,
                BarrelRollDirection = direction,
                BarrelRollShift = shift,
                FinalPosition = finalPosition
            };
        }
    }

    // Decides if a boost or barrel roll gets the ship a shot or gets it out of enemy firing arcs.
    // Options that would overlap a ship, an obstacle or leave the board are dropped here, so the AI doesn't
    // choose an action that would fail. The planning subphases still run the game's legality checks
    // on the options in order and fail the action if none of them is allowed.
    public static class RepositionSubSystem
    {
        private const int BasePriority = 40;
        private const int GainShotPriorityBonus = 20;
        private const int EscapeAllArcsPriorityBonus = 15;

        public static bool IsEnabledFor(GenericShip ship)
        {
            return ship.Owner is AggressorAiPlayer
                && Mods.ModsManager.Mods[typeof(Mods.ModsList.AIBoostTestModSE)].IsOn;
        }

        public static int CalculateUtility(bool hasShot, int enemiesInShotRange, int enemiesTargetingThisShip)
        {
            return ((hasShot) ? 30 : 0)
                + 5 * Mathf.Min(enemiesInShotRange, 2)
                - 20 * enemiesTargetingThisShip;
        }

        // Priority of the action and the options that improve the ship's position, best first
        public static int GetBoostPriority(GenericShip ship, GenericAction action, out List<RepositionOption> beneficialOptions)
        {
            return GetPriority(ship, GetBoostOptions(ship, action), out beneficialOptions);
        }

        public static int GetBarrelRollPriority(GenericShip ship, GenericAction action, out List<RepositionOption> beneficialOptions)
        {
            return GetPriority(ship, GetBarrelRollOptions(ship, action), out beneficialOptions);
        }

        // Options to try when the action was chosen without a plan (e.g. a free action from a card ability)
        public static List<RepositionOption> GetBoostOptionsForPlanning(GenericShip ship, GenericAction action)
        {
            return RankOptions(ship, GetBoostOptions(ship, action));
        }

        public static List<RepositionOption> GetBarrelRollOptionsForPlanning(GenericShip ship, GenericAction action)
        {
            return RankOptions(ship, GetBarrelRollOptions(ship, action));
        }

        private static int GetPriority(GenericShip ship, List<RepositionOption> options, out List<RepositionOption> beneficialOptions)
        {
            RepositionOption current = EvaluateCurrentPosition(ship);
            beneficialOptions = RankOptions(ship, options)
                .Where(n => n.Utility > current.Utility)
                .ToList();

            if (beneficialOptions.Count == 0) return 0;

            RepositionOption best = beneficialOptions.First();
            int priority = BasePriority;
            if (!current.HasShot && best.HasShot) priority += GainShotPriorityBonus;
            if (current.EnemiesTargetingThisShip > 0 && best.EnemiesTargetingThisShip == 0) priority += EscapeAllArcsPriorityBonus;

            return priority;
        }

        private static List<RepositionOption> GetBoostOptions(GenericShip ship, GenericAction action)
        {
            List<RepositionOption> options = new List<RepositionOption>();

            foreach (BoostMove move in ship.GetAvailableBoostTemplates(action))
            {
                // AI doesn't perform red actions, see AggressorAiPlayer.PerformActionFromList
                if (move.IsRed || move.IsPurple) continue;

                Transform boostHelper = ship.GetBoosterHelper().Find(move.Name);
                Transform finalBase = boostHelper?.Find("Finisher/BasePosition");
                if (finalBase == null) continue;

                RepositionOption option = RepositionOption.ForBoost(
                    move.Name,
                    new ShipPositionInfo(new Vector3(finalBase.position.x, 0, finalBase.position.z), finalBase.eulerAngles)
                );
                option.IsObstructed = IsTemplateOverObstacle(ship, boostHelper.gameObject);
                options.Add(option);
            }

            return options;
        }

        private static List<RepositionOption> GetBarrelRollOptions(GenericShip ship, GenericAction action)
        {
            List<RepositionOption> options = new List<RepositionOption>();

            List<Direction> directions = new List<Direction>() { Direction.Left, Direction.Right };
            // Center first, so it is preferred when positions are equally good
            List<Direction> shifts = new List<Direction>() { Direction.None, Direction.Top, Direction.Bottom };

            foreach (ManeuverTemplate template in ship.GetAvailableBarrelRollTemplates(action).Where(n => n.Bearing == ManeuverBearing.Straight))
            {
                foreach (Direction direction in directions)
                {
                    // Separate instance, so the template objects shared by the ship base are not touched
                    ManeuverTemplate probeTemplate = new ManeuverTemplate(template.Bearing, template.Direction, template.Speed, isSideTemplate: template.IsSideTemplate);
                    probeTemplate.ApplyTemplate(ship, (direction == Direction.Left) ? ship.GetLeft() : ship.GetRight(), direction);
                    bool isTemplateObstructed = IsTemplateOverObstacle(ship, probeTemplate.Collider.gameObject);

                    foreach (Direction shift in shifts)
                    {
                        RepositionOption option = RepositionOption.ForBarrelRoll(
                            template,
                            direction,
                            shift,
                            BarrelRollPlanningSubPhase.GetFinalPositionInfo(ship, probeTemplate, direction, shift)
                        );
                        option.IsObstructed = isTemplateObstructed;
                        options.Add(option);
                    }

                    probeTemplate.DestroyTemplateImmediately();
                }
            }

            return options;
        }

        // Scores every option and drops the ones that would leave the board, best first
        private static List<RepositionOption> RankOptions(GenericShip ship, List<RepositionOption> options)
        {
            ShipPositionInfo realPosition = ship.GetPositionInfo();

            try
            {
                foreach (RepositionOption option in options)
                {
                    // The ship is moved and returned within one frame, so this is never rendered
                    ship.SetPositionInfo(option.FinalPosition);
                    Physics.SyncTransforms();

                    option.IsOffTheBoard = Board.IsOffTheBoard(ship);
                    if (option.IsOffTheBoard || option.IsObstructed) continue;

                    option.IsObstructed = IsBaseOverAnotherShip(ship) || IsBaseOverObstacle(ship);
                    if (!option.IsObstructed) EvaluatePosition(ship, option);
                }
            }
            finally
            {
                ship.SetPositionInfo(realPosition);
                Physics.SyncTransforms();
            }

            // OrderByDescending is stable, so equally good options keep their preferred order
            return options
                .Where(n => !n.IsOffTheBoard && !n.IsObstructed)
                .OrderByDescending(n => n.Utility)
                .ToList();
        }

        private static bool IsBaseOverAnotherShip(GenericShip ship)
        {
            return IsOverlappingAny(
                ship.Collider,
                Roster.AllShips.Values.Where(n => n != ship).Select(n => n.Collider as Collider)
            );
        }

        private static bool IsBaseOverObstacle(GenericShip ship)
        {
            return !ship.IsIgnoreObstacles && IsOverlappingAny(ship.Collider, GetObstacleColliders());
        }

        // Ships that can move through obstacles during a boost or barrel roll still hit them, so the AI avoids those too
        private static bool IsTemplateOverObstacle(GenericShip ship, GameObject template)
        {
            if (ship.IsIgnoreObstacles) return false;

            // Planning templates are hidden while not in use
            bool wasActive = template.activeSelf;
            template.SetActive(true);
            try
            {
                Collider templateCollider = template.GetComponentInChildren<ObstaclesStayDetectorForced>()?.GetComponent<Collider>();
                return IsOverlappingAny(templateCollider, GetObstacleColliders());
            }
            finally
            {
                template.SetActive(wasActive);
            }
        }

        private static IEnumerable<Collider> GetObstacleColliders()
        {
            return ObstaclesManager.GetPlacedObstacles().Select(n => n.Collider as Collider);
        }

        // Checked immediately, unlike the trigger based detectors used by the planning subphases
        private static bool IsOverlappingAny(Collider collider, IEnumerable<Collider> others)
        {
            if (collider == null) return false;

            foreach (Collider other in others)
            {
                if (other == null || other == collider) continue;

                if (Physics.ComputePenetration(
                    collider, collider.transform.position, collider.transform.rotation,
                    other, other.transform.position, other.transform.rotation,
                    out Vector3 direction, out float distance
                )) return true;
            }

            return false;
        }

        private static RepositionOption EvaluateCurrentPosition(GenericShip ship)
        {
            RepositionOption current = RepositionOption.ForBoost(null, ship.GetPositionInfo());
            EvaluatePosition(ship, current);
            return current;
        }

        private static void EvaluatePosition(GenericShip ship, RepositionOption option)
        {
            option.EnemiesInShotRange = ship.Owner.EnemyShips.Values.Count(enemy =>
                ship.GetAllWeapons().Any(weapon => new ShotInfo(ship, enemy, weapon).IsShotAvailable)
            );
            option.HasShot = option.EnemiesInShotRange > 0;
            option.EnemiesTargetingThisShip = ActionsHolder.CountEnemiesTargeting(ship);
        }
    }
}
