using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicCacheParser
{
    class MusicFileInfo
    {
        private String format;
        private double volume;
        private int size;
        private String id;

        public string Format { get => format; set => format = value; }
        public double Volume { get => volume; set => volume = value; }
        public int Size { get => size; set => size = value; }
        public string Id { get => id; set => id = value; }
    }
    class MusicInf
    {
        
    }
    class MusicParser
    {
        public MusicParser(Form1 f1)
        {
            this.form1 = f1;
            init();
        }
        private MusicCacheParserConfig.MusicParserConfig config;
        private static readonly HttpClient client = new HttpClient();
        private Form1 form1;
        private ConcurrentDictionary<String,MusicFileInfo> neteaseInfoMap=new ConcurrentDictionary<string, MusicFileInfo>();
        public const byte NETEASE_CODE = 0xA3;
        public const string NETEASE_DETAIL_INFO = "http://music.163.com/api/song/detail/?ids=%5B{0:G}%5D";
        public const string NETEASE_LRYRIC = "http://music.163.com/api/song/lyric?os=pc&id={0:G}&lv=-1&kv=-1&tv=-1";
        private delegate void addToList(string text);
        private addToList methodAddToList;
        private string tmpPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
        //private string outputPath = "Z:\\自动导出\\";
        private string neteaseCachePath = @"%LocalAppData%\\Netease\\CloudMusic\\Cache";
        //public string OutputPath { get => outputPath; set => outputPath = value; }
        public string NeteaseCachePath { get => neteaseCachePath; set => neteaseCachePath = value; }
        private FileSystemWatcher neteaseFSW=new FileSystemWatcher();
        private void init()
        {
            config = form1.Config;
            NeteaseCachePath = System.Environment.ExpandEnvironmentVariables(config.NeteaseMusic.CachePath+"\\Cache");
            methodAddToList = t =>
            {
                form1.ListBox1.Items.Add(t);
            };
            tmpPath = config.CustomTmpPath != "" ? tmpPath : System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
            Directory.CreateDirectory(tmpPath);
            try
            {
                if (config.NeteaseMusic.AutoParse)
                {
                    startAutoParser_netease();
                }
            }
            catch (Exception e) { }
            
        }
        public void end()
        {
            neteaseFSW.EnableRaisingEvents = false;
            neteaseFSW.Dispose();
        }
        ~MusicParser()
        {
        }
        public void startAutoParser_netease()
        {
            neteaseFSW.BeginInit();
            neteaseFSW.Filter = "*.uc";
            neteaseFSW.NotifyFilter = NotifyFilters.FileName;
            neteaseFSW.Path = NeteaseCachePath;
            neteaseFSW.Created += new FileSystemEventHandler(autoParseNetease);
            neteaseFSW.EnableRaisingEvents = true;
            neteaseFSW.EndInit();
        }
        private void autoParseNetease(object sender, FileSystemEventArgs e)
        {
            if (!Directory.Exists(tmpPath))
            {
                Directory.CreateDirectory(tmpPath);
            }
            Thread t = new Thread(() =>
              {
                  doAutoParseNeteaseFile(e.FullPath);
              });
            t.Start();
            //MessageBox.Show(e.FullPath);
            //var task=doAutoParseNeteaseFile(e.FullPath);
            return;
        }
        private void addToFList(string text)
        {

            form1.Invoke(methodAddToList, text);
            //form1.ListBox1.ref
        }
        private byte[] wgetBytes(String url)
        {
            try
            {
                var array = client.GetByteArrayAsync(url);
                array.Wait();
                return array.Result;
            }
            catch (Exception e)
            {

                addToFList(e.ToString());
            }
            return null;
        }
        private String wget(String url)
        {
            var bytes= wgetBytes(url);
            if (bytes == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(bytes.ToArray());
        }
        private bool canAccess(String path)
        {
            bool can = false;
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(path);
                can = true;
            }
            catch
            {

            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return can;
        }
        private async Task doAutoParseNeteaseFile(string path)
        {
            int time = 0;
            try
            {
                addToFList(Path.GetFileName(path) + " is detected");
                Thread.Sleep(120000);
                while (!canAccess(path))
                {
                    Thread.Sleep(60000);
                }
                int ret;
                while ((ret=tryParseNeteaseF(path))==-3)
                {
                    Thread.Sleep(60000);
                    if (time++ > 10)
                    {
                        addToFList(Path.GetFileName(path) + " is not download complete" + ret);
                        return;
                    }
                }
                addToFList(Path.GetFileName(path) + "'s result is " + ret);
            }catch(Exception e)
            {
                addToFList(e.ToString());
            }
        }
        public bool tryParseNeteaseFile(string path)
        {
            return tryParseNeteaseFile(path, 0)<0?false:true;
        }
        private string getNeteaseFileMD5(FileStream fs)
        {
            StringBuilder sb = new StringBuilder();
            using (fs)
            {
                var buf = new byte[16000];
                var md5 = new MD5CryptoServiceProvider();
                int current;
               
                while ((current = fs.Read(buf, 0, 16000)) != -1 && current != 0)
                {
                    for(int i = 0; i < current; i++)
                    {
                        buf[i] = (byte)(buf[i] ^ NETEASE_CODE);
                    }
                    md5.TransformBlock(buf, 0, current, null, 0);
                }
                md5.TransformFinalBlock(buf, 0, 0);
                foreach(var i in md5.Hash)
                {
                    sb.Append(i.ToString("X2").ToLower());
                }
                return sb.ToString();
            }
        }
        public int tryParseNeteaseF(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var header = new byte[4];
            var fs=File.OpenRead(path);
            fs.Read(header, 0, 4);
            fs.Seek(0, SeekOrigin.Begin);
            var hash = getNeteaseFileMD5(fs);
            var names = name.Split('-');
            if (names[2] != hash)
            {
                //MessageBox.Show(names[2] + "\n" + hash);
                return -3;
            }
            for (int i = 0; i < 4; i++)
            {
                header[i] = (byte)(header[i] ^ NETEASE_CODE);
            }
            string type=Encoding.GetEncoding("iso-8859-1").GetString(header);
            MusicFileFormat format=MusicFileFormat.UNKONWN;
            switch (type)
            {
                case "RIFF":
                    format = MusicFileFormat.WAV;
                    break;
                case "fLaC":
                    format = MusicFileFormat.FLAC;
                    break;
                default:
                    if (type.StartsWith("ID3"))
                    {
                        format = MusicFileFormat.MP3;
                    }
                    else
                    {
                        byte h2=header[1];
                        int t=(byte)(h2 << 5) >> 6;
                        if (t == 1)
                        {
                            format = MusicFileFormat.MP3;
                        }
                        //MessageBox.Show(t.ToString());
                    }
                    break;
            }
            if (format == MusicFileFormat.UNKONWN)
            {
                return -1;
            }
            foreach(var f in config.Formats)
            {
                if (!f.Enabled&&f.Type==format.ToString())
                {
                    addToFList("Format " + format.ToString() + " is disabled to output.");
                    return -2;
                }
            }
            MusicFileInfo minfo=new MusicFileInfo();
            minfo.Format = format.ToString().ToLower();
            minfo.Size = 0;
            minfo.Volume = 0;
            minfo.Id= names[0]; ;
            addToFList("Found "+format.ToString()+" "+name);
            ParseNeteaseFile(path, minfo);
            //MessageBox.Show(format.ToString());
            return 1;
        }
        public int tryParseNeteaseFile(string path, long ucsize)
        {
            var ucfile = path + ".uc";
            var idxfile = path + ".idx";
            var infofile = path + ".info";
            if (!(File.Exists(ucfile) && File.Exists(idxfile) && File.Exists(infofile)))
            {
                //addToFList(path + " ");
                return -1;
            }
            var name = Path.GetFileName(path);
            if (ucsize == 0) {
                var ucattr = new FileInfo(path + ".uc");
                ucsize = (int)ucattr.Length;
            }
            MusicFileInfo minfo;
            if (neteaseInfoMap.ContainsKey(name))
            {
                minfo = neteaseInfoMap[name];
            }else{
                var idxinfo=File.ReadAllText(path + ".idx");
                var idx=QuickType.NeteaseIDX.FromJson(idxinfo);
                var infojson = File.ReadAllText(path + ".info");
                var info = QuickType.NeteaseInfoSerialize.FromJson(infojson);
                minfo = new MusicFileInfo();
                minfo.Format = info.Format;
                minfo.Volume = info.Volume;
                minfo.Size = (int)idx.Size;
                minfo.Id = name.Split('-')[0];
                neteaseInfoMap.TryAdd(name, minfo);
            }


            foreach (var f in config.Formats)
            {
                if (!f.Enabled && f.Type == minfo.Format.ToString().ToUpper())
                {
                    addToFList("Format " + minfo.Format.ToString() + " is disabled to output.");
                    return -2;
                }
            }
            if (minfo.Size != ucsize)
            {
                addToFList(ucfile + " 's size is wrong.");
                return -1;
            }
            
            ParseNeteaseFile(ucfile, minfo);
            //Messagebox(Path.GetFileName(path));
            return 0;
        }
        private void ParseNeteaseFile(string path,MusicFileInfo info)
        {
            byte[] music = File.ReadAllBytes(path);
            for(int i = 0; i < music.Length; i++)
            {
                music[i] = (byte)(music[i] ^ NETEASE_CODE);
            }
            var av=info.Id;
            var url = String.Format(NETEASE_DETAIL_INFO, info.Id);
            var json = wget(url);
            if (json == null)
            {
                return;
            }
            var songDetail = QuickType.NeteaseMusicDetails.FromJson(json);
            var song1 = songDetail.Songs[0];

            foreach (char i in Path.GetInvalidFileNameChars())
            {
                song1.Album.Name = song1.Album.Name.Replace(i, '_');
                song1.Artists[0].Name = song1.Artists[0].Name.Replace(i, '_');
                song1.Name = song1.Name.Replace(i, '_');
            }
            var outpath = config.SavePath;
            outpath=outpath.Replace("{album}",song1.Album.Name);
            outpath=outpath.Replace("{artist}", song1.Artists[0].Name);
            outpath=outpath.Replace("{title}", song1.Name);
            
            Directory.CreateDirectory(outpath);
            var outname = config.SaveFileName + "." + info.Format;
            outname = outname.Replace("{album}", song1.Album.Name);
            outname = outname.Replace("{artist}", song1.Artists[0].Name);
            outname = outname.Replace("{title}", song1.Name);
            var outfile = outpath + "\\"+ outname;
            
            if (File.Exists(outfile))
            {
                return;
            }
            var album_pic= wgetBytes(song1.Album.PicUrl);
            var album_pic_nf = tmpPath +"\\"+ Guid.NewGuid().ToString() + ".jpg";
            var nf = tmpPath+"\\"+Guid.NewGuid().ToString()+ "."+info.Format;
            try
            {
                var mf = File.Create(nf);
                mf.Write(music, 0, music.Length);
                mf.Close();
                var mf2 = File.Create(album_pic_nf);
                mf2.Write(album_pic, 0, album_pic.Length);
                mf2.Close();
                var tagf = TagLib.File.Create(nf);
                var tag = tagf.Tag;
                //MessageBox.Show(tagf.TagTypes.ToString());
                if (tagf.TagTypes.HasFlag(TagLib.TagTypes.FlacMetadata))
                {
                    var pic = new TagLib.Picture(album_pic_nf);
                    tag.Pictures = new TagLib.IPicture[] { pic };
                    var lryric = wget(String.Format(NETEASE_LRYRIC, info.Id));
                    if (lryric != null)
                    {
                        var lrcjson=JObject.Parse(lryric);
                        if (lrcjson["lrc"] != null)
                        {
                            tag.Lyrics = (string)lrcjson["lrc"]["lyric"];
                        }
                    }
                }
               
                tag.Track = (uint)song1.Position;
                tag.Album = song1.Album.Name;
                tag.Title = song1.Name;
                var date = DateTimeOffset.FromUnixTimeMilliseconds(song1.Album.PublishTime);
                tag.Year = (uint)date.Year;
                if (song1.Disc != null)
                {
                    string s;
                    int disc;
                    if (int.TryParse(song1.Disc, out disc)) {
                        tag.Disc = (uint)disc;
                    }
                }
                var arts = new String[song1.Artists.Length];
                var a = 0;
                foreach (var i in song1.Artists)
                {
                    arts[a++] = i.Name;
                }
                tag.AlbumArtists = arts;
                tagf.Save();
                if (!File.Exists(outfile))
                {
                    addToFList("Try move to " + outfile);
                    File.Move(nf, outfile);
                }
                addToFList(Path.GetFileName(path) + " is parsed to " + Path.GetFileName(outfile));
            }
            finally
            {
                File.Delete(nf);
                File.Delete(album_pic_nf);

            }
            //tag.Artists = song1//.Artists[0];
        }

    }
}
