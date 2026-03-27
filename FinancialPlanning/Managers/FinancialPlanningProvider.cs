using FinancialPlanning.Contracts;
using Newtonsoft.Json;

namespace FinancialPlanning.Managers
{
    public class FinancialPlanningProvider : IFinancialPlanningProvider
    {
        public IEnumerable<Payment> BuildAmoritizationSchedule(Liability liability)
        {
            decimal principalAmount = liability.Principal;
            var payments = new List<Payment>();
            while (principalAmount > 0)
            {
                var payment = new Payment();
                decimal interest = 0;
                switch (liability.FrequencyPaymentType)
                {
                    case Contracts.Enums.FrequencyPaymentType.Monthly:
                        interest = liability.InterestRate / 100 / 365 * 30;
                        break;
                }
                
                payment.Interest = interest * principalAmount;
                if (principalAmount + payment.Interest + liability.EscrowMonthlyAmount >= liability.FrequencyPayment)
                {
                    payment.Amount = liability.FrequencyPayment;
                }
                else
                {
                    payment.Amount = principalAmount + payment.Interest + liability.EscrowMonthlyAmount;
                }

                payment.Principal = payment.Amount - payment.Interest - liability.EscrowMonthlyAmount;
                principalAmount -= payment.Principal;
                payment.RemainingPrincipal = principalAmount;
                
                payments.Add(payment);
            }

            return payments;
        }

        public IEnumerable<Asset> ProjectInvestmentGrowth(int years, IEnumerable<Asset> assets, decimal yearlyContribution)
        {
            var returnAssets = JsonConvert.DeserializeObject<IEnumerable<Asset>>(JsonConvert.SerializeObject(assets));
            foreach (var asset in returnAssets)
            {
                decimal contributionExtra = (decimal)(yearlyContribution / returnAssets.Count() * (decimal)Math.Pow(((1 + (double)asset.ExpectedReturnRate / 100 / 12) - 1) / (double)(asset.ExpectedReturnRate / 100 / 12), 12 * years));
                asset.Amount = asset.Amount * (decimal)Math.Pow((double)(1 + (asset.ExpectedReturnRate / 100 / 12)), 12 * years) + contributionExtra;
            }

            return returnAssets;
        }

        public PlanningResponse CalculateRetirementPlan(PlanningData data, IEnumerable<Asset> investments)
        {
            // start at 70 for maximum social security benefit
            // assume age 100 for death
            int retirementAge = data.AgeYears > 70 ? data.AgeYears : 70;

            // calculate end life asset amount with no delay in social security and full delay
            PlanningResponse lastSuccessfulResponse = new PlanningResponse();
            bool shouldContinueCalculating = true;
            while (shouldContinueCalculating)
            {
                // calculate final investment value
                IEnumerable<Asset> retirementAssets = this.ProjectInvestmentGrowth(retirementAge - data.AgeYears, investments, data.YearlyContribution);
                decimal retirementAmount = retirementAssets.Sum(x => x.Amount);
                PlanningResponse delayedSocialSecurityResult = this.GetPlanningResponse(retirementAge, retirementAmount, data, data.RetirementRoi, true);
                if (delayedSocialSecurityResult.EndingTotalRetirement > 0)
                {
                    shouldContinueCalculating = true;
                    lastSuccessfulResponse = delayedSocialSecurityResult;
                    PlanningResponse undelayedSocialSecurityResult = this.GetPlanningResponse(retirementAge, retirementAmount, data, data.RetirementRoi, true);
                    if (undelayedSocialSecurityResult.EndingTotalRetirement > 0)
                    {
                        lastSuccessfulResponse = undelayedSocialSecurityResult;
                    }
                    else
                    {
                        shouldContinueCalculating = false;
                    }

                    retirementAge -= 1;
                }
                else
                {
                    shouldContinueCalculating = false;
                }
            }

            return lastSuccessfulResponse;
        }

