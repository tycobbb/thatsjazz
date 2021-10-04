using UnityEngine;

/// produces any note in the chromatic scale
public class Instrument: MonoBehaviour {
    // -- config --
    /// the chromatic scale (usually C3-based), must have 12 notes
    [SerializeField] AudioClip[] mScale;

    // -- lifecycle --
    void Awake() {
        if (mScale.Length % 12 != 0) {
            Debug.LogError($"{this} has an incomplete octave");
        }
    }

    // -- queries --
    /// find a random audio clip
    public AudioClip RandClip() {
        return mScale[Random.Range(0, Length)];
    }

    /// find the clip for a tone
    public AudioClip FindClip(Tone tone) {
        return mScale[tone.Steps % Length];
    }

    /// find the clips for the given chord w/ a pre-allocated array
    public int FindClipsForChord(AudioClip[] clips, Chord chord) {
        // get the number of notes we need or can fit
        var n = Mathf.Min(chord.Length, clips.Length);

        // grab all those clips
        for (var i = 0; i < n; i++) {
            clips[i] = FindClip(chord[i]);
        }

        return n;
    }

    /// the length of the scale
    int Length {
        get => mScale.Length;
    }
}