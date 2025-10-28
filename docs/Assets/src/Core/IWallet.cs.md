# Assets/src/Core/IWallet.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `IWallet` interface for managing currency transactions.

## Public API
- Namespace: `CHAL.Systems.Economy`
- Types
  - `public interface IWallet`
    - Public methods:
      - `int GetCurrency(string currencyId);`
      - `bool CanSpend(string currencyId, int amount);`
      - `bool SpendCurrency(string currencyId, int amount);`
      - `void Refund(string currencyId, int amount);`

## Key Behavior & Side Effects
- `GetCurrency`: Retrieves the amount of specified currency.
- `CanSpend`: Checks if the specified amount of currency can be spent.
- `SpendCurrency`: Deducts the specified amount of currency if possible.
- `Refund`: Adds the specified amount of currency back to the wallet.

## Constraints & Failure Modes
- Methods may need to handle cases where `currencyId` is invalid or the wallet has insufficient funds.
```
