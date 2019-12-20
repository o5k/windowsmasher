using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WindowSmasher {
    class Program {
        static void Main(string[] args) {
            if (!File.Exists("zones.txt")) {
                File.WriteAllText("zones.txt", "# Zones file for WindowSmasher\n#\n# Syntax: <profile number><letter> <width>x<height> <x>,<y>\n# Example: 1A 1920x540 0,0");
            }
            List<Preset> Presets = ReadConfig("zones.txt");
            ZoneDisplay zd = new ZoneDisplay(Presets);

            Application.Run(zd);
        }

        /// <summary>
        /// Reads the config.
        /// </summary>
        /// <param name="filename">The path of the file to read the config from.</param>
        /// <returns>A list of Presets that match this config.</returns>
        public static List<Preset> ReadConfig(string filename) {
            List<Preset> presets = new List<Preset>();

            string[] lines = File.ReadAllLines(filename);

            foreach (string line in lines) {
                if (line.StartsWith("#")) {
                    continue;
                }

                string[] bits = line.Split(' ');
                if (bits.Length != 3) {
                    continue;
                }

                // Segment 1: Preset # and zone letter
                char presetNum = bits[0][0];
                char letter = bits[0][1];

                // Segment 2: Size
                string[] size = bits[1].Split('x');
                if (size.Length != 2) {
                    continue;
                }
                int width = int.Parse(size[0]);
                int height = int.Parse(size[1]);

                // Segment 3: Position
                string[] pos = bits[2].Split(',');
                if (pos.Length != 2) {
                    continue;
                }
                int left = int.Parse(pos[0]);
                int top = int.Parse(pos[1]);

                // Create the Zone
                Zone z = new Zone(letter, new System.Drawing.Rectangle(left, top, width, height));

                // Attach it to an existing preset # if it exists, else create one
                Preset p = presets.Find(o => o.Number == presetNum);
                if (p != null) {
                    p.Zones.Add(z);
                } else {
                    p = new Preset(presetNum);
                    p.Zones.Add(z);
                    presets.Add(p);
                }
            }

            return presets;
        }
    }
}
