﻿using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using ConcurrentCollections;
using Statiq.Common;

namespace Statiq.Testing
{
    public class TestFileProvider : IFileProvider, IEnumerable<NormalizedPath>
    {
        public TestFileProvider()
        {
        }

        public TestFileProvider(params NormalizedPath[] directories)
        {
            Directories.AddRange(directories);
        }

        public ICollection<NormalizedPath> Directories { get; } = new ConcurrentHashSet<NormalizedPath>();

        public ConcurrentDictionary<NormalizedPath, StringBuilder> Files { get; } = new ConcurrentDictionary<NormalizedPath, StringBuilder>();

        public IDirectory GetDirectory(NormalizedPath path) => new TestDirectory(this, path);

        public IFile GetFile(NormalizedPath path) => new TestFile(this, path);

        public void AddDirectory(NormalizedPath path)
        {
            if (path.IsNullOrEmpty)
            {
                return;
            }

            Directories.Add(path);

            // Add all parents
            NormalizedPath parent = path.Parent;
            while (!parent.IsNullOrEmpty)
            {
                Directories.Add(parent);
                parent = parent.Parent;
            }
        }

        public void AddFile(NormalizedPath path, string content = "")
        {
            if (path.IsNullOrEmpty)
            {
                return;
            }

            // Add the directory (and parents)
            Files[path] = new StringBuilder(content);
            AddDirectory(path.Parent);
        }

        public void Add(NormalizedPath path, string content = "") => AddFile(path, content);

        public IEnumerator<NormalizedPath> GetEnumerator() => Files.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
