using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCacheParser.MetaJson
{
    public partial class NeteaseMeta
    {
        [JsonProperty("album")]
        public string Album { get; set; }
        [JsonProperty("albumId")]
        public long AlbumId { get; set; }
        [JsonProperty("albumPic")]
        public string AlbumPic { get; set; }
        [JsonProperty("albumPicDocId")]
        public string AlbumPicDocId { get; set; }
        [JsonProperty("alias")]
        public string[] Alias { get; set; }
        [JsonProperty("artist")]
        public string[][] Artist { get; set; }
        [JsonProperty("musicId")]
        public long MusicId { get; set; }
        [JsonProperty("musicName")]
        public string MusicName { get; set; }
        [JsonProperty("mvId")]
        public long MvId { get; set; }
        [JsonProperty("transNames")]
        public string[] TransNames { get; set; }
        [JsonProperty("format")]
        public string Format { get; set; }
        [JsonProperty("bitrate")]
        public int Bitrate { get; set; }
        [JsonProperty("duration")]
        public long Duration { get; set; }
        [JsonProperty("mp3DocId")]
        public string Mp3DocId { get; set; }
    }
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    public partial class NeteaseMeta
    {

        public static NeteaseMeta FromJson(string json) => JsonConvert.DeserializeObject<NeteaseMeta>(json, Converter.Settings);
        public string ToJson() => JsonConvert.SerializeObject(this, Converter.Settings);
    }
}
