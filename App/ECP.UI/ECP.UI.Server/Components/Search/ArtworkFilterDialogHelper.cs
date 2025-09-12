using ECP.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ECP.UI.Server.Components.Search
{
    public class ArtworkFilterDialogHelper
    {
        [CascadingParameter]
        IMudDialogInstance MudDialog { get; set; } = null!;

        private ArtworkFilters Filters = new();

        private void Submit()
        {
            MudDialog.Close(DialogResult.Ok(Filters));
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void ClearAllFilters()
        {
            Filters = new ArtworkFilters();
        }

        private bool HasActiveFilters()
        {
            return !string.IsNullOrEmpty(Filters.Artist) ||
                   !string.IsNullOrEmpty(Filters.Subject) ||
                   !string.IsNullOrEmpty(Filters.Type) ||
                   !string.IsNullOrEmpty(Filters.Material) ||
                   Filters.DateFrom.HasValue ||
                   Filters.DateTo.HasValue;
        }

        private string GetDateRangeText()
        {
            if (Filters.DateFrom.HasValue && Filters.DateTo.HasValue)
                return $"Date: {Filters.DateFrom.Value:MMM yyyy} - {Filters.DateTo.Value:MMM yyyy}";
            else if (Filters.DateFrom.HasValue)
                return $"Date: From {Filters.DateFrom.Value:MMM yyyy}";
            else if (Filters.DateTo.HasValue)
                return $"Date: Until {Filters.DateTo.Value:MMM yyyy}";
            return "";
        }
    }
}
