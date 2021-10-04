using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player: MonoBehaviour {
    // -- statics --
    /// the layer for toys
    static int sToyLayer = -1;

    // -- tuning --
    [Header("config")]
    [Tooltip("the player's name")]
    [SerializeField] Name mName = Name.W;

    [Tooltip("the player's musical key")]
    [SerializeField] Root mKeyOf = Root.C;

    [Header("tuning")]
    [Tooltip("the magnitude of the move")]
    [SerializeField] float mMoveMag = 2.0f;

    [Tooltip("the bias of an off key move")]
    [SerializeField] float mMoveOffKey = 0.3f;

    [Tooltip("the magnitude of the bounce force")]
    [SerializeField] float mBounceMag = 5.0f;

    [Tooltip("the scale when normal. time is duration.")]
    [SerializeField] Linear<Vector3> mScaleDefault = Linear<Vector3>.Zero;

    [Tooltip("the scale when squished. time is duration.")]
    [SerializeField] Linear<Vector3> mScaleSquished = Linear<Vector3>.Zero;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the player's model transform")]
    [SerializeField] Transform mModel;

    [Tooltip("the player's head collider")]
    [SerializeField] Collider mHead;

    [Tooltip("the player's foot's rigidbody")]
    [SerializeField] Rigidbody mFoot;

    [Tooltip("the pos where the move is applied")]
    [SerializeField] Transform mMovePos;

    [Tooltip("the music player")]
    [SerializeField] Musicker mMusic;

    [Tooltip("the toy to move towards")]
    [SerializeField] Transform mToy;

    // -- props --
    /// the musical key
    Key mKey;

    /// the current move dir
    Vector2 mMoveDir;

    /// the current squish velocity
    Vector3 mSquishVel = Vector3.zero;

    /// the player's chord when bouncing
    Progression mProg;

    /// a buffer to raycast for bounce targets
    RaycastHit[] mBounceHits = new RaycastHit[3];

    /// the player's inputs
    PlayerInput.PlayerActions mInputs;

    // -- lifecycle --
    void Awake() {
        // set props
        mInputs = new PlayerInput().Player;

        // set music props
        mKey = new Key(mKeyOf);
        mProg = new Progression(
            mKey.Chord(Tone.II, Quality.Min7),
            mKey.Chord(Tone.V, Quality.Dom7),
            mKey.Chord(Tone.I, Quality.Maj7),
            mKey.Chord(Tone.I, Quality.Maj7)
        );

        // set statics
        if (sToyLayer == -1) {
            sToyLayer = LayerMask.NameToLayer("Toy");
        }
    }

    void OnEnable() {
        mInputs.Enable();
    }

    void Update() {
        ReadMove();
    }

    void FixedUpdate() {
        Move();
        Squish();
        TryBounce();
    }

    void OnDisable() {
        mInputs.Disable();
    }

    // -- commands --
    /// read move inputs
    void ReadMove() {
        // zero out move dir
        mMoveDir = Vector2.zero;

        // aggregate each direction
        var i = mInputs;
        ReadDir(i.Up, Name.W, Vector2.up);
        ReadDir(i.Left, Name.A, Vector2.left);
        ReadDir(i.Down, Name.S, Vector2.down);
        ReadDir(i.Right, Name.D, Vector2.right);
    }

    /// read input for a specific direction and add it
    void ReadDir(InputAction input, Name onKeyName, Vector2 offKeyDir) {
        // skip if not pressed
        if (!input.IsPressed()) {
            return;
        }

        // if off key, add the off key dir
        if (mName != onKeyName) {
            mMoveDir += offKeyDir * mMoveOffKey;
            return;
        }

        // otherwise on key, so move towards the toy
        var delta = mToy.position - transform.position;
        var track = new Vector2(delta.x, delta.z);
        mMoveDir += track.normalized;
    }

    /// move the player
    void Move() {
        // apply move dir to the xz plane
        var dir = Vector3.zero;
        dir.x = mMoveDir.x;
        dir.z = mMoveDir.y;

        // apply the biased move force
        mFoot.AddForceAtPosition(
            dir * mMoveMag,
            mMovePos.position,
            ForceMode.Acceleration
        );
    }

    /// squish the player, flattening them and increasing their surface area
    void Squish() {
        var scale = mScaleDefault;

        // if squished, target that scale
        if (mInputs.Squish.IsPressed()) {
            scale = mScaleSquished;
        }

        // animate towards scale
        var next = Vector3.SmoothDamp(
            mModel.localScale,
            scale.Dst,
            ref mSquishVel,
            scale.Time
        );

        // update scale
        mModel.localScale = next;
    }

    /// bounce stuff above the player when un-squishing
    void TryBounce() {
        var v = mSquishVel.y;

        // don't bounce unless un-squishing
        if (v < 0.01f) {
            return;
        }

        // using the player's rotation
        var t = transform;

        // get the cast size & dir
        var rct = mHead.bounds;
        var dir = t.up;
        var ext = rct.extents;
        ext.y = 0.1f;

        // try to raycast for things above us
        var nHits = Physics.BoxCastNonAlloc(
            rct.center + dir * rct.size.y,
            ext,
            dir,
            mBounceHits,
            t.rotation,
            0.0f
        );

        // bounce any hits
        for (var i = 0; i < nHits; i++) {
            var hit = mBounceHits[i];
            hit.rigidbody.AddForce(mBounceMag * v * dir);
        }

        if (nHits != 0 && mMusic.IsAvailable()) {
            mMusic.PlayProgression(mProg);
        }
    }

    // -- c/music
    /// sings a tone in the player's key, or sometimes other notes that will
    /// sound nice in the key.
    public void Sing(Tone tone) {
        var sampled = Random.Range(0, 10) switch {
            0 => Tone.I,
            1 => Tone.III,
            2 => Tone.V,
            3 => Tone.VII,
            4 => Tone.I.Octave(),
            _ => tone,
        };

        mMusic.PlayTone(sampled, mKey);
    }

    // -- queries --
    /// get the player's key
    public Key Key {
        get => mKey;
    }
}