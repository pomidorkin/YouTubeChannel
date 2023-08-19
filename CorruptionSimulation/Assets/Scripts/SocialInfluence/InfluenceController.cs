using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfluenceController : MonoBehaviour
{
    public List <InfluenceSlime> slimes;
    public List<float> greenSlimesThreshold;
    public List<float> redSlimesThreshold;

    public delegate void ChangeOpinionAction();
    public event ChangeOpinionAction ChangeOpinionCommand;

    public delegate void InteractAction();
    public event InteractAction InteractCommand;

    public delegate void ResetAction();
    public event ResetAction ResetCommand;

    [SerializeField] bool allowedToInteract = true;
    private bool allSlimesReachedPosition;

    [SerializeField] public bool generalThreshold = false;
    [SerializeField] public int threshold = 15;
    [SerializeField] public int minThreshold = 0;
    [SerializeField] [Range(0, 100)] public float maxThreshold = 100;
    [SerializeField] private int numberOfIterations = 20;
    [SerializeField] public float maxDistance = 40f;
    private int iterationIndex = 0;

    private bool allChangedOpinion = false;


    float totalGreenThreshold;
    float totalRedThreshold;
    private float avgGreenThreshold = 0;
    private float avgRedThreshold = 0;

    // Charts
    //[SerializeField] PieOpinionChart chartController;
    [SerializeField] PopulationThresholdChart populationChart;
    [SerializeField] AvgThresholdChart avgThresholdChart;

    //[SerializeField] TMP_Text greenAvgText;
    //[SerializeField] TMP_Text redAvgText;

    private void Start()
    {
        //ChangeOpinionCommand();
        InteractCommand();
    }

    private void UpdateChart()
    {
        int redSlimes = 0;
        int greenSlimes = 0;
        foreach (InfluenceSlime item in slimes)
        {
            if (item.opinion == 1)
            {
                greenSlimes++;
            }
            else if (item.opinion == -1)
            {
                redSlimes++;
            }
        }
        //chartController.AddDataToChart(redSlimes, greenSlimes);
        if ((iterationIndex % 5) == 0)
        {
            populationChart.AddDataToChart(redSlimes, greenSlimes);
        }
    }

    public float GetMaxWeight()
    {
        return slimes.Count * maxDistance;
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
        if (allowedToInteract)
        {
            if (allChangedOpinion && (iterationIndex <= numberOfIterations) && (slimes.Count > 0) && allSlimesReachedPosition)
            {
                iterationIndex++;
                //dayText.text = "День " + iterationIndex;
                InteractCommand();
                Debug.Log("Day " + iterationIndex);
            }
        }
        else
        {
            if (allChangedOpinion && (iterationIndex <= numberOfIterations) && (slimes.Count > 0))
            {
                iterationIndex++;
                //dayText.text = "День " + iterationIndex;
                ResetAverageThreshold();
                ResetCommand();
                StartCoroutine(WaitAndChangeOpinion());
                Debug.Log("Day " + iterationIndex);
            }
        }
    }

    public bool CheckIfAllReachedPosition()
    {
        allSlimesReachedPosition = true;
        foreach (InfluenceSlime slime in slimes)
        {
            if (!slime.positionReached)
            {
                allSlimesReachedPosition = false;
            }
        }

        if (allSlimesReachedPosition)
        {
            UpdateChart();
            ResetAverageThreshold();
            ResetCommand();
            ChangeOpinionCommand();
        }

        return allSlimesReachedPosition;
    }

    private IEnumerator WaitAndChangeOpinion()
    {
        yield return new WaitForSeconds(1f);
        ChangeOpinionCommand();
    }
    private void ResetAverageThreshold()
    {
        if (greenSlimesThreshold.Count > 0 && redSlimesThreshold.Count > 0)
        {
            for (int i = 0; i < greenSlimesThreshold.Count; i++)
            {
                totalGreenThreshold += greenSlimesThreshold[i];
            }
            avgGreenThreshold = (float) totalGreenThreshold / greenSlimesThreshold.Count;

            for (int i = 0; i < redSlimesThreshold.Count; i++)
            {
                totalRedThreshold += redSlimesThreshold[i];
            }
            avgRedThreshold = (float) totalRedThreshold / redSlimesThreshold.Count;
            //greenAvgText.text = "Порог Зелёных: " + avgGreenThreshold.ToString("F1");
            //redAvgText.text = "Порог Красных: " + avgRedThreshold.ToString("F1");

            Debug.Log("Average Green Threshold: " + avgGreenThreshold + ", Average Red Threshold: " + avgRedThreshold);
        }

        if ((iterationIndex % 5) == 0)
        {
            avgThresholdChart.AddDataToChart(avgRedThreshold, avgGreenThreshold);
        }

        greenSlimesThreshold.Clear();
        redSlimesThreshold.Clear();
        totalGreenThreshold = 0;
        totalRedThreshold = 0;
    }
}
