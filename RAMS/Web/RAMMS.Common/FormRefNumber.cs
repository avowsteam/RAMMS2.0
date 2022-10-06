﻿using System;
using System.Collections.Generic;
using System.Text;
namespace RAMMS.Common.RefNumber
{
    public class FormRefNumber
    {
        public const string NewRunningNumber = "????";
        public const string FormS1Header = "MM/Form S1/{RMU}/{Date}/{WeekNo}/{" + NewRunningNumber + "}";
        public const string FormS1Detail = "MM/Form S1/{RMU}/{Date}/{WeekNo}/{S1PKID}/{" + NewRunningNumber + "}";
        public const string FormS2Header = "MM/Form S2/{RMU}/{Quarter}/{Year}/{ActCode}/{" + NewRunningNumber + "}";
        public const string FormDHeader = "ERT/FORM D/{WeekNo}- {MonthNo}-{Year}/{CrewUnit}/{" + NewRunningNumber + "}";
        public const string FormC1C2 = "CI/Form C1/C2/{AssetID}/{Year}";
        public const string FormG1G2 = "CI/Form G1/G2/{AssetID}/{Year}";
        public const string FormR1R2 = "CI/Form R1/R2/{AssetID}/{Year}";
        public const string FormF4Header = "CI/Form F4/{RoadCode}/{Year}";
        public const string FormF5Header = "CI/Form F5/{RoadCode}/{Year}";
        public const string FormFCHeader = "CI/Form FC/{RoadCode}/{Year}";
        public const string FormB1B2 = "CI/Form B1/B2/{AssetID}/{Year}";
        public const string FormFDHeader = "CI/Form FD/{RoadCode}/{Year}";
        public const string FormV1Header = "{RMU}/V1/{Crew}/{ActivityCode}/{YYYYMMDD}";
        public const string FormV2Header = "{RMU}/V2/{CrewUnit}/{ActCode}/{Year}{MonthNo}{Day}/{" + NewRunningNumber + "}";
        public const string FormV3Header = "{RMU}/V3/{Crew}/{ActivityCode}/{YYYYMMDD}/{" + NewRunningNumber + "}";
        public const string FormV4Header = "{RMU}/V4/{Crew}/{ActivityCode}/{YYYYMMDD}";
        public const string FormV5Header = "{RMU}/V5/{Crew}/{ActivityCode}/{YYYYMMDD}";

        public const string FormQA1Header = "{RMU}/QA1/{CrewUnit}/{ActCode}/{Year}{MonthNo}{Day}/{" + NewRunningNumber + "}";
        public const string FormF3Header = "CI/Form F3/{RoadCode}/{Year}";
        public const string FormF1Header = "CI/Form F1/{RoadCode}/{Year}";
        public const string FormTHeader = "CI/Form T/{RoadCode}/{YYYYMMDD}";
        public const string FormMHeader = "CI/Form M/{RoadCode}/{ActivityCode}/{Year}{MonthNo}{Day}/{" + NewRunningNumber + "}";
        public const string FormB13Header = "AWPB/Form B13/{RMU}/{YYYY}/{RevisionNo}";
        public const string FormB15Header = "AWPB/Form B15/{RMU}/{YYYY}/{RevisionNo}";
        public const string FormB14Header = "AWPB/Form B14/{RMU}/{YYYY}/{RevisionNo}";
        public const string FormT3Header = "AWPB/Form T3/{RMU}/{YYYY}/{RevisionNo}";
        public const string FormB12Header = "AWPB/Form B12/{YYYY}/{RevisionNo}";

        public static string GetRefNumber(FormType type, IDictionary<string, string> values)
        {
            string format = GetFormat(type);
            foreach (var item in values)
            {
                format = format.Replace("{" + item.Key + "}", item.Value);
            }
            return format;
        }
        public static string GetRawRefNumber(FormType type)
        {
            string format = GetFormat(type);
            return format.Replace("{" + NewRunningNumber + "}", NewRunningNumber);
        }
        private static string GetFormat(FormType type)
        {
            string format = string.Empty;
            switch (type)
            {
                case FormType.FormS1Header:
                    format = FormS1Header;
                    break;
                case FormType.FormS1Details:
                    format = FormS1Detail;
                    break;
                case FormType.FormS2Header:
                    format = FormS2Header;
                    break;
                case FormType.FormDHeader:
                    format = FormDHeader;
                    break;
                case FormType.FormC1C2:
                    format = FormC1C2;
                    break;
                case FormType.FormG1G2:
                    format = FormG1G2;
                    break;
                case FormType.FormR1R2:
                    format = FormR1R2;
                    break;
                case FormType.FormF4Header:
                    format = FormF4Header;
                    break;
                case FormType.FormF5Header:
                    format = FormF5Header;
                    break;
                case FormType.FormFCHeader:
                    format = FormFCHeader;
                    break;
                case FormType.FormB1B2:
                    format = FormB1B2;
                    break;
                case FormType.FormFDHeader:
                    format = FormFDHeader;
                    break;
                case FormType.FormV1Header:
                    format = FormV1Header;
                    break;
                case FormType.FormV2Header:
                    format = FormV2Header;
                    break;



                case FormType.FormQA1Header:
                    format = FormQA1Header;
                    break;
                case FormType.FormV3Header:
                    format = FormV3Header;
                    break;
                case FormType.FormV4Header:
                    format = FormV4Header;
                    break;
                case FormType.FormV5Header:
                    format = FormV5Header;
                    break;
                case FormType.FormF3Header:
                    format = FormF3Header;
                    break;
                case FormType.FormF1Header:
                    format = FormF1Header;
                    break;
                case FormType.FormTHeader:
                    format = FormTHeader;
                    break;
                case FormType.FormM:
                    format = FormMHeader;
                    break;
                case FormType.FormB13:
                    format = FormB13Header;
                    break;
                case FormType.FormB15:
                    format = FormB15Header;
                    break;
                case FormType.FormB14:
                    format = FormB14Header;
                    break;
                case FormType.FormT3:
                    format = FormT3Header;
                    break;
                case FormType.FormB12:
                    format = FormB12Header;
                    break;

            }
            return format;
        }
    }
    public enum FormType
    {
        FormS1Header,
        FormS1Details,
        FormS2Header,
        FormDHeader,
        FormB1B2,
        FormC1C2,
        FormG1G2,
        FormR1R2,
        FormF4Header,
        FormF5Header,
        FormFCHeader,
        FormFDHeader,
        FormV1Header,
        FormV2Header,

        FormQA1Header,

        FormV3Header,
        FormV4Header,
        FormV5Header,
        FormF3Header,
        FormF1Header,
        FormTHeader,
        FormM,
        FormB13,
        FormB15,
        FormB14,
        FormT3,
        FormB12
    }

}

