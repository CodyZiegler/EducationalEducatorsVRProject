using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// Plays a Sound Clip OnCollisionEnter
    /// </summary>
    public class CollisionSound : MonoBehaviour {

        public AudioClip CollisionAudio;
        AudioSource audioSource;
        float startTime;

        Collider col;

        void Start() {
            audioSource = GetComponent<AudioSource>();
            startTime = Time.time;
            col = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision) {

            // Just spawned, don't fire collision sound immediately
            if(Time.time - startTime < 0.1f) {
                return;
            }

            // No Collider present, don't play sound
            if(!col.enabled) {
                return;
            }

            if(audioSource && CollisionAudio) {
                // Play Shot
                if (audioSource.isPlaying) {
                    audioSource.Stop();
                }

                audioSource.clip = CollisionAudio;
                audioSource.pitch = Time.timeScale;
                audioSource.Play();
            }
        }
    }
}
