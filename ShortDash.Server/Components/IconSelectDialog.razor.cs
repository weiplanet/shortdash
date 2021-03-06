using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;
using ShortDash.Server.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace ShortDash.Server.Components
{
    public sealed partial class IconSelectDialog : ComponentBase, IDisposable
    {
        private readonly List<string> icons = new List<string>();
        private Timer searchTextTimer;

        [Parameter]
        public string BackgroundColorHtml { get; set; }

        [Parameter]
        public string CurrentValue { get; set; }

        [Parameter]
        public string TextClass { get; set; }

        [CascadingParameter]
        private BlazoredModalInstance BlazoredModal { get; set; }

        private List<string> FilteredIcons { get; } = new List<string>();

        private string SearchText { get; set; }

        [Inject]
        private IWebHostEnvironment WebHostEnvironment { get; set; }

        public static Task<ModalResult> ShowAsync(IModalService modalService, string currentValue, Color backgroundColor)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(CurrentValue), currentValue);
            parameters.Add(nameof(BackgroundColorHtml), backgroundColor.ToHtmlString());
            parameters.Add(nameof(TextClass), backgroundColor.TextClass());
            var modal = modalService.Show<IconSelectDialog>("", parameters);
            return modal.Result;
        }

        public void Dispose()
        {
            searchTextTimer?.Dispose();
        }

        protected async override Task OnInitializedAsync()
        {
            searchTextTimer = new Timer(300);
            searchTextTimer.Elapsed += FilterIcons;
            searchTextTimer.AutoReset = false;

            icons.Clear();
            var data = await File.ReadAllTextAsync(Path.Combine(WebHostEnvironment.WebRootPath, "lib/font-awesome/font-awesome.txt"));
            var iconNames = data.Split('\n');
            foreach (var name in iconNames)
            {
                icons.Add(name);
            }
            FilterIcons();
        }

        private Task CloseDialog()
        {
            return BlazoredModal.Cancel();
        }

        private void FilterIcons(object source, ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                FilterIcons();
                StateHasChanged();
            });
        }

        private void FilterIcons(bool allowShowAll = false)
        {
            FilteredIcons.Clear();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                if (allowShowAll)
                {
                    FilteredIcons.AddRange(icons);
                }
            }
            else
            {
                var normalizedSearchText = new string(SearchText.ToLower().Where(x => char.IsLetterOrDigit(x)).ToArray());
                FilteredIcons.AddRange(icons.Where(x => x.Replace("-", string.Empty).Contains(normalizedSearchText)));
            }
        }

        private Task OkClick()
        {
            return BlazoredModal.Close(ModalResult.Ok(CurrentValue));
        }

        private void SearchTextKeyUp()
        {
            searchTextTimer.Stop();
            searchTextTimer.Start();
        }

        private void SetCurrentValue(string value)
        {
            CurrentValue = value;
        }
    }
}
