using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicCacheParserConfig;
namespace MusicCacheParser
{
    public partial class ConfigForm : Form
    {
        private MusicParserConfig config;
        private Form1 f1;
        public ConfigForm(Form1 f1)
        {
            config = f1.Config;
            InitializeComponent();
            loadconf();
            this.f1 = f1;
                
        }
        private void loadconf()
        {
            foreach(Format f in config.Formats)
            {
                addFormat(f.Type, f.Enabled); 
            }
            tempPath.Text = config.CustomTmpPath;
            savePath.Text = config.SavePath;
            saveFilename.Text = config.SaveFileName;
            neteaseAutoParse.Checked = config.NeteaseMusic.AutoParse;
            neteaseCachePath.Text = config.NeteaseMusic.CachePath;
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
            Close();
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            foreach (var f in flowLayoutPanel1.Controls)
            {
                foreach (var f2 in config.Formats)
                {
                    if (((CheckBox)f).Text == f2.Type)
                    {
                        f2.Enabled = ((CheckBox)f).Checked;
                        break;
                    }
                }
            }
            config.CustomTmpPath = tempPath.Text;
            config.SavePath = savePath.Text;
            config.SaveFileName = saveFilename.Text;
            config.NeteaseMusic.AutoParse = neteaseAutoParse.Checked;
            config.NeteaseMusic.CachePath = neteaseCachePath.Text;
            File.WriteAllText("config.json", MusicCacheParserConfig.Serialize.ToJson(config));
            f1.reload();
            f1.Enabled = true;
        }
    }
}
