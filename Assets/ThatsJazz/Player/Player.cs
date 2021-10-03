using UnityEngine;

public class Player: MonoBehaviour {
    // -- tuning --
    [Header("tuning")]
    [Tooltip("the magnitude of the move acceleration")]
    [SerializeField] float mMoveMag = 2.0f;

    [Tooltip("the scale when normal. time is duration.")]
    [SerializeField] Linear<Vector3> mScaleDefault = new Linear<Vector3>(
        dst: Vector3.one,
        time: 3.0f
    );

    [Tooltip("the scale when squished. time is duration.")]
    [SerializeField] Linear<Vector3> mScaleSquished = new Linear<Vector3>(
        dst: new Vector3(1.2f, 0.7f, 1.2f),
        time: 1.0f
    );

    [Tooltip("the player's musical key")]
    [SerializeField] Root mRoot = Root.C;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the player's model transform")]
    [SerializeField] Transform mModel;

    [Tooltip("the player's foot's rigidbody")]
    [SerializeField] Rigidbody mFoot;

    [Tooltip("the pos where the move is applied")]
    [SerializeField] Transform mMovePos;

    // -- props --
    /// the musical key
    Key mKey;

    /// the current move dir
    Vector3 mMoveDir;

    /// the current squish velocity
    Vector3 mSquishVel = Vector3.zero;

    /// the player's inputs
    PlayerInput.PlayerActions mInputs;

    // -- lifecycle --
    void Awake() {
        // set props
        mKey = new Key(mRoot);
        mInputs = new PlayerInput().Player;
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
    }

    void OnDisable() {
        mInputs.Disable();
    }

    // -- commands --
    /// read move inputs
    void ReadMove() {
        var input = mInputs.Move.ReadValue<Vector2>();
        mMoveDir.x = input.x;
        mMoveDir.z = input.y;
    }

    /// move the player
    void Move() {
        mFoot.AddForceAtPosition(
            mMoveDir * mMoveMag,
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

    // -- queries --
    /// get the player's key
    public Key Key {
        get => mKey;
    }
}