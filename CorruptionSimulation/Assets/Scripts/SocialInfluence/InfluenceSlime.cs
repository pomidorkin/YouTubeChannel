using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceSlime : MonoBehaviour
{
    [SerializeField] InfluenceController influenceController;
    public int opinion = 1;
    [SerializeField] int threshold = 6;
    [SerializeField] float maxDistance = 10f;
    public List<InfluenceSlime> society;

    [SerializeField] Renderer renderer;
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;

    public bool opinionReviewed = false;

    private void OnEnable()
    {
        influenceController.slimes.Add(this);
        influenceController.ChangeOpinionCommand += TryChangeOpinion;
    }

    private void OnDisable()
    {
        influenceController.ChangeOpinionCommand -= TryChangeOpinion;
    }

    void linearThresholdModel(List<InfluenceSlime> neighbors, float threshold)
    {
        float sumOfWeights = 0;
        for (int i = 0; i < neighbors.Count; i++)
        {
            float dist = Vector3.Distance(neighbors[i].transform.position, transform.position);
            Debug.Log("dist: " + dist);
            sumOfWeights += (maxDistance - dist) * neighbors[i].opinion;
        }

        Debug.Log("sumOfWeights: " + sumOfWeights);

        if ((sumOfWeights >= threshold && opinion == -1) || (sumOfWeights <= -threshold && opinion == 1))
        {
            // Opinion changed
            Debug.Log("return: 1");
            if (opinion == 1)
            {
                renderer.material = redMat;
                opinion = -1;
            }
            else
            {
                renderer.material = greenMat;
                opinion = 1;
            }
        }
    }

    private void TryChangeOpinion()
    {
        if (society.Count == 0)
        {
            PopulateSociety();
        }
        linearThresholdModel(society, threshold);
        opinionReviewed = true;
        influenceController.CheckIfAllProcessedStep();
    }

    private void PopulateSociety()
    {
        foreach (InfluenceSlime slime in influenceController.slimes)
        {
            if (slime != this)
            {
                society.Add(slime);
            }
        }
    }
}
