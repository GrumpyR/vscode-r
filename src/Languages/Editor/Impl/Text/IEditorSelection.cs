﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Languages.Core.Text;

namespace Microsoft.Languages.Editor.Text {
    public interface IEditorSelection {
        SelectionMode Mode { get; }
        ITextRange SelectedRange { get; }
    }
}
