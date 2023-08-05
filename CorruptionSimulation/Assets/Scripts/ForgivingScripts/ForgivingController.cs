using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

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
    [SerializeField] PiePopulationChart chartController;
    [SerializeField] ChartController lineChartController;

    [SerializeField] TMP_Text dayText;

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
    private bool allTriedReproduce = false;

    public int newEvilSlimesAmount = 0;
    public int newEgoistSlimesAmount = 0;
    public int newKindlimesAmount = 0;

    public int maxResource = 15;
    public int currentResource;

    private bool newSlimeHasBeenSpawned = false;
    [SerializeField] bool reproductionAllowed = true;

    [SerializeField] float radius = 4f;

    private void Start()
    {
        currentResource = maxResource - allSlimes.Count;
        NextStepCommand();
        UpdateChart();
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
            dayText.text = "День " + iterationIndex;
            ReproduceCommand();
            Debug.Log("Day " + iterationIndex);
            //ResetCommand();
            //NextStepCommand();
        }
    }

    public void CheckIfAllHaveReproduced()
    {
        allTriedReproduce = true;
        foreach (ForgivingParent slime in allSlimes)
        {
            if (!slime.triedReproduce)
            {
                allTriedReproduce = false;
            }
        }
        Debug.Log("allTriedReproduce: " + allTriedReproduce + ", (iterationIndex < numberOfIterations): " + (iterationIndex < numberOfIterations));
        // Interation end
        if (allTriedReproduce && (iterationIndex < numberOfIterations))
        {
            allTriedReproduce = false;
            SpawnNewSlime();
            //UpdateChart();

            ResetCommand();
            if (!newSlimeHasBeenSpawned)
            {
                NextStepCommand();
            }
            else
            {
                // TODO: Код распределяющий новые позиции
                UpdateChart();
                NewPositionSetter();
            }
            //SendHuntCommand();
        }

    }

    private void UpdateChart()
    {
        int egoistSlimeCount = 0;
        int evilSlimeCount = 0;
        int kindSlimesCount = 0;
        currentResource = maxResource - allSlimes.Count;
        foreach (ForgivingParent item in allSlimes)
        {
            if (item.isEgoist)
            {
                egoistSlimeCount++;
            }
            else if (!item.isEgoist && item.isEvil)
            {
                evilSlimeCount++;
            }
            else
            {
                kindSlimesCount++;
            }
        }
        chartController.AddDataToChart(kindSlimesCount, egoistSlimeCount, evilSlimeCount);
        lineChartController.AddDataToChart(kindSlimesCount, egoistSlimeCount, evilSlimeCount, iterationIndex);
    }

    private void NewPositionSetter()
    {
        for (int i = 0; i < allSlimes.Count; i++)
        {
            float angle = i * Mathf.PI * 2 / allSlimes.Count;
            float x = Mathf.Cos(angle) * radius; // 3 is a radius
            float z = Mathf.Sin(angle) * radius; // 3 is a radius
            Vector3 pos = transform.position + new Vector3(x, 0, z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
            allSlimes[i].initialPos = pos;
            allSlimes[i].agent.SetDestination(pos);
            allSlimes[i].animator.SetTrigger("Jump");
            /* allSlimes[i].transform.position = pos;
             allSlimes[i].transform.rotation = rot;*/
        }
        newSlimeHasBeenSpawned = false;
    }

    public void IncreaseSpawningAmount(bool evil, bool kind)
    {
        if (evil)
        {
            newEvilSlimesAmount += 1;
        }
        else if (!evil && !kind)
        {
            newEgoistSlimesAmount += 1;
        }
        else
        {
            newKindlimesAmount += 1;
        }
    }

    private void SpawnNewSlime()
    {
        if (reproductionAllowed)
        {
            if (newEvilSlimesAmount > 0 || newEgoistSlimesAmount > 0 || newKindlimesAmount > 0)
            {
                newSlimeHasBeenSpawned = true;
                for (int i = 0; i < newEvilSlimesAmount; i++)
                {
                    Instantiate(evilSlimePrefab, positionManager.GetRandomGeneralPositionInRange(), Quaternion.identity);
                }
                for (int i = 0; i < newEgoistSlimesAmount; i++)
                {
                    Instantiate(egoistSlimePrefab, positionManager.GetRandomGeneralPositionInRange(), Quaternion.identity);
                }
                for (int i = 0; i < newKindlimesAmount; i++)
                {
                    Instantiate(kindSlimePrefab, positionManager.GetRandomGeneralPositionInRange(), Quaternion.identity);
                }
                newEgoistSlimesAmount = 0;
                newEvilSlimesAmount = 0;
                newKindlimesAmount = 0;
            }
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
        UpdateChart();
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
