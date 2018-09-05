using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicCacheParser
{
    public partial class Config : Form
    {
        private MusicCacheParserConfig.MusicParserConfig config;
        public Config(Form1 f1)
        {
            config = f1.Config;
            InitializeComponent();
        }

        private void addFormat(String format,bool v)
        {
            var box = new CheckBox();
            box.Text = format;
            box.Checked = v;
            flowLayoutPanel1.Controls.Add(box);
        }

        private void Config_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
