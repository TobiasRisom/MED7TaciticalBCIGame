using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private Dictionary<string, List<AudioClip>> audioClipDict;
    private AudioSource audioSource;

    public Vector2 PassiveInterval;
    public bool active = true;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        AudioList[] audioLists = GetComponents<AudioList>();
        audioClipDict = new Dictionary<string, List<AudioClip>>();
        foreach (AudioList audioList in audioLists) {
            audioClipDict.Add(audioList.Category, audioList.AudioClips);
        }
        StartCoroutine(PlayPassive());
    }

    void Update() {
    }

    private IEnumerator PlayPassive() {
        if (!active) yield return null;
        float waitTime = Random.Range(PassiveInterval.x, PassiveInterval.y);
        if (audioClipDict.ContainsKey("Passive")) {
            while (true) {
                yield return new WaitForSeconds(waitTime);
                if (!audioSource.isPlaying) PlayCategory("Passive");
            }
        }
    }
    
    public void PlayCategory(string category) {
        if (!active) return;
        AudioClip audioClip = GetRandomAudioClip(category);
        PlayClip(audioClip);
    }

    public void PlayClip(AudioClip audioClip) {
        if (!active) return;
        audioSource.PlayOneShot(audioClip);
    }

    private AudioClip GetRandomAudioClip(string category) {
        if (!audioClipDict.ContainsKey(category)) {
            Debug.LogError("AudioManager: Could not find category " + category);
            return null;
        }
        List<AudioClip> audioClips = audioClipDict[category];
        if (audioClips.Count <= 0) {
            Debug.LogError("AudioManager: " + category + " is empty.");
            return null;
        }
        int randomIndex = Random.Range(0, audioClips.Count);
        AudioClip randomAudioClip = audioClips[randomIndex];
        return randomAudioClip;
    }
}