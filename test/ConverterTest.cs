using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using DataFormatConverter;
using Newtonsoft.Json;

namespace DataFormatConverter.Tests
{
    public class Trade
    {
        public string EXT_SEC_ID { get; set; }

        public string SEC_ID { get; set; }

        public string ORDTR { get; set; }

        public string REF_ID { get; set; }

    }

    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }

    public class DataFormatConverterTest
    {
        private readonly Converter _converter;
        private readonly string _xml_doc;
        private readonly string _csv_doc;

        public DataFormatConverterTest()
        {
            _converter = new Converter();
            _xml_doc = @"<Trade delimiter='~' header='true' date='2017-01-18 06:09:03' fileName='CRD.CS_SMG_DW_APAC.Trade.0000389020' sequence='389020' OUTPUT_LIMIT='500' TRADED_SEC_EXPORT_ID='0' OUTPUT_TYPE='xml' part='13'>
  <row>
    <EXT_SEC_ID>11644324</EXT_SEC_ID>    <!--InstrumentId-->
    <SEC_ID>248357100</SEC_ID>
    <ORDTR>SELLL</ORDTR> <!---TradeAction: BUYL=B, SELLL=S, BUYS=BC, SELLS=SS-->
    <REF_ID>1634086</REF_ID>
  </row>
  <row>
    <EXT_SEC_ID>11644324</EXT_SEC_ID>
    <SEC_ID>248357100</SEC_ID>
    <ORDTR>SELLL</ORDTR>
    <REF_ID>1637606</REF_ID>
  </row>
</Trade>";
            _csv_doc = @"AccountId|InstrumentId|TradeNumber|TradeVersion|TradeAction|CounterpartyId|StrategyId|CurrencyIdLocal|CurrencyIdSettle|FxRate|Quantity|PriceLocal|TradeDate|SettleDate|AllocationNumber|PricingFactor|TradingFactor|AccruedInterestLocal|PrincipalLocal|CommissionLocal|FeesLocal|ChargesLocal|LeviesLocal|SecFeesLocal|NetProceedsLocal|PriceSettlement|AccruedInterestSettlement|PrincipalSettlement|CommissionSettlement|FeesSettlement|ChargesSettlement|LeviesSettlement|SecFeesSettlement|NetProceedsSettlement|Comments|Yield|ClearingBrokerId|ClearingLocation|ProductDescription|DeliveryInstruction|SettlementInstruction|ExecutingUser|TransactionId|OrderId|AuditKey|TimeId|AsOfDateTime|Taxes|OtherFees|VAT|CorrectionFlag|CancelFlag|OASYSFlag|ExchangeId|Custodian|SecurityId|TraderId|ManagerId|IsProcessed|TradeId|CounterpartyOfficeId|ReasonCodeId|CommissionBasis|CancelReasonCode|TradeType|TargetQuantity|OriginalFace|Factor|LookupOrderId|DataProviderKey2|SwapBasketId|SecType|FxType|FromCurrency|ToCurrency|ExecutionAmount|NetTradeFlag|CommissionAmount|USDFxRate|NetCounterAmount|GenevaId|FixedCurrencyInd|GenericInv|TargetAmount|SwapInstrumentId|AccountStrategy|AllocationReason|BrokerReason|IsBondFuture|DataProviderKey3|DataProviderKey4|GSTLocal|NFALocal|GSTSettlement|NFASettlement|ExecutionDateTime|AccountFwdHedgeClass|TradeFwdHedgeClass|NDFFlag|FixingDate|BMSTradeID|BMSAssetId|GlobalFacilityAmount|ParNearPar|TradesFlat|DelayedCompIndex|DelayedCompRate|DelayedCompAccrualDaysPerYear|FormOfPurchase|PurchaseType|MarketTradeType|FacilityTradeDocType|SaleClass|FeeCode1|FeeCode1Value|FeeCode2|FeeCode2Value|QtyRecalculateFlag|TradeAccruedInterestType|Markitwire_ID|AllocationId|ProtectionSide|InitialMarginAmount|InitialMarginPct|InitialMarginCurrency|ConfirmationPlatform|CalculationAgent|MasterDocumentDate|RemainingParty|Amortization|TradeKeyValues|AllocationCount|FeeCode3|FeeCode3Value|FeeCode4|FeeCode4Value|FeeCode5|FeeCode5Value|FeeCode6|FeeCode6Value|ClearingBroker|ClearingHouse|TradeRefId|SpecialInstructions|InitialMargin|InitialMarginType|InitialMarginSwapCurrency|RestructuringType|IsSynthetic|FileName|CDSTradeType|MasterDocumentTransType|ActualSettleDate|Transferor|Transferee|MessageId|SMGOrderId|SMGTradeId|NetSettlementAmount|DMAIndicator|BreakClauseDate
QKFX4HK|11637333|18890072|1|B|JPMA||AUD|USD|.7499000000|733293.0000000000|4.356893720000000|20161215|20161219|0|.0000000000|.0000000000|.0000000000|3194879.6666199600|1.0000063630|.0000000000|.0000000000|.0000000000|.0000000000|.0000000000|3.267234600628000|.0000000000|2395840.2619983080|.7499047716|.0000000000|.0000000000|.0000000000|.0000000000|.0000000000||.0000000000||||||User1|18890072|243712458|481354c5-d183-4a9e-846e-13b21f932346|20161215|2016-12-15 06:31:25|0|0|0|N|N|N|ASX|||crilhac1||N|2486057|JPMA|0|BP||A|733293.0000000000|.0000000000|1.0000000000|0|SCG.AX||REIT|ALL|||3194879.6700000000|A|319.4900000000|.7406000000|3195199.1600000000|||0|3270486.7800000000|0|LPXD PARIS|DMA||N|||.0000000000|.0000000000|.0000000000|.0000000000|2016-12-15 16:10:00||||1900-01-01 00:00:00|||||||||||||||.0000000000||.0000000000|N||||||||||||||||.0000000000||.0000000000||.0000000000||.0000000000|||2433283|||||||CRD.CS_SMG_DW_APAC.Trade.0000359644||||||212854|SMG_243712458|SMG_18890072|2396079.8500|SMG_DMA|
QKFX4HK|11637319|18890073|1|B|JPMA||AUD|USD|.7499000000|54454.0000000000|2.162433610000000|20161215|20161219|0|.0000000000|.0000000000|.0000000000|117753.1597989400|1.0003977810|.0000000000|.0000000000|.0000000000|.0000000000|.0000000000|1.621608964139000|.0000000000|88303.0945332251|.7501982960|.0000000000|.0000000000|.0000000000|.0000000000|.0000000000||.0000000000||||||User1|18890073|243712460|3b577425-ccdc-4763-b9cf-abbe8d183ef0|20161215|2016-12-15 06:31:25|0|0|0|N|N|N|ASX|||crilhac1||N|2486058|JPMA|0|BP||A|54454.0000000000|.0000000000|1.0000000000|0|SCP.AX||REIT|ALL|||117753.1600000000|A|11.7800000000|.7406000000|117764.9400000000|||0|119254.2600000000|0|LPXD PARIS|DMA||N|||.0000000000|.0000000000|.0000000000|.0000000000|2016-12-15 14:46:00||||1900-01-01 00:00:00|||||||||||||||.0000000000||.0000000000|N||||||||||||||||.0000000000||.0000000000||.0000000000||.0000000000|||2433284|||||||CRD.CS_SMG_DW_APAC.Trade.0000359644||||||212854|SMG_243712460|SMG_18890073|88311.9300|SMG_DMA|
";

        }

        [Fact]
        public void XML_to_CSV_Test()
        {
            var result = _converter.XML_to_CSV(_xml_doc);

            // multiline string below
            var expected =
@"11644324,248357100,SELLL,1634086
11644324,248357100,SELLL,1637606
";
            Assert.Equal(result, expected);
        }

        [Fact]
        public void EmptyString_to_CSV_Test()
        {
            var result = _converter.XML_to_CSV("");
            var expected = "";
            Assert.Equal(result, expected);
        }

        [Fact]
        public void CSV_to_Order()
        {
            var results = _converter.CSV_to_Order(_csv_doc);
            var expected = new TOrder[] { new TOrder() {
                                            AccountId = "QKFX4HK",
                                            InstrumentId = 11637333,
                                            TNumber = 18890072,
                                            TVersion = 1,
                                            TAction = "B",
                                            CorrectFlag = "N",
                                            CancelFlag = "N",
                                            NDDFlag = String.Empty},
                                          new TOrder() {
                                            AccountId = "QKFX4HK",
                                            InstrumentId = 11637319,
                                            TNumber = 18890073,
                                            TVersion = 1,
                                            TAction = "B",
                                            CorrectFlag = "N",
                                            CancelFlag = "N",
                                            NDDFlag = String.Empty}
            };

            var i = 0;

            foreach (TOrder result in results)
            {
                Assert.Equal(expected[i].AccountId, result.AccountId);
                Assert.Equal(expected[i].InstrumentId, result.InstrumentId);
                Assert.Equal(expected[i].TNumber, result.TNumber);
                Assert.Equal(expected[i].TVersion, result.TVersion);
                Assert.Equal(expected[i].TAction, result.TAction);
                Assert.Equal(expected[i].CorrectFlag, result.CorrectFlag);
                Assert.Equal(expected[i].CancelFlag, result.CancelFlag);
                Assert.Equal(expected[i].NDDFlag, result.NDDFlag);
                i++;
            }
        }


        [Fact]
        public void DeserializeJSON_Test()
        {
            var expected = new List<String>();

            expected.Add(@"{""EXT_SEC_ID"":11644324,""SEC_ID"":248357100,""ORDTR"":""SELLL"",""REF_ID"":1634086}");
			expected.Add(@"{""EXT_SEC_ID"":11644324,""SEC_ID"":248357100,""ORDTR"":""SELLL"",""REF_ID"":1637606}");

			var tradelist = _converter.XML_to_TradeJson(_xml_doc);
            var i = 0;
            foreach (var trade in tradelist) {
                var tradeObj = JsonConvert.SerializeObject(trade);
                Console.WriteLine($"JSON MSG: {tradeObj}");
                Assert.Equal(expected[i], tradeObj);
                i++;
            }
        }

        [Fact]
        public void XMLFile_to_JSON()
        {
            String xml_doc = File.ReadAllText("../../../tradexml.xml");

            var tradelist = _converter.XML_to_TradeJson(xml_doc);

            foreach (var trade in tradelist)
            {
                var tradeObj = JsonConvert.SerializeObject(trade);
                Console.WriteLine($"JSON MSG: {tradeObj}");
            }
        }

    }
}
