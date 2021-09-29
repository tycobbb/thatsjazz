/// a western-musical key
public readonly struct Key {
    // -- props --
    /// the root note for this key
    readonly Note mRoot;

    // -- lifetime --
    /// create a new key with a root
    public Key(Note root) {
        mRoot = root;
    }

    // -- queries --
    /// get a note for a tone
    public Note Note(Tone tone) {
        return (Note)(((int)mRoot + tone.NumSteps()) % 12);
    }

    /// get a chord for the tones
    public Chord Chord(params Tone[] tones) {
        // get the notes for these tones
        var n = tones.Length;
        var notes = new Note[n];

        for (var i = 0; i < n; i++) {
            notes[i] = Note(tones[i]);
        }

        // build a chord
        return new Chord(notes);
    }
}