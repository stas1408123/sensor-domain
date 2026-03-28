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
