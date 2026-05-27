import { useState, useCallback, useMemo } from 'react';
import { useQuery } from '@apollo/client/react';
import {
  Container,
  Grid,
  Typography,
  CircularProgress,
  Alert,
  Stack,
  TextField,
  Button,
  Pagination,
  Divider,
} from '@mui/material';
import { GET_ROOMS_WITH_PAGINATION_AND_DATE_FILTER } from '../graphql/operations';
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
        <Grid
          key={room.id}
          size={GRID_ITEM_SIZE}
          sx={{ display: 'flex', minWidth: 0 }}
        >
          <RoomCard room={room} onOpen={() => onRoomOpen(room)} />
        </Grid>
      ))}
    </Grid>
  );
}

function RoomsPage() {
  const [fromDateInput, setFromDateInput] = useState('');
  const [toDateInput, setToDateInput] = useState('');
  const [fromDate, setFromDate] = useState('');
  const [toDate, setToDate] = useState('');
  const [page, setPage] = useState(1);
  const [pageSize] = useState(9);
  const [detailsRoom, setDetailsRoom] = useState<RoomViewModel | null>(null);

  const variables = useMemo(
    () => ({
      from: fromDate ? new Date(fromDate).toISOString() : null,
      to: toDate ? new Date(toDate).toISOString() : null,
      page,
      pageSize,
    }),
    [fromDate, toDate, page, pageSize],
  );

  const { data, loading, error } = useQuery<{ rooms: RoomViewModel[] }>(
    GET_ROOMS_WITH_PAGINATION_AND_DATE_FILTER,
    { variables },
  );

  const closeDetails = useCallback(() => {
    setDetailsRoom(null);
  }, []);

  const handleApplyFilter = useCallback(() => {
    setFromDate(fromDateInput);
    setToDate(toDateInput);
    setPage(1);
  }, [fromDateInput, toDateInput]);

  const handleResetFilter = useCallback(() => {
    setFromDateInput('');
    setToDateInput('');
    setFromDate('');
    setToDate('');
    setPage(1);
  }, []);

  const rooms = data?.rooms ?? [];
  const hasNextPage = rooms.length === pageSize;
  const paginationCount = hasNextPage ? page + 1 : page;

  return (
    <Container
      maxWidth="lg"
      style={{ marginTop: '2rem', marginBottom: '2rem' }}
    >
      <Stack direction="column" spacing={2}>
        <Typography variant="h4" component="h1" gutterBottom>
          Rooms overview
        </Typography>

        <Stack
          direction={{ xs: 'column', md: 'row' }}
          spacing={2}
          alignItems={{ xs: 'stretch', md: 'center' }}
        >
          <TextField
            label="From"
            type="datetime-local"
            size="small"
            value={fromDateInput}
            onChange={(event) => setFromDateInput(event.target.value)}
            InputLabelProps={{ shrink: true }}
          />
          <TextField
            label="To"
            type="datetime-local"
            size="small"
            value={toDateInput}
            onChange={(event) => setToDateInput(event.target.value)}
            InputLabelProps={{ shrink: true }}
          />
          <Button variant="contained" onClick={handleApplyFilter}>
            Apply
          </Button>
          <Button variant="outlined" onClick={handleResetFilter}>
            Reset
          </Button>
        </Stack>

        <Divider />

        {loading && (
          <Stack direction="row" alignItems="center" spacing={2}>
            <CircularProgress size={24} />
            <Typography>Loading rooms from GraphQL...</Typography>
          </Stack>
        )}

        {error && (
          <Alert severity="error">Error loading rooms: {error.message}</Alert>
        )}

        {data && rooms.length > 0 && (
          <RoomsGrid rooms={rooms} onRoomOpen={setDetailsRoom} />
        )}

        {data && rooms.length === 0 && !loading && !error && (
          <Typography>No rooms found.</Typography>
        )}

        <Stack direction="row" justifyContent="center">
          <Pagination
            page={page}
            count={paginationCount}
            color="primary"
            onChange={(_, value) => setPage(value)}
            showFirstButton
            showLastButton
          />
        </Stack>
      </Stack>

      <RoomDetailsDialog room={detailsRoom} onClose={closeDetails} />
    </Container>
  );
}

export default RoomsPage;
