using System.Collections;
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

    /// pre-allocated storage for audio clips
    AudioClip[] mClips;

    // -- lifecycle --
    void Awake() {
        // create any necessary audio sources
        var go = gameObject;

        for (var i = mSources.Count; i < mNumSources; i++) {
            var source = go.AddComponent<AudioSource>();
            mSources.Add(source);
        }

        // set props
        mClips = new AudioClip[4];
    }

    // -- commands --
    /// play the current tone in the line and advance it
    public void PlayLine(Line line) {
        PlayTone(line.Curr());
        line.Advance();
    }

    /// play the current chord in a progression and advance it
    public void PlayProgression(Progression prog) {
        PlayChord(prog.Curr());
        prog.Advance();
    }

    /// play the clip for a tone
    public void PlayTone(Tone tone) {
        PlayClip(mInstrument.FindClip(tone));
    }

    /// play the clips in the chord. pass an interval to arpeggiate.
    public void PlayChord(Chord chord, float interval = 0.0f) {
        StartCoroutine(PlayChordAsync(chord, interval));
    }

    /// play the clips in the chord. pass an interval to arpeggiate.
    public IEnumerator PlayChordAsync(Chord chord, float interval = 0.0f) {
        var nClips = mInstrument.FindClipsForChord(
            mClips,
            chord
        );

        for (var i = 0; i < nClips; i++) {
            PlayClip(mClips[i]);

            if (interval != 0.0) {
                yield return new WaitForSeconds(interval);
            }
        }
    }

    /// play a clip on the next source
    void PlayClip(AudioClip clip) {
        var i = mNextSource;

        // find the audio source
        var source = mSources[i];

        // play the clip
        source.clip = clip;
        source.Play();

        // advance the source
        mNextSource = (i + 1) % mNumSources;
    }
}