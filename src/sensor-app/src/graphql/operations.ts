import { gql } from '@apollo/client';

export const GET_ROOMS = gql`
  query GetRooms {
    rooms {
      id
      name
      airQualities {
        id
        roomId
        co2
        pm25
        humidity
        timestamp
      }
      energies {
        id
        roomId
        consumptionEnergy
        timestamp
      }
      motions {
        id
        roomId
        motionDetected
        timestamp
      }
    }
  }
`;

export const GET_ROOMS_WITH_PAGINATION_AND_DATE_FILTER = gql`
  query GetRoomsWithPaginationAndDateFilter(
    $from: DateTime
    $to: DateTime
    $page: Int
    $pageSize: Int
  ) {
    rooms(from: $from, to: $to, page: $page, pageSize: $pageSize) {
      id
      name
      airQualities {
        id
        roomId
        co2
        pm25
        humidity
        timestamp
      }
      energies {
        id
        roomId
        consumptionEnergy
        timestamp
      }
      motions {
        id
        roomId
        motionDetected
        timestamp
      }
    }
  }
`;

export const GET_ROOM_BY_ID = gql`
  query GetRoomById($id: UUID!) {
    roomById(id: $id) {
      id
      name
      airQualities {
        id
        roomId
        co2
        pm25
        humidity
        timestamp
      }
      energies {
        id
        roomId
        consumptionEnergy
        timestamp
      }
      motions {
        id
        roomId
        motionDetected
        timestamp
      }
    }
  }
`;
