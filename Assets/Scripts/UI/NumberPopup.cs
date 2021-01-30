using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPopup : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Color color;
    [SerializeField] private TextMesh numberText;

    void Start()
    {
        mainCamera = Camera.main;

        numberText.color = color;
    }

    private void Update()
    {
        //transform.LookAt(mainCamera.transform.position);
    }

    public void SetNumberText(float number)
    {
        numberText.text = number.ToString();
    }
}
