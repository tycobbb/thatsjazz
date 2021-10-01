using UnityEngine;

public class Toy: MonoBehaviour {
    // -- config --
    /// the musicker
    [SerializeField] Musicker mMusicker;

    // -- props --
    /// the toy's current musical key
    readonly Key mKey = Key.C;

    /// the toy's chord progression
    Progression mProg;

    // -- lifecycle --
    void Awake() {
        mProg = new Progression(
            mKey.Chord(Tone.II, Quality.Min7),
            mKey.Chord(Tone.V, Quality.Dom7),
            mKey.Chord(Tone.I, Quality.Maj7),
            mKey.Chord(Tone.I, Quality.Maj7)
        );
    }

    // -- commands --
    /// play some music
    void PlayChord() {
        mMusicker.PlayChord(mProg.Curr(), 0.1f);
        mProg.Advance();
    }

    // -- events --
    void OnCollisionEnter(Collision other) {
        PlayChord();
    }
}