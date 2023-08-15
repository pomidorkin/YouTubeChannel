using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SlimeSpawner : MonoBehaviour
{
    [SerializeField] GameObject slimePrefab;
    [SerializeField] int numberOfSLimes;
    [SerializeField] float distance;

    private void OnEnable()
    {
        for (int i = 0; i < numberOfSLimes; i++)
        {

            Instantiate(slimePrefab, new Vector3(0, 0, i * -distance), Quaternion.identity);
        }
    }

}
