﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Statiq.Common;

namespace Statiq.Core
{
    /// <summary>
    /// Reads text provided to the application on startup.
    /// </summary>
    /// <remarks>
    /// This modules creates a single document from text provided to Statiq on startup. In most cases, this will be text or file contents
    /// "piped" to the Statiq.exe via the command line from a file or prior chained executable. Also known as "Standard Input" or "STDIN".
    /// </remarks>
    /// <category>Input/Output</category>
    public class ReadApplicationInput : Module
    {
        /// <inheritdoc />
        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            // If ApplicationInput is empty, return nothing
            if (string.IsNullOrWhiteSpace(context.ApplicationState.Input))
            {
                return Array.Empty<IDocument>();
            }

            return context.CreateDocument(
                await context.GetContentProviderAsync(context.ApplicationState.Input))
                .Yield();
        }
    }
}
