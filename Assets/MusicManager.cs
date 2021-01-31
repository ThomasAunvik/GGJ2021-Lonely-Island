using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private float normalizedTime;
    [Range(0.1f, 10.0f)] //Min. range must be 0.1f or greater.
    [SerializeField]
    private float inDuration = 5.0f;
    [Range(0.1f, 10.0f)]
    [SerializeField]
    private float outDuration = 5.0f;

    bool inBattle = false;
    //Prevents a coroutine from being triggered multiple times before finishing.
    bool isPlaying = false;

    AudioSource battleMusic;
    AudioSource exploreMusic;

    [SerializeField] GameObject battleMusicObj;
    [SerializeField] GameObject exploreMusicObj;
    [SerializeField] GameObject player;

    [SerializeField] AudioClip overworldExploreClip;
    [SerializeField] AudioClip dungeonExploreClip;
    [SerializeField] AudioClip battleClip;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        battleMusic = battleMusicObj.GetComponent<AudioSource>();
        battleMusic.Play();
        exploreMusic = exploreMusicObj.GetComponent<AudioSource>();
        exploreMusic.Play();

        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            exploreMusic.clip = overworldExploreClip;
        }
        else if (SceneManager.GetActiveScene().name == "Dungeon")
        {
            exploreMusic.clip = dungeonExploreClip;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            StartCoroutine(FadeOut(exploreMusic, outDuration));
            StartCoroutine(FadeIn(battleMusic, 1.0f, inDuration));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Switching to exploration Music");
            StartCoroutine(FadeOut(battleMusic, outDuration));
            StartCoroutine(FadeIn(exploreMusic, 1.0f, inDuration));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Look for enemies

        //Change music when in range
    }



    IEnumerator FadeIn(AudioSource source, float finish, float duration)
    {

        isPlaying = true;

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            normalizedTime = t / duration;
            source.volume = Mathf.Lerp(source.volume, finish, normalizedTime);
            yield return null;
        }

        isPlaying = false;
        StopCoroutine("FadeIn");

    }

    IEnumerator FadeOut(AudioSource source, float duration)
    {

        isPlaying = true;

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            normalizedTime = t / duration;
            source.volume = Mathf.Lerp(source.volume, 0.0f, normalizedTime);
            yield return null;
        }

        source.volume = 0.0f;

        isPlaying = false;
        StopCoroutine("FadeOut");

    }

}
