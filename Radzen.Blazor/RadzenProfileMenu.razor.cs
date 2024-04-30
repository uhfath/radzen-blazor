using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Radzen.Blazor
{
    /// <summary>
    /// RadzenProfileMenu component.
    /// </summary>
    /// <example>
    /// <code>
    /// &lt;RadzenProfileMenu&gt;
    ///     &lt;RadzenProfileMenuItem Text="Data"&gt;
    ///         &lt;RadzenProfileMenuItem Text="Orders" Path="orders" /&gt;
    ///         &lt;RadzenProfileMenuItem Text="Employees" Path="employees" /&gt;
    ///     &lt;/RadzenProfileMenuItemItem&gt;
    /// &lt;/RadzenProfileMenu&gt;
    /// </code>
    /// </example>
    public partial class RadzenProfileMenu : RadzenComponentWithChildren<RadzenProfileMenu.RadzenProfileMenuContext>
    {
        /// <inheritdoc />
        protected override string GetComponentCssClass()
        {
            return "rz-menu rz-profile-menu";
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        [Parameter]
        public RenderFragment Template { get; set; }

        /// <summary>
        /// Show/Hide the "arrow down" icon
        /// </summary>
        /// <value>Show the "arrow down" icon.</value>
        [Parameter]
        public bool ShowIcon { get; set; } = true;

        string contentStyle = "display:none;position:absolute;z-index:1;";
        string iconStyle = "transform: rotate(0deg);";

        /// <summary>
        /// Toggles the menu open/close state.
        /// </summary>
        /// <param name="args">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        public async Task Toggle(MouseEventArgs args)
        {
            contentStyle = contentStyle.IndexOf("display:none;") != -1 ? "display:block;" : "display:none;position:absolute;z-index:1;";
            iconStyle = iconStyle.IndexOf("rotate(0deg)") != -1 ? "transform: rotate(-180deg);" : "transform: rotate(0deg);";
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            contentStyle = "display:none;";
            iconStyle = "transform: rotate(0deg);";
            StateHasChanged();
        }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        internal int focusedIndex = -1;
        private readonly RadzenProfileMenuContext menuContext = new();

        private void OnMenuMouseDown(MouseEventArgs eventArgs)
        {
            menuContext.CtrlKey = eventArgs.CtrlKey;
            menuContext.ShiftKey = eventArgs.ShiftKey;
            menuContext.AltKey = eventArgs.AltKey;
            menuContext.MetaKey = eventArgs.MetaKey;
        }

        bool preventKeyPress = true;
        async Task OnKeyPress(KeyboardEventArgs args)
        {
            var key = args.Code != null ? args.Code : args.Key;

            if (key == "ArrowUp" || key == "ArrowDown")
            {
                preventKeyPress = true;

                focusedIndex = Math.Clamp(focusedIndex + (key == "ArrowUp" ? -1 : 1), 0, items.Count - 1);
            }
            else if (key == "Space" || key == "Enter")
            {
                preventKeyPress = true;

                if (focusedIndex >= 0 && focusedIndex < items.Count)
                {
                    var item = items[focusedIndex];

                    if (item.Path != null)
                    {
                        NavigationManager.NavigateTo(item.Path);
                    }
                    else
                    {
                        await item.OnClick.InvokeAsync(new MouseEventArgs());
                    }
                }
                else
                {
                    await Toggle(new MouseEventArgs());

                    if (contentStyle.IndexOf("display:none;") == -1)
                    {
                        focusedIndex = 0;
                    }
                }
            }
            else if (key == "Escape")
            {
                preventKeyPress = true;

                Close();
            }
            else
            {
                preventKeyPress = false;
            }
        }

        internal bool IsFocused(RadzenProfileMenuItem item)
        {
            return items.IndexOf(item) == focusedIndex && focusedIndex != -1;
        }

        internal List<RadzenProfileMenuItem> items = new List<RadzenProfileMenuItem>();
        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddItem(RadzenProfileMenuItem item)
        {
            if (items.IndexOf(item) == -1)
            {
                items.Add(item);
            }
        }

        /// <summary>
        /// Menu context
        /// </summary>
        public record RadzenProfileMenuContext
        {
            /// <summary>
            /// true if the control key was down when the event was fired. false otherwise.
            /// </summary>
            public bool CtrlKey { get; set; }

            /// <summary>
            /// true if the shift key was down when the event was fired. false otherwise.
            /// </summary>
            public bool ShiftKey { get; set; }

            /// <summary>
            /// true if the alt key was down when the event was fired. false otherwise.
            /// </summary>
            public bool AltKey { get; set; }

            /// <summary>
            /// true if the meta key was down when the event was fired. false otherwise.
            /// </summary>
            public bool MetaKey { get; set; }
        }
    }
}
