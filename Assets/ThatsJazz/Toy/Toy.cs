using UnityEngine;

public class Toy: MonoBehaviour {
    // -- config --
    /// the musicker
    [SerializeField] Musicker mMusicker;

    // -- props --
    /// the toy's current musical key
    readonly Key mKey = Key.C;

    /// the toy's line
    Line mLine;

    // -- lifecycle --
    void Awake() {
        mLine = new Line(
            Tone.I.Octave(),
            Tone.V,
            Tone.IV,
            Tone.III,
            Tone.I
        );
    }

    // -- commands --
    /// play some music
    void Play() {
        mMusicker.PlayLine(mLine);
    }

    // -- events --
    void OnCollisionEnter(Collision other) {
        Play();
    }
}