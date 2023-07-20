using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionSlime : SlimeParent
{
    [SerializeField] AltruistController altruistController;

    private bool goingHunting = false;

    private void OnEnable()
    {
        if (altruistController == null)
        {
            altruistController = FindObjectOfType<AltruistController>();
        }
        altruistController.altruistSlimes.Add(this);
        altruistController.HuntCommand += Hunt;
        altruistController.EatCommand += Eat;
        altruistController.ResetCommand += ResetSlime;
        altruistController.AllReachedPositionCommand += AllSlimesReachedDestination;
        altruistController.TryReproduceCommand += Reproduce;
        altruist = false;
        currupted = true;
    }

    private void AllSlimesReachedDestination()
    {
        if (goingHunting)
        {
            goingHunting = false;
            DoHuntCalculations();
        }
    }

    private void ResetSlime()
    {
        hasEaten = false;
        hasHunted = false;
        triedReproduce = false;
        goingHunting = false;
        positionReached = false;
    }

    private void OnDisable()
    {
        altruistController.HuntCommand -= Hunt;
        altruistController.EatCommand -= Eat;
        altruistController.ResetCommand -= ResetSlime;
        altruistController.TryReproduceCommand -= Reproduce;
        altruistController.AllReachedPositionCommand -= AllSlimesReachedDestination;
    }

    private void Update()
    {
        /*if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if (!positionReached)
                    {
                        animator.SetTrigger("StopJumping");
                        positionReached = true;

                        if (goingHunting && altruistController.CheckIfAllReachedPosition(true))
                        {
                            altruistController.ResetAllReachedPosition();
                        }
                        else if (altruistController.CheckIfAllReachedPosition(false))
                        {
                            altruistController.ResetAllReachedPosition();
                            altruistController.CheckIfAllHunted();
                        }
                    }
                }
            }
        }*/
    }

    protected override void Hunt()
    {
        goingHunting = true;
        positionReached = true;
        if (goingHunting && altruistController.CheckIfAllReachedPosition(true))
        {
            altruistController.ResetAllReachedPosition();
        }

    }

    private void DoHuntCalculations()
    {
        positionReached = true;
        Debug.Log("Max Res Reached (From Slime Script): " + altruistController.resourceManager.MaxResReached());
        hasHunted = true;
    }

    private void Eat()
    {
        if (altruistController.TakeFromCoffers(1))
        {
            hasEaten = true;
            Debug.Log("Eating 1 emerald, emerald amount = " + altruistController.GetCoffersAmount());
            altruistController.CheckIfAllHaveEaten();
        }
        else
        {
            //isAlive = false;
            Debug.Log("Run out of emeralds, die");
            altruistController.CheckIfAllHaveEaten();
            Die();
        }
    }

    private void Reproduce()
    {
        if (altruistController.TakeFromCoffers(1))
        {
            altruistController.IncreaseSpawningAmount(false, true);
            Debug.Log("Eating 1 more emerald to reproduce, emerald amount = " + altruistController.GetCoffersAmount());
        }
        else
        {
            Debug.Log("Not enough emeralds, no reproducing today");
        }
        triedReproduce = true;
        altruistController.CheckIfAllHaveReproduced();
    }

    private void Die()
    {
        altruistController.altruistSlimes.Remove(this);
        altruistController.CheckIfAllHaveEaten();
        Destroy(gameObject);
    }
}
