using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ship;
using Obstacles;
using Remote;

public class ObstaclesStayDetectorForced: MonoBehaviour {

    public bool checkCollisionsNow = false;

    public bool OverlapsShipNow
    {
        get { return OverlappedShipsNow.Count > 0; }
    }

    public bool OverlapsAsteroidNow
    {
        get { return OverlappedAsteroidsNow.Count > 0; }
    }

    public List<GenericShip> OverlappedShipsNow = new List<GenericShip>();
    public List<GenericRemote> OverlapedRemotesNow = new List<GenericRemote>();
    public bool OffTheBoardNow = false;
    public List<Collider> OverlapedMinesNow = new List<Collider>();
    public List<GenericObstacle> OverlappedAsteroidsNow = new List<GenericObstacle>();
    public bool OverlapsCurrentShipNow { get; private set; }

    private GenericShip theShip; 
    public GenericShip TheShip {
        get {
            return theShip ?? Selection.ThisShip;
        }
        set {
            theShip = value;
        }
    }

    public void ReCheckCollisionsStart()
    {
        OverlappedShipsNow = new List<GenericShip>();
        OverlapedRemotesNow = new List<GenericRemote>();
        OffTheBoardNow = false;
        OverlapedMinesNow = new List<Collider>();
        OverlappedAsteroidsNow = new List<GenericObstacle> ();
        OverlapsCurrentShipNow = false;

        checkCollisionsNow = true;
    }

    public void ReCheckCollisionsFinish()
    {
        checkCollisionsNow = false;
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (checkCollisionsNow)
        {
            if (collisionInfo.tag == "Obstacle")
            {
                GenericObstacle obstacle = ObstaclesManager.GetChosenObstacle(collisionInfo.transform.name);
                if (!OverlappedAsteroidsNow.Contains(obstacle)) OverlappedAsteroidsNow.Add(obstacle);
            }
            else if (collisionInfo.tag == "Mine")
            {
                if (!OverlapedMinesNow.Contains(collisionInfo)) OverlapedMinesNow.Add(collisionInfo);
            }
            else if (collisionInfo.name.StartsWith("OffTheBoard"))
            {
                OffTheBoardNow = true;
            }
            else if (collisionInfo.name == "ObstaclesStayDetector")
            {
                if (collisionInfo.tag != TheShip.GetTag())
                {
                    GenericShip ship = Roster.GetShipById(collisionInfo.tag);
                    if (ship != null && !OverlappedShipsNow.Contains(ship)) OverlappedShipsNow.Add(ship);
                }
                else if (collisionInfo.tag == TheShip.GetTag())
                {
                    OverlapsCurrentShipNow = true;
                }
            }
            else if (collisionInfo.name == "RemoteCollider")
            {
                if (collisionInfo.tag != this.tag)
                {
                    if (!OverlapedRemotesNow.Contains(Roster.GetShipById(collisionInfo.tag) as GenericRemote))
                    {
                        OverlapedRemotesNow.Add(Roster.GetShipById(collisionInfo.tag) as GenericRemote);
                    }
                }
            }
        }
    }

}
