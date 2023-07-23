using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorSlimeSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] slimes;
    [SerializeField] float radius = 3;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        for (int i = 0; i < slimes.Length; i++)
        {
            float angle = i * Mathf.PI * 2 / slimes.Length;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, 0 , z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
            slimes[i].transform.position = pos;
            slimes[i].transform.rotation = rot;
        }
    }
}
