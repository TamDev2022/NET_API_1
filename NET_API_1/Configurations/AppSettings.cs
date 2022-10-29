namespace NET_API_1.Configurations
{
    public record AppSettings
    {
        public record JWTSettings
        {
            public string? Issuer { get; set; }
            public string? Audience { get; set; }
            public string SecretKey { get; init; } = "defaultsecretkey";
            public string? Prefix { get; init; }
            public int Expires { get; init; }
        }

        public record BlobAzureSettings
        {
            public string? BlobConnectionString { get; set; }
            public string? BlobContainerName { get; set; }

        }
        public record MailSettings
        {
            public string? Mail { get; set; }
            public string? DisplayName { get; set; }
            public string? Password { get; set; }
            public string? Host { get; set; }
            public int Port { get; set; }

        }

    }
}
