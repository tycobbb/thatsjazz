using System.Linq;

/// a chord w/ a key and quality
public readonly struct Chord {
    // -- props --
    /// the list of notes
    readonly Note[] mNotes;

    // -- lifetime --
    /// create a chord from a key and tones
    public Chord(Note[] notes) {
        mNotes = notes;
    }

    // -- queries --
    /// the number of notes in this chord
    public int Length {
        get => mNotes.Length;
    }

    /// get the note at the index
    public Note FindNote(int i) {
        return mNotes[i];
    }

    // -- debugging --
    public override string ToString() {
        return string.Join(" ", mNotes.Select((n) => n.ToString()));
    }
}