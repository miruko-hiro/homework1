using UnityEngine;

namespace GameMechanics.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private GameObject soundObjectPrefab;
        public bool OnSound { get; set; } = true;

        public SoundObject CreateSoundObjectDontDestroy()
        {
            if (!OnSound) return null;
            
            var go = CreateObject();
            DontDestroyOnLoad(go);
            return go.GetComponent<SoundObject>();
        }
        public SoundObject CreateSoundObject()
        {
            if (!OnSound) return null;
            
            return CreateObject().GetComponent<SoundObject>();
        }

        private GameObject CreateObject()
        {
            return Instantiate(soundObjectPrefab);
        }
    }
}