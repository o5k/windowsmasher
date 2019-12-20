using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WindowSmasher {
    /// <summary>
    /// A zone which a window can be snapped into.
    /// </summary>
    public class Zone {
        /// <summary>
        /// The letter belonging to this zone.
        /// </summary>
        public char Letter { get; private set; }
        /// <summary>
        /// This zone's position and size.
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Construct a new Zone.
        /// </summary>
        /// <param name="letter">The letter for this Zone.</param>
        /// <param name="bounds">The position and size of this Zone.</param>
        public Zone(char letter, Rectangle bounds) {
            Letter = letter;
            Bounds = bounds;
        }
    }
}
