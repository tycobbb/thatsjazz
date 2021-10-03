using UnityEngine;

public class Toy: MonoBehaviour {
    // -- tuning --
    [Header("tuning")]
    [Tooltip("the minimum impluse (sqr mag) to play sound on contact")]
    [SerializeField] float mMinImpluse = 2.0f;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the music player")]
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
    void OnCollisionEnter(Collision collision) {
        // ignore really weak hits
        if (collision.impulse.sqrMagnitude < mMinImpluse) {
            return;
        }

        Key? key = null;

        // use the player's key if we hit a player
        var player = collision.gameObject.GetComponent<Player>();
        if (player) {
            key = player.Key;
        }

        // play some music
        Play(key);
    }
}