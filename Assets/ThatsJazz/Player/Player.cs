using UnityEngine;
using UnityEngine.Serialization;

public class Player: MonoBehaviour {
    // -- statics --
    /// the layer for toys
    static int sToyLayer = -1;

    // -- tuning --
    [FormerlySerializedAs("mKeyRoot")]
    [Header("tuning")]
    [Tooltip("the player's musical key")]
    [SerializeField] Root mKeyOf = Root.C;

    [Tooltip("the magnitude of the move")]
    [SerializeField] float mMoveMag = 2.0f;

    [Tooltip("the bias of the move")]
    [SerializeField] Box2 mMoveBias = Box2.Zero;

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

    // -- props --
    /// the musical key
    Key mKey;

    /// the current move dir
    Vector2 mMoveDir;

    /// the current squish velocity
    Vector3 mSquishVel = Vector3.zero;

    /// a buffer to raycast for bounce targets
    RaycastHit[] mBounceHits = new RaycastHit[3];

    /// the player's inputs
    PlayerInput.PlayerActions mInputs;

    // -- lifecycle --
    void Awake() {
        // set props
        mKey = new Key(mKeyOf);
        mInputs = new PlayerInput().Player;

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
        var i = mInputs;
        var b = mMoveBias;

        // aggregate biased dir
        var dir = Vector2.zero;
        if (i.Up.IsPressed()) {
            dir.y += b.Y2;
        }

        if (i.Down.IsPressed()) {
            dir.y += b.Y1;
        }

        if (i.Left.IsPressed()) {
            dir.x += b.X1;
        }

        if (i.Right.IsPressed()) {
            dir.x += b.X2;
        }

        // and store it for movement later
        mMoveDir = dir;
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
            var body = hit.rigidbody;
            body.AddForce(mBounceMag * v * dir);
        }
    }

    // -- queries --
    /// get the player's key
    public Key Key {
        get => mKey;
    }
}