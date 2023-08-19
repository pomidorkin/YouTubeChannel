using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] Transform mainCamera;
    Transform unit;
    [SerializeField] Transform worldSpaceCanvas;

    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        unit = transform.parent;
        transform.SetParent(worldSpaceCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        transform.position = unit.position + offset;
    }
}
