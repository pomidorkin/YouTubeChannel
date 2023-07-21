using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForgivingController : MonoBehaviour
{
    /*public class DoorOpenedEventArgs : EventArgs
    {
        public Transform PositinToSpawnTheRoom { get; set; }
        public bool IsRightDoor { get; set; }
    }

    public delegate void DoorOpenedEvent(object source, DoorOpenedEventArgs args);
    public event DoorOpenedEvent OnDoorOpenedEvent;*/
    [SerializeField] GameObject kindSlimePrefab;
    [SerializeField] GameObject evilSlimePrefab;
    [SerializeField] GameObject egoistSlimePrefab;

    [SerializeField] PositionManager positionManager;

    public List<ForgivingParent> allSlimes;

    public class SlimeDieEventArgs : EventArgs
    {
        public ForgivingParent Slime { get; set; }
    }

    public delegate void SlimeDieAction(object source, SlimeDieEventArgs args);
    public event SlimeDieAction SlimeDied;

    public delegate void ResetAction();
    public event ResetAction ResetCommand;

    public delegate void NextStep();
    public event NextStep NextStepCommand;

    public delegate void ReproduceAction();
    public event ReproduceAction ReproduceCommand;

    [SerializeField] private int numberOfIterations = 20;
    private int iterationIndex = 0;
    [SerializeField] private int reproduceSequenceFrequency = 5;
    private int reproduceCounter = 0;
    [SerializeField] int maxPopulation = 50;

    private bool allProcessedStep = false;
    private bool allSlimesReachedPosition = false;

    private void Start()
    {
        NextStepCommand();
    }

    public void CheckIfAllProcessedStep()
    {
        allProcessedStep = true;
        foreach (ForgivingParent slime in allSlimes)
        {

            Debug.Log("Day " + iterationIndex + ", slime porecessed step " + slime.stepFinished);
            if (!slime.stepFinished)
            {
                allProcessedStep = false;
                break;
            }

        }
        if (allProcessedStep && (iterationIndex <= numberOfIterations) && (allSlimes.Count > 0) && allSlimesReachedPosition)
        {
            iterationIndex++;
            reproduceCounter++;
            if (reproduceCounter >= reproduceSequenceFrequency && (maxPopulation > allSlimes.Count))
            {
                reproduceCounter = 0;
                ReproduceCommand();
            }
            Debug.Log("Day " + iterationIndex);
            ResetCommand();
            NextStepCommand();
        }
    }

    public bool CheckIfAllReachedPosition()
    {
        allSlimesReachedPosition = true;
        foreach (ForgivingParent slime in allSlimes)
        {
            if (!slime.positionReached)
            {
                allSlimesReachedPosition = false;
                break;
            }
        }
        if (allSlimesReachedPosition)
        {
            //AllReachedPositionCommand();
        }

        return allSlimesReachedPosition;
    }

    public void SlimeDiedCommand(ForgivingParent slime)
    {
        Debug.Log("SlimeDied!");
        allSlimes.Remove(slime);
        if (allSlimes.Count > 0)
        {
            SlimeDied(this, new SlimeDieEventArgs { Slime = slime });
        }
    }

    public ForgivingParent AskRandomSlime()
    {
        bool availableSlimesLeft = false;
        foreach (ForgivingParent slime in allSlimes)
        {
            if (!slime.isBeingAskedForHelp && !slime.infected)
            {
                availableSlimesLeft = true;
                break;
            }
        }
        int i;
        do
        {
            i = UnityEngine.Random.Range(0, allSlimes.Count);
        } while (allSlimes[i].isBeingAskedForHelp && allSlimes[i].isBeingAskedForHelp);
        
        return allSlimes[i];
    }

    public void SpawnNewSlime(int val)
    {
        switch (val)
        {
            case 1:
                Instantiate(kindSlimePrefab, positionManager.GetRandomGeneralPositionInRange(), Quaternion.identity);
                break;
            case 2:
                Instantiate(evilSlimePrefab, positionManager.GetRandomGeneralPositionInRange(), Quaternion.identity);
                break;
            case 3:
                Instantiate(egoistSlimePrefab, positionManager.GetRandomGeneralPositionInRange(), Quaternion.identity);
                break;
        }
    }
}
