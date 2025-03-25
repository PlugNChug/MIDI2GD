using GeometryDashAPI.Data;
using GeometryDashAPI.Levels;
using GeometryDashAPI.Levels.GameObjects.Triggers;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Threading.Channels;

// Default settings
string configName = @"InstrumentMappings.txt";
int defaultInst = 14219;
Dictionary<int, int> instAssigns = new Dictionary<int, int>() { 
    { 1, 14219 },
    { 2, 3930 },
    { 3, 3804 },
    { 4, 3948 },
    { 5, 9628 },
    { 6, 22236 },
    { 7, 18454 },
    { 8, 4167 },
    { 9, 18462 },
    { 10, 4386 },
    { 11, 4216 },
    { 12, 4225 },
    { 13, 20613 },
};

Dictionary<short, int> chanAssigns = new Dictionary<short, int>();

// Store paths
string levelName;
string midiPath;

int[] LoadInstrumentMappings() // Load SFX IDs from text file
{
    try
    {
        // Read existing mappings
        string[] fileData = File.ReadAllLines(configName);
        List<int> sfxIDs = new List<int>();

        foreach (string sfxID in fileData)
        {
            if (!sfxID.Contains('#') && sfxID.Length > 0)
            {
                sfxIDs.Add(int.Parse(sfxID));
            }
        }
        int idCount = sfxIDs.Count;
        for (int i = 0; i < 16 - idCount; i++)
        {
            sfxIDs.Add(defaultInst);
        }

        return sfxIDs.ToArray();
    }
    catch
    {
        // No mappings file located, so load the default mappings
        StreamWriter streamWriter = File.CreateText(configName);
        streamWriter.Write(MIDI2GD.InstrumentMappings.newInstrumentMappings);
        streamWriter.Close();

        return LoadInstrumentMappings();
    }
}

MidiFile AskForMIDI() // Get MIDI file path from user
{
    midiPath = Console.ReadLine().Replace("\"", string.Empty);
    MidiFile midiFile = new MidiFile();
    try
    {
        midiFile = MidiFile.Read(midiPath);
        foreach (short channel in midiFile.GetChannels().OrderBy(c => Int32.Parse(c.ToString())))
        {
            if (channel + 1 == 10)
            {
                continue;
            }
            Console.Clear();
            Console.Write(String.Format("Enter the number of the instrument to assign to channel "));
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(channel + 1);
            Console.ForegroundColor = defaultColor;
            Console.Write(String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n{9}\n{10}\n{11}\n{12}\n\n" +
                "Or, enter \"0\" to ignore this channel.\n" +
                "Instrument number:\n",
                "1 - \"Silk\" Synth Lead (ID 14219)", "2 - \"Mosquito\" Saw Synth Lead (ID 3930)", "3 - \"Tech\" Bass (ID 3804)",
                "4 - \"Crush\" Brass (ID 3948)", "5 - Violin (ID 9628)", "6 - French Horn Hit (ID 22236)",
                "7 - Orchestral Hit (ID 18454)", "8 - Guitar (ID 4167)", "9 - Piano Hit (High Notes) (ID 18462)", "10 - Piano Hit (Low Notes) (ID 4386)",
                "11 - Mallets (ID 4216)", "12 - Church Organ (ID 4225)", "13 - Large (Tubular) Bell (ID 20613)"));

            int inst = -1;
            while (!(int.TryParse(Console.ReadLine(), out inst) && (inst >= 0 && inst < instAssigns.Count() + 1)))
            {
                Console.Write("Invalid input; enter a number that's listed above.\n" + "Instrument number:");
            }
            if (inst == 0)
            {
                chanAssigns.Add(channel, -1);
                continue;
            }
            chanAssigns.Add(channel, instAssigns[inst]);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Error loading the midi. Error:");
        Console.WriteLine(e.Message);
        Console.WriteLine("Path to MIDI file:");
        AskForMIDI();
    }
    return midiFile;
}
Level AskForLevel(LocalLevels local) // Get Level name from user
{
    levelName = Console.ReadLine();
    Level level = new Level();
    try
    {
        level = local.GetLevel(levelName, revision: 0).LoadLevel();
    }
    catch (Exception e)
    {
        Console.WriteLine("Error loading level. Does it exist in-game? Error:");
        Console.WriteLine(e.Message);
        Console.WriteLine("Name of level (must be in your created levels):");
        AskForLevel(local);
    }
    return level;
}

void ImportToLevel(MidiFile midiFile, Level level, LocalLevels local) // Import MIDI notes as SFX objects in Level
{
    IEnumerable<Note> notes = midiFile.GetNotes();
    int[] sfxIDs = LoadInstrumentMappings();
    TempoMap tempoMap = midiFile.GetTempoMap();

    Console.WriteLine("Enter the X offset integer that the notes will begin on (leave blank for the start of the level)");
    int xOffset = 0;
    xOffset = int.TryParse(Console.ReadLine(), out int x) ? x : 0;

    Console.WriteLine("Enter the whole number percentage (for example, entering \"45\" represents 45% volume) of the overall volume of the output (leave blank for 100%)");
    int volMod = 0;
    volMod = int.TryParse(Console.ReadLine(), out int v) ? v : 0;
    if (volMod < 0 || volMod > 100)
    {
        volMod = 100;
    }

    int uniqueId = 1;
    foreach (Note note in notes)
    {
        if (uniqueId % 11 == 0)
            uniqueId = 1;
        else
            uniqueId++;

        // Console.WriteLine(String.Format("Pitch: {0} {1} Volume: {2} Channel: {3}", note.NoteName, note.Octave, note.Velocity, note.Channel));

        MIDI2GD.InsertSFX.NoteToSFX(chanAssigns, tempoMap, level, note, uniqueId, xOffset, volMod);
    }
}

float ToSecondsValue(MetricTimeSpan metricTime) // Convert metric time to seconds only value
{
    return (metricTime.Hours * 3600) + (metricTime.Minutes * 60) + metricTime.Seconds + (((float)metricTime.Milliseconds) / 1000);
}

// Do the stuff
Console.WriteLine("Loading local levels...");
LocalLevels local = LocalLevels.LoadFile();

Console.WriteLine("Name of level (must be in your created levels):");
Level level = AskForLevel(local);

Console.WriteLine("Path to MIDI file:");
MidiFile midiFile = AskForMIDI();

Console.WriteLine("Working...");
ImportToLevel(midiFile, level, local);

Console.WriteLine("Saving level...");
local.GetLevel(levelName, revision: 0).SaveLevel(level);
local.Save();

Console.WriteLine("Done!");
// Done doing the stuff
