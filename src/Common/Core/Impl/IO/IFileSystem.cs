﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Microsoft.Common.Core.IO {
    public interface IFileSystem {
        IFileSystemWatcher CreateFileSystemWatcher(string directory, string filter);
        IDirectoryInfo GetDirectoryInfo(string directoryPath);
        bool FileExists(string fullPath);
        bool DirectoryExists(string fullPath);

        long FileSize(string path);

        FileAttributes GetFileAttributes(string fullPath);
        string ToLongPath(string path);
        string ToShortPath(string path);

        string ReadAllText(string path);
        void WriteAllText(string path, string content);

        IEnumerable<string> FileReadAllLines(string path);
        void FileWriteAllLines(string path, IEnumerable<string> contents);

        byte[] FileReadAllBytes(string path);
        void FileWriteAllBytes(string path, byte[] bytes);

        Stream CreateFile(string path);
        Stream FileOpen(string path, FileMode mode);

        Version GetFileVersion(string path);
        void DeleteFile(string path);
        void DeleteDirectory(string path, bool recursive);
        string[] GetFileSystemEntries(string path, string searchPattern, SearchOption options);
        void CreateDirectory(string path);

        string[] GetFiles(string path);
        string[] GetFiles(string path, string pattern);
        string[] GetFiles(string path, string pattern, SearchOption option);
        string[] GetDirectories(string path);

        string GetDownloadsPath(string fileName = "");

        string CompressFile(string path, string relativeTodir = null);
        string CompressFiles(IEnumerable<string> paths, string relativeToDir, IProgress<string> progress, CancellationToken ct);
        string CompressDirectory(string path);
        string CompressDirectory(string path, Matcher matcher, IProgress<string> progress, CancellationToken ct);
    }
}
