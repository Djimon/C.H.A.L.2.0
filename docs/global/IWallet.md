# global.IWallet

_Automatically generated/updated from `Assets/src/Core/IWallet.cs`._

1) Purpose
- Defines public interface IWallet in CHAL.Core.
- Declares currency-related operations: get balance, check spend feasibility, spend, and refund.
- Provides no implementation; serves as a contract.

2) Public API
- Namespace/module: CHAL.Core
- Types
  - public interface IWallet
    - int GetCurrency(string currencyId)
    - bool CanSpend(string currencyId, int amount)
    - bool SpendCurrency(string currencyId, int amount)
    - void Refund(string currencyId, int amount)

3) Key Behavior & Side Effects
- No implementations in this file; behavior defined by implementers.
- GetCurrency(string) returns an int (no semantics defined here).
- CanSpend(string, int) indicates feasibility (true/false).
- SpendCurrency(string, int) indicates success/failure via return value.
- Refund(string, int) returns void (no side effects defined here).

4) Constraints & Failure Modes
- No constraints or guards defined in this file.
- Null/empty handling, threading, and async behavior are not specified.
- No performance/allocation hints.

5) Example
```csharp
using CHAL.Core;

public class DemoWallet : IWallet
{
    public int GetCurrency(string currencyId) => 0;
    public bool CanSpend(string currencyId, int amount) => false;
    public bool SpendCurrency(string currencyId, int amount) => false;
    public void Refund(string currencyId, int amount) { }
}
```

6) Unknowns
- Semantics of currencyId and currency units.
- Exact thread-safety guarantees.
- Persistence, synchronization, and error details for failures.
- Any additional constraints imposed by implementers.
