using System.Collections.Generic;
using UnityEngine;

/// a thing that plays music
public class Musicker: MonoBehaviour {
    // -- config --
    /// the number of audio sources to create or keep
    [SerializeField] int mNumSources = 4;

    /// the audio source to realize sound
    [SerializeField] List<AudioSource> mSources;

    /// their current instrument
    [SerializeField] Instrument mInstrument;

    // -- props --
    /// the index of the next available audio source
    int mNextSource = 0;

    // -- lifecycle --
    void Awake() {
        // create any necessary audio sources
        var go = gameObject;

        for (var i = mSources.Count; i < mNumSources; i++) {
            var source = go.AddComponent<AudioSource>();
            mSources.Add(source);
        }
    }

    // -- commands --
    /// play some music
    void Play() {
        // find the audio source
        var source = mSources[mNextSource];

        // play the clip
        source.clip = mInstrument.AnyNote();
        source.Play();

        // advance the source
        mNextSource += 1;
    }

    // -- events --
    void OnCollisionEnter(Collision other) {
        Play();
    }
}