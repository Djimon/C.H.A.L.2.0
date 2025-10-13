namespace CHAL.Systems.Economy
{

    public interface IWallet
    {
        int GetCurrency(string currencyId);

        bool CanSpend(string currencyId, int amount);

        bool SpendCurrency(string currencyId, int amount);

        void Refund(string currencyId, int amount);
    }
}