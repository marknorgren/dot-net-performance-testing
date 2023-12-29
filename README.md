# ToDoApi

## Performance Testing

### Setup

1. Start the docker container

Attempt to run at 10% of a single CPU core, with 512MB of memory, and expose port 8181 to the host machine.

`docker run --cpuset-cpus=0 --cpu-quota=10000 --cpu-period=100000 --memory=512m -p 8181:80 todoapi`

1. Start the k6 test

`k6 run --config performance/k6config.js test.js`

### Results

```
➜  performance git:(main) ✗ k6 run --config k6-config.json test.js

          /\      |‾‾| /‾‾/   /‾‾/   
     /\  /  \     |  |/  /   /  /    
    /  \/    \    |     (   /   ‾‾\  
   /          \   |  |\  \ |  (‾)  | 
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: test.js
     output: -

  scenarios: (100.00%) 1 scenario, 4000 max VUs, 1m10s max duration (incl. graceful stop):
           * default: Up to 4000 looping VUs for 40s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)


     data_received..................: 2.4 MB 46 kB/s
     data_sent......................: 1.2 MB 22 kB/s
     http_req_blocked...............: avg=271.79µs min=1µs      med=6µs   max=14.68ms p(90)=674µs  p(95)=737µs 
     http_req_connecting............: avg=232.77µs min=0s       med=0s    max=14.6ms  p(90)=581µs  p(95)=627µs 
     http_req_duration..............: avg=9.92s    min=999.9ms  med=6.76s max=22.21s  p(90)=19.31s p(95)=20.81s
       { expected_response:true }...: avg=9.92s    min=999.9ms  med=6.76s max=22.21s  p(90)=19.31s p(95)=20.81s
     http_req_failed................: 0.00%  ✓ 0          ✗ 9627  
     http_req_receiving.............: avg=52.29µs  min=6µs      med=27µs  max=99.46ms p(90)=61µs   p(95)=98µs  
     http_req_sending...............: avg=47.54µs  min=3µs      med=24µs  max=2.04ms  p(90)=87µs   p(95)=119µs 
     http_req_tls_handshaking.......: avg=0s       min=0s       med=0s    max=0s      p(90)=0s     p(95)=0s    
     http_req_waiting...............: avg=9.92s    min=999.79ms med=6.76s max=22.21s  p(90)=19.31s p(95)=20.81s
     http_reqs......................: 9627   185.024835/s
     iteration_duration.............: avg=10.91s   min=2s       med=7.76s max=23.2s   p(90)=20.3s  p(95)=21.8s 
     iterations.....................: 9627   185.024835/s
     vus............................: 1      min=1        max=4000
     vus_max........................: 4000   min=4000     max=4000


running (0m52.0s), 0000/4000 VUs, 9627 complete and 0 interrupted iterations
default ✓ [======================================] 0000/4000 VUs  40s```


```

Adding a `Thread.Sleep(1000)` in [Program.cs](./Program.cs).

```
➜  performance git:(main) ✗ k6 run --config k6-config.json test.js

          /\      |‾‾| /‾‾/   /‾‾/   
     /\  /  \     |  |/  /   /  /    
    /  \/    \    |     (   /   ‾‾\  
   /          \   |  |\  \ |  (‾)  | 
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: test.js
     output: -

  scenarios: (100.00%) 1 scenario, 4000 max VUs, 1m10s max duration (incl. graceful stop):
           * default: Up to 4000 looping VUs for 40s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)


     data_received..................: 2.4 MB 47 kB/s
     data_sent......................: 1.2 MB 23 kB/s
     http_req_blocked...............: avg=284.51µs min=0s  med=5µs   max=18.3ms  p(90)=700.7µs p(95)=760µs   
     http_req_connecting............: avg=246.52µs min=0s  med=0s    max=18.22ms p(90)=603µs   p(95)=650.34µs
     http_req_duration..............: avg=9.71s    min=1s  med=7.99s max=22.2s   p(90)=18.6s   p(95)=19.59s  
       { expected_response:true }...: avg=9.71s    min=1s  med=7.99s max=22.2s   p(90)=18.6s   p(95)=19.59s  
     http_req_failed................: 0.00%  ✓ 0          ✗ 9714  
     http_req_receiving.............: avg=523.46µs min=5µs med=26µs  max=2s      p(90)=62µs    p(95)=104.34µs
     http_req_sending...............: avg=40.8µs   min=2µs med=20µs  max=2.16ms  p(90)=85µs    p(95)=105µs   
     http_req_tls_handshaking.......: avg=0s       min=0s  med=0s    max=0s      p(90)=0s      p(95)=0s      
     http_req_waiting...............: avg=9.71s    min=1s  med=7.99s max=22.2s   p(90)=18.6s   p(95)=19.59s  
     http_reqs......................: 9714   189.354278/s
     iteration_duration.............: avg=10.71s   min=2s  med=8.99s max=23.2s   p(90)=19.6s   p(95)=20.6s   
     iterations.....................: 9714   189.354278/s
     vus............................: 500    min=4        max=4000
     vus_max........................: 4000   min=4000     max=4000


running (0m51.3s), 0000/4000 VUs, 9714 complete and 0 interrupted iterations
default ✓ [======================================] 0000/4000 VUs  40s

```