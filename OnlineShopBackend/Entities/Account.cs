using OnlineShopBackend.Interfaces;

namespace OnlineShopBackend.Entities
{
    public class Account : IEntity
    {
        public Guid Id { get; init; }

        public string? Name { get; set; }

        public string? Email { get; set; }

    }
}
