using System.Linq;

/// a chord w/ a key and quality
public readonly struct Chord {
    // -- props --
    /// the chord tones
    readonly Tone[] mTones;

    // -- lifetime --
    /// create a chord from a list of tones
    public Chord(params Tone[] tones) {
        mTones = tones;
    }

    // -- queries --
    /// the number of notes in this chord
    public int Length {
        get => mTones.Length;
    }

    /// the tone at the position
    public Tone this[int i] {
        get => mTones[i];
    }

    // -- debugging --
    public override string ToString() {
        return string.Join(" ", mTones.Select((n) => n.ToString()));
    }
}