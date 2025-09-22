# Exhibition-Curation-Platform

An application for art enthusiasts to search, explore, and curate their own digital art collections from a variety of external gallery APIs.

## Features 

- Multi-API Artwork Search: Search for artworks from multiple external art gallery and museum APIs.
- Filtering and Sorting: Refine search results by artist, date, medium, and more with added sorting functionality.
- Personal Collections: Save favorite artworks to personal collections for easy access.

## Technologies Used 

### Backend
- ASP.NET: Provides the core server-side logic and API framework.
- NUnit: Used for comprehensive unit testing of the backend logic.

### Frontend
- Blazor: A .NET web framework for building interactive web UI.
- MudBlazor: A Material Design component library for a clean and professional UI.
- Blazored-Local-Storage: Manages client-side data persistence for user collections.

## Getting Started

To run the application locally you must:

1. Clone the Repository: <br>
`git clone https://github.com/bart1012/Exhibition-Curation-Platform.git; cd Exhibition-Curation-Platform;`
2. Run the Projects: <br>
The solution includes two main projects: ECP.UI.Server (the Blazor front-end) and ECP.API (the back-end API). Currently the front-end is configured to use the Azure-hosted API service but if you prefer to run both projects locally, you can change the target URL in ECP.UI.Server/Services/ArtworkService.cs. When doing so, you will need to run both projects simultaneously.

    - Open the solution in Visual Studio.
    - Right-click the solution in the Solution Explorer and select Set Startup Projects....
    - Choose Multiple startup projects and set both ECP.UI.Server and ECP.API to Start.
    - Press F5 to run the application.

## Current Clients
The back-end API currently utilises the The Cleveland Museum of Art Open Access API and the The Art Institute of Chicago API. 

## Expansion

- User Authentication: Implement a robust registration and login system to allow for secure, persistent user collections on a backend database (e.g., Firestore).
- Expanded API Integrations: Add support for more art APIs by creating new client classes and updating the ArtworkMapper.cs.
- Advanced Collection Features: Introduce new functionality such as sharing collections, adding annotations, and creating public galleries.



