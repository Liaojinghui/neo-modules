#nullable enable
namespace Neo.Plugins.WebSocketServer.Filters;

public class BlockFilter
{
    public int? Primary { get; set; }
    public uint? Since { get; set; }
    public uint? Till { get; set; }
}

public class TxFilter
{
    public UInt160? Sender { get; set; }
    public UInt160? Signer { get; set; }
}

public class NotificationFilter
{

    public UInt160? Contract { get; set; }
    public string? Name { get; set; }

}

public class ExecutionFilter
{
    public string? State { get; set; }
    public UInt256? Container { get; set; }

}
public interface IComparator
{
    WssEventId WssEventId { get; }
    object Filter { get; }
}

public interface IContainer
{
    WssEventId WssEventId { get; }
    object EventPayload { get; }
}
