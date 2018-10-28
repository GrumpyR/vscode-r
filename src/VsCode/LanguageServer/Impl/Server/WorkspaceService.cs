﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// Based on https://github.com/CXuesong/LanguageServer.NET

using System.Collections.Generic;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;
using LanguageServer.VsCode.Contracts;
using Microsoft.R.LanguageServer.Commands;
using Microsoft.R.LanguageServer.Server.Settings;

namespace Microsoft.R.LanguageServer.Server {
    [JsonRpcScope(MethodPrefix = "workspace/")]
    public sealed class WorkspaceService : LanguageServiceBase {
        /// <summary>
        /// Called by VS Code when configuration (settings) change.
        /// https://github.com/Microsoft/language-server-protocol/blob/master/protocol.md#didchangeconfiguration-notification
        /// </summary>
        /// <param name="settings"></param>
        [JsonRpcMethod(IsNotification = true)]
        public void DidChangeConfiguration(SettingsRoot settings)
            => Services.GetService<ISettingsManager>().UpdateSettings(settings.R);

        [JsonRpcMethod(IsNotification = true)]
        public void DidChangeWatchedFiles(ICollection<FileEvent> changes) { }

        [JsonRpcMethod]
        public Task<object> ExecuteCommand(string command, object[] arguments)
            => Services.GetService<IController>().ExecuteAsync(command, arguments);
    }
}
