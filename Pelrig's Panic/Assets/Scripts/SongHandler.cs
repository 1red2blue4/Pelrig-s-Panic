using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongHandler : MonoBehaviour {

    [SerializeField] private AudioSource newSong;
    [SerializeField] private AudioSource selfSong;
    private float timer;

    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (timer >= 1.0f && !selfSong.isPlaying && !newSong.isPlaying)
        {
            newSong.Play();
        }

        timer += Time.deltaTime;

	}
}
