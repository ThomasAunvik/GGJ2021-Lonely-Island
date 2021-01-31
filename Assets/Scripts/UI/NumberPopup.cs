using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPopup : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private TMPro.TMP_Text numberText;
    [SerializeField] private float killTime = 0.4f;
    [SerializeField] private float upSpeed = 1;
    private float timer;

    void Start()
    {
        numberText.color = color;
    }

    private void Update()
    {
        transform.position = transform.position + (Vector3.up * upSpeed * Time.deltaTime);
        
        timer += Time.deltaTime;
        if (timer >= killTime)
            Destroy(gameObject);
    }

    public void SetNumberText(float number)
    {
        numberText.text = number.ToString();
    }
}
