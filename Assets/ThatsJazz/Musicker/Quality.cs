/// a chord quality
public struct Quality {
    // -- props --
    /// the tones in this quality
    Tone[] mTones;

    // -- lifetime --
    /// create a quality w/ the tones
    public Quality(params Tone[] tones) {
        mTones = tones;
    }

    // -- queries --
    /// the number of tones in he quality
    public int Length {
        get => mTones.Length;
    }

    /// the tone at the position
    public Tone this[int i] {
        get => mTones[i];
    }

    // -- factories --
    /// a perfect fifth (w/ octave)
    public static Quality P5 {
        get => new Quality(
            Tone.I,
            Tone.V,
            Tone.I.Octave()
        );
    }

    /// a major 7th chord quality
    public static Quality Maj7 {
        get => new Quality(
            Tone.I,
            Tone.III,
            Tone.V,
            Tone.VII
        );
    }

    /// a dominant 7th chord quality
    public static Quality Dom7 {
        get => new Quality(
            Tone.I,
            Tone.III,
            Tone.V,
            Tone.VII.Flat()
        );
    }

    /// a dominant 7th chord quality
    public static Quality Min7 {
        get => new Quality(
            Tone.I,
            Tone.III.Flat(),
            Tone.V,
            Tone.VII.Flat()
        );
    }

    /// a dominant 7th chord quality
    public static Quality Min7Flat5 {
        get => new Quality(
            Tone.I,
            Tone.III.Flat(),
            Tone.V.Flat(),
            Tone.VII.Flat()
        );
    }

    /// a dominant 7th chord quality
    public static Quality Dim7 {
        get => new Quality(
            Tone.I,
            Tone.III.Flat(),
            Tone.V.Flat(),
            Tone.VII.Flat(2)
        );
    }
}