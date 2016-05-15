
RESOURCES FILES:
- Files ( text files, etc ) should be placed in the _Resources directory in a respective folder
- Files should be set to "embedded resource"
- Files can be then accessed using the ResourceLoader ( see example )


FOLDER STRUCTURE:
- _Resouces  : folder to contain files needed for unit/bdd tests
- _Common    : code that is common just to unit-tests
- Unit       : unit tests ( lowest level tests )
- Component  : component level tests ( a single-modular component that does not depend on other components )
- Integration: tests that user multiple components 


MOCK OBJECTS
- Moq framework is currently used.
- For quick access to Fake services ( e.g. AccountService ), you may want to create your own service builder that returns a mock
object. ( see example )
