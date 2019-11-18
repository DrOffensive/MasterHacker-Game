using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{

    public List<HasckOSSound> sounds;

    [System.Serializable]
    public class HasckOSSound
    {
        public string name;
        public AudioClip clip;
        public string key;
    }

    public AudioClip GetClip(string key)
    {
        foreach (HasckOSSound sound in sounds)
        {
            if (key.Equals(sound.key))
                return sound.clip;
        }

        return null;
    }
}
