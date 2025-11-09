# Assets/src/Core/IWallet.cs

_Automatically generated/updated from `Assets/src/Core/IWallet.cs`._

# Purpose
- Defines the `IWallet` interface for managing currency transactions.

# Public API
- Namespace: `CHAL.Core`
- Types
  - public interface `IWallet`
    - Public methods:
      - `int GetCurrency(string currencyId);` - Retrieves the amount of specified currency.
      - `bool CanSpend(string currencyId, int amount);` - Checks if the specified amount of currency can be spent.
      - `bool SpendCurrency(string currencyId, int amount);` - Deducts the specified amount of currency if possible.
      - `void Refund(string currencyId, int amount);` - Adds the specified amount of currency back to the wallet.

# Key Behavior & Side Effects
- `GetCurrency` returns the current balance of the specified currency.
- `CanSpend` checks availability before attempting to spend currency.
- `SpendCurrency` modifies the wallet's state by deducting currency.
- `Refund` modifies the wallet's state by adding currency.

# Constraints & Failure Modes
- Assumes valid `currencyId` and non-negative `amount` for all methods.
- Behavior on invalid `currencyId` or insufficient funds is not defined in this interface.

# Example
```csharp
public class Wallet : IWallet
{
    private Dictionary<string, int> currencies = new Dictionary<string, int>();

    public int GetCurrency(string currencyId) => currencies.TryGetValue(currencyId, out var amount) ? amount : 0;

    public bool CanSpend(string currencyId, int amount) => GetCurrency(currencyId) >= amount;

    public bool SpendCurrency(string currencyId, int amount)
    {
        if (CanSpend(currencyId, amount))
        {
            currencies[currencyId] -= amount;
            return true;
        }
        return false;
    }

    public void Refund(string currencyId, int amount)
    {
        if (currencies.ContainsKey(currencyId))
        {
            currencies[currencyId] += amount;
        }
        else
        {
            currencies[currencyId] = amount;
        }
    }
}
```

# Unknowns
- Specific behavior on currencyId validity and error handling is not defined.
