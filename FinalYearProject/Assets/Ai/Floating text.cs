using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floatingtext : MonoBehaviour
{
    Transform mainCamera;
    Transform unit;
    Transform worldSpaceCanvas;

    public Vector3 offset;
    void Start()
    {
        mainCamera = Camera.main.transform;
        unit = transform.parent.parent;
        worldSpaceCanvas = GameObject.FindAnyObjectByType<Canvas>().transform;

        transform.SetParent(worldSpaceCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position-mainCamera.position);
        transform.position = unit.position+ offset;
    }
}
