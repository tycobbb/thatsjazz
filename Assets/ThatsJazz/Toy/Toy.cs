using UnityEngine;

public class Toy: MonoBehaviour {
    // -- config --
    /// the musicker
    [SerializeField] Musicker mMusicker;

    // -- props --
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
    void Play(Key? key) {
        mMusicker.PlayLine(mLine, key);
    }

    // -- events --
    void OnCollisionEnter(Collision other) {
        Key? key = null;

        // use the player's key if we hit a player
        var player = other.gameObject.GetComponent<Player>();
        if (player) {
            key = player.Key;
        }

        // play some music
        Play(key);
    }
}