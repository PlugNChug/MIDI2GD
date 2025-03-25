namespace MIDI2GD
{
    class InstrumentMappings
    {
        public static string newInstrumentMappings = "# This file maps each MIDI channel to a different SFX ID, downloaded SFX will be located in \"C:/Users/user/AppData/Local/GeometryDash/\"\r\n\r\n# Synth Lead\r\n14219\r\n\r\n# Kick\r\n14186\r\n\r\n# Cymbal\r\n14181\r\n\r\n# Bass\r\n3937\r\n\r\n# Cymbal Crash\r\n3836";
        public static Dictionary<short, int[]> drumPad = new Dictionary<short, int[]> {   // Format is { key, { sfxID, start, fade-in, end, fade-out, speed, pitch } }
                { 35, new int[] { 14186, 0, 0, 0, 0, 0, 0 } },
                { 36, new int[] { 14186, 0, 0, 0, 0, 0, 0 } },
                { 37, new int[] { 14181, 0, 0, 0, 0, 0, 0 } },
                { 38, new int[] { 14182, 0, 0, 150, 250, 0, 0 } },
                { 39, new int[] { 14185, 0, 0, 170, 100, 0, 0 } },
                { 40, new int[] { 14183, 0, 0, 200, 250, 0, 0 } },
                { 41, new int[] { 3891, 0, 0, 0, 250, -8, 0 } },
                { 42, new int[] { 18682, 0, 0, 0, 0, 0, 0 } },
                { 43, new int[] { 3891, 0, 0, 0, 250, -6, 0 } },
                { 44, new int[] { 18680, 0, 0, 0, 0, 0, 0 } },
                { 45, new int[] { 3891, 0, 0, 0, 250, -4, 0 } },
                { 46, new int[] { 18713, 0, 0, 0, 0, 0, 0 } },
                { 47, new int[] { 3891, 0, 0, 0, 250, -2, 0 } },
                { 48, new int[] { 3891, 0, 0, 0, 250, 0, 0 } },
                { 49, new int[] { 3840, 0, 0, 0, 0, 0, 0 } },
                { 50, new int[] { 3891, 0, 0, 0, 250, 2, 0 } },
                { 51, new int[] { 18710, 0, 0, 0, 0, 0, 0 } },
                { 52, new int[] { 18711, 0, 0, 0, 0, 0, 0 } },
                { 53, new int[] { 18711, 0, 0, 0, 0, 0, 0 } },
                { 54, new int[] { 1120, 1460, 50, 1650, 150, 1, 0 } },
                { 55, new int[] { 3840, 0, 0, 0, 0, 0, 0 } },
                { 56, new int[] { 2832, 0, 0, 300, 50, 0, 0 } },
                { 57, new int[] { 3841, 0, 0, 0, 0, 0, 0 } },
                { 58, new int[] { 436, 50, 0, 0, 50, -6, 12 } },
                { 59, new int[] { 3906, 0, 0, 0, 0, 0, 0 } },
                { 60, new int[] { 18707, 0, 0, 0, 0, 12, 0 } },
                { 61, new int[] { 18707, 0, 0, 0, 0, 0, 0 } },
                { 62, new int[] { 18708, 0, 0, 100, 25, 6, 0 } },
                { 63, new int[] { 18705, 0, 0, 0, 0, 0, 0 } },
                { 64, new int[] { 18705, 0, 0, 0, 0, -3, 0 } },
                { 65, new int[] { 18697, 0, 0, 0, 0, 0, 0 } },
                { 66, new int[] { 18697, 0, 0, 125, 25, -6, 0 } },
                { 67, new int[] { 9851, 0, 0, 0, 0, -4, 0 } },
                { 68, new int[] { 9851, 0, 0, 0, 0, -9, 0 } },
                { 69, new int[] { 9924, 110, 0, 450, 200, 0, 0 } },
                { 70, new int[] { 9924, 110, 0, 450, 200, 4, 0 } },
                { 71, new int[] { 3582, 0, 0, 250, 200, 0, 0 } },
                { 72, new int[] { 3581, 0, 0, 80, 80, -5, 0 } },
                { 73, new int[] { 13667, 0, 0, 0, 0, 10, 0 } },
                { 74, new int[] { 13667, 0, 0, 0, 0, 0, 0 } },
                { 75, new int[] { 2317, 0, 0, 50, 0, 0, 0 } },
                { 76, new int[] { 439, 70, 0, 0, 25, 0, 0 } },
                { 77, new int[] { 439, 0, 0, 70, 25, 0, 0 } },
                { 78, new int[] { 20758, 50, 25, 170, 50, -11, 0 } },
                { 79, new int[] { 20758, 420, 25, 550, 50, -12, 2 } },
                { 80, new int[] { 4202, 150, 0, 250, 50, 10, 5 } },
                { 81, new int[] { 4202, 150, 0, 1800, 400, 10, 5 } },
            };

        public static Dictionary<int, int[]> instData = new Dictionary<int, int[]>() {    // Format is { sfxID, { root note, start, fade-in, end, fade-out, force stop? } }
            { 14219, new int[] { 72, 0, 0, 0, 50, 1 } },            // Silk Synth
            { 3930, new int[] { 62, 0, 0, 0, 50, 1 } },             // Mosquito Saw Synth
            { 3804, new int[] { 35, 0, 0, 0, 50, 0 } },             // Bass
            { 3948, new int[] { 75, 0, 0, 0, 50, 1 } },             // Electric Brass
            { 9628, new int[] { 72, 200, 50, 1020, 50, 0 } },      // Violin
            { 22236, new int[] { 58, 100, 50, 480, 50, 0 } },      // French Horn Hit
            { 18454, new int[] { 75, 0, 0, 320, 50, 0 } },         // Orchestral Hit
            { 4167, new int[] { 45, 0, 0, 1300, 50, 0 } },         // Guitar
            { 18462, new int[] { 72, 500, 0, 670, 50, 0 } },       // Short Piano Hit (High)
            { 4386, new int[] { 51, 0, 0, 350, 50, 0 } },          // Short Piano Hit (Low)
            { 4216, new int[] { 72, 83, 0, 0, 50, 0 } },            // Mallets
            { 4225, new int[] { 65, 50, 50, 1310, 50, 0 } },       // Organ
            { 20613, new int[] { 66, 0, 0, 0, 50, 0 } },            // Large Bell
        };
    }
}