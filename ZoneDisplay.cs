using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowSmasher {
    /// <summary>
    /// The form that displays the zones and handles the keybinds. In other words, it does everything.
    /// </summary>
    public partial class ZoneDisplay : Form {
        List<Preset> presets;
        char currentPreset = '1';
        Bitmap image;
        int ActivityTimer = 0;

        public ZoneDisplay(List<Preset> p) {
            InitializeComponent();
            presets = p;
        }

        private void ZoneDisplay_Load(object sender, EventArgs e) {
            Refit();

            int initialStyle = WINAPI.GetWindowLong(Handle, -20);
            WINAPI.SetWindowLong(Handle, -20, initialStyle | 0x80000 | 0x20);
            ChangePresetImage();
            Ping();

            WINAPI.RegisterHotKey(Handle, 0, WINAPI.MOD_WIN | WINAPI.MOD_NOREPEAT, 0x6E); // WIN + PERIOD // Flash zones
            WINAPI.RegisterHotKey(Handle, 1, WINAPI.MOD_WIN | WINAPI.MOD_NOREPEAT, 0x0D); // WIN + ENTER  // Attach active window to cursor's zone

            // This is stupid, but why cares
            for (int i = 0; i < 10; i++) {
                WINAPI.RegisterHotKey(Handle, 10 + i, WINAPI.MOD_WIN | WINAPI.MOD_NOREPEAT, 0x60 + i); // WIN + NUMKEY(i) // Switch to preset i
            }
        }

        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x0312) {
                int id = m.WParam.ToInt32();

                if (id == 0) {
                    // Win-Period
                    Refit();
                    Ping();
                }
                if (id == 1) {
                    // Win-Enter
                    Refit();
                    Attach();
                    Ping();
                }

                if (id >= 10 && id < 20) {
                    // Switch to preset id-10
                    Refit();
                    currentPreset = (id - 10).ToString()[0];
                    ChangePresetImage();
                    Ping();
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Relocates the ZoneDisplay to its proper position.
        /// </summary>
        void Refit() {
            Location = SystemInformation.VirtualScreen.Location;
            Size = SystemInformation.VirtualScreen.Size;
        }

        /// <summary>
        /// Begin attaching the current window to the pointed-at Zone.
        /// </summary>
        void Attach() {
            // Get the active window.
            IntPtr hWnd = WINAPI.GetForegroundWindow();

            // Get cursor position.
            Point c = PointToClient(Cursor.Position);

            // Get the first rectangle that matches.
            Preset p = presets.Find(o => o.Number == currentPreset);
            if (p != null) {
                foreach (Zone z in p.Zones) {
                    if (c.X >= z.Bounds.Left && c.X < z.Bounds.Right && c.Y >= z.Bounds.Top && c.Y < z.Bounds.Bottom) {
                        // It matches, attach it!
                        Point screenPoint = PointToScreen(new Point(z.Bounds.X, z.Bounds.Y));
                        WINAPI.SetWindowPos(hWnd, IntPtr.Zero, screenPoint.X, screenPoint.Y, z.Bounds.Width, z.Bounds.Height, WINAPI.SetWindowPosFlags.DoNotChangeOwnerZOrder | WINAPI.SetWindowPosFlags.IgnoreZOrder);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a new image and updates the form to match.
        /// </summary>
        public void ChangePresetImage() {
            Preset p = presets.Find(o => o.Number == currentPreset);
            if (p == null) {
                p = new Preset(currentPreset);
            }

            UpdateImage(p.Render());
        }

        /// <summary>
        /// Updates the form with a given Bitmap.
        /// </summary>
        /// <param name="b">The new Bitmap.</param>
        public void UpdateImage(Bitmap b) {
            if (image != null) {
                image.Dispose(); // thank you!
            }
            image = b;
            pictureBox.Image = image;
        }

        /// <summary>
        /// Flash the visible Zones.
        /// </summary>
        public void Ping() {
            ActivityTimer = 6;
            fadeTimer.Enabled = true;
            if (!Visible) {
                Show();
            }
        }

        private void fadeTimer_Tick(object sender, EventArgs e) {
            ActivityTimer--;
            if (ActivityTimer == 0) {
                fadeTimer.Enabled = false;
                Hide();
            }
        }
    }
}
