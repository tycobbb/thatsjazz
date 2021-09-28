using UnityEngine;

/// produces any note in the chromatic scale
public class Instrument: MonoBehaviour {
    // -- config --
    /// the chromatic scale (usually C3-based), must have 12 notes
    [SerializeField] AudioClip[] mScale;

    // -- lifecycle --
    void Awake() {
        if (mScale.Length != 12) {
            Debug.LogError($"{this} did not contain 12 notes");
        }
    }

    // -- queries --
    public AudioClip AnyNote() {
        return mScale[Random.Range(0, mScale.Length)];
    }
}