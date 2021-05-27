using EPiServer.Commerce.Order;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Extensions
{
    public static class CartExtensions
    {
        public static void AddValidationIssues(this Dictionary<ILineItem, List<ValidationIssue>> issues, ILineItem lineItem, ValidationIssue issue)
        {
            if (!issues.ContainsKey(lineItem))
            {
                issues.Add(lineItem, new List<ValidationIssue>());
            }

            if (!issues[lineItem].Contains(issue))
            {
                issues[lineItem].Add(issue);
            }
        }

        public static bool HasItemBeenRemoved(this Dictionary<ILineItem, List<ValidationIssue>> issuesPerLineItem, ILineItem lineItem)
        {
            if (issuesPerLineItem.TryGetValue(lineItem, out var issues))
            {
                return issues.Any(x => x == ValidationIssue.RemovedDueToInactiveWarehouse ||
                        x == ValidationIssue.RemovedDueToCodeMissing ||
                        x == ValidationIssue.RemovedDueToInsufficientQuantityInInventory ||
                        x == ValidationIssue.RemovedDueToInvalidPrice ||
                        x == ValidationIssue.RemovedDueToMissingInventoryInformation ||
                        x == ValidationIssue.RemovedDueToNotAvailableInMarket ||
                        x == ValidationIssue.RemovedDueToUnavailableCatalog ||
                        x == ValidationIssue.RemovedDueToUnavailableItem);
            }

            return false;
        }
    }
}