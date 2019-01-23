using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] images;
    [SerializeField] private float[] times;
    private Sprite currentImage;
    private int currentTime;
    private float timer;

    void Start()
    {
        currentTime = 0;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    private void Animate()
    {
        currentImage = images[currentTime];
        gameObject.GetComponent<SpriteRenderer>().sprite = currentImage;

        if (timer >= times[currentTime])
        {
            timer = 0.0f;
            currentTime++;
            if (currentTime >= times.Length)
            {
                currentTime = 0;
            }
        }


        timer += Time.deltaTime;
    }
}
