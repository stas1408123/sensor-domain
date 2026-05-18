import { useCallback } from 'react';
import { useQuery } from '@apollo/client/react';
import {
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Divider,
  Stack,
  Typography,
} from '@mui/material';
import { GET_ROOM_BY_ID } from '../../graphql/operations';
import { useRoomUpdateNotification } from '../../hooks/useRoomNotification';
import type {
  GetRoomByIdData,
  GetRoomByIdVariables,
  RoomViewModel,
} from '../../types/graphql';
import { RoomMetricCharts } from './RoomMetricCharts';

interface RoomDetailsDialogProps {
  room: RoomViewModel | null;
  onClose: () => void;
}

function normalizeRoomId(id: string): string {
  return id.replace(/-/g, '').toLowerCase();
}

function mergeRoomMetrics(
  initial: RoomViewModel | null,
  fetched: RoomViewModel | undefined,
): RoomViewModel | null {
  if (!fetched) {
    return initial;
  }
  if (!initial) {
    return fetched;
  }

  return {
    ...fetched,
    airQualities:
      fetched.airQualities && fetched.airQualities.length > 0
        ? fetched.airQualities
        : initial.airQualities,
    energies:
      fetched.energies && fetched.energies.length > 0
        ? fetched.energies
        : initial.energies,
    motions:
      fetched.motions && fetched.motions.length > 0
        ? fetched.motions
        : initial.motions,
  };
}

export function RoomDetailsDialog({ room, onClose }: RoomDetailsDialogProps) {
  const open = Boolean(room);
  const roomId = room?.id ?? '';

  const { data, loading, refetch } = useQuery<
    GetRoomByIdData,
    GetRoomByIdVariables
  >(GET_ROOM_BY_ID, {
    variables: { id: roomId },
    skip: !open,
  });

  const displayRoom = mergeRoomMetrics(room, data?.roomById);

  const handleRoomUpdate = useCallback(
    (message: { roomId: string }) => {
      if (normalizeRoomId(message.roomId) !== normalizeRoomId(roomId)) {
        return;
      }
      void refetch();
    },
    [roomId, refetch],
  );

  useRoomUpdateNotification(handleRoomUpdate, open);

  return (
    <Dialog
      open={open}
      onClose={onClose}
      maxWidth="md"
      fullWidth
      scroll="paper"
      slotProps={{
        paper: {
          sx: (theme) => ({
            backgroundColor: theme.palette.grey[300],
            backgroundImage: 'none',
            boxShadow: theme.shadows[8],
          }),
        },
      }}
    >
      {displayRoom && (
        <>
          <DialogTitle sx={{ textAlign: 'center', bgcolor: 'transparent' }}>
            {displayRoom.name}
          </DialogTitle>
          <DialogContent dividers sx={{ bgcolor: 'transparent' }}>
            <Stack spacing={2}>
              <Typography variant="body2" color="text.secondary">
                Room ID: {displayRoom.id}
              </Typography>

              {loading && (
                <Stack direction="row" alignItems="center" spacing={1}>
                  <CircularProgress size={18} />
                  <Typography variant="body2" color="text.secondary">
                    Refreshing metrics…
                  </Typography>
                </Stack>
              )}

              <Divider />

              <RoomMetricCharts room={displayRoom} />

              <Divider />
            </Stack>
          </DialogContent>
          <DialogActions
            sx={{ justifyContent: 'center', bgcolor: 'transparent' }}
          >
            <Button onClick={onClose} variant="contained">
              Close
            </Button>
          </DialogActions>
        </>
      )}
    </Dialog>
  );
}
