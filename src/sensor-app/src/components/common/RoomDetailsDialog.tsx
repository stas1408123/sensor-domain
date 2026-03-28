import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Divider,
  Stack,
  Typography,
} from '@mui/material';
import type { RoomViewModel } from '../../types/graphql';
import { RoomMetricCharts } from './RoomMetricCharts';

interface RoomDetailsDialogProps {
  room: RoomViewModel | null;
  onClose: () => void;
}

export function RoomDetailsDialog({ room, onClose }: RoomDetailsDialogProps) {
  const open = Boolean(room);

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
      {room && (
        <>
          <DialogTitle sx={{ textAlign: 'center', bgcolor: 'transparent' }}>
            {room.name}
          </DialogTitle>
          <DialogContent dividers sx={{ bgcolor: 'transparent' }}>
            <Stack spacing={2}>
              <Typography variant="body2" color="text.secondary">
                Room ID: {room.id}
              </Typography>

              <Divider />

              <RoomMetricCharts room={room} />

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
