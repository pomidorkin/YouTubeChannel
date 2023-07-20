using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IndividualistSlime : SlimeParent
{
    NavMeshAgent agent;
    [SerializeField] CapitalistController capitalistController;
    [SerializeField] GameObject emerald;
    private int emeraldAmount = 0;
    private bool goingHunting = false;
    private Animator animator;

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
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        altruist = true;
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
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if (!positionReached)
                    {
                        animator.SetTrigger("StopJumping");
                        positionReached = true;

                        if (goingHunting && capitalistController.CheckIfAllReachedPosition(true))
                        {
                            capitalistController.ResetAllReachedPosition();
                        }
                        else if (capitalistController.CheckIfAllReachedPosition(false))
                        {
                            capitalistController.ResetAllReachedPosition();
                            capitalistController.CheckIfAllHunted();
                        }
                    }
                }
            }
        }
    }

    protected override void Hunt()
    {
        animator.SetTrigger("Jump");
        goingHunting = true;
        agent.SetDestination(capitalistController.GetPositionManager().GetRandomForestPosition());

    }

    private void DoHuntCalculations()
    {
        positionReached = false;
        int rnd = Random.RandomRange(1, 101);
        Debug.Log("Max Res Reached (From Slime Script): " + capitalistController.resourceManager.MaxResReached());
        if (rnd <= ResourceChanceController.ResourceChance && (capitalistController.resourceManager.MaxResReached() == false))
        {
            if (rnd <= 10)
            {
                emeraldAmount += 2;
                capitalistController.AddToCoffers(1);
                capitalistController.resourceManager.IncreaseResourceCounter(3);
                Debug.Log("Found 3 emeralds, emerald amount = " + capitalistController.GetCoffersAmount());
            }
            else
            {
                emeraldAmount += 2;
                capitalistController.resourceManager.IncreaseResourceCounter(2);
                Debug.Log("Found 2 emeralds, emerald amount = " + capitalistController.GetCoffersAmount());
            }
            emerald.SetActive(true);
        }
        else
        {
            Debug.Log("Found 0 emeralds, emerald amount = " + capitalistController.GetCoffersAmount());
        }
        hasHunted = true;
        animator.SetTrigger("Jump");
        agent.SetDestination(capitalistController.GetPositionManager().GetRandomHomePosition(false));
    }

    private void Eat()
    {
        emerald.SetActive(false);
        if ((emeraldAmount - 1) > 0)
        {
            hasEaten = true;
            emeraldAmount--;
            capitalistController.CheckIfAllHaveEaten();
        } else if (capitalistController.TakeFromCoffers(1))
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
        if ((emeraldAmount - 1) > 0)
        {
            capitalistController.IncreaseSpawningAmount(true, false);
        }
        else if (capitalistController.TakeFromCoffers(1))
        {
            capitalistController.IncreaseSpawningAmount(true, false);
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
