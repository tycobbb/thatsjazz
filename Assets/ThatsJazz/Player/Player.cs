using UnityEngine;
using UnityEngine.InputSystem;

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

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the player's root transform")]
    [SerializeField] Transform mRoot;

    [Tooltip("the player's foot's rigidbody")]
    [SerializeField] Rigidbody mFoot;

    [Tooltip("the pos where the move is applied")]
    [SerializeField] Transform mMovePos;

    // -- props --
    /// the player's inputs
    PlayerInput.PlayerActions mInputs;

    /// the current squish velocity
    Vector3 mSquishVel = Vector3.zero;

    // -- lifecycle --
    void Awake() {
        // set props
        mInputs = new PlayerInput().Player;
    }

    void OnEnable() {
        mInputs.Enable();
    }

    void FixedUpdate() {
        // squish the player
        Squish();

        // move the player
        Move();
    }

    void OnDisable() {
        mInputs.Disable();
    }

    // -- commands --
    /// move the player
    void Move() {
        var input = mInputs.Move.ReadValue<Vector2>();
        var dir = new Vector3(input.x, 0.0f, input.y);

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
            mRoot.localScale,
            scale.Dst,
            ref mSquishVel,
            scale.Time
        );

        // update scale
        mRoot.localScale = next;
    }
}