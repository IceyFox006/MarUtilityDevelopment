/*
 * Marlow Greenan
 * Created: 7/20/2026
 * Last Updated: 7/20/2026 by Marlow Greenan
 * 
 * Links the material's texture to the current sprite.
 */
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace MarUtility.Material
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LinkSpriteMaterialTexture : MonoBehaviour
    {
        [SerializeField]
        private InitializeTime _initializeTime;

        [SerializeField, OnValueChanged("OnVC_Sprite")]
        private Sprite _sprite;
        [SerializeField]
        private string _textureNameID = "_Texture";

        private SpriteRenderer renderer;

        public Sprite Sprite 
        { get => _sprite; 
          set
            {
                _sprite = value; 
                renderer.sprite = _sprite;
                renderer.material.SetTexture(_textureNameID, _sprite.texture);
            }
        }

        private void Awake()
        {
            if (_initializeTime == InitializeTime.AWAKE)
                Initialize();
        }
        private void Start()
        {
            if (_initializeTime == InitializeTime.START)
                Initialize();
        }
        public void Initialize()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        private void OnVC_Sprite()
        {
            if (EditorApplication.isPlaying)
            {
                DebugMessages.SetPlaytestOnly("_sprite");
                return;
            }
            renderer = GetComponent<SpriteRenderer>();
            Sprite = _sprite;
        }
    }
}

