# How to mock User object?

- Class User has protected setter `Id`.

- Class User has no public constructor (only static `Create` method).

Question: How to mock User objects so that `SomeService.HaveSameId` method passes?
