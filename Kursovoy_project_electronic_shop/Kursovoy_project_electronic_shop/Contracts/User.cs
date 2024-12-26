namespace Kursovoy_project_electronic_shop.Contracts
{
    public class User
    {
        public Guid UserUid { get; init; }

        public required string Name { get; init; }

        public required string Login { get; init; }

        public required string Password { get; init; }

        public required string? Email { get; init; }

        public bool IsAdmin { get; init; } = false;
    }
}
