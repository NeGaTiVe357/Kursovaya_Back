namespace Kursovoy_project_electronic_shop.Contracts
{
    public class UserRegisterCredentials
    {
        public required string Name { get; init; }

        public required string Login { get; init; }

        public required string Password { get; init; }
    }
}
