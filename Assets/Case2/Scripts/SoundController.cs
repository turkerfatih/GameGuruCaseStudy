using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Case2
{
    public class SoundController : MonoBehaviour
    {
        public AudioSource AudioSource;
        public AudioClip PlacementSound;
        public AudioClip CutSound;

        [Range(0,3)]
        public float MinimumPitch;
        [Range(0,3)]
        public float MaximumPitch;
        [Min(1)]
        public int PitchStep;

        private int step;
        private void OnEnable()
        {
            EventBus.OnPiecePlaced += OnPiecePlaced;
        }
        private void OnDisable()
        {
            EventBus.OnPiecePlaced -= OnPiecePlaced;
        }
        
        private void OnPiecePlaced(Placement placement)
        {
            if (placement == Placement.Perfect)
            { 
                step = Mathf.Clamp(++step, 0,PitchStep);
                var ratio = (float)step / PitchStep;
                var pitch = Mathf.Lerp(MinimumPitch, MaximumPitch, ratio);
                var volumeScale = 0.5f+ratio/2;
                AudioSource.panStereo = 0;
                AudioSource.pitch = pitch;
                AudioSource.PlayOneShot(PlacementSound,volumeScale);
            }
            else
            {
                var panStereo = placement == Placement.RightCut ? 0.5f : -0.5f;
                AudioSource.panStereo = panStereo;
                AudioSource.pitch = 1;
                AudioSource.PlayOneShot(CutSound);
                step = -1;
            }
            
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                OnPiecePlaced(Placement.Perfect);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                OnPiecePlaced(Random.value>0.5? Placement.LeftCut : Placement.RightCut);
            }
        }
    }
#endif
}