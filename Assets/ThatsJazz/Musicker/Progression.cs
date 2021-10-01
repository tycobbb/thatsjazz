/// a chord progression
public struct Progression {
    // -- props --
    /// the index of the curren chord
    int mCurr;

    /// the chords in this progression
    readonly Chord[] mChords;

    // -- lifetime --
    /// create a new progression
    public Progression(params Chord[] chords) {
        mCurr = 0;
        mChords = chords;
    }

    // -- commands --
    /// move to the next chord
    public void Advance() {
        var next = mCurr + 1;
        mCurr = next % mChords.Length;
    }

    // -- queries --
    /// get the current chord
    public Chord Curr() {
        return mChords[mCurr];
    }
}