# global.IWallet

_Automatically generated/updated from `Assets/src/Core/IWallet.cs`._

# Purpose
- Defines the `IWallet` interface for managing currency transactions.

# Public API
- Namespace: `CHAL.Systems.Economy`
- Types
  - public interface `IWallet`
    - Public methods:
      - `int GetCurrency(string currencyId);` 
      - `bool CanSpend(string currencyId, int amount);`
      - `bool SpendCurrency(string currencyId, int amount);`
      - `void Refund(string currencyId, int amount);`

# Key Behavior & Side Effects
- `GetCurrency`: Retrieves the amount of specified currency.
- `CanSpend`: Checks if the specified amount of currency can be spent.
- `SpendCurrency`: Deducts the specified amount of currency if possible.
- `Refund`: Adds the specified amount of currency back to the wallet.

# Constraints & Failure Modes
- Assumes valid `currencyId` and non-negative `amount` for methods.
- Behavior on invalid inputs is not defined in this interface.

# Example
```csharp
public class Wallet : IWallet
{
    // Implementation of IWallet methods
}
```

# Unknowns
- Specific implementation details and state management of the wallet are not provided.

