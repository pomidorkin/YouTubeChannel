using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalistController : MonoBehaviour
{

    [SerializeField] GameObject individualistSlimePrefab;
    [SerializeField] GameObject egoistSlimePrefab;
    [SerializeField] GameObject corruptedSlimePrefab;
    [SerializeField] PositionManager positionManager;
    [SerializeField] CapitalistChartController chartController;

    [SerializeField] public CapitalistResourceManager resourceManager;

    public List<SlimeParent> allSlimes;
    private bool allHunted = false;
    private bool allAte = false;
    private bool allTriedReproduce = false;
    private bool allSlimesReachedPosition = false;
    private int coffers = 0;
    public int newIndividualistSlimesAmount = 0;
    public int newEgoistSlimesAmount = 0;
    public int newCorruptedSlimesAmount = 0;

    [SerializeField] private int numberOfIterations = 20;
    private int iterationIndex = 0;

    public delegate void HuntAction();
    public event HuntAction HuntCommand;

    public delegate void EatAction();
    public event EatAction EatCommand;

    public delegate void ResetAction();
    public event ResetAction ResetCommand;

    public delegate void TryReproduceAction();
    public event TryReproduceAction TryReproduceCommand;

    public delegate void AllReachedPositionAction();
    public event AllReachedPositionAction AllReachedPositionCommand;

    private void Start()
    {
        SendHuntCommand();
    }

    public void CheckIfAllHunted()
    {
        allHunted = true;
        foreach (SlimeParent slime in allSlimes)
        {
            if (!slime.hasHunted)
            {
                allHunted = false;
            }
        }
        if (allHunted)
        {
            Debug.Log("Sending an eat command");
            EatCommand();
        }
    }

    public void CheckIfAllHaveEaten()
    {
        allAte = true;
        foreach (SlimeParent slime in allSlimes)
        {
            if (!slime.hasEaten)
            {
                allAte = false;
            }
        }
        if (allAte)
        {
            TryReproduceCommand();
        }
    }

    public bool CheckIfAllReachedPosition(bool isGoingForHumt)
    {
        allSlimesReachedPosition = true;
        foreach (SlimeParent slime in allSlimes)
        {
            if (!slime.positionReached)
            {
                allSlimesReachedPosition = false;
            }
        }
        if (allSlimesReachedPosition && isGoingForHumt)
        {
            AllReachedPositionCommand();
        }

        return allSlimesReachedPosition;
    }

    public void ResetAllReachedPosition()
    {
        allSlimesReachedPosition = false;
    }

    public void CheckIfAllHaveReproduced()
    {
        allTriedReproduce = true;
        foreach (SlimeParent slime in allSlimes)
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

            allAte = false;
            allHunted = false;
            allTriedReproduce = false;
            SpawnNewSlime();

            int altruistSlimeCount = 0;
            int egoistSlimes = 0;
            int corruptedSlimes = 0;
            foreach (SlimeParent item in allSlimes)
            {
                if (item.altruist)
                {
                    altruistSlimeCount++;
                }
                else if (!item.altruist && !item.currupted)
                {
                    egoistSlimes++;
                }
                else
                {
                    corruptedSlimes++;
                }
            }
            chartController.AddDataToChart(altruistSlimeCount, egoistSlimes, corruptedSlimes);

            ResetCommand();
            SendHuntCommand();
        }

    }

    public void IncreaseSpawningAmount(bool altruist, bool corrupted)
    {
        if (altruist)
        {
            newIndividualistSlimesAmount += 1;
        }
        else if (!altruist && !corrupted)
        {
            newEgoistSlimesAmount += 1;
        }
        else
        {
            newCorruptedSlimesAmount += 1;
        }
    }

    private void SpawnNewSlime()
    {
        if (newIndividualistSlimesAmount > 0 || newEgoistSlimesAmount > 0)
        {
            for (int i = 0; i < newIndividualistSlimesAmount; i++)
            {
                Instantiate(individualistSlimePrefab, positionManager.GetRandomSpawnPosition(false), Quaternion.identity);
            }
            for (int i = 0; i < newEgoistSlimesAmount; i++)
            {
                Instantiate(egoistSlimePrefab, positionManager.GetRandomSpawnPosition(false), Quaternion.identity);
            }
            for (int i = 0; i < newCorruptedSlimesAmount; i++)
            {
                Instantiate(corruptedSlimePrefab, positionManager.GetRandomSpawnPosition(false), Quaternion.identity);
            }
            newEgoistSlimesAmount = 0;
            newIndividualistSlimesAmount = 0;
            newCorruptedSlimesAmount = 0;
        }
    }

    public int GetCoffersAmount()
    {
        return coffers;
    }

    public void AddToCoffers(int amount)
    {
        if (!resourceManager.MaxResReached())
        {
            coffers += amount;
        }
    }

    public bool TakeFromCoffers(int amount)
    {
        if ((coffers - amount) >= 0)
        {
            coffers -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SendHuntCommand()
    {
        iterationIndex += 1;
        Debug.Log("iterationIndex: " + iterationIndex);
        HuntCommand();
    }

    public PositionManager GetPositionManager()
    {
        return positionManager;
    }
}
