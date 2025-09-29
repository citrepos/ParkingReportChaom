using System.Linq;
using ParkingManagementReport.Common;

namespace ParkingManagementReport.Utilities.Formatters
{
    internal class NumericFormatters
    {
        internal static int[] GetPromotionRange(string startPromotionId, string endPromotionId)
        {
            if (int.TryParse(startPromotionId, out int start) && int.TryParse(endPromotionId, out int end))
            {
                if (start > end)
                    (start, end) = (end, start); // Ensure ascending

                return Enumerable.Range(start, end - start + 1)
                                 .Where(id => AppGlobalVariables.PromotionNamesById.ContainsKey(id))
                                 .ToArray();
            }

            return null;
        }
    }
}