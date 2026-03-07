export interface AirQualityViewModel {
  id: string;
  roomId: string;
  co2: number;
  pm25: number;
  humidity: number;
  timestamp: string; // ISO 8601 DateTime
}

export interface EnergyViewModel {
  id: string;
  roomId: string;
  consumptionEnergy: number;
  timestamp: string; // ISO 8601 DateTime
}

export interface MotionViewModel {
  id: string;
  roomId: string;
  motionDetected: boolean;
  timestamp: string; // ISO 8601 DateTime
}

export interface RoomViewModel {
  id: string;
  name: string;
  airQualities?: AirQualityViewModel[];
  energies?: EnergyViewModel[];
  motions?: MotionViewModel[];
}

// Optional aggregate types for queries
export interface RoomQueryResult {
  rooms: RoomViewModel[];
  roomById: RoomViewModel;
}

export interface GetRoomsData {
  rooms: RoomViewModel[];
}

export interface GetRoomByIdData {
  roomById: RoomViewModel;
}

export interface GetRoomByIdVariables {
  id: string;
}

