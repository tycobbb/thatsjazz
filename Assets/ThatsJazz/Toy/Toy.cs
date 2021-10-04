using UnityEngine;
using UnityEngine.Serialization;

/// a bouncing toy
public class Toy: MonoBehaviour {
    // -- deps --
    /// the score module
    Score mScore;

    // -- tuning --
    [FormerlySerializedAs("mMinImpluse")]
    [Header("tuning")]
    [Tooltip("the minimum impluse (sqr mag) to play sound on contact")]
    [SerializeField] float mMinImpulse = 2.0f;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the music player")]
    [SerializeField] Musicker mMusicker;

    // -- props --
    /// the toy's line
    Line mLine;

    // -- lifecycle --
    void Awake() {
        // set deps
        mScore = Score.Get;

        // set props
        mLine = new Line(
            Tone.I.Octave(),
            Tone.V,
            Tone.IV,
            Tone.III,
            Tone.I
        );
    }

    // -- commands --
    /// missed everything (hit the ground)
    void Miss() {
        mScore.RecordMiss();

        // play some music
        mMusicker.PlayRand();
        mMusicker.PlayLine(mLine);
    }

    /// hit a player
    void Hit(Player player) {
        mScore.RecordHit();

        // play some music
        mMusicker.PlayLine(mLine, player.Key);
    }

    /// hit something else
    void Hit() {
        mMusicker.PlayLine(mLine);
    }

    // -- events --
    void OnCollisionEnter(Collision collision) {
        // ignore really weak hits
        if (collision.impulse.sqrMagnitude < mMinImpulse) {
            return;
        }

        // a miss if we hit the ground
        if (collision.gameObject.CompareTag("Ground")) {
            Miss();
            return;
        }

        // otherwise, see if we hit something significant, like a player
        var player = collision.gameObject.GetComponent<Player>();
        if (player) {
            Hit(player);
        } else {
            Hit();
        }
    }
}