using System.Text;

namespace WalkingTec.Mvvm.Core
{
    public class JwtOption
    {
        public const int MinimumSecurityKeySizeInBytes = 32;

        public string Issuer { get; set; } = "http://localhost";
        public string Audience { get; set; } = "http://localhost";
        public int Expires { get; set; } = 3600;
        public string SecurityKey { get; set; } = "wtm";
        public string LoginPath { get; set; }

        public byte[] GetSecurityKeyBytes()
        {
            return Encoding.UTF8.GetBytes(NormalizeSecurityKey(SecurityKey));
        }

        public static string NormalizeSecurityKey(string securityKey)
        {
            var normalizedKey = securityKey ?? string.Empty;
            while (Encoding.UTF8.GetByteCount(normalizedKey) < MinimumSecurityKeySizeInBytes)
            {
                normalizedKey += "x";
            }

            return normalizedKey;
        }
    }
}
