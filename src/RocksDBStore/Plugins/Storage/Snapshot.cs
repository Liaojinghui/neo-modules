// Copyright (C) 2015-2023 The Neo Project.
//
// The Neo.Plugins.Storage.RocksDBStore is free software distributed under the MIT software license,
// see the accompanying file LICENSE in the main directory of the
// project or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Persistence;
using RocksDbSharp;
using System;
using System.Collections.Generic;

namespace Neo.Plugins.Storage
{
    internal class Snapshot : ISnapshot
    {
        private readonly RocksDb _db;
        private readonly RocksDbSharp.Snapshot _snapshot;
        private readonly WriteBatch _batch;
        private readonly ReadOptions _options;

        public Snapshot(RocksDb db)
        {
            this._db = db;
            this._snapshot = db.CreateSnapshot();
            this._batch = new WriteBatch();

            _options = new ReadOptions();
            _options.SetFillCache(false);
            _options.SetSnapshot(_snapshot);
        }

        public void Commit()
        {
            _db.Write(_batch, Options.WriteDefault);
        }

        public void Delete(byte[] key)
        {
            _batch.Delete(key);
        }

        public void Put(byte[] key, byte[] value)
        {
            _batch.Put(key, value);
        }

        public IEnumerable<(byte[] Key, byte[] Value)> Seek(byte[] keyOrPrefix, SeekDirection direction)
        {
            if (keyOrPrefix == null) keyOrPrefix = Array.Empty<byte>();

            using var it = _db.NewIterator(readOptions: _options);

            if (direction == SeekDirection.Forward)
                for (it.Seek(keyOrPrefix); it.Valid(); it.Next())
                    yield return (it.Key(), it.Value());
            else
                for (it.SeekForPrev(keyOrPrefix); it.Valid(); it.Prev())
                    yield return (it.Key(), it.Value());
        }

        public bool Contains(byte[] key)
        {
            return _db.Get(key, Array.Empty<byte>(), 0, 0, readOptions: _options) >= 0;
        }

        public byte[] TryGet(byte[] key)
        {
            return _db.Get(key, readOptions: _options);
        }

        public void Dispose()
        {
            _snapshot.Dispose();
            _batch.Dispose();
        }
    }
}
