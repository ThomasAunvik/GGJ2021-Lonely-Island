using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Canvas))]
public class UIWorldLookAt : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private Camera mainCam;

    private void Awake()
    {
        if(mainCam == null) mainCam = Camera.main;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = mainCam;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
    }
}
