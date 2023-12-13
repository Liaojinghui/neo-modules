// Copyright (C) 2015-2023 The Neo Project.
//
// The Neo.Network.RPC is free software distributed under the MIT software license,
// see the accompanying file LICENSE in the main directory of the
// project or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Json;
using System.Linq;

namespace Neo.Network.RPC.Models
{
    public class RpcPeers
    {
        public RpcPeer[] Unconnected { get; set; }

        public RpcPeer[] Bad { get; set; }

        public RpcPeer[] Connected { get; set; }

        public JObject ToJson()
        {
            return new JObject
            {
                ["unconnected"] = new JArray(Unconnected.Select(p => p.ToJson())),
                ["bad"] = new JArray(Bad.Select(p => p.ToJson())),
                ["connected"] = new JArray(Connected.Select(p => p.ToJson()))
            };
        }

        public static RpcPeers FromJson(JObject json)
        {
            return new RpcPeers
            {
                Unconnected = ((JArray)json["unconnected"]).Select(p => RpcPeer.FromJson((JObject)p)).ToArray(),
                Bad = ((JArray)json["bad"]).Select(p => RpcPeer.FromJson((JObject)p)).ToArray(),
                Connected = ((JArray)json["connected"]).Select(p => RpcPeer.FromJson((JObject)p)).ToArray()
            };
        }
    }

    public class RpcPeer
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public JObject ToJson()
        {
            return new JObject
            {
                ["address"] = Address,
                ["port"] = Port
            };
        }

        public static RpcPeer FromJson(JObject json)
        {
            return new RpcPeer
            {
                Address = json["address"].AsString(),
                Port = int.Parse(json["port"].AsString())
            };
        }
    }
}
