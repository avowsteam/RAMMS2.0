using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.Common
{
    public class StatusList
    {

        public const string Saved = "Saved";
        public const string Submitted = "Submitted";
        public const string Rejected = "Rejected";
        public const string Approved = "Approved";
        public const string Verified = "Verified";
        public const string Proposed = "Proposed";
        public const string Facilitated = "Facilitated";
        public const string Agreed = "Agreed";
        public const string Endorsed = "Endorsed";

        public const string CertifiedbySP = "Certified by SP";
        public const string CertifiedbyEC = "Certified by EC";
        public const string CertifiedbySO = "Certified by SO";

        public const string Supervisor = "Supervisor";
        public const string Executive = "Executive";
        public const string HeadMaintenance = "HeadMaintenance";
        public const string JKRSSuperior = "JKRSSuperior";        
        public const string VerifiedJKRSSuperior = "Verified JKRSSuperior";
        public const string ProcessedJKRSSuperior = "Processed JKRSSuperior";
        public const string AgreedJKRSSuperior = "Agreed JKRSSuperior";
        public const string RegionManager = "Region Manager";
        public const string Completed = "Completed";

        public const string N1Init = "Open";
        public const string N1Issued = "Issued";
        public const string N1Received = "Received";
        public const string N1CorrectiveCompleted = "Corrective Action Completed";
        public const string N1CorrectiveAccepted = "Corrective Action Accepted";
        //public const string N1Verified = "Verified";

        public const string N2Init = "Open";
        public const string N2Issued = "Issued";
        public const string N2Received = "Received";
        public const string N2CorrectiveCompleted = "Corrective Action Completed";
        public const string N2CorrectiveAccepted = "Corrective Action Accepted";
        public const string N2PreventRecurrenceAccepted = "Prevent Recurrence Accepted";

        public const string S1Init = "Open";
        public const string S1Planned = "Planned";
        public const string S1Vetted = "Vetted";

        public const string S2Init = "Open";
        public const string S2Submitted = "Submitted";
        public const string S2Vetted = "Vetted";

        public const string FormW1Saved = "Saved";
        public const string FormW1Submitted = "Submitted";
        public const string FormW1Rejected = "Rejected";
        public const string FormW1Approved = "Approved";

        public const string FormW2Submitted = "Submitted";
        public const string FormW2Saved = "Saved";
        public const string FormW2Received = "Received";
        public const string FormW2Rejected = "Rejected";

        public const string FormWCSubmitted = "Submitted";
        public const string FormWCSaved = "Saved";

        public const string FormWGSubmitted = "Submitted";
        public const string FormWGSaved = "Saved";

        public const string FormWDSubmitted = "Submitted";
        public const string FormWDSaved = "Saved";

        public const string FormWNSubmitted = "Submitted";
        public const string FormWNSaved = "Saved";

        public const string FormV2Saved = "Saved";
        public const string FormV2Submitted = "Submitted";
        public const string FormV2Verified = "Verified";
        public const string FormV2Rejected = "Rejected";
        public const string FormV2Approved = "Approved";

        public const string FormQA1Saved = "Saved";
        public const string FormQA1Submitted = "Submitted";
        public const string FormQA1Rejected = "Rejected";
        public const string FormQA1Approved = "Checked";
        public const string FormQA1Verified = "Executed";

        public const string FormXInit = "Open";
        public const string FormXWorkCompleted = "Work Completed";
        public const string FormXVerified= "Verified";
        public const string FormXVetted = "Vetted";

        public const string FormAInit = "Open";
        public const string FormAInspected = "Inspected";
        public const string FormAExecutiveApproved = "Executive Approved";
        public const string FormAHeadMaintenanceApproved = "Head Maintenance Approved";        
        public const string FormARegionManagerApproved = "Region Manager Approved";
        //public const string FormAJKRSSuperior = "JKRS Approved";

        public const string FormJInit = "Open";
        public const string FormJInspected = "Inspected";
        public const string FormJChecked = "Checked";
        public const string FormJHeadMaintenanceApproved = "Head Maintenance Approved";
        public const string FormJRegionManagerApproved = "Region Manager Approved";

        public const string FormHInit = "Open";
        public const string FormHReported = "Reported"; //Opp Executive
        public const string FormHVerified = "Verified"; //Head Maintenance       
        public const string FormHRegionManagerApproved = "Region Manager Approved";
        public const string FormHReceived = "Received";//JKRS
        public const string FormHVetted = "Vetted";//JKRS

        // Missing from old Code

        public const string FormC1C2Init = "Open"; //To Supervisor
        public const string FormC1C2Inspected = "Inspected"; //To Opp Executive
        public const string FormC1C2ExecutiveApproved = "Executive"; // To Opp Head
        public const string FormC1C2HeadMaintenanceApproved = "Head Maintenance"; //To Region Manager        
        public const string FormC1C2RegionManagerApproved = "Region Manager"; // To JKRS

        public const string FormB1B2Init = "Open";
        public const string FormB1B2Inspected = "Inspected";
        public const string FormB1B2ExecutiveApproved = "Executive";
        public const string FormB1B2HeadMaintenanceApproved = "Head Maintenance";
        public const string FormB1B2RegionManagerApproved = "Region Manager";

        public const string FormF2Init = "Open";
        public const string FormF2Inspected = "Inspected";
        public const string FormF2ExecutiveApproved = "Executive";
        public const string FormF2HeadMaintenanceApproved = "Head Maintenance";
        public const string FormF2RegionManagerApproved = "Region Manager";

        public const string FormF3Init = "Open";
        public const string FormF3Inspected = "Inspected";
        public const string FormF3ExecutiveApproved = "Executive";
        public const string FormF3HeadMaintenanceApproved = "Head Maintenance";
        public const string FormF3RegionManagerApproved = "Region Manager";

        public const string FormF4Init = "Open";
        public const string FormF4Inspected = "Inspected";
        public const string FormF4ExecutiveApproved = "Executive";
        public const string FormF4HeadMaintenanceApproved = "Head Maintenance";
        public const string FormF4RegionManagerApproved = "Region Manager";

        public const string FormF5Init = "Open";//To Supervisor
        public const string FormF5Inspected = "Inspected"; //To Opp Executive
        public const string FormF5ExecutiveApproved = "Executive"; // To Opp Head
        public const string FormF5HeadMaintenanceApproved = "Head Maintenance"; //To Region Manager        
        public const string FormF5RegionManagerApproved = "Region Manager"; // To JKRS

        public const string FormFCInit = "Open";//To Supervisor
        public const string FormFCInspected = "Inspected"; //To Opp Executive
        public const string FormFCExecutiveApproved = "Executive"; // To Opp Head
        public const string FormFCHeadMaintenanceApproved = "Head Maintenance"; //To Region Manager        
        public const string FormFCRegionManagerApproved = "Region Manager"; // To JKRS

        public const string FormFDInit = "Initialize";//To Supervisor
        public const string FormFDInspected = "Inspected"; //To Opp Executive
        public const string FormFDExecutiveApproved = "Executive"; // To Opp Head
        public const string FormFDHeadMaintenanceApproved = "Head Maintenance"; //To Region Manager        
        public const string FormFDRegionManagerApproved = "Region Manager"; // To JKRS

        public const string FormFSInit = "Open";//To Opp Executive        
        public const string FormFSSummarized = "Summarized"; // To Opp Head
        public const string FormFSHeadMaintenanceApproved = "Executive"; //To Region Manager        
        public const string FormFSRegionManagerApproved = "Region Manager"; // To JKRS

        public const string FormV1Saved = "Saved";
        public const string FormV1Submitted = "Submitted";
        public const string FormV1Rejected = "Rejected";
        public const string FormV1Approved = "Approved";
        public const string FormV1Verified = "Verified";

        public const string FormG1G2Saved = "Saved";
        public const string FormG1G2Submitted = "Submitted";
        public const string FormG1G2Rejected = "Rejected";
        public const string FormG1G2Approved = "Approved";
        public const string FormG1G2Verified = "Verified";

        public const string FormR1R2Saved = "Saved";
        public const string FormR1R2Submitted = "Submitted";
        public const string FormR1R2Rejected = "Rejected";
        public const string FormR1R2Approved = "Approved";
        public const string FormR1R2Verified = "Verified";

        public const string FormMSaved = "Saved";
        public const string FormMSubmitted = "Submitted";
        public const string FormMRejected = "Rejected";
        public const string FormMApproved = "Approved";
        public const string FormMVerified = "Verified";

        public const string FormB15Saved = "Saved";
        public const string FormB15Submitted = "Submitted";
        public const string FormB15Rejected = "Rejected";
        public const string FormB15Approved = "Approved";
        public const string FormB15Verified = "Verified";
        public const string FormB15Agreed = "Agreed";

        public const string FormB14Saved = "Saved";
        public const string FormB14Submitted = "Submitted";
        public const string FormB14Rejected = "Rejected";
        public const string FormB14Approved = "Approved";
        public const string FormB14Verified = "Verified";
        public const string FormB14Agreed = "Agreed";

        public const string FormT3Saved = "Saved";
        public const string FormT3Submitted = "Submitted";
        public const string FormT3Initialize = "Initialize";

        public const string FormMapSaved = "Saved";
        public const string FormMapSubmitted = "Submitted";
        public const string FormMapApproved = "Approved";
        public const string FormMapVerified = "Verified";

    }
}
