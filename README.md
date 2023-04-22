# how-to
A wide ranging project showing how to setup different technologies

# Tips, Tracks, and Sundries

### Docker
- To build a container: `docker build -t some-tag .`
- To run a container: `docker run -d -p 8090:80 some-tag`
- To kill a container: `docker kill abc123` where abc123 is the first 4-6 character of the id returned on run
- Building, running, and killing Docker containers via Powershell is recommend
- To attach to a container, I recommend using VS Code and the Docker extension
- You can inject environment variables into docker in the following way ...