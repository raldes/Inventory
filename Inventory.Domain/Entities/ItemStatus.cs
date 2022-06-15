
using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities
{
    public class ItemStatus : Enumeration
    {
        public static ItemStatus Draft   = new ItemStatus(1, nameof(Draft).ToLowerInvariant());
        public static ItemStatus Created = new ItemStatus(2, nameof(Created).ToLowerInvariant());
        public static ItemStatus Updated = new ItemStatus(3, nameof(Updated).ToLowerInvariant());
        public static ItemStatus Expired = new ItemStatus(4, nameof(Expired).ToLowerInvariant());
        public static ItemStatus Removed = new ItemStatus(5, nameof(Removed).ToLowerInvariant());

        public ItemStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<ItemStatus> List() =>
            new[] { Draft, Created, Updated, Expired, Removed};

        public static ItemStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new ItemDomainException($"Possible values for ItemStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static ItemStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ItemDomainException($"Possible values for ItemStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
