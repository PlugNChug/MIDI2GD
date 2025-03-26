using GeometryDashAPI.Data;
using GeometryDashAPI.Levels;
using GeometryDashAPI.Levels.GameObjects.Triggers;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace MIDI2GD
{
    class InsertSFX
    {

        public static void NoteToSFX(Dictionary<short, int> chanAssigns, TempoMap tempoMap, Level level, Note note, int uniqueId, int xOffset, int volMod)
        {
            MetricTimeSpan metricTime = note.TimeAs<MetricTimeSpan>(tempoMap);
            MetricTimeSpan metricLength = note.LengthAs<MetricTimeSpan>(tempoMap);
            BarBeatTicksTimeSpan time = note.TimeAs<BarBeatTicksTimeSpan>(tempoMap);
            BarBeatTicksTimeSpan length = note.LengthAs<BarBeatTicksTimeSpan>(tempoMap);
            float volume = note.Velocity / 128f * (volMod / 100f);
            float xPos = ToXOffset(time) + xOffset;
            float xLength = ToXOffset(length);
            float yPos = (note.NoteNumber - 60) * 60 + 15;

            if (note.Channel + 1 == 10) // MIDI channel 10 is usually reserved for drums
            {
                Console.WriteLine(note.NoteName + "" + note.Octave + " " + time.ToString());
                try
                {
                    int[] drumData = InstrumentMappings.drumPad[note.NoteNumber];
                    
                    level.AddBlock(new SfxTrigger()
                    {
                        EditorL = (Int16)(note.Channel + 10),
                        EditorL2 = 99,
                        PositionX = xPos,
                        PositionY = yPos,
                        Volume = volume,
                        Groups = new int[] { note.Channel + 1 },
                        SongId = drumData[0],
                        Start = drumData[1],
                        FadeIn = drumData[2],
                        End = drumData[3],
                        FadeOut = drumData[4],
                        Speed = drumData[5],
                        Pitch = drumData[6]
                    });
                } catch
                {
                    Console.WriteLine("Invalid note, skipping...");
                }
            }
            else
            {
                if (chanAssigns[note.Channel] == -1) return;

                int[] instrumentData = InstrumentMappings.instData[chanAssigns[note.Channel]];
                int speed = Math.Min(12, Math.Max(-12, note.NoteNumber - instrumentData[0]));
                int pitchDist = note.NoteNumber - instrumentData[0];
                int pitch = (pitchDist / 12 > 0 || pitchDist / 12 < 0) ? pitch = pitchDist % 12 : 0;
                int prevUnique = uniqueId - 1 <= 0 ? 10 : uniqueId - 1;

                level.AddBlock(new EditSfxTrigger()
                {
                    EditorL = (Int16)(note.Channel + 10),
                    EditorL2 = 99,
                    PositionX = xPos - 2f,
                    PositionY = yPos,
                    Groups = new int[] { note.Channel + 1 },
                    // GroupId = note.Channel + 1,
                    SfxGroup = 2000 + note.Channel + note.NoteNumber,
                    // UniqueId = prevUnique,
                    ChangeVolume = true,
                    Volume = volume,
                    Stop = Convert.ToBoolean(instrumentData[5]),
                });
                level.AddBlock(new SfxTrigger()
                {
                    EditorL = (Int16)(note.Channel + 10),
                    EditorL2 = 99,
                    PositionX = xPos,
                    PositionY = yPos,
                    Pitch = pitch,
                    Speed = speed,
                    SongId = chanAssigns[note.Channel],
                    Volume = volume,
                    Groups = new int[] { note.Channel + 1 },
                    SfxGroup = 2000 + note.Channel + note.NoteNumber,
                    // UniqueId = uniqueId,
                    Start = instrumentData[1],
                    FadeIn = instrumentData[2],
                    End = Convert.ToBoolean(instrumentData[5]) ? (int)((1 + (speed / 12f)) * metricLength.Milliseconds) : instrumentData[3],
                    FadeOut = instrumentData[4],

                });
                level.AddBlock(new EditSfxTrigger()
                {
                    EditorL = (Int16)(note.Channel + 10),
                    EditorL2 = 99,
                    PositionX = xPos + xLength - 2f,
                    PositionY = yPos,
                    Groups = new int[] { note.Channel + 1 },
                    // GroupId = note.Channel + 1,
                    SfxGroup = 2000 + note.Channel + note.NoteNumber,
                    ChangeVolume = true,
                    Volume = 0f,
                    Duration = 0.1f
                    // Stop = true
                });
            }
        }

        static float ToSecondsValue(MetricTimeSpan metricTime) // Convert metric time to seconds only value
        {
            return (metricTime.Hours * 3600) + (metricTime.Minutes * 60) + metricTime.Seconds + (((float)metricTime.Milliseconds) / 1000);
        }

        static float ToXOffset(BarBeatTicksTimeSpan pos)
        {
            return (float)(pos.Bars * 16 + pos.Beats * 4 + (Math.Floor(pos.Ticks / 10f) * 10 / 480f) * 8) * 60 + 15;
        }
    }
}