        public IEnumerable<LoanToPay> CalculateWhichLoansToPayForInterestSaving(decimal extraAmount, IEnumerable<Liability> liabilities)
        {
            liabilities = JsonConvert.DeserializeObject<IEnumerable<Liability>>(JsonConvert.SerializeObject(liabilities));
            var loansToPay = new List<LoanToPay>();
            while (extraAmount > 0 && liabilities.Any(x => x.Principal > 0))
            {
                var loanToPay = liabilities.Where(x => x.Principal > 0).OrderByDescending(x => x.InterestRate).FirstOrDefault();
                IEnumerable<Payment> payments = this.BuildAmoritizationSchedule(loanToPay);
                decimal principalToPay = extraAmount <= loanToPay.Principal ? extraAmount : loanToPay.Principal;
                loanToPay.Principal -= principalToPay;
                IEnumerable<Payment> newPayments = this.BuildAmoritizationSchedule(loanToPay);
                loansToPay.Add(new LoanToPay
                {
                    ExtraPrincipal = principalToPay,
                    InterestSaved = payments.Sum(x => x.Interest) - newPayments.Sum(x => x.Interest),
                    Name = loanToPay.Name,
                    PaymentsSaved = payments.Count() - newPayments.Count()
                });

                extraAmount -= principalToPay;
            }

            return loansToPay;
        }

        private PlanningResponse GetPlanningResponse(int retirementAge, decimal retirementAmount, PlanningData planningData, decimal averageROI, bool delaySocialSecurity)
        {
            var planningResponse = new PlanningResponse { StartingTotalRetirement = retirementAmount, RetirementAge = retirementAge };

            var retirementYearData = new List<RetirementYearBreakdown>();
            decimal endingRetirementAmount = retirementAmount;
            var currentRetirementAge = retirementAge;
            const int deathAge = 95;
            while (retirementAge < deathAge)
            {
                var retirementYearBreakdown = new RetirementYearBreakdown { StartingInvestmentAmount = endingRetirementAmount, RetirementAgeYear = retirementAge };
                decimal investmentReturn = endingRetirementAmount * averageROI / 100;
                decimal socialSecurity = 0;
                if (!delaySocialSecurity && retirementAge >= 67)
                {
                    socialSecurity = planningData.ExpectedSocialSecurityYearly;
                }
                else if (delaySocialSecurity && retirementAge >= 70)
                {
                    socialSecurity = planningData.ExpectedSocialSecurityYearly * 1.24M;
                }

                retirementYearBreakdown.SocialSecurityPayment = socialSecurity;
                decimal requiredYearlyIncome = planningData.CurrentYearlySalary * .8M;
                if ((investmentReturn + socialSecurity) >= requiredYearlyIncome)
                {
                    retirementYearBreakdown.InvestmentReturnAmount = investmentReturn;
                    retirementYearBreakdown.InvestmentWithdrawlAmount = 0;
                    decimal returnSurplus = investmentReturn + socialSecurity - requiredYearlyIncome;
                    retirementYearBreakdown.EndingInvestmentAmount = endingRetirementAmount + returnSurplus;
                    endingRetirementAmount += returnSurplus;
                }
                else
                {
                    retirementYearBreakdown.InvestmentReturnAmount = investmentReturn;
                    decimal investmentWithdrawl = requiredYearlyIncome - socialSecurity - investmentReturn;
                    retirementYearBreakdown.InvestmentWithdrawlAmount = investmentWithdrawl;
                    retirementYearBreakdown.EndingInvestmentAmount = endingRetirementAmount - investmentWithdrawl;
                    endingRetirementAmount -= investmentWithdrawl;
                }

                retirementYearData.Add(retirementYearBreakdown);
                retirementAge += 1;
            }

            planningResponse.EndingTotalRetirement = retirementYearData.Last().EndingInvestmentAmount;
            planningResponse.RetirementYearData = retirementYearData;
            planningResponse.DelaySocialSecurityAge = delaySocialSecurity;
            return planningResponse;
        }
    }
}
