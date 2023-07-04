using MusicCacheParser.MetaJson;
using Newtonsoft.Json.Linq;
using QuickType;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
        private int bitrate;
        private string md5;
        public string Format { get => format; set => format = value; }
        public double Volume { get => volume; set => volume = value; }
        public int Size { get => size; set => size = value; }
        public string Id { get => id; set => id = value; }
        public int Bitrate { get => bitrate; set => bitrate = value; }
        public string Md5 { get => md5; set => md5 = value; }
    }

    class Utils
    {
       public static byte[] Hex2Binary(string hex)
        {
            var chars = hex.ToCharArray();
            var bytes = new List<byte>();
            for (int index = 0; index < chars.Length; index += 2)
            {
                var chunk = new string(chars, index, 2);
                bytes.Add(byte.Parse(chunk, NumberStyles.AllowHexSpecifier));
            }
            return bytes.ToArray();
        }
    }
    class MusicParser
    {
        public MusicParser(Form1 f1)
        {
            this.form1 = f1;
            init();
        }
        private MusicCacheParserConfig.MusicParserConfig config;
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        });
        private Form1 form1;
        private ConcurrentDictionary<String, MusicFileInfo> neteaseInfoMap = new ConcurrentDictionary<string, MusicFileInfo>();
        public const byte NETEASE_CODE = 0xA3;
        public const string NETEASE_DETAIL_INFO = "http://music.163.com/api/song/detail/?ids=%5B{0:G}%5D";
        public const string NETEASE_LRYRIC = "http://music.163.com/api/song/lyric?os=pc&id={0:G}&lv=-1&kv=-1&tv=-1";
        private string tmpPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
        private string neteaseCachePath = @"%LocalAppData%\\Netease\\CloudMusic\\Cache";
        private byte[] encrypt163key = Utils.Hex2Binary("2331346C6A6B5F215C5D2630553C2728");
        public string NeteaseCachePath { get => neteaseCachePath; set => neteaseCachePath = value; }
        private FileSystemWatcher neteaseFSW = new FileSystemWatcher();
        static MusicParser(){
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36");
            client.DefaultRequestHeaders.Add("X-Real-IP", "59.111.181.60");
        }
        private void init()
        {
            config = form1.Config;
            NeteaseCachePath = System.Environment.ExpandEnvironmentVariables(config.NeteaseMusic.CachePath+"\\Cache");
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
            ThreadPool.QueueUserWorkItem((run)=> {
                doAutoParseNeteaseFile(e.FullPath);
            });
            return;
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

                form1.addToFList(e.ToString());
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
        private void doAutoParseNeteaseFile(string path)
        {
            int time = 0;
            try
            {
                form1.addToFList(Path.GetFileName(path) + " is detected");
                Thread.Sleep(30000);
                while (!canAccess(path))
                {
                    Thread.Sleep(5000);
                }
                int ret=-3;
                while (ret==-3)
                {
                    try
                    {
                        ret = tryParseNeteaseF(path);
                        Thread.Sleep(20000);
                    }catch(Exception e){
                        form1.addToFList(e.ToString());
                    }
                }
                form1.addToFList(Path.GetFileName(path) + "'s result is " + ret);
            }catch(Exception e)
            {
                form1.addToFList(e.ToString());
            }
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
                    form1.addToFList("Format " + format.ToString() + " is disabled to output.");
                    return -2;
                }
            }
            MusicFileInfo minfo=new MusicFileInfo();
            minfo.Format = format.ToString().ToLower();
            minfo.Size = 0;
            minfo.Volume = 0;
            minfo.Id= names[0]; ;
            minfo.Bitrate = int.Parse(names[1]);
            minfo.Md5 = hash;
            form1.addToFList("Found "+format.ToString()+" "+name);
            ParseNeteaseFile(path, minfo);
            //MessageBox.Show(format.ToString());
            return 1;
        }
        private static readonly Regex findPicId = new Regex(@"/(\d+)\.\w+$");
        private String mk163Key(MusicFileInfo fileinfo, NeteaseMusicDetails details)
        {
            var song = details.Songs[0];
            var album = song.Album;
            var artists = song.Artists.Select(i => new string[] { i.Name, i.Id.ToString() }).ToArray();
            string picid;
            if(album.Pic == 0)
            {
                picid = findPicId.Match(album.PicUrl).Groups[1].Value;
            }
            else
            {
                picid = album.Pic.ToString();
            }
            NeteaseMeta meta = new NeteaseMeta {
                Artist = artists,
                Album = album.Name,
                AlbumId = album.Id,
                AlbumPic = album.PicUrl,
                AlbumPicDocId = picid,
                Alias = album.Alias == null ? new string[0] : album.Alias,
                MusicId = song.Id,
                MusicName = song.Name,
                MvId = song.Mvid,
                TransNames = new string[0],
                Format = fileinfo.Format,
                Bitrate = fileinfo.Bitrate * 1000,
                Duration = song.Duration,
                Mp3DocId = fileinfo.Md5
            };
            var comment = "music:" + meta.ToJson();
            {
                var aes = new AesCryptoServiceProvider();
                aes.Mode = CipherMode.ECB;
                //aes.Padding = PaddingMode.PKCS7;
                //aes.BlockSize = 8;
                var enc = aes.CreateEncryptor();
                var bytes = Encoding.UTF8.GetBytes(comment);
                bytes = Padding.PKCS7.Encoding(bytes, 8);
                bytes = enc.TransformFinalBlock(bytes,0,bytes.Length);
                comment = "163 key(Don\'t modify):"+ Convert.ToBase64String(bytes);
                
            }
            return comment;
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
                var arts = song1.Artists.Select(i => i.Name).ToArray();
                tag.AlbumArtists = arts;
                tag.Performers = arts;
                //开始构建163Key
                if (config.NeteaseMusic.Enable163Key)
                {
                    var key163 = mk163Key(info, songDetail);
                    //tag.Description = key163;
                    tag.Comment = key163;
                }
                //结束构建163Key
                tagf.Save();
                if (!File.Exists(outfile))
                {
                    form1.addToFList("Try move to " + outfile);
                    File.Move(nf, outfile);
                }
                form1.addToFList(Path.GetFileName(path) + " is parsed to " + Path.GetFileName(outfile));
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
