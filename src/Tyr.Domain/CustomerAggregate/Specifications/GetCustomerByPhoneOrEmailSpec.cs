using Ardalis.Specification;
using Tyr.Domain.CustomerAggregate;

namespace Tyr.Domain.CustomerAggregate.Specifications
{
    public class GetCustomerByPhoneOrEmailSpec : Specification<Customer>
    {
        public GetCustomerByPhoneOrEmailSpec(string phone, string? email)
        {
            var phoneNormalized = phone?.Trim();
            var emailNormalized = string.IsNullOrWhiteSpace(email) ? null : email!.Trim().ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(phoneNormalized) && !string.IsNullOrWhiteSpace(emailNormalized))
            {
                Query.Where(c => c.Phone == phoneNormalized || c.Email == emailNormalized);
            }
            else if (!string.IsNullOrWhiteSpace(phoneNormalized))
            {
                Query.Where(c => c.Phone == phoneNormalized);
            }
            else if (!string.IsNullOrWhiteSpace(emailNormalized))
            {
                Query.Where(c => c.Email == emailNormalized);
            }
            else
            {
                // No criteria - return empty set
                Query.Where(c => false);
            }
        }
    }
}
