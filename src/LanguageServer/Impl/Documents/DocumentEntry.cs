﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Common.Core.Services;
using Microsoft.Common.Core.Threading;
using Microsoft.Languages.Core.Text;
using Microsoft.Languages.Editor.Completions;
using Microsoft.Languages.Editor.Text;
using Microsoft.R.Editor.Completions;
using Microsoft.R.Editor.Document;
using Microsoft.R.LanguageServer.Completions;
using Microsoft.R.LanguageServer.Extensions;
using Microsoft.R.LanguageServer.Formatting;
using Microsoft.R.LanguageServer.Symbols;
using Microsoft.R.LanguageServer.Text;
using Microsoft.R.LanguageServer.Validation;

namespace Microsoft.R.LanguageServer.Documents {
    internal sealed class DocumentEntry : IDisposable {
        private readonly IServiceContainer _services;
        private readonly CompletionManager _completionManager;
        private readonly SignatureManager _signatureManager;
        private readonly DiagnosticsPublisher _diagnosticsPublisher;
        private readonly CodeFormatter _formatter;
        private readonly DocumentSymbolsProvider _symbolsProvider;

        public IEditorBuffer EditorBuffer { get; }
        public IREditorDocument Document { get; }

        public DocumentEntry(string content, Uri uri, IServiceContainer services) {
            _services = services;

            EditorBuffer = new EditorBuffer(content, "R");
            Document = new REditorDocument(EditorBuffer, services, false);

            _completionManager = new CompletionManager(services);
            _signatureManager = new SignatureManager(services);
            _diagnosticsPublisher = new DiagnosticsPublisher(services.GetService<IVsCodeClient>(), Document, uri, services);
            _formatter = new CodeFormatter(_services);
            _symbolsProvider = new DocumentSymbolsProvider();
        }

        public async Task ProcessChangesAsync(TextDocumentContentChangedEvent[] contentChanges) {
            await _services.MainThread().SwitchToAsync();

            foreach (var change in contentChanges) {
                if (change.range == null) {
                    continue;
                }

                var position = EditorBuffer.ToStreamPosition(change.Range.Start);
                var range = new TextRange(position, change.rangeLength);
                if (!string.IsNullOrEmpty(change.text)) {
                    // Insert or replace
                    if (change.rangeLength == 0) {
                        EditorBuffer.Insert(position, change.Text);
                    } else {
                        EditorBuffer.Replace(range, change.Text);
                    }
                } else {
                    EditorBuffer.Delete(range);
                }
            }
        }

        [DebuggerStepThrough]
        public void Dispose() => Document?.Close();

        [DebuggerStepThrough]
        public CompletionList GetCompletions(Position position)
            => _completionManager.GetCompletions(CreateContext(position));

        public Task<SignatureHelp> GetSignatureHelpAsync(Position position)
            => _signatureManager.GetSignatureHelpAsync(CreateContext(position));

        [DebuggerStepThrough]
        public Task<Hover> GetHoverAsync(Position position, CancellationToken ct)
            => _signatureManager.GetHoverAsync(CreateContext(position), ct);

        [DebuggerStepThrough]
        public Task<TextEdit[]> FormatAsync()
            => _formatter.FormatAsync(EditorBuffer.CurrentSnapshot);

        [DebuggerStepThrough]
        public Task<TextEdit[]> FormatRangeAsync(Range range)
            => _formatter.FormatRangeAsync(EditorBuffer.CurrentSnapshot, range);

        [DebuggerStepThrough]
        public Task<TextEdit[]> AutoformatAsync(Position position, string typeChar)
            => _formatter.AutoformatAsync(EditorBuffer.CurrentSnapshot, position, typeChar);

        [DebuggerStepThrough]
        public SymbolInformation[] GetSymbols(Uri uri)
            => _symbolsProvider.GetSymbols(Document, uri);

        private IRIntellisenseContext CreateContext(Position position) {
            var bufferPosition = EditorBuffer.ToStreamPosition(position);
            var session = new EditorIntellisenseSession(new EditorView(EditorBuffer, position.ToStreamPosition(EditorBuffer.CurrentSnapshot)), _services);
            return new RIntellisenseContext(session, EditorBuffer, Document.EditorTree, bufferPosition);
        }
    }
}
