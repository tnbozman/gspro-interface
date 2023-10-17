using Microsoft.VisualBasic.Logging;
using OcrChangeDetection.Models;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OcrChangeDetection.UI
{
    public partial class Form1 : Form
    {
        private bool _isDrawing;
        private Point? _startPoint = null;
        private RegionConfig _currentRegion = null;
        private List<RegionConfig> _regions = new List<RegionConfig>();
        private RegionConfig _trigger;
        private Bitmap _desktopScreenshot;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.Paint += Form1_Paint;

            _desktopScreenshot = CaptureDesktop();
            this.BackgroundImage = _desktopScreenshot;
            this.WindowState = FormWindowState.Maximized; // covers the entire screen
            this.FormBorderStyle = FormBorderStyle.None;
        }

        public Bitmap CaptureDesktop()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            return screenshot;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Check if the 'R' key is pressed
            if (keyData == Keys.R)
            {
                RefreshScreenshot();
                return true;  // indicate that you handled the key
            }else if (keyData == Keys.M)
            {
                if(this.FormBorderStyle == FormBorderStyle.None)
                {
                    this.FormBorderStyle = FormBorderStyle.FixedDialog;
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                }
                
                return true;  // indicate that you handled the key
             }
            else if (keyData == Keys.X) {
                this.Close();
            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void RefreshScreenshot()
        {
            if (_desktopScreenshot != null)
            {
                _desktopScreenshot.Dispose();
            }

            _desktopScreenshot = CaptureDesktop();
            this.BackgroundImage = _desktopScreenshot;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            _startPoint = e.Location;
            _isDrawing = true;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing && _startPoint.HasValue)
            {
                _currentRegion = CreateRegion(_startPoint.Value, e);
                Invalidate();
            }
        }

        private RegionConfig CreateRegion(Point point, MouseEventArgs e)
        {
            return new RegionConfig
            {
                X = point.X,
                Y = point.Y,
                Width = e.X - point.X,
                Height = e.Y - point.Y
            };
        }

        private RegionConfig UpdateRegion(RegionConfig region, Point point, MouseEventArgs e)
        {
            if(region == null)
            {
                return CreateRegion(point, e);
            }
            region.Width = e.X - point.X;
            region.Height = e.Y - point.Y;
            return region;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDrawing && _startPoint.HasValue)
            {
                _currentRegion = UpdateRegion(_currentRegion, _startPoint.Value, e);

                // Pop up the label input dialog
                using (var dialog = new LabelInputDialog())
                {
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        // Store the entered label with the region
                        _currentRegion.Label = dialog.EnteredLabel; // Assuming you've exposed the TextBox value through a property named EnteredLabel in LabelInputDialog
                    }
                    else
                    {
                        // If the user cancels the label input, you can decide to discard the region or keep it without a label
                        _currentRegion.Label = "Unnamed";
                    }

                    if(_currentRegion.Label == "trigger")
                    {
                        _trigger = _currentRegion;
                    }
                }

                _regions.Add(_currentRegion);
                _currentRegion = null;

                _isDrawing = false;
                this.Invalidate();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var region in _regions)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Blue)), region.X, region.Y, region.Width, region.Height);
                e.Graphics.DrawRectangle(Pens.Blue, region.X, region.Y, region.Width, region.Height);
                e.Graphics.DrawString(region.Label, this.Font, Brushes.White, region.X, region.Y - 20); // -20 positions the label above the rectangle
            }

            if (_trigger != null)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Yellow)), _trigger.X, _trigger.Y, _trigger.Width, _trigger.Height);
                e.Graphics.DrawRectangle(Pens.Yellow, _trigger.X, _trigger.Y, _trigger.Width, _trigger.Height);
                e.Graphics.DrawString(_trigger.Label, this.Font, Brushes.White, _trigger.X, _trigger.Y - 20); // -20 positions the label above the rectangle
            }

            if (_currentRegion != null)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Red)), _currentRegion.X, _currentRegion.Y, _currentRegion.Width, _currentRegion.Height);
                e.Graphics.DrawRectangle(Pens.Red, _currentRegion.X, _currentRegion.Y, _currentRegion.Width, _currentRegion.Height);
                e.Graphics.DrawString(_currentRegion.Label, this.Font, Brushes.White, _currentRegion.X, _currentRegion.Y - 20);
            }
        }

    }
}