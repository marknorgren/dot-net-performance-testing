# README


## Docker


### Single CPU

This locks docker to a single CPU. 

`docker run --cpuset-cpus=0 --cpu-quota=100000 --cpu-period=100000 --memory=1g -p 8181:80 todoapi`

### Multi CPU

This locks docker to 2 CPUs.

`docker run --cpuset-cpus=0,1 --cpu-quota=100000 --cpu-period=100000 --memory=1g -p 8181:80 todoapi`