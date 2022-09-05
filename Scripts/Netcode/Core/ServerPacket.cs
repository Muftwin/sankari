using ENet;

namespace Sankari.Netcode;

public class ServerPacket : GamePacket
{
    public Peer[] Peers { get; private set; }

    public ServerPacket(byte opcode, PacketFlags packetFlags, APacket writable = null, params Peer[] peers)
    {
        using (var writer = new PacketWriter())
        {
            writer.Write(opcode);
            writable?.Write(writer);

            Data = writer.Stream.ToArray();
            Size = writer.Stream.Length;
        }

        Opcode = opcode;
        PacketFlags = packetFlags;
        Peers = peers;
    }
}
