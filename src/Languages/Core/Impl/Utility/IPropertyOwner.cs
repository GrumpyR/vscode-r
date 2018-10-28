﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Common.Core.Services;

namespace Microsoft.Languages.Core.Utility {
    /// <summary>
    /// Provides ownership of an arbitrary set of properties.
    /// </summary>
    /// <remarks>This interface should be implemented for classes that need to control the lifetimes of their properties.</remarks>
    public interface IPropertyOwner {
        /// <summary>
        /// The collection of properties controlled by the property owner.
        /// </summary>
        PropertyDictionary Properties { get; }
    }
}
