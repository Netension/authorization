using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netension.Authorization.OIDC.Converters
{
    public class JwtSecurityTokenJsonConverter : JsonConverter<JwtSecurityToken>
    {
        public override JwtSecurityToken Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var token = reader.GetString();
            if (string.IsNullOrWhiteSpace(token)) return null;

            return new JwtSecurityToken(token);
        }

        public override void Write(Utf8JsonWriter writer, JwtSecurityToken value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(new ReadOnlySpan<char>($"{value.EncodedHeader}.{value.EncodedPayload}.{value.RawSignature}".ToCharArray()));
        }
    }
}
