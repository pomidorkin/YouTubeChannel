using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AltruistSlime : SlimeParent
{
    NavMeshAgent agent;
    [SerializeField] AltruistController altruistController;
    [SerializeField] GameObject emerald;
    //private int emeraldAmount = 0;
    /*public bool hasEaten = false;
    private bool isAlive = true;
    public bool hasHunted = false;
    public bool triedReproduce = false;*/

    // Navigation
    //public bool positionReached = false;
    private bool goingHunting = false;

    // Animation
    private Animator animator;
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
        altruistController.HuntCommand -= Hunt;
        altruistController.EatCommand -= Eat;
        altruistController.ResetCommand -= ResetSlime;
        altruistController.TryReproduceCommand -= Reproduce;
        altruistController.AllReachedPositionCommand -= AllSlimesReachedDestination;
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
        }
    }

    protected override void Hunt()
    {
        animator.SetTrigger("Jump");
        goingHunting = true;
        agent.SetDestination(altruistController.GetPositionManager().GetRandomForestPosition());

    }

    private void DoHuntCalculations()
    {
        positionReached = false;
        int rnd = Random.RandomRange(1, 101);
        Debug.Log("Max Res Reached (From Slime Script): " + altruistController.resourceManager.MaxResReached());
        if (rnd <= ResourceChanceController.ResourceChance && (altruistController.resourceManager.MaxResReached() == false))
        {
            if (rnd <= 10)
            {
                altruistController.AddToCoffers(3);
                altruistController.resourceManager.IncreaseResourceCounter(3);
                Debug.Log("Found 3 emeralds, emerald amount = " + altruistController.GetCoffersAmount());
            }
            else
            {
                altruistController.AddToCoffers(2);
                altruistController.resourceManager.IncreaseResourceCounter(2);
                Debug.Log("Found 2 emeralds, emerald amount = " + altruistController.GetCoffersAmount());
            }
            emerald.SetActive(true);
        }
        else
        {
            Debug.Log("Found 0 emeralds, emerald amount = " + altruistController.GetCoffersAmount());
        }
        hasHunted = true;
        animator.SetTrigger("Jump");
        agent.SetDestination(altruistController.GetPositionManager().GetRandomHomePosition(true));
    }

    private void Eat()
    {
        emerald.SetActive(false);
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
            altruistController.IncreaseSpawningAmount(true, false);
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
