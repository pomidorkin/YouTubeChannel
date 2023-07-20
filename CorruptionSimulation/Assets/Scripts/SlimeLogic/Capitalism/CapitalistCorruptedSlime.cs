using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalistCorruptedSlime : SlimeParent
{
    [SerializeField] CapitalistController capitalistController;
    private bool goingHunting = false;

    private void OnEnable()
    {
        if (capitalistController == null)
        {
            capitalistController = FindObjectOfType<CapitalistController>();
        }
        capitalistController.allSlimes.Add(this);
        capitalistController.HuntCommand += Hunt;
        capitalistController.EatCommand += Eat;
        capitalistController.ResetCommand += ResetSlime;
        capitalistController.AllReachedPositionCommand += AllSlimesReachedDestination;
        capitalistController.TryReproduceCommand += Reproduce;
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
        capitalistController.HuntCommand -= Hunt;
        capitalistController.EatCommand -= Eat;
        capitalistController.ResetCommand -= ResetSlime;
        capitalistController.TryReproduceCommand -= Reproduce;
        capitalistController.AllReachedPositionCommand -= AllSlimesReachedDestination;
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
        if (goingHunting && capitalistController.CheckIfAllReachedPosition(true))
        {
            capitalistController.ResetAllReachedPosition();
        }

    }

    private void DoHuntCalculations()
    {
        positionReached = true;
        Debug.Log("Max Res Reached (From Slime Script): " + capitalistController.resourceManager.MaxResReached());
        hasHunted = true;
    }

    private void Eat()
    {
        if (capitalistController.TakeFromCoffers(1))
        {
            hasEaten = true;
            Debug.Log("Eating 1 emerald, emerald amount = " + capitalistController.GetCoffersAmount());
            capitalistController.CheckIfAllHaveEaten();
        }
        else
        {
            //isAlive = false;
            Debug.Log("Run out of emeralds, die");
            capitalistController.CheckIfAllHaveEaten();
            Die();
        }
    }

    private void Reproduce()
    {
        if (capitalistController.TakeFromCoffers(1))
        {
            capitalistController.IncreaseSpawningAmount(false, true);
            Debug.Log("Eating 1 more emerald to reproduce, emerald amount = " + capitalistController.GetCoffersAmount());
        }
        else
        {
            Debug.Log("Not enough emeralds, no reproducing today");
        }
        triedReproduce = true;
        capitalistController.CheckIfAllHaveReproduced();
    }

    private void Die()
    {
        capitalistController.allSlimes.Remove(this);
        capitalistController.CheckIfAllHaveEaten();
        Destroy(gameObject);
    }
}
