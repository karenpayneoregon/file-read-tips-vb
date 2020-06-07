using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsControls
{
    public class NumericTextBox : TextBox
    {
        private int WM_KEYDOWN = 0x0100;
        int WM_PASTE = 0x0302;
        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == WM_KEYDOWN)
            {
                var keys = (Keys)msg.WParam.ToInt32();

                bool numbers = ((keys >= Keys.D0 && keys <= Keys.D9) ||
                                (keys >= Keys.NumPad0 && keys <= Keys.NumPad9)) &&
                               ModifierKeys != Keys.Shift;

                bool ctrl = keys == Keys.Control;

                bool ctrlZ = keys == Keys.Z && ModifierKeys == Keys.Control,
                    ctrlX = keys == Keys.X && ModifierKeys == Keys.Control,
                    ctrlC = keys == Keys.C && ModifierKeys == Keys.Control,
                    ctrlV = keys == Keys.V && ModifierKeys == Keys.Control,
                    del = keys == Keys.Delete,
                    bksp = keys == Keys.Back,
                    arrows = (keys == Keys.Up)
                    | (keys == Keys.Down)
                    | (keys == Keys.Left)
                    | (keys == Keys.Right);

                if (numbers | ctrl | del | bksp | arrows | ctrlC | ctrlX | ctrlZ)
                {
                    return false;
                }

                else if (ctrlV)
                {
                    // handle pasting from clipboard
                    var clipboardData = Clipboard.GetDataObject();
                    var input = (string)clipboardData.GetData(typeof(string));
                    foreach (var character in input)
                    {
                        if (!char.IsDigit(character)) return true;
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return base.PreProcessMessage(ref msg);
            }
        }
        /// <summary>
        /// Get int value from Text property or 0 if not an int
        /// </summary>
        [Browsable(false)]
        public int AsInteger => int.TryParse(Text, out var value) ? value : 0;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PASTE)
            {
                var clipboardData = Clipboard.GetDataObject();
                var input = (string)clipboardData.GetData(typeof(string));
                foreach (var character in input)
                {
                    if (!char.IsDigit(character))
                    {
                        m.Result = (IntPtr)0;
                        return;
                    }
                }
            }
            base.WndProc(ref m);
        }
    }
}
