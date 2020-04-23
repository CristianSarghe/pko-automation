using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKO_BOT.Business
{
    public static class Helpers
    {
        public static void PrintBytes(byte[] array, Form form, RichTextBox textBox)
        {
            string str = "";

            foreach (byte b in array)
            {
                str += (b + " ");
            }

            form.Invoke(new Action(() =>
            {
                textBox.AppendText(str);
            }));
        }
    }
}
