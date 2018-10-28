﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Microsoft.R.Host.Client {
    public class BrokerStateChangedEventArgs : EventArgs {
        public bool IsConnected { get; }
        public BrokerStateChangedEventArgs(bool isConnected) {
            IsConnected = isConnected;
        }
    }
}