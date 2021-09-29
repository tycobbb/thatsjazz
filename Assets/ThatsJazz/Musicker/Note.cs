/// a western-musical note
public enum Note {
    C,
    CsDf,
    D,
    DsEf,
    E,
    F,
    FsGf,
    G,
    GsAf,
    A,
    AsBf,
    B,
}

// -- queries --
public static class NoteExt {
    /// converts this note into a tone
    public static Tone IntoTone(this Note note) {
        return new Tone((int)note);
    }
}