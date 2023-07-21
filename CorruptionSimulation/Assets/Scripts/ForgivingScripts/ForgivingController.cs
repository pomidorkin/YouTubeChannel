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

    [SerializeField] private int numberOfIterations = 20;
    private int iterationIndex = 0;

    private bool allProcessedStep = false;

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
        if (allProcessedStep && (iterationIndex <= numberOfIterations) && (allSlimes.Count > 0))
        {
            iterationIndex++;
            Debug.Log("Day " + iterationIndex);
            ResetCommand();
            NextStepCommand();
        }
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
        int i = UnityEngine.Random.Range(0, allSlimes.Count);
        return allSlimes[i];
    }
}
