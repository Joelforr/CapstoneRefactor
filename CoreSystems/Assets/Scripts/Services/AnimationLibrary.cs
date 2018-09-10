using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AnimationLibrary
{

    public enum CharacterTags
    {
        _Gry,
        _Purp,
    }

    public enum AnimationTags
    {
        Attack_D_A,
        Attack_D_G,
        Attack_S_A,
        Attack_S_G,
        Attack_Sl,
        Attack_U_A,
        Attack_U_G,
        Dodge,
        Fall,
        Gaurd,
        Hurt,
        Idle,
        Jump,
        Run,
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


        #region Dummy_Gray
        Dictionary<AnimationTags, XAnimation> gry = new Dictionary<AnimationTags, XAnimation>();
        gry.Add(AnimationTags.Attack_D_A, Resources.Load("Data/XAnimationData/_Gray/Attack_Down_A") as XAnimation);
        gry.Add(AnimationTags.Attack_D_G, Resources.Load("Data/XAnimationData/_Gray/Attack_Down_G") as XAnimation);
        gry.Add(AnimationTags.Attack_S_A, Resources.Load("Data/XAnimationData/_Gray/Attack_Side_A") as XAnimation);
        gry.Add(AnimationTags.Attack_S_G, Resources.Load("Data/XAnimationData/_Gray/Attack_Side_G") as XAnimation);
        gry.Add(AnimationTags.Attack_Sl, Resources.Load("Data/XAnimationData/_Gray/Attack_Slide") as XAnimation);
        gry.Add(AnimationTags.Attack_U_A, Resources.Load("Data/XAnimationData/_Gray/Attack_Up_A") as XAnimation);
        gry.Add(AnimationTags.Attack_U_G, Resources.Load("Data/XAnimationData/_Gray/Attack_Up_G") as XAnimation);

        gry.Add(AnimationTags.Dodge, Resources.Load("Data/XAnimationData/_Gray/Dodge") as XAnimation);
        gry.Add(AnimationTags.Fall, Resources.Load("Data/XAnimationData/_Gray/Fall") as XAnimation);
        //gry.Add(AnimationTags.Gaurd, Resources.Load("Data/XAnimationData/_Gray/Gaurd") as XAnimation);
        gry.Add(AnimationTags.Hurt, Resources.Load("Data/XAnimationData/_Gray/Hurt") as XAnimation);

        gry.Add(AnimationTags.Idle, Resources.Load("Data/XAnimationData/_Gray/Idle") as XAnimation);
        gry.Add(AnimationTags.Jump, Resources.Load("Data/XAnimationData/_Gray/Jump") as XAnimation);
        gry.Add(AnimationTags.Run, Resources.Load("Data/XAnimationData/_Gray/Run") as XAnimation);
        #endregion



        #region Dmy_Purple
        Dictionary<AnimationTags, XAnimation> purp = new Dictionary<AnimationTags, XAnimation>();
        purp.Add(AnimationTags.Attack_D_A, Resources.Load("Data/XAnimationData/_Purp/Attack_Down_A") as XAnimation);
        purp.Add(AnimationTags.Attack_D_G, Resources.Load("Data/XAnimationData/_Purp/Attack_Down_G") as XAnimation);
        purp.Add(AnimationTags.Attack_S_A, Resources.Load("Data/XAnimationData/_Purp/Attack_Side_A") as XAnimation);
        purp.Add(AnimationTags.Attack_S_G, Resources.Load("Data/XAnimationData/_Purp/Attack_Side_G") as XAnimation);
        purp.Add(AnimationTags.Attack_Sl, Resources.Load("Data/XAnimationData/_Purp/Attack_Slide") as XAnimation);
        purp.Add(AnimationTags.Attack_U_A, Resources.Load("Data/XAnimationData/_Purp/Attack_Up_A") as XAnimation);
        purp.Add(AnimationTags.Attack_U_G, Resources.Load("Data/XAnimationData/_Purp/Attack_Up_G") as XAnimation);

        purp.Add(AnimationTags.Dodge, Resources.Load("Data/XAnimationData/_Purp/Dodge") as XAnimation);
        purp.Add(AnimationTags.Fall, Resources.Load("Data/XAnimationData/_Purp/Fall") as XAnimation);
        //prp.Add(AnimationTags.Gaurd, Resources.Load("Data/XAnimationData/_Purp/Gaurd") as XAnimation);
        purp.Add(AnimationTags.Hurt, Resources.Load("Data/XAnimationData/_Purp/Hurt") as XAnimation);

        purp.Add(AnimationTags.Idle, Resources.Load("Data/XAnimationData/_Purp/Idle") as XAnimation);
        purp.Add(AnimationTags.Jump, Resources.Load("Data/XAnimationData/_Purp/Jump") as XAnimation);
        purp.Add(AnimationTags.Run, Resources.Load("Data/XAnimationData/_Purp/Run") as XAnimation);
        #endregion



        //CHARACTER DICTIONARY POPULATION
        character_dictionary.Add(CharacterTags._Gry, gry);
        character_dictionary.Add(CharacterTags._Purp, purp);

    }

    public XAnimation GetXAnimation(CharacterTags c_tag, AnimationTags a_tag)
    {
        return character_dictionary[c_tag][a_tag];
    }

}
