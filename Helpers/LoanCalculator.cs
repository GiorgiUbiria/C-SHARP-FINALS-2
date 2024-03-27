using Finals.Enums;

namespace Finals.Helpers;

public class LoanCalculator
{
    public static int CalculateFinalAmount(int money, LoanPeriod period)
    {
        int finalAmount = money;
        switch (period)
        {
            case LoanPeriod.HalfYear:
                finalAmount += (int)((double)money * 0.05);
                break;
            case LoanPeriod.OneYear:
                finalAmount += (int)((double)money * 0.10);
                break;
            case LoanPeriod.TwoYears:
                finalAmount += (int)((double)money * 0.15);
                break;
            case LoanPeriod.FiveYears:
                finalAmount += (int)((double)money * 0.20);
                break;
            case LoanPeriod.TenYears:
                finalAmount += (int)((double)money * 0.30);
                break;
            default:
                break;
        }
        return finalAmount;
    }
}