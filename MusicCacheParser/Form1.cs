using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicCacheParser
{
    public partial class Form1 : Form
    {
        private MusicParser parser;
        private MusicCacheParserConfig.MusicParserConfig config;
        public MusicCacheParserConfig.MusicParserConfig Config { get => this.config; }
        private string[] args;
        private bool show_init = true;
        public Form1(string[] args)
        {
            String conf;
            InitializeComponent();
            if (!File.Exists("config.json")) {
                conf = UTF8Encoding.UTF8.GetString(Resource1.config);
                File.WriteAllText("config.json", conf);
            }
            else
            {
                conf = File.ReadAllText("config.json"); 
            }
            config=MusicCacheParserConfig.MusicParserConfig.FromJson(conf);
            parser = new MusicParser(this);
            this.args = args;
            if (args.Contains("hide"))
            {
                show_init = false;
            }
        }
        protected override void OnShown(EventArgs e)
        {
            if (show_init)
            {
                base.OnShown(e);
            }
            else
            {
                this.Hide();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ConfigForm cf = new ConfigForm(this);
            cf.Show();
            this.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void button3_Click(object sender, EventArgs e)
        {
            parse_all();
        }
        Object parse_all_lock = new object();
        private void parse_all()
        {
            lock (parse_all_lock)
            {
                ThreadPool.QueueUserWorkItem((cb) =>
                {
                    lock (parse_all_lock)
                    {
                        var files = Directory.GetFiles(parser.NeteaseCachePath);
                        foreach (var f in files)
                        {
                            if (f.EndsWith(".uc"))
                            {
                                try
                                {
                                    var nf = Path.GetDirectoryName(f) + "\\" + Path.GetFileNameWithoutExtension(f);
                                    parser.tryParseNeteaseFile(nf);
                                }
                                catch (Exception err)
                                {
                                    listBox1.Items.Add(err.ToString());
                                }
                            }
                        }
                    }
                });
            }
        }
        public void reload()
        {
            parser.end();
            parser = null;
            parser = new MusicParser(this);
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 退出QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close = true;
            Application.Exit();
        }

        private void 导出全部缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parse_all();
        }
        private bool close = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
