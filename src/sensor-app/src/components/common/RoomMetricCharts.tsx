import { Box, Typography } from '@mui/material';
import {
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';
import type { RoomViewModel } from '../../types/graphql';

function formatChartTick(iso: string) {
  try {
    const d = new Date(iso);
    return `${d.getMonth() + 1}/${d.getDate()} ${d.getHours()}:${String(d.getMinutes()).padStart(2, '0')}`;
  } catch {
    return iso;
  }
}

function sortByTime<T extends { timestamp: string }>(items: T[]): T[] {
  return [...items].sort(
    (a, b) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime(),
  );
}

interface RoomMetricChartsProps {
  room: RoomViewModel;
}

export function RoomMetricCharts({ room }: RoomMetricChartsProps) {
  const airSorted = sortByTime(room.airQualities ?? []);
  const airData = airSorted.map((a) => ({
    time: formatChartTick(a.timestamp),
    ts: a.timestamp,
    co2: a.co2,
    pm25: a.pm25,
    humidity: a.humidity,
  }));

  const energySorted = sortByTime(room.energies ?? []);
  const energyData = energySorted.map((e) => ({
    time: formatChartTick(e.timestamp),
    ts: e.timestamp,
    kWh: e.consumptionEnergy,
  }));

  const motionSorted = sortByTime(room.motions ?? []);
  const motionData = motionSorted.map((m) => ({
    time: formatChartTick(m.timestamp),
    ts: m.timestamp,
    motion: m.motionDetected ? 1 : 0,
  }));

  const chartHeight = 220;

  return (
    <Box sx={{ mt: 1 }}>
      <Typography variant="subtitle1" fontWeight={600} gutterBottom>
        Air quality (over time)
      </Typography>
      {airData.length > 0 ? (
        <ResponsiveContainer width="100%" height={chartHeight}>
          <LineChart
            data={airData}
            margin={{ top: 8, right: 8, left: 0, bottom: 0 }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
              dataKey="time"
              tick={{ fontSize: 11 }}
              interval="preserveStartEnd"
            />
            <YAxis yAxisId="co2" tick={{ fontSize: 11 }} width={42} />
            <YAxis
              yAxisId="other"
              orientation="right"
              tick={{ fontSize: 11 }}
              width={36}
            />
            <Tooltip
              labelFormatter={(_, payload) =>
                payload?.[0]?.payload?.ts
                  ? new Date(payload[0].payload.ts).toLocaleString()
                  : ''
              }
            />
            <Legend />
            <Line
              yAxisId="co2"
              type="monotone"
              dataKey="co2"
              name="CO₂ (ppm)"
              stroke="#5c6bc0"
              dot={false}
              strokeWidth={2}
            />
            <Line
              yAxisId="other"
              type="monotone"
              dataKey="pm25"
              name="PM2.5"
              stroke="#26a69a"
              dot={false}
              strokeWidth={2}
            />
            <Line
              yAxisId="other"
              type="monotone"
              dataKey="humidity"
              name="Humidity %"
              stroke="#ffa726"
              dot={false}
              strokeWidth={2}
            />
          </LineChart>
        </ResponsiveContainer>
      ) : (
        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
          No air quality data for chart.
        </Typography>
      )}

      <Typography
        variant="subtitle1"
        fontWeight={600}
        gutterBottom
        sx={{ mt: 2 }}
      >
        Energy consumption (over time)
      </Typography>
      {energyData.length > 0 ? (
        <ResponsiveContainer width="100%" height={chartHeight}>
          <LineChart
            data={energyData}
            margin={{ top: 8, right: 8, left: 0, bottom: 0 }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
              dataKey="time"
              tick={{ fontSize: 11 }}
              interval="preserveStartEnd"
            />
            <YAxis tick={{ fontSize: 11 }} width={48} />
            <Tooltip
              labelFormatter={(_, payload) =>
                payload?.[0]?.payload?.ts
                  ? new Date(payload[0].payload.ts).toLocaleString()
                  : ''
              }
            />
            <Legend />
            <Line
              type="monotone"
              dataKey="kWh"
              name="kWh"
              stroke="#42a5f5"
              dot={false}
              strokeWidth={2}
            />
          </LineChart>
        </ResponsiveContainer>
      ) : (
        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
          No energy data for chart.
        </Typography>
      )}

      <Typography
        variant="subtitle1"
        fontWeight={600}
        gutterBottom
        sx={{ mt: 2 }}
      >
        Motion (0 = none, 1 = detected)
      </Typography>
      {motionData.length > 0 ? (
        <ResponsiveContainer width="100%" height={chartHeight}>
          <LineChart
            data={motionData}
            margin={{ top: 8, right: 8, left: 0, bottom: 0 }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
              dataKey="time"
              tick={{ fontSize: 11 }}
              interval="preserveStartEnd"
            />
            <YAxis
              domain={[-0.1, 1.1]}
              ticks={[0, 1]}
              tick={{ fontSize: 11 }}
              width={32}
            />
            <Tooltip
              labelFormatter={(_, payload) =>
                payload?.[0]?.payload?.ts
                  ? new Date(payload[0].payload.ts).toLocaleString()
                  : ''
              }
              formatter={(value) => {
                const n = value == null ? NaN : Number(value);
                return [n === 1 ? 'Detected' : 'None', 'Motion'];
              }}
            />
            <Legend />
            <Line
              type="stepAfter"
              dataKey="motion"
              name="Motion"
              stroke="#ab47bc"
              dot
              strokeWidth={2}
            />
          </LineChart>
        </ResponsiveContainer>
      ) : (
        <Typography variant="body2" color="text.secondary">
          No motion data for chart.
        </Typography>
      )}
    </Box>
  );
}
