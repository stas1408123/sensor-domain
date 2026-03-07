import { useState, useCallback } from 'react';
import { Container, Grid, Typography, CircularProgress, Alert, Stack } from '@mui/material';
import { useRooms } from '../hooks/useRooms';
import { RoomCard, RoomDetailsDialog } from '../components';
import type { RoomViewModel } from '../types/graphql';

const GRID_COLUMNS = { xs: 1, sm: 3, md: 3, lg: 3 } as const;
const GRID_ITEM_SIZE = { xs: 1, sm: 1, md: 1, lg: 1 } as const;

function RoomsGrid({
  rooms,
  onRoomOpen,
}: {
  rooms: RoomViewModel[];
  onRoomOpen: (room: RoomViewModel) => void;
}) {
  return (
    <Grid container spacing={2} columns={GRID_COLUMNS}>
      {rooms.map((room) => (
        <Grid key={room.id} size={GRID_ITEM_SIZE} sx={{ display: 'flex', minWidth: 0 }}>
          <RoomCard room={room} onOpen={() => onRoomOpen(room)} />
        </Grid>
      ))}
    </Grid>
  );
}

function RoomsPage() {
  const { data, loading, error } = useRooms();
  const [detailsRoom, setDetailsRoom] = useState<RoomViewModel | null>(null);

  const closeDetails = useCallback(() => {
    setDetailsRoom(null);
  }, []);

  return (
    <Container maxWidth="lg" style={{ marginTop: '2rem', marginBottom: '2rem' }}>
      <Stack direction="column" spacing={2}>
        <Typography variant="h4" component="h1" gutterBottom>
          Rooms overview
        </Typography>

        {loading && (
          <Stack direction="row" alignItems="center" spacing={2}>
            <CircularProgress size={24} />
            <Typography>Loading rooms from GraphQL...</Typography>
          </Stack>
        )}

        {error && <Alert severity="error">Error loading rooms: {error.message}</Alert>}

        {data && data.rooms.length > 0 && (
          <RoomsGrid rooms={data.rooms} onRoomOpen={setDetailsRoom} />
        )}

        {data && data.rooms.length === 0 && !loading && !error && (
          <Typography>No rooms found.</Typography>
        )}
      </Stack>

      <RoomDetailsDialog room={detailsRoom} onClose={closeDetails} />
    </Container>
  );
}

export default RoomsPage;
