using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Radzen.Blazor.Rendering;

namespace Radzen.Blazor
{
    /// <summary>
    /// RadzenCard component.
    /// </summary>
    public partial class RadzenCard : RadzenComponentWithChildren
    {
        /// <summary>
        /// Gets or sets the card shadow size.
        /// </summary>
        /// <value>The card shadow size.</value>
        [Parameter]
        public int? Shadow { get; set; }

        /// <inheritdoc />
        protected override string GetComponentCssClass()
        {
            return ClassList.Create("rz-card")
                .Add($"rz-variant-{Variant.ToString().ToLowerInvariant()}")
                .Add($"rz-shadow-{Shadow ?? 2}", Shadow.HasValue || Variant == Variant.Filled)
                .ToString();
        }

        /// <summary>
        /// Gets or sets the card variant.
        /// </summary>
        /// <value>The card variant.</value>
        [Parameter]
        public Variant Variant { get; set; } = Variant.Filled;
    }
}