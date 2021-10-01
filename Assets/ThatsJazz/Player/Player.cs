using UnityEngine;
using UnityEngine.InputSystem;

public class Player: MonoBehaviour {
    // -- tuning --
    [Header("tuning")]
    [Tooltip("the magnitude of the move acceleration")]
    [SerializeField] float mMoveMag = 2.0f;

    // -- nodes --
    [Header("nodes")]
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

    // -- lifecycle --
    void Awake() {
        // find inputs
        var ia = mInputs.actions;
        mMove = ia.FindAction("Move");
        mSquish = ia.FindAction("Squish");
    }

    void FixedUpdate() {
        // move the player
        Move();
    }

    // -- commands --
    /// move the character
    void Move() {
        var pos = mMove.ReadValue<Vector2>();
        var dir = new Vector3(pos.x, 0.0f, pos.y);

        mFoot.AddForceAtPosition(
            dir * mMoveMag,
            mMovePos.position,
            ForceMode.Acceleration
        );
    }
}