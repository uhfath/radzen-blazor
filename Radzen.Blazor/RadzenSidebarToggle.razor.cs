﻿using Microsoft.AspNetCore.Components;
using System;

namespace Radzen.Blazor
{
    /// <summary>
    /// RadzenSidebarToggle component.
    /// </summary>
    public partial class RadzenSidebarToggle : RadzenComponent
    {
        /// <summary>
        /// Gets or sets the click callback.
        /// </summary>
        /// <value>The click callback.</value>
        [Parameter]
        public EventCallback<EventArgs> Click { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Handles the <see cref="E:Click" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public async System.Threading.Tasks.Task OnClick(EventArgs args)
        {
            await Click.InvokeAsync(args);
        }

        /// <inheritdoc />
        protected override string GetComponentCssClass()
        {
            return "rz-sidebar-toggle";
        }

        /// <summary>
        /// Gets or sets the add button aria-label attribute.
        /// </summary>
        [Parameter]
        public string ToggleAriaLabel { get; set; } = "Toggle";
    }
}
