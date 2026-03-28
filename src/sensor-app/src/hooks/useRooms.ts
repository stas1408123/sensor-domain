import { useQuery } from '@apollo/client/react';
import { GET_ROOMS, GET_ROOM_BY_ID } from '../graphql/operations';
import type {
  GetRoomsData,
  GetRoomByIdData,
  GetRoomByIdVariables,
} from '../types/graphql';

export function useRooms() {
  return useQuery<GetRoomsData>(GET_ROOMS);
}

export function useRoomById(id: string) {
  return useQuery<GetRoomByIdData, GetRoomByIdVariables>(GET_ROOM_BY_ID, {
    variables: { id },
  });
}
