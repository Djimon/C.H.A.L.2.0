# global.IWallet

_Automatically generated/updated from `Assets/src/Core/IWallet.cs`._

1) Purpose
- Defines public interface IWallet in the CHAL.Core namespace
- Declares currency-related operations (query, spend check, spend, refund)
- Provides no implementation; acts as a contract for wallet-like components

2) Public API
- Namespace/module: CHAL.Core
- Types
  - public interface IWallet
    - int GetCurrency(string currencyId)
    - bool CanSpend(string currencyId, int amount)
    - bool SpendCurrency(string currencyId, int amount)
    - void Refund(string currencyId, int amount)

3) Key Behavior & Side Effects
- None specified (no implementation or behavior defined in this file)

4) Constraints & Failure Modes
- None specified (no guards, validation, or threading/asynchrony details provided)

5) Example
- (none derivable from this file)

6) Unknowns
- Exact semantics of currencyId (valid values, case sensitivity, existence checks)
- Mapping between currencies and their balances
- How GetCurrency determines the returned amount
- Behavior when currencies are invalid or insufficient funds
- Thread-safety, synchronization, and potential side effects on calls
- Error handling and exception semantics for invalid inputs or failed operations
- Any persistence or lifecycle considerations for implementations
