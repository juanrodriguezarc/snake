using System.Collections;
using System.Collections.Generic;
using Snake.Utility;
using UnityEngine;

namespace Snake.Systems
{
    public class SoundManager : SingletonPersistent<SoundManager>
    {
        [SerializeField]
        private AudioSource _musicSource, _effectSource;

        public void PlaySound(AudioClip clip)
        {
            _effectSource.PlayOneShot(clip);
        }

    }
}