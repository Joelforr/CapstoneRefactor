using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXLibrary{

	public enum SFXTags
    {
        Attack,
        Death,
        Dodge,
        Footsteps,
        Impact,
        Jump
     
    }

    private Dictionary<SFXTags, AudioClip> sound_dictionary;

    public SFXLibrary()
    {
        sound_dictionary = new Dictionary<SFXTags, AudioClip>();
        Populate();
    }

    private void Populate()
    {
        sound_dictionary.Add(SFXTags.Dodge, Resources.Load("SFX/Dodge") as AudioClip);
        sound_dictionary.Add(SFXTags.Footsteps, Resources.Load("SFX/Footsteps") as AudioClip);
        sound_dictionary.Add(SFXTags.Impact, Resources.Load("SFX/Impact") as AudioClip);
        sound_dictionary.Add(SFXTags.Jump, Resources.Load("SFX/Jump") as AudioClip);
    }

    public AudioClip GetSFX(SFXTags tag)
    {
        return sound_dictionary[tag];
    }
}
