# how-to
This repository's purpose is to document common patterns and practices I have developed over the years to avoid having to always go to stackoverflow to find the answer again.

# Projects

# Tips, Tracks, and Sundries

### Docker
- To build a container: `docker build -t some-tag .`
- To run a container: `docker run -d -p 8090:80 some-tag`
- To kill a container: `docker kill abc123` where abc123 is the first 4-6 character of the id returned on run
- Building, running, and killing Docker containers via Powershell is recommend
- To attach to a container, I recommend using VS Code and the Docker extension
- You can inject environment variables into docker in the following way ...

### Resources
- [Online Guid / UUID Generator](https://guidgenerator.com/online-guid-generator.aspx)
- [HTTP Status Codes](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status)
- [ASP.NET Core Tag Helpers](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/built-in/?view=aspnetcore-7.0)
- [Docker Compose File Build](https://docs.docker.com/compose/compose-file/build/)