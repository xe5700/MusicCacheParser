using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCacheParserConfig
{

    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MusicParserConfig
    {
        [JsonProperty("neteaseMusic")]
        public NeteaseMusic NeteaseMusic { get; set; }

        [JsonProperty("savePath")]
        public string SavePath { get; set; }

        [JsonProperty("saveFileName")]
        public string SaveFileName { get; set; }

        [JsonProperty("formats")]
        public Format[] Formats { get; set; }

        [JsonProperty("customTmpPath")]
        public String CustomTmpPath { get; set; }
    }

    public partial class Format
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

    public partial class NeteaseMusic
    {
        [JsonProperty("cachePath")]
        public string CachePath { get; set; }

        [JsonProperty("autoParse")]
        public bool AutoParse { get; set; }
    }

    public partial class MusicParserConfig
    {
        public static MusicParserConfig FromJson(string json) => JsonConvert.DeserializeObject<MusicParserConfig>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MusicParserConfig self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
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
}
