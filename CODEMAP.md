# LunchSplit Assignment 3 Code Map

- **SolutionFile**: `LunchSplit.slnx`
- **CoreSourceRoot**: `LunchSplit`
- **PaymentNamespace**: `LunchSplit.Payments`
- **InfraSourceRoot**: `LunchSplit` (`SquarePaymentService.cs`)
- **TestProjectRoot**: `LunchSplitTests`
- **Tier1TestClass**: `LunchSplitTests/SquareSandboxTests.cs`
- **Tier2TestClass**: `LunchSplitTests/PaymentProcessorMoqTests.cs`

## Sandbox configuration

Set these environment variables before running the Square Sandbox tests:

```bash
export SQUARE_SANDBOX_ACCESS_TOKEN="your-sandbox-token"
export SQUARE_SANDBOX_LOCATION_ID="your-sandbox-location-id"
```

The tests never store credentials in the repository. Without these variables,
the five live Sandbox tests are reported as skipped so local unit tests remain
deterministic.
