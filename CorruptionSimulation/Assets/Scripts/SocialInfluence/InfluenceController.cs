using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceController : MonoBehaviour
{
    public List <InfluenceSlime> slimes;

    public delegate void ChangeOpinionAction();
    public event ChangeOpinionAction ChangeOpinionCommand;

    //public delegate void InteractAction();
    //public event InteractAction InteractCommand;

    private bool allChangedOpinion = false;

    private void Start()
    {
        ChangeOpinionCommand();
    }

    public void CheckIfAllProcessedStep()
    {
        allChangedOpinion = true;
        foreach (InfluenceSlime slime in slimes)
        {
            if (!slime.opinionReviewed)
            {
                allChangedOpinion = false;
                break;
            }

        }
        /*if (allChangedOpinion && (iterationIndex <= numberOfIterations) && (allSlimes.Count > 0) && allSlimesReachedPosition)
        {
            iterationIndex++;
            dayText.text = "Δενό " + iterationIndex;
            ReproduceCommand();
            Debug.Log("Day " + iterationIndex);
        }*/
    }
}
