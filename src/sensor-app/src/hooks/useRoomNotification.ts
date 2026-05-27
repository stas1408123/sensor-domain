import { useEffect, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { NOTIFICATION_HUB_URL } from '../constants';

export interface RoomUpdateMessage {
  roomId: string;
  type: number | string;
}

type RoomUpdateListener = (message: RoomUpdateMessage) => void;

let connection: signalR.HubConnection | null = null;
let startPromise: Promise<void> | null = null;
const listeners = new Set<RoomUpdateListener>();

function getConnection(): signalR.HubConnection {
  if (!connection) {
    connection = new signalR.HubConnectionBuilder()
      .withUrl(NOTIFICATION_HUB_URL)
      .withAutomaticReconnect()
      .build();

    connection.on('RoomUpdate', (message: RoomUpdateMessage) => {
      listeners.forEach((listener) => listener(message));
    });
  }
  return connection;
}

async function ensureConnected(): Promise<void> {
  const hub = getConnection();
  if (hub.state === signalR.HubConnectionState.Connected) {
    return;
  }
  if (hub.state === signalR.HubConnectionState.Connecting && startPromise) {
    await startPromise;
    return;
  }
  startPromise = hub.start();
  await startPromise;
}

export function useRoomUpdateNotification(
  onUpdate: (message: RoomUpdateMessage) => void,
  enabled: boolean,
) {
  const onUpdateRef = useRef(onUpdate);

  useEffect(() => {
    onUpdateRef.current = onUpdate;
  }, [onUpdate]);

  useEffect(() => {
    if (!enabled) {
      return;
    }

    const listener: RoomUpdateListener = (message) => {
      onUpdateRef.current(message);
    };

    listeners.add(listener);
    void ensureConnected().catch(() => {
      listeners.delete(listener);
    });

    return () => {
      listeners.delete(listener);
    };
  }, [enabled]);
}
