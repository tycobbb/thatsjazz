using UnityEngine;

public class Toy: MonoBehaviour {
    // -- config --
    /// the musicker
    [SerializeField] Musicker mMusicker;

    // -- props --
    /// the toy's current musical key
    readonly Key mKey = Key.C;

    /// how many times the toy has bounced
    int mPlayCount = 0;

    // -- commands --
    /// play some music
    void PlayChord() {
        mMusicker.PlayChord(GetChord(), 0.1f);
        mPlayCount += 1;
    }

    // -- queries --
    /// get the chord to play
    Chord GetChord() {
        return (mPlayCount % 4) switch {
            0 => mKey.Chord(Tone.II, Quality.Min7),
            1 => mKey.Chord(Tone.V, Quality.Dom7),
            2 => mKey.Chord(Tone.I, Quality.Maj7),
            3 => mKey.Chord(Tone.I, Quality.Maj7),
            _ => mKey.Chord(Tone.VII, Quality.Dim7),
        };
    }

    // -- events --
    void OnCollisionEnter(Collision other) {
        PlayChord();
    }
}