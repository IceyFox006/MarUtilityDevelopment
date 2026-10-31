using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DChar_", menuName = "Scriptable Objects/Dialogue/Character")]
public class SO_DialogueCharacter : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private int _relationPoints;

    //EMOTIONS
    [SerializeField, BoxGroup("Emote")]
    private EEmoteType _emoteType; 
    [SerializeField, BoxGroup("Emote"), ShowIf("_emoteType", EEmoteType.SPRITE)]
    private Dictionary<EEmotion, Sprite> _spriteEmotes;
    [SerializeField, BoxGroup("Emote"), ShowIf("_emoteType", EEmoteType.ANIMATION)]
    private Dictionary<EEmotion, string> _animationEmotes;

    #region GS
    public string Name { get => _name; }
    #endregion

    public enum EEmoteType
    {
        SPRITE,
        ANIMATION
    }

}

public enum EEmotion
{
    NEUTRAL,

}


