/// a western-musical key
public readonly struct Key {
    // -- props --
    /// the root tone for this key
    readonly Tone mRoot;

    // -- lifetime --
    /// create a new key with the root
    public Key(Tone root) {
        mRoot = root;
    }

    public Key(Root root) {
        mRoot = new Tone((int)root);
    }

    // -- queries --
    /// transpose tone to this key
    public Tone Transpose(Tone tone) {
        return tone.From(mRoot);
    }

    /// get a chord for the tones
    public Chord Chord(Tone root, Quality quality) {
        // get root note in key
        root = root.From(mRoot);

        // get the tones w/ this root and quality
        var n = quality.Length;
        var tones = new Tone[n];

        for (var i = 0; i < n; i++) {
            var tone = quality[i];
            tones[i] = tone.From(root);
        }

        // build a chord
        return new Chord(tones);
    }
}