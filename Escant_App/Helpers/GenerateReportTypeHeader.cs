using DataModel.Enums;
using Escant_App.Models;
using ManijodaServicios.Resources.Texts;
using Escant_App.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Helpers
{
    public static class GenerateReportTypeHeader
    {
        public static string GetReportHeader(PeriodType PeriodType, DateTime? fromDate, DateTime? toDate)
        {
            switch (PeriodType)
            {
                case PeriodType.Today:
                    return TextsTranslateManager.Translate("Today");
                case PeriodType.Yesterday:
                    return TextsTranslateManager.Translate("Yesterday");
                case PeriodType.LastWeek:
                    return TextsTranslateManager.Translate("Lastweek");
                case PeriodType.LastMonth:
                    return TextsTranslateManager.Translate("Lastmonth");
                case PeriodType.ThisMonth:
                    return TextsTranslateManager.Translate("Thismonth");
                case PeriodType.Option:
                    //return TextsTranslateManager.Translate("CustomPeriod");
                    return string.Format(TextsTranslateManager.Translate("CustomPeriod"), fromDate.Value.ToString(SystemConstants.DATE_FORMAT_DDMMYYYY), toDate.Value.ToString(SystemConstants.DATE_FORMAT_DDMMYYYY));
                default:
                    return TextsTranslateManager.Translate("SelectPeriod");
            }
        }

    }
}
