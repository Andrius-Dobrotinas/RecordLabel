# EntityPlus
An Entity Framework extension library that aims to provide tools for easier entity manipulation

So far, it comprises facilities for:
* retrieving entity data, such as primary keys, navigation properties and their relationship types, from DbContext.
* automatically updating all entities in the underlying context by simply passing in models with the desired values. All navigation property resolution and updating is done automatically without the need to specify udpate action for each navigation property.

TODOs:
* Write some unit tests (define a test Db context) possible using EF InMemory.
* Migrate to Entity Framework Core (7).
