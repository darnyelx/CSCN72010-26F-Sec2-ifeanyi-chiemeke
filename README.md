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

Two tests are supposed to fail until a later assignment:

- `Format_CsvReceiptExporter_MatchesSchema`
- `CalculateShares_PaymentRequest_ValidatesStripeMock`

The other tests should pass.
