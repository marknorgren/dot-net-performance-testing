import http from 'k6/http';
import { sleep } from 'k6';

export default function () {
    const now = new Date();
  const res = http.get(`http://localhost:8181/parse?dateInput=${now.toISOString()}`);
  sleep(1);
}
