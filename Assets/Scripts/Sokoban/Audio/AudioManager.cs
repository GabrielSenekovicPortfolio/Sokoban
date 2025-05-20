using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * The Audio Manager is used by all scripts that have a sound effect to play. 
 * It can be called from anywhere to put the sound into the audiosource that belongs to the manager.
 * It cannot play more than one sound at a time right now, simply because that functionality is not required by the game.
 */
public class AudioManager : MonoBehaviour
{
    [System.Serializable]class Entry
    {
        public string ID;
        public AudioClip clip;
    }
    [SerializeField] List<Entry> sounds;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string ID)
    {
        Entry entry = sounds.FirstOrDefault(e => e.ID == ID);
        if(entry != null)
        {
            source.clip = entry.clip;
            source.Play();
        }
        else
        {
            Debug.LogError("Could not find audio file: " + ID);
        }
    }
}
