﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.R.Components.InteractiveWorkflow;

namespace Microsoft.R.Components.ConnectionManager {
    public interface IConnectionManagerProvider {
        IConnectionManager CreateConnectionManager(IRInteractiveWorkflow interactiveWorkflow);
    }
}
