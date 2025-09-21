using ECP.Shared;

namespace ECP.UI.Server.Components.Search
{
    public class ArtworkFilterDialogHelper(ArtworkFilters filters)
    {

        private ArtworkFilters _filters = filters;

        private bool HasActiveFilters()
        {
            return !string.IsNullOrEmpty(filters.Artist) ||
                   !string.IsNullOrEmpty(filters.Subject) ||
                   !string.IsNullOrEmpty(filters.Type) ||
                   !string.IsNullOrEmpty(filters.Material) ||
                   filters.DateFrom.HasValue ||
                   filters.DateTo.HasValue;
        }

        public string BuildFilterQuery()
        {
            if (!HasActiveFilters())
            {
                return string.Empty;
            }

            var filters = GetActiveFilters();

            if (filters.Count == 0)
                return string.Empty;

            var filterStrings = filters.Select(f =>
                $"&filters={Uri.EscapeDataString(f.Key).ToLowerInvariant()}:{Uri.EscapeDataString(f.Value)}");

            return string.Join("", filterStrings);
        }

        private Dictionary<string, string> GetActiveFilters()
        {
            var dict = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(_filters.Artist))
                dict["Artist"] = _filters.Artist;

            if (!string.IsNullOrEmpty(_filters.Subject))
                dict["Subject"] = _filters.Subject;

            if (!string.IsNullOrEmpty(_filters.Type))
                dict["Type"] = _filters.Type;

            if (!string.IsNullOrEmpty(_filters.Material))
                dict["Material"] = _filters.Material;

            if (_filters.DateFrom.HasValue || _filters.DateTo.HasValue)
                dict["Date"] = _filters.Date;

            return dict;
        }
    }
}
