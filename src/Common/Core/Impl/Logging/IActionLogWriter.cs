﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microsoft.Common.Core.Logging {
    public interface IActionLogWriter {
        void Write(MessageCategory category, string message);
        void Flush();
    }
}