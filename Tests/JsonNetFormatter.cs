﻿// Code From: http://blogs.msdn.com/b/henrikn/archive/2012/02/18/using-json-net-with-asp-net-web-api.aspx

using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tests
{
    public class JsonNetFormatter : MediaTypeFormatter
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonNetFormatter(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();

            // Fill out the mediatype and encoding we support
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            Encoding = new UTF8Encoding(false, true);
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(IKeyValueModel))
            {
                return false;
            }

            return true;
        }

        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        protected override Task<object> OnReadFromStreamAsync(Type type, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            // Create a serializer
            JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

            // Create task reading the content
            return Task.Factory.StartNew(() =>
            {
                using (var streamReader = new StreamReader(stream, Encoding))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return serializer.Deserialize(jsonTextReader, type);
                    }
                }
            });
        }

        protected override Task OnWriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            // Create a serializer
            JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

            // Create task writing the serialized content
            return Task.Factory.StartNew(() =>
            {
                using (JsonTextWriter jsonTextWriter = new JsonTextWriter(new StreamWriter(stream, Encoding)) { CloseOutput = false })
                {
                    serializer.Serialize(jsonTextWriter, value);
                    jsonTextWriter.Flush();
                }
            });
        }
    }
}