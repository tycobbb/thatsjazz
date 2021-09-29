/// a western-musical tone
public enum Tone {
    I,
    II,
    III,
    IV,
    V,
    VI,
    VII,
}

// -- queries --
public static class ToneExt {
    /// get the number of steps for a given tone
    public static int NumSteps(this Tone tone) {
        return tone switch {
            Tone.I => 0,
            Tone.II => 2,
            Tone.III => 4,
            Tone.IV => 5,
            Tone.V => 7,
            Tone.VI => 9,
            Tone.VII => 11,
            _ => 0,
        };
    }
}