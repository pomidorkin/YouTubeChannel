using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForgivingParent : MonoBehaviour
{
    [SerializeField] ForgivingController forgivingController;
    [SerializeField] public Animator animator;
    [SerializeField] GameObject mushroom;
    private Dictionary<ForgivingParent, bool> memorizedSlimes; // false == no help
    public bool infected = false;
    [SerializeField] private int enegry = 6;
    [SerializeField] int maxEnergy = 6;
    [SerializeField] int energyToReproduce = 3;
    public int infectionChance = 10;
    [SerializeField] public bool isEvil = false;
    [SerializeField] public bool isEgoist = false;
    public bool stepFinished = false;
    public bool isBeingAskedForHelp = false;

    private ForgivingParent slimeHelper;

    public NavMeshAgent agent;
    public bool positionReached = false;

    public Vector3 initialPos;
    private bool goingForHelp = false;

    public bool triedReproduce;

    private void OnEnable()
    {
        forgivingController = FindObjectOfType<ForgivingController>();
        forgivingController.allSlimes.Add(this);
        memorizedSlimes = new Dictionary<ForgivingParent, bool>();
        agent = GetComponent<NavMeshAgent>();
        enegry = maxEnergy;
        initialPos = transform.position;

        forgivingController.ResetCommand += ResetSlime;
        forgivingController.SlimeDied += CheckIfMemorizedDied;
        forgivingController.NextStepCommand += ProcessNextStep;
        forgivingController.ReproduceCommand += Reproduce;

    }

    private void OnDisable()
    {
        forgivingController.ResetCommand -= ResetSlime;
        forgivingController.SlimeDied -= CheckIfMemorizedDied;
        forgivingController.NextStepCommand -= ProcessNextStep;
        forgivingController.ReproduceCommand -= Reproduce;
    }

    private void Reproduce()
    {
        // Conditions for reproducing
        if ((enegry - energyToReproduce) > 0 && !infected && forgivingController.currentResource > 0)
        {
            DecreaseEnergy(energyToReproduce);
            forgivingController.currentResource--;
            if (isEvil)
            {
                forgivingController.IncreaseSpawningAmount(true, false);
            }
            else if (isEgoist)
            {
                forgivingController.IncreaseSpawningAmount(false, false);
            }
            else
            {
                forgivingController.IncreaseSpawningAmount(false, true);
            }
        }
        triedReproduce = true;
        forgivingController.CheckIfAllHaveReproduced();
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
                        //animator.SetTrigger("StopJumping");
                        if (goingForHelp)
                        {
                            AskForHelp(slimeHelper);
                            if (agent.isActiveAndEnabled)
                            {
                                goingForHelp = false;
                                agent.SetDestination(initialPos);
                                agent.stoppingDistance = 0;
                                //animator.SetTrigger("Jump");
                            }
                            else
                            {
                                forgivingController.CheckIfAllReachedPosition();
                                forgivingController.CheckIfAllProcessedStep();
                            }
                            
                        }
                        else
                        {
                            stepFinished = true;
                            positionReached = true;
                            animator.SetTrigger("StopJumping");
                            forgivingController.CheckIfAllReachedPosition();
                            forgivingController.CheckIfAllProcessedStep();
                        }
                    }
                }
            }
        }
    }

    private void ResetSlime()
    {
        if (enegry < maxEnergy)
        {
            enegry++;
        }
        isBeingAskedForHelp = false;
        positionReached = false;
        stepFinished = false;
        triedReproduce = false;
    }

    private void CheckIfMemorizedDied(object source, ForgivingController.SlimeDieEventArgs args)
    {
        if (memorizedSlimes.ContainsKey(args.Slime))
        {
            memorizedSlimes.Remove(args.Slime);
        }
    }

    private void ProcessNextStep()
    {
        //stepFinished = false;
        if (!infected)
        {
            int rnd = UnityEngine.Random.Range(1, 101);
            if (rnd <= infectionChance)
            {
                infected = true;
                mushroom.SetActive(true);
                //Debug.Log("I AM INFECTED!" + gameObject.name + "Infection random number = " + rnd);
                //AskForHelp();
            }
            stepFinished = true;
            positionReached = true;
            forgivingController.CheckIfAllReachedPosition();
            forgivingController.CheckIfAllProcessedStep();
        } else if (infected)
        {
            if (forgivingController.allSlimes.Count > 1)
            {
                if (isEvil)
                {
                    slimeHelper = forgivingController.AskRandomSlime();
                    if (memorizedSlimes.ContainsKey(slimeHelper))
                    {
                        bool isGood;
                        memorizedSlimes.TryGetValue(slimeHelper, out isGood);
                        while (memorizedSlimes.ContainsKey(slimeHelper) && !isGood)
                        {
                            slimeHelper = forgivingController.AskRandomSlime();
                            memorizedSlimes.TryGetValue(slimeHelper, out isGood);
                        }
                    }
                    goingForHelp = true;
                    animator.SetTrigger("Jump");
                    agent.stoppingDistance = 2;
                    agent.SetDestination(slimeHelper.transform.position);
                }
                else
                {
                    slimeHelper = forgivingController.AskRandomSlime();
                    goingForHelp = true;
                    animator.SetTrigger("Jump");
                    agent.stoppingDistance = 2;
                    agent.SetDestination(slimeHelper.transform.position);
                }
                //AskForHelp();
            }
        }
    }

    private void AskForHelp(ForgivingParent slimeHelper)
    {
        Debug.Log("Asking for help!" + gameObject.name);
        DecreaseEnergy(energyToReproduce);
        //ForgivingParent slimeHelper = forgivingController.AskRandomSlime();
        bool helpReply = slimeHelper.HelpReply(this);
        if (helpReply)
        {
            mushroom.SetActive(false);
            infected = false;
            //enegry = maxEnergy; // “ут нужно определить и высчитать, что происходит тогда, когда мы лечим. ¬с€ энерги€ возвращаетс€, нисколько не возвращаетс€ или чуть-чуть возвращаетс€
            enegry += 2;
            //Debug.Log("Help Received! " + gameObject.name + ", The helper is " + slimeHelper.name);
            if (isEvil && !memorizedSlimes.ContainsKey(slimeHelper))
            {
                memorizedSlimes.Add(slimeHelper, true);
            }
        }
        if (!helpReply && isEvil && !memorizedSlimes.ContainsKey(slimeHelper))
        {
            memorizedSlimes.Add(slimeHelper, false);
        }
    }

    public bool HelpReply(ForgivingParent helplessSlime)
    {
        isBeingAskedForHelp = true;
        if ((enegry - energyToReproduce) > 0)
        {
            if (isEvil)
            {
                if (memorizedSlimes.ContainsKey(helplessSlime))
                {
                    // If val == true, it means that the slime has helped in the past
                    bool val;
                    memorizedSlimes.TryGetValue(helplessSlime, out val);
                    if (val)
                    {
                        //Debug.Log("I am zlopamyatniy and I am helping! " + gameObject.name);
                        DecreaseEnergy(energyToReproduce);
                        return true;
                    }
                    else
                    {
                        //Debug.Log("I am zlopamyatniy and I am NOT helping! " + gameObject.name);
                        return false;
                    }
                }
                else
                {
                    //Debug.Log("I am zlopamyatniy and I am helping! " + gameObject.name);
                    DecreaseEnergy(energyToReproduce);
                    return true;
                }
            }
            else if (isEgoist)
            {
                //Debug.Log("I am an egoist and I am NOT helping! " + gameObject.name); // “ут баг какой-то: MissingReferenceException:
                // The object of type 'ForgivingParent' has
                // been destroyed but you are still trying to access it.
                // Your script should either check if it is null or you
                // should not destroy the object.
                return false;
            }
            else
            {
                //Debug.Log("I am a good guy and I am helping! " + gameObject.name); // And here
                DecreaseEnergy(energyToReproduce);
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    private void DecreaseEnergy(int energyAmounr)
    {
        if ((enegry - energyAmounr) > 0)
        {
            enegry -= energyAmounr;
        }
        else
        {
            if (gameObject) // Null pointer Exception here sometimes
            {
                forgivingController.SlimeDiedCommand(this);
                Destroy(gameObject);
            }
        }
    }
}
