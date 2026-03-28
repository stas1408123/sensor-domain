import {
  Card,
  CardActionArea,
  CardContent,
  Typography,
  Stack,
} from '@mui/material';
import type { RoomViewModel } from '../../types/graphql';

interface RoomCardProps {
  room: RoomViewModel;
  onOpen: () => void;
}

const cardSx = {
  backgroundColor: '#1565c0',
  color: '#fff',
  border: 'none',
  willChange: 'filter',
  transition: 'filter 300ms',
  width: '100%',
  minWidth: 0,
  height: '100%',
  minHeight: 220,
  display: 'flex',
  flexDirection: 'column',
  '& .MuiCardContent-root': {
    flex: 1,
    display: 'flex',
    flexDirection: 'column',
    minWidth: 0,
    overflow: 'hidden',
  },
  '&:hover': {
    filter: 'drop-shadow(0 0 2em rgba(33, 150, 243, 0.8))',
  },
  '& .MuiCardActionArea-root': {
    height: '100%',
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'stretch',
    justifyContent: 'flex-start',
  },
  '& .MuiTypography-root': {
    color: 'rgba(255, 255, 255, 0.9)',
  },
  '& .MuiTypography-body2': {
    color: 'rgba(255, 255, 255, 0.85)',
    overflowWrap: 'break-word',
  },
};

export function RoomCard({ room, onOpen }: RoomCardProps) {
  const latestAirQuality =
    room.airQualities && room.airQualities.length > 0
      ? room.airQualities[room.airQualities.length - 1]
      : undefined;

  const latestEnergy =
    room.energies && room.energies.length > 0
      ? room.energies[room.energies.length - 1]
      : undefined;

  const latestMotion =
    room.motions && room.motions.length > 0
      ? room.motions[room.motions.length - 1]
      : undefined;

  return (
    <Card variant="outlined" sx={cardSx}>
      <CardActionArea
        onClick={onOpen}
        aria-label={`Open details for ${room.name}`}
      >
        <CardContent>
          <Typography variant="h6" gutterBottom>
            {room.name}
          </Typography>

          <Stack spacing={1}>
            <Typography variant="body2" color="text.secondary">
              Air quality:{' '}
              {latestAirQuality
                ? `CO₂: ${latestAirQuality.co2} ppm, PM2.5: ${latestAirQuality.pm25} µg/m³, Humidity: ${latestAirQuality.humidity}%`
                : 'No data'}
            </Typography>

            <Typography variant="body2" color="text.secondary">
              Energy:{' '}
              {latestEnergy
                ? `${latestEnergy.consumptionEnergy} kWh`
                : 'No data'}
            </Typography>

            <Typography variant="body2" color="text.secondary">
              Motion:{' '}
              {latestMotion
                ? latestMotion.motionDetected
                  ? 'Detected'
                  : 'Not detected'
                : 'No data'}
            </Typography>
          </Stack>
        </CardContent>
      </CardActionArea>
    </Card>
  );
}
