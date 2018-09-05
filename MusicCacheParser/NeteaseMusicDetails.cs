// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var neteaseMusicDetails = NeteaseMusicDetails.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class NeteaseMusicDetails
    {
        [JsonProperty("songs")]
        public Song[] Songs { get; set; }

        [JsonProperty("equalizers")]
        public Equalizers Equalizers { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }
    }

    public partial class Equalizers
    {
    }

    public partial class Song
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("alias")]
        public object[] Alias { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("fee")]
        public long Fee { get; set; }

        [JsonProperty("copyrightId")]
        public long CopyrightId { get; set; }

        [JsonProperty("disc")]
        public string Disc { get; set; }

        [JsonProperty("no")]
        public long No { get; set; }

        [JsonProperty("artists")]
        public Artist[] Artists { get; set; }

        [JsonProperty("album")]
        public Album Album { get; set; }

        [JsonProperty("starred")]
        public bool Starred { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("starredNum")]
        public long StarredNum { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("playedNum")]
        public long PlayedNum { get; set; }

        [JsonProperty("dayPlays")]
        public long DayPlays { get; set; }

        [JsonProperty("hearTime")]
        public long HearTime { get; set; }

        [JsonProperty("ringtone")]
        public string Ringtone { get; set; }

        [JsonProperty("crbt")]
        public object Crbt { get; set; }

        [JsonProperty("audition")]
        public object Audition { get; set; }

        [JsonProperty("copyFrom")]
        public string CopyFrom { get; set; }

        [JsonProperty("commentThreadId")]
        public string CommentThreadId { get; set; }

        [JsonProperty("rtUrl")]
        public object RtUrl { get; set; }

        [JsonProperty("ftype")]
        public long Ftype { get; set; }

        [JsonProperty("rtUrls")]
        public object[] RtUrls { get; set; }

        [JsonProperty("copyright")]
        public long Copyright { get; set; }

        [JsonProperty("transName")]
        public object TransName { get; set; }

        [JsonProperty("sign")]
        public object Sign { get; set; }

        [JsonProperty("rtype")]
        public long Rtype { get; set; }

        [JsonProperty("rurl")]
        public object Rurl { get; set; }

        [JsonProperty("mvid")]
        public long Mvid { get; set; }

        [JsonProperty("bMusic")]
        public Music BMusic { get; set; }

        [JsonProperty("mp3Url")]
        public object Mp3Url { get; set; }

        [JsonProperty("hMusic")]
        public object HMusic { get; set; }

        [JsonProperty("mMusic")]
        public Music MMusic { get; set; }

        [JsonProperty("lMusic")]
        public Music LMusic { get; set; }
    }

    public partial class Album
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("picId")]
        public long PicId { get; set; }

        [JsonProperty("blurPicUrl")]
        public string BlurPicUrl { get; set; }

        [JsonProperty("companyId")]
        public long CompanyId { get; set; }

        [JsonProperty("pic")]
        public long Pic { get; set; }

        [JsonProperty("picUrl")]
        public string PicUrl { get; set; }

        [JsonProperty("publishTime")]
        public long PublishTime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("briefDesc")]
        public string BriefDesc { get; set; }

        [JsonProperty("artist")]
        public Artist Artist { get; set; }

        [JsonProperty("songs")]
        public object[] Songs { get; set; }

        [JsonProperty("alias")]
        public object[] Alias { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("copyrightId")]
        public long CopyrightId { get; set; }

        [JsonProperty("commentThreadId")]
        public string CommentThreadId { get; set; }

        [JsonProperty("artists")]
        public Artist[] Artists { get; set; }

        [JsonProperty("subType")]
        public string SubType { get; set; }

        [JsonProperty("transName")]
        public object TransName { get; set; }
    }

    public partial class Artist
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("picId")]
        public long PicId { get; set; }

        [JsonProperty("img1v1Id")]
        public long Img1V1Id { get; set; }

        [JsonProperty("briefDesc")]
        public string BriefDesc { get; set; }

        [JsonProperty("picUrl")]
        public string PicUrl { get; set; }

        [JsonProperty("img1v1Url")]
        public string Img1V1Url { get; set; }

        [JsonProperty("albumSize")]
        public long AlbumSize { get; set; }

        [JsonProperty("alias")]
        public object[] Alias { get; set; }

        [JsonProperty("trans")]
        public string Trans { get; set; }

        [JsonProperty("musicSize")]
        public long MusicSize { get; set; }
    }

    public partial class Music
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("sr")]
        public long Sr { get; set; }

        [JsonProperty("dfsId")]
        public long DfsId { get; set; }

        [JsonProperty("bitrate")]
        public long Bitrate { get; set; }

        [JsonProperty("playTime")]
        public long PlayTime { get; set; }

        [JsonProperty("volumeDelta")]
        public double VolumeDelta { get; set; }
    }

    public partial class NeteaseMusicDetails
    {
        public static NeteaseMusicDetails FromJson(string json) => JsonConvert.DeserializeObject<NeteaseMusicDetails>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NeteaseMusicDetails self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
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

    internal class ParseIntegerConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseIntegerConverter Singleton = new ParseIntegerConverter();
    }
}

// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var neteaseMusicDetails = NeteaseMusicDetails.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class NeteaseIDX
    {
        [JsonProperty("size")]
        [JsonConverter(typeof(ParseIntegerConverter))]
        public long Size { get; set; }

        [JsonProperty("t")]
        public bool T { get; set; }

        [JsonProperty("zone")]
        public string[] Zone { get; set; }
    }

    public partial class NeteaseIDX
    {
        public static NeteaseIDX FromJson(string json) => JsonConvert.DeserializeObject<NeteaseIDX>(json, QuickType.Converter.Settings);
    }

    public static class NeteaseIDXSerialize
    {
        public static string ToJson(this NeteaseIDX self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

}

// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var neteaseMusicDetails = NeteaseMusicDetails.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class NeteaseInfo
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }
    }

    public partial class NeteaseInfoSerialize
    {
        public static NeteaseInfo FromJson(string json) => JsonConvert.DeserializeObject<NeteaseInfo>(json, QuickType.Converter.Settings);
    }
    public static partial class NeteaseInfoSerialize
    {
        public static string ToJson(this NeteaseInfo self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }
}
