using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowSmasher {
    /// <summary>
    /// A collection of Zones.
    /// </summary>
    public class Preset {
        /// <summary>
        /// The preset's number.
        /// </summary>
        public char Number { get; private set; }
        /// <summary>
        /// The zones in this preset.
        /// </summary>
        public List<Zone> Zones { get; private set; }
        /// <summary>
        /// Construct a new preset.
        /// </summary>
        /// <param name="number">The preset number.</param>
        public Preset(char number) {
            Number = number;
            Zones = new List<Zone>();
        }

        /// <summary>
        /// Render a preset into a full-size display of all the zones in it.
        /// </summary>
        /// <returns>A Bitmap which you should dispose else it'll leak memory thanks</returns>
        public Bitmap Render() {
            // Save the environment! Please dispose the Bitmap after use. Thank you!
            Rectangle s = SystemInformation.VirtualScreen;
            
            Bitmap b = new Bitmap(s.Width, s.Height);
            using (Graphics g = Graphics.FromImage(b)) {
                g.FillRectangle(Brushes.Magenta, 0, 0, b.Width, b.Height);

                using (Pen p = new Pen(Color.FromArgb(0x00, 0x5A, 0xFF), 16)) {
                    using (Brush br = new SolidBrush(Color.FromArgb(0xDF, 0xEA, 0xFF))) {
                        foreach (Zone z in Zones) {
                            g.DrawRectangle(p, new Rectangle(z.Bounds.Left + 8, z.Bounds.Top + 8, z.Bounds.Width - 16, z.Bounds.Height - 16));
                            g.DrawString($"{Number}{z.Letter}", SystemFonts.CaptionFont, br, z.Bounds.Location);
                        }
                    }
                }  
            }

            return b;
        }
    }
}
