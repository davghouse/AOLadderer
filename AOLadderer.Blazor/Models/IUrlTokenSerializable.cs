using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace AOLadderer.Blazor.Models
{
    public interface IUrlTokenSerializable
    {
        void UrlTokenSerialize(Queue<object> data);
        void UrlTokenDeserialize(Queue<object> data);
    }

    public static class IUrlTokenSerializableExtensions
    {
        public static string UrlTokenSerialize(this IUrlTokenSerializable serializable)
        {
            var data = new Queue<object>();
            serializable.UrlTokenSerialize(data);
            string json = Json.Serialize(data);

            // TODO: Compress JSON when DeflateStream is fixed, see: https://github.com/aspnet/Blazor/issues/445
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json))
                .Replace('+', '-').Replace('/', '_');
        }

        public static void UrlTokenDeserialize(this IUrlTokenSerializable serializable, string token)
        {
            string json = Encoding.UTF8.GetString(Convert.FromBase64String(
                token.Replace('-', '+').Replace('_', '/')));

            var data = Json.Deserialize<object[]>(json);
            var data2 = new Queue<object>(data);
            serializable.UrlTokenDeserialize(data2);
        }
    }
}
