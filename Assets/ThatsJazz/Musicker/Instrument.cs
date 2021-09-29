using UnityEngine;

/// produces any note in the chromatic scale
public class Instrument: MonoBehaviour {
    // -- config --
    /// the chromatic scale (usually C3-based), must have 12 notes
    [SerializeField] AudioClip[] mScale;

    // -- lifecycle --
    void Awake() {
        if (mScale.Length != 12) {
            Debug.LogError($"{this} did not contain 12 notes");
        }
    }

    // -- queries --
    /// gets a random audio clip
    public AudioClip AnyClip() {
        return mScale[Random.Range(0, mScale.Length)];
    }

    /// find the clips for the given chord w/ a pre-allocated array
    public int FindClipsForChord(AudioClip[] clips, Chord chord) {
        // get the number of notes we need or can fit
        var n = Mathf.Min(chord.Length, clips.Length);

        // grab all those clips
        for (var i = 0; i < n; i++) {
            clips[i] = mScale[(int)chord.FindNote(i)];
        }

        return n;
    }
}