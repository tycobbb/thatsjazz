using UnityEngine;

/// a thing that plays music
public class Musicker: MonoBehaviour {
    // -- config --
    /// the audio source to realize sound
    [SerializeField] AudioSource mSource;

    /// their current instrument
    [SerializeField] Instrument mInstrument;

    // -- commands --
    /// play some music
    void Play() {
        mSource.clip = mInstrument.AnyNote();
        mSource.Play();
    }

    // -- events --
    void OnCollisionEnter(Collision other) {
        Play();
    }
}