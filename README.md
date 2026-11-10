# LunchSplit

LunchSplit is a small C# console program that splits a restaurant bill.

The solution has two projects:

- `LunchSplit` is the program.
- `LunchSplitTests` holds the MSTest tests.

This project targets .NET 8.

## Build

```
dotnet build
```

## Run

```
dotnet run --project LunchSplit
```

That prints one sample receipt for a $100.00 bill with $13.00 tax and a 15 percent tip.

## Test

```
dotnet test
```

Assignment 3 adds a payment boundary with a Square Sandbox adapter and Moq
unit tests. The five Sandbox tests use these environment variables:

```bash
export SQUARE_SANDBOX_ACCESS_TOKEN="your-sandbox-token"
export SQUARE_SANDBOX_LOCATION_ID="your-sandbox-location-id"
```

The Sandbox tests are skipped when credentials are not configured. The Moq
tests run without network access.
