using UnityEngine;

namespace GameMechanics.Sound
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private GameObject musicObjectPrefab;
        public bool OnMusic { get; set; } = true;

        public MusicObject CreateMusicObjectDontDestroy()
        {
            if (!OnMusic) return null;
            
            var go = CreateObject();
            DontDestroyOnLoad(go);
            return go.GetComponent<MusicObject>();
        }
        public MusicObject CreateMusicObject()
        {
            if (!OnMusic) return null;
            
            return CreateObject().GetComponent<MusicObject>();
        }

        private GameObject CreateObject()
        {
            return Instantiate(musicObjectPrefab);
        }
    }
}