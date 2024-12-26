namespace Kursovoy_project_electronic_shop.Configuration
{
    public class JwtAuthenticationOptions
    {
        public required string Key { get; init; }

        public required string Issuer { get; init; }

        public required string Audience { get; init; }
    }
}
