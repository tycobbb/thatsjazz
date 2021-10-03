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

    [Tooltip("the player's input")]
    [SerializeField] PlayerInput mInputs;

    // -- inputs --
    /// the move input
    InputAction mMove;

    /// the squish input
    InputAction mSquish;

    // -- props --
    /// the current squish velocity
    Vector3 mSquishVel = Vector3.zero;

    // -- lifecycle --
    void Awake() {
        // find inputs
        var ia = mInputs.actions;
        mMove = ia.FindAction("Move");
        mSquish = ia.FindAction("Squish");
    }

    void FixedUpdate() {
        // squish the player
        Squish();

        // move the player
        Move();
    }

    // -- commands --
    /// move the player
    void Move() {
        var input = mMove.ReadValue<Vector2>();
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
        if (mSquish.IsPressed()) {
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