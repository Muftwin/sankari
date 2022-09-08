﻿namespace Sankari.Netcode;

// Received from Game Client
public enum ClientPacketOpcode
{
    Ping,
    Lobby,
    PlayerPosition,
    PlayerMovementDirections,
    PlayerRotation,
    PlayerShoot,
    PlayerJoinServer
}

// Sent to Game Client
public enum ServerPacketOpcode
{
    Pong,
    Lobby,
    PlayerTransforms,
    Game,
    EnemyPositions,
    PlayerJoined,
    PlayersOnServer
}

public enum GameOpcode
{
    EnemiesSpawned
}

public enum LobbyOpcode
{
    LobbyCreate,
    LobbyJoin,
    LobbyLeave,
    LobbyKick,
    LobbyInfo,
    LobbyChatMessage,
    LobbyReady,
    LobbyCountdownChange,
    LobbyGameStart
}

public enum DisconnectOpcode
{
    Disconnected,
    Timeout,
    Maintenance,
    Restarting,
    Stopping,
    Kicked,
    Banned
}
