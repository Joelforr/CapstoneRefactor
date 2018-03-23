using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AnimationLibrary
{

    public enum CharacterTags
    {
        Dummy,
    }

    public enum AnimationTags
    {
        Attack_F,
        Attack_Dg,
        Attack_U,
        Hurt,
        Idle,
        Jump,
        Walk,
    }

    private Dictionary<CharacterTags, Dictionary<AnimationTags, XAnimation>> character_dictionary;
    private Dictionary<AnimationTags, XAnimation> animation_dictionary;

    public AnimationLibrary()
    {
        character_dictionary = new Dictionary<CharacterTags, Dictionary<AnimationTags, XAnimation>>();
        Populate();
    }

    private void Populate()
    {
        /*
        string[] dirs = Directory.GetDirectories(Application.dataPath + "/Resources/Data/XAnimationData");
        foreach (string dir in dirs)
        {

        }
        */

        //Dummy
        Dictionary<AnimationTags, XAnimation> dummy_= new Dictionary<AnimationTags, XAnimation>();
        dummy_.Add(AnimationTags.Attack_F, Resources.Load("Data/XAnimationData/F_Slash_XAnimation") as XAnimation);
        dummy_.Add(AnimationTags.Hurt, Resources.Load("Data/XAnimationData/Knockback_XAnimation") as XAnimation);
        dummy_.Add(AnimationTags.Idle, Resources.Load("Data/XAnimationData/Idle_XAnimation") as XAnimation);
        dummy_.Add(AnimationTags.Jump, Resources.Load("Data/XAnimationData/Jump_XAnimation") as XAnimation);
        dummy_.Add(AnimationTags.Jump, Resources.Load("Data/XAnimationData/Run_XAnimation") as XAnimation);


        //CHARACTER DICTIONARY POPULATION
        character_dictionary.Add(CharacterTags.Dummy, dummy_);

    }

    public XAnimation GetXAnimation(CharacterTags c_tag, AnimationTags a_tag)
    {
        return character_dictionary[c_tag][a_tag];
    }

}
