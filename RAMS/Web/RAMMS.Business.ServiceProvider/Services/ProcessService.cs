﻿using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using RAMMS.Common;
using RAMMS.Domain.Models;

namespace RAMMS.Business.ServiceProvider.Services
{
    public class ProcessService : IProcessService
    {
        private readonly Repository.RAMMSContext context;
        private readonly ISecurity security;
        public ProcessService(IRepositoryUnit repoUnit, ISecurity Security)
        {
            context = repoUnit._context;
            security = Security;
        }
        public async Task<int> Save(DTO.RequestBO.ProcessDTO process)
        {
            int iResult = 0;

            switch (process.Form)
            {
                case "FormA":
                    iResult = await SaveFormA(process);
                    break;
                case "FormB1B2":
                    iResult = await SaveFormB1B2(process);
                    break;
                case "FormC1C2":
                    iResult = await SaveFormC1C2(process);
                    break;
                case "FormD":
                    iResult = await SaveFormD(process);
                    break;
                case "FormF2":
                    iResult = await SaveFormF2(process);
                    break;
                case "FormF4":
                    iResult = await SaveFormF4(process);
                    break;
                case "FormF5":
                    iResult = await SaveFormF5(process);
                    break;
                case "FormFC":
                    iResult = await SaveFormFC(process);
                    break;
                case "FormFD":
                    iResult = await SaveFormFD(process);
                    break;
                case "FormFS":
                    iResult = await SaveFormFS(process);
                    break;
                case "FormH":
                    iResult = await SaveFormH(process);
                    break;
                case "FormJ":
                    iResult = await SaveFormJ(process);
                    break;
                case "FormN1":
                    iResult = await SaveFormN1(process);
                    break;
                case "FormN2":
                    iResult = await SaveFormN2(process);
                    break;
                case "FormS1":
                    iResult = await SaveFormS1(process);
                    break;
                case "FormS2":
                    iResult = await SaveFormS2(process);
                    break;
                case "FormX":
                    iResult = await SaveFormX(process);
                    break;
                case "FormW1":
                    iResult = await SaveFormW1(process);
                    break;
                case "FormW2":
                    iResult = await SaveFormW2(process);
                    break;
                case "FormWC":
                    iResult = await SaveFormW1(process);
                    break;
                case "FormWG":
                    iResult = await SaveFormW2(process);
                    break;
                case "FormV1":
                    iResult = await SaveFormV1(process);
                    break;
                case "FormV2":
                    iResult = await SaveFormV2(process);
                    break;
                case "FormV3":
                    iResult = await SaveFormV3(process);
                    break;
                case "FormV4":
                    iResult = await SaveFormV4(process);
                    break;
                case "FormQA1":
                    iResult = await SaveFormQA1(process);
                    break;
                case "FormF3":
                    iResult = await SaveFormF3(process);
                    break;
                case "FormG1G2":
                    iResult = await SaveFormG1G2(process);
                    break;
                case "FormR1R2":
                    iResult = await SaveFormR1R2(process);
                    break;
                case "FormF1":
                    iResult = await SaveFormF1(process);
                    break;
                case "frmM":
                    iResult = await SaveFormM(process);
                    break;
                case "FormT":
                    iResult = await SaveFormT(process);
                    break;
                case "FormB13":
                    iResult = await SaveFormB13(process);
                    break;
                case "frmB15":
                    iResult = await SaveFormB15(process);
                    break;
                case "frmB14":
                    iResult = await SaveFormB14(process);
                    break;
                case "FormPA":
                    iResult = await SaveFormPA(process);
                    break;

            }
            return iResult;
        }
        public void SaveNotification(RmUserNotification notification, bool isContextSave)
        {
            context.RmUserNotification.Add(notification);
            if (isContextSave)
                context.SaveChanges();
        }
        public List<Dictionary<string, object>> AuditHistory(string formName, int RefId)
        {
            string logs = string.Empty;
            switch (formName)
            {
                case "FormA":
                    logs = this.context.RmFormAHdr.Where(x => x.FahPkRefNo == RefId).Select(x => x.FahAuditLog).FirstOrDefault();
                    break;
                case "FormB1B2":
                    logs = this.context.RmFormB1b2BrInsHdr.Where(x => x.FbrihPkRefNo == RefId).Select(x => x.FbrihAuditLog).FirstOrDefault();
                    break;
                case "FormC1C2":
                    logs = this.context.RmFormCvInsHdr.Where(x => x.FcvihPkRefNo == RefId).Select(x => x.FcvihAuditLog).FirstOrDefault();
                    break;
                case "FormD":
                    logs = this.context.RmFormDHdr.Where(x => x.FdhPkRefNo == RefId).Select(x => x.FdhAuditLog).FirstOrDefault();
                    break;
                case "FormF2":
                    logs = this.context.RmFormF2GrInsHdr.Where(x => x.FgrihPkRefNo == RefId).Select(x => x.FgrihAuditLog).FirstOrDefault();
                    break;
                case "FormF4":
                    logs = this.context.RmFormF4InsHdr.Where(x => x.FivahPkRefNo == RefId).Select(x => x.FivahAuditLog).FirstOrDefault();
                    break;
                case "FormF5":
                    logs = this.context.RmFormF5InsHdr.Where(x => x.FvahPkRefNo == RefId).Select(x => x.FvahAuditLog).FirstOrDefault();
                    break;
                case "FormFC":
                    logs = this.context.RmFormFcInsHdr.Where(x => x.FcihPkRefNo == RefId).Select(x => x.FcihAuditLog).FirstOrDefault();
                    break;
                case "FormFD":
                    logs = this.context.RmFormFdInsHdr.Where(x => x.FdihPkRefNo == RefId).Select(x => x.FdihAuditLog).FirstOrDefault();
                    break;
                case "FormFS":
                    logs = this.context.RmFormFsInsHdr.Where(x => x.FshPkRefNo == RefId).Select(x => x.FshAuditLog).FirstOrDefault();
                    break;
                case "FormH":
                    logs = this.context.RmFormHHdr.Where(x => x.FhhPkRefNo == RefId).Select(x => x.FhhAuditLog).FirstOrDefault();
                    break;
                case "FormJ":
                    logs = this.context.RmFormJHdr.Where(x => x.FjhPkRefNo == RefId).Select(x => x.FjhAuditLog).FirstOrDefault();
                    break;
                case "FormN1":
                    logs = this.context.RmFormN1Hdr.Where(x => x.FnihPkRefNo == RefId).Select(x => x.FnihAuditLog).FirstOrDefault();
                    break;
                case "FormN2":
                    logs = this.context.RmFormN2Hdr.Where(x => x.FnthPkRefNo == RefId).Select(x => x.FnthAuditLog).FirstOrDefault();
                    break;
                case "FormS1":
                    logs = this.context.RmFormS1Hdr.Where(x => x.FsihPkRefNo == RefId).Select(x => x.FsihAuditLog).FirstOrDefault();
                    break;
                case "FormS2":
                    logs = this.context.RmFormS2Hdr.Where(x => x.FsiihPkRefNo == RefId).Select(x => x.FsiihAuditLog).FirstOrDefault();
                    break;
                case "FormW1":
                    logs = this.context.RmIwFormW1.Where(x => x.Fw1PkRefNo == RefId).Select(x => x.Fw1AuditLog).FirstOrDefault();
                    break;
                case "FormW2":
                    logs = this.context.RmIwFormW2.Where(x => x.Fw2PkRefNo == RefId).Select(x => x.Fw2AuditLog).FirstOrDefault();
                    break;
                case "FormWC":
                    logs = this.context.RmIwFormWc.Where(x => x.FwcPkRefNo == RefId).Select(x => x.FwcAuditLog).FirstOrDefault();
                    break;
                case "FormWG":
                    logs = this.context.RmIwFormWg.Where(x => x.FwgPkRefNo == RefId).Select(x => x.FwgAuditLog).FirstOrDefault();
                    break;
                case "FormWD":
                    logs = this.context.RmIwFormWd.Where(x => x.FwdPkRefNo == RefId).Select(x => x.FwdAuditLog).FirstOrDefault();
                    break;
                case "FormWN":
                    logs = this.context.RmIwFormWn.Where(x => x.FwnPkRefNo == RefId).Select(x => x.FwnAuditLog).FirstOrDefault();
                    break;
                case "FormV1":
                    logs = this.context.RmFormV1Hdr.Where(x => x.Fv1hPkRefNo == RefId).Select(x => x.Fv1hAuditLog).FirstOrDefault();
                    break;
                case "FormV2":
                    logs = this.context.RmFormV2Hdr.Where(x => x.Fv2hPkRefNo == RefId).Select(x => x.Fv2hAuditLog).FirstOrDefault();
                    break;
                case "FormQA1":
                    logs = this.context.RmFormQa1Hdr.Where(x => x.Fqa1hPkRefNo == RefId).Select(x => x.Fqa1hAuditLog).FirstOrDefault();
                    break;
                case "FormV3":
                    logs = this.context.RmFormV3Hdr.Where(x => x.Fv3hPkRefNo == RefId).Select(x => x.Fv3hAuditLog).FirstOrDefault();
                    break;
                case "FormV4":
                    logs = this.context.RmFormV4Hdr.Where(x => x.Fv4hPkRefNo == RefId).Select(x => x.Fv4hAuditLog).FirstOrDefault();
                    break;
                case "FormV5":
                    logs = this.context.RmFormV5Hdr.Where(x => x.Fv5hPkRefNo == RefId).Select(x => x.Fv5hAuditLog).FirstOrDefault();
                    break;
                case "FormF3":
                    logs = this.context.RmFormF3Hdr.Where(x => x.Ff3hPkRefNo == RefId).Select(x => x.Ff3hAuditLog).FirstOrDefault();
                    break;

                case "FormG1G2":
                    logs = this.context.RmFormG1Hdr.Where(x => x.Fg1hPkRefNo == RefId).Select(x => x.Fg1hAuditLog).FirstOrDefault();
                    break;
                case "FormR1R2":
                    logs = this.context.RmFormR1Hdr.Where(x => x.Fr1hPkRefNo == RefId).Select(x => x.Fr1hAuditLog).FirstOrDefault();
                    break;

                case "FormF1":
                    logs = this.context.RmFormF1Hdr.Where(x => x.Ff1hPkRefNo == RefId).Select(x => x.Ff1hAuditLog).FirstOrDefault();
                    break;
                case "FormM":
                    logs = this.context.RmFormMHdr.Where(x => x.FmhPkRefNo == RefId).Select(x => x.FmhAuditLog).FirstOrDefault();
                    break;
                case "FormT":
                    logs = this.context.RmFormTHdr.Where(x => x.FmtPkRefNo == RefId).Select(x => x.FmtAuditLog).FirstOrDefault();
                    break;
                case "FormB13":
                    logs = this.context.RmB13ProposedPlannedBudget.Where(x => x.B13pPkRefNo == RefId).Select(x => x.B13pAuditLog).FirstOrDefault();
                    break;
                case "FormB15":
                    logs = this.context.RmB15Hdr.Where(x => x.B15hPkRefNo == RefId).Select(x => x.B15hAuditlog).FirstOrDefault();
                    break;
                case "FormB14":
                    logs = this.context.RmB14Hdr.Where(x => x.B14hPkRefNo == RefId).Select(x => x.B14hAuditlog).FirstOrDefault();
                    break;
                case "FormT3":
                    logs = this.context.RmT3Hdr.Where(x => x.T3hPkRefNo == RefId).Select(x => x.T3hAuditlog).FirstOrDefault();
                    break;
                case "FormT4":
                    logs = this.context.RmT4DesiredBdgtHeader.Where(x => x.T4dbhPkRefNo == RefId).Select(x => x.T4dbhAuditLog).FirstOrDefault();
                    break;
                case "FormP1":
                    logs = this.context.RmPaymentCertificateHeader.Where(x => x.PchPkRefNo == RefId).Select(x => x.PchAuditLog).FirstOrDefault();
                    break;
                case "FormPA":
                    logs = this.context.RmPaymentCertificateMamw.Where(x => x.PcmamwPkRefNo == RefId).Select(x => x.PcmamwAuditLog).FirstOrDefault();
                    break;
                case "FormPB":
                    logs = this.context.RmPbIw.Where(x => x.PbiwPkRefNo == RefId).Select(x => x.PbiwAuditLog).FirstOrDefault();
                    break;
            }
            return Utility.ProcessLog(logs);
        }
        private async Task<int> SaveFormN1(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormN1Hdr.Where(x => x.FnihPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FnihSubmitSts && (form.FnihStatus == Common.StatusList.N1Init))
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.JKRSSuperiorOfficerSO;
                    form.FnihStatus = process.IsApprove ? Common.StatusList.N1Issued : StatusList.N1Init;
                    strTitle = "Issued";
                }
                else if (form.FnihStatus == Common.StatusList.N1Issued)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.JKRSSuperiorOfficerSO;
                    form.FnihStatus = process.IsApprove ? Common.StatusList.N1Received : StatusList.N1Init;
                    if (!process.IsApprove) { form.FnihSubmitSts = false; }
                    form.FnihUseridRcvd = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnihUsernameRcvd = !process.IsApprove ? null : process.UserName;
                    form.FnihDesignationRcvd = !process.IsApprove ? null : process.UserDesignation;
                    strTitle = "Received";
                }
                else if (form.FnihStatus == Common.StatusList.N1Received)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperRegionManager;
                    strTitle = "Corrective Action Completed";
                    form.FnihStatus = process.IsApprove ? Common.StatusList.N1CorrectiveCompleted : StatusList.N1Issued;
                    form.FnihUseridCorrective = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnihUsernameCorrective = !process.IsApprove ? null : process.UserName;
                    form.FnihDesignationCorrective = !process.IsApprove ? null : process.UserDesignation;
                    form.FnihDtCorrective = !process.IsApprove ? null : process.ApproveDate;
                }
                else if (form.FnihStatus == Common.StatusList.N1CorrectiveCompleted)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperRegionManager;
                    strTitle = "Corrective Action Accepted";
                    form.FnihStatus = process.IsApprove ? Common.StatusList.N1CorrectiveAccepted : StatusList.N1Received;
                    form.FnihUseridAccptd = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnihUsernameAccptd = !process.IsApprove ? null : process.UserName;
                    form.FnihDesignationAccptd = !process.IsApprove ? null : process.UserDesignation;
                    form.FnihDtAccptd = !process.IsApprove ? null : process.ApproveDate;
                }
                else if (form.FnihStatus == Common.StatusList.N1CorrectiveAccepted)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.JKRSSuperiorOfficerSO;
                    strTitle = "Verified";
                    form.FnihStatus = process.IsApprove ? Common.StatusList.Completed : StatusList.N1CorrectiveCompleted;
                    form.FnihUseridVer = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnihUsernameVer = !process.IsApprove ? null : process.UserName;
                    form.FnihDesignationVer = !process.IsApprove ? null : process.UserDesignation;
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FnihUseridIssued.HasValue)
                            lstNotUserId.Add(form.FnihUseridIssued.Value);
                        if (form.FnihUseridRcvd.HasValue)
                            lstNotUserId.Add(form.FnihUseridRcvd.Value);
                        if (form.FnihUseridCorrective.HasValue)
                            lstNotUserId.Add(form.FnihUseridCorrective.Value);
                        if (form.FnihUseridAccptd.HasValue)
                            lstNotUserId.Add(form.FnihUseridAccptd.Value);
                        if (form.FnihUseridVer.HasValue)
                            lstNotUserId.Add(form.FnihUseridVer.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FnihAuditLog = Utility.ProcessLog(form.FnihAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form N1 (" + form.FnihNcnNo + ")";
                strNotURL = "/MAM/EditFormN1?headerId=" + form.FnihPkRefNo.ToString() + "&view=1";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormN2(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormN2Hdr.Where(x => x.FnthPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FnihSubmitSts && (form.FnthStatus == Common.StatusList.N2Init))
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.JKRSSuperiorOfficerSO;
                    form.FnthStatus = process.IsApprove ? Common.StatusList.N2Issued : StatusList.N2Init;
                    strTitle = "Issued";
                }
                else if (form.FnthStatus == Common.StatusList.N2Issued)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.JKRSSuperiorOfficerSO;
                    form.FnthStatus = process.IsApprove ? Common.StatusList.N2Received : StatusList.N2Init;
                    if (!process.IsApprove) { form.FnihSubmitSts = false; }
                    form.FnthUseridRcvd = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnthUsernameRcvd = !process.IsApprove ? null : process.UserName;
                    form.FnthDesignationRcvd = !process.IsApprove ? null : process.UserDesignation;
                    strTitle = "Received";
                }
                else if (form.FnthStatus == Common.StatusList.N2Received)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperRegionManager;
                    strTitle = "Corrective Action Completed";
                    form.FnthStatus = process.IsApprove ? Common.StatusList.N2CorrectiveCompleted : StatusList.N2Issued;
                    form.FnthUseridCorrective = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnthUsernameCorrective = !process.IsApprove ? null : process.UserName;
                    form.FnthDesignationCorrective = !process.IsApprove ? null : process.UserDesignation;
                    form.FnthDtCorrective = !process.IsApprove ? null : process.ApproveDate;
                }
                else if (form.FnthStatus == Common.StatusList.N2CorrectiveCompleted)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperRegionManager;
                    strTitle = "Corrective Action Accepted";
                    form.FnthStatus = process.IsApprove ? Common.StatusList.N2CorrectiveAccepted : StatusList.N2Received;
                    form.FnthUseridAccptd = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnthUsernameAccptd = !process.IsApprove ? null : process.UserName;
                    form.FnthDesignationAccptd = !process.IsApprove ? null : process.UserDesignation;
                    form.FnthDtAccptd = !process.IsApprove ? null : process.ApproveDate;
                }
                else if (form.FnthStatus == Common.StatusList.N2CorrectiveAccepted)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.JKRSSuperiorOfficerSO;
                    strTitle = "Corrective Action Accepted";
                    form.FnthStatus = process.IsApprove ? Common.StatusList.N2PreventRecurrenceAccepted : StatusList.N2CorrectiveCompleted;
                    form.FnthUseridPreventive = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnthUsernamePreventive = !process.IsApprove ? null : process.UserName;
                    form.FnthDesignationPreventive = !process.IsApprove ? null : process.UserDesignation;
                    form.FnthDtPreventive = !process.IsApprove ? null : process.ApproveDate;
                }
                else if (form.FnthStatus == Common.StatusList.N2PreventRecurrenceAccepted)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.JKRSSuperiorOfficerSO;
                    strTitle = "Verified";
                    form.FnthStatus = process.IsApprove ? Common.StatusList.Completed : StatusList.N2CorrectiveAccepted;
                    form.FnthUseridVer = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FnthUsernameVer = !process.IsApprove ? null : process.UserName;
                    form.FnthDesignationVer = !process.IsApprove ? null : process.UserDesignation;
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FnthUseridIssued.HasValue)
                            lstNotUserId.Add(form.FnthUseridIssued.Value);
                        if (form.FnthUseridRcvd.HasValue)
                            lstNotUserId.Add(form.FnthUseridRcvd.Value);
                        if (form.FnthUseridCorrective.HasValue)
                            lstNotUserId.Add(form.FnthUseridCorrective.Value);
                        if (form.FnthUseridAccptd.HasValue)
                            lstNotUserId.Add(form.FnthUseridAccptd.Value);
                        if (form.FnthUseridVer.HasValue)
                            lstNotUserId.Add(form.FnthUseridVer.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FnthAuditLog = Utility.ProcessLog(form.FnthAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form N2 (" + form.FnthNcrNo + ")";
                strNotURL = "/MAM/EditFormN2?headerId=" + form.FnthPkRefNo.ToString() + "&view=1";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormS1(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormS1Hdr.Where(x => x.FsihPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FsihSubmitSts == true && (form.FsihStatus == Common.StatusList.S1Init))
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FsihStatus = process.IsApprove ? Common.StatusList.S1Planned : StatusList.S1Init;
                    strTitle = "Planned";
                }
                else if (form.FsihStatus == Common.StatusList.S1Planned)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperationsExecutive;
                    form.FsihStatus = process.IsApprove ? Common.StatusList.S1Vetted : StatusList.S1Init;
                    if (!process.IsApprove) { form.FsihSubmitSts = false; }
                    form.FsiihUseridVet = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FsiihUserNameVet = !process.IsApprove ? null : process.UserName;
                    form.FsiihUserDesignationVet = !process.IsApprove ? null : process.UserDesignation;
                    form.FsiihDtVet = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = "Vetted";
                }
                else if (form.FsihStatus == Common.StatusList.S1Vetted)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperationsExecutive;
                    strTitle = "Agreed";
                    form.FsihStatus = process.IsApprove ? Common.StatusList.Completed : StatusList.S1Planned;
                    form.FsiihUseridAgrd = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FsiihUserNameAgrd = !process.IsApprove ? null : process.UserName;
                    form.FsiihUserDesignationAgrd = !process.IsApprove ? null : process.UserDesignation;
                    form.FsiihDtAgrd = !process.IsApprove ? null : process.ApproveDate;

                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FsiihUseridPlan.HasValue)
                            lstNotUserId.Add(form.FsiihUseridPlan.Value);
                        if (form.FsiihUseridVet.HasValue)
                            lstNotUserId.Add(form.FsiihUseridVet.Value);
                        if (form.FsiihUseridAgrd.HasValue)
                            lstNotUserId.Add(form.FsiihUseridAgrd.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FsihAuditLog = Utility.ProcessLog(form.FsihAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form S1 (" + form.FsihRefId + ")";
                strNotURL = "/FormS1/View/" + form.FsihPkRefNo.ToString();
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormS2(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormS2Hdr.Where(x => x.FsiihPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FsiihSubmitSts == true && (form.FsiihStatus == Common.StatusList.S2Init))
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FsiihStatus = process.IsApprove ? Common.StatusList.S2Submitted : StatusList.S2Init;
                    strTitle = "Submitted";
                }
                else if (form.FsiihStatus == Common.StatusList.S2Submitted)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperationsExecutive;
                    form.FsiihStatus = process.IsApprove ? Common.StatusList.S2Vetted : StatusList.S2Init;
                    if (!process.IsApprove) { form.FsiihSubmitSts = false; }
                    form.FsiihUseridVet = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FsiihUserNameVet = !process.IsApprove ? null : process.UserName;
                    form.FsiihUserDesignationVet = !process.IsApprove ? null : process.UserDesignation;
                    form.FsiihDtVet = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = "Vetted";
                }
                else if (form.FsiihStatus == Common.StatusList.S2Vetted)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperationsExecutive;
                    strTitle = "Agreed";
                    form.FsiihStatus = process.IsApprove ? Common.StatusList.Completed : StatusList.S2Submitted;
                    form.FsiihUseridAgrd = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FsiihUserNameAgrd = !process.IsApprove ? null : process.UserName;
                    form.FsiihUserDesignationAgrd = !process.IsApprove ? null : process.UserDesignation;
                    form.FsiihDtAgrd = !process.IsApprove ? null : process.ApproveDate;

                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FsiihUseridSub.HasValue)
                            lstNotUserId.Add(form.FsiihUseridSub.Value);
                        if (form.FsiihUseridSchld.HasValue)
                            lstNotUserId.Add(form.FsiihUseridSchld.Value);
                        if (form.FsiihUseridPrioritised.HasValue)
                            lstNotUserId.Add(form.FsiihUseridPrioritised.Value);
                        if (form.FsiihUseridVet.HasValue)
                            lstNotUserId.Add(form.FsiihUseridVet.Value);
                        if (form.FsiihUseridAgrd.HasValue)
                            lstNotUserId.Add(form.FsiihUseridAgrd.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FsiihAuditLog = Utility.ProcessLog(form.FsiihAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form S2 (" + form.FsiihRefId + ")";
                strNotURL = "/FormS2/AddS2?id=" + form.FsiihPkRefNo.ToString() + "&isview=false";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormW1(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmIwFormW1.Where(x => x.Fw1PkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";

                if (process.Stage == Common.StatusList.FormW1Submitted)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperationsExecutive;

                    form.Fw1Status = process.IsApprove ? Common.StatusList.FormW1Approved : StatusList.FormW1Rejected;

                    if (process.IsApprove)
                    {
                        strTitle = "Approved";
                        List<int> lstNotUserId = new List<int>();
                        if (form.Fw1UseridReq.HasValue)
                            lstNotUserId.Add(form.Fw1UseridReq.Value);
                        if (form.Fw1UseridVer.HasValue)
                            lstNotUserId.Add(form.Fw1UseridVer.Value);

                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                    else
                    {
                        strTitle = "Rejected";
                    }
                }
                form.Fw1AuditLog = Utility.ProcessLog(form.Fw1AuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form W1 (" + form.Fw1PkRefNo + ")";
                strNotURL = "/InstructedWorks/EditFormW1?id=" + form.Fw1PkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormW2(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmIwFormW2.Where(x => x.Fw2PkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";

                if (process.Stage == Common.StatusList.FormW2Submitted)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.JKRSSuperiorOfficerSO;
                    strTitle = "Received";
                    form.Fw2Status = process.IsApprove ? Common.StatusList.FormW2Received : StatusList.FormW2Rejected;

                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.Fw2UseridIssu.HasValue)
                            lstNotUserId.Add(form.Fw2UseridIssu.Value);
                        if (form.Fw2UseridReq.HasValue)
                            lstNotUserId.Add(form.Fw2UseridReq.Value);

                        form.Fw2UseridReq = Convert.ToInt32(process.UserID);
                        form.Fw2UsernameReq = process.UserName;
                        form.Fw2DesignationReq = process.UserDesignation;
                        form.Fw2DtReq = process.ApproveDate;
                        form.Fw2SignReq = true;

                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                    else
                    {
                        strTitle = StatusList.FormW2Received;
                    }
                }
                form.Fw2AuditLog = Utility.ProcessLog(form.Fw2AuditLog, strTitle, process.IsApprove ? "Recieved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form W2 (" + form.Fw2PkRefNo + ")";
                strNotURL = "/InstructedWorks/EditFormW2?id=" + form.Fw2PkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormG1G2(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormG1Hdr.Where(x => x.Fg1hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.FormG1G2Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Fg1hStatus = process.IsApprove ? Common.StatusList.FormG1G2Verified : Common.StatusList.FormG1G2Saved;
                    strTitle = "Verified By";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.FormG1G2Saved;
                }
                else if (process.Stage == Common.StatusList.FormG1G2Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.Fg1hStatus = process.IsApprove ? Common.StatusList.FormG1G2Approved : Common.StatusList.FormG1G2Submitted;
                    strTitle = "Audited By";
                    strStatus = "Audited";
                    strNotStatus = Common.StatusList.FormG1G2Verified;
                    form.Fg1hAuditedBy = Convert.ToInt32(process.UserID);
                    form.Fg1hAuditedName = process.UserName;
                    form.Fg1hAuditedDesig = process.UserDesignation;
                    form.Fg1hAuditedDt = process.ApproveDate;
                    form.Fg1hAuditedSign = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();

                    if (form.Fg1hInspectedBy.HasValue)
                        lstNotUserId.Add(form.Fg1hInspectedBy.Value);
                    if (form.Fg1hAuditedBy.HasValue)
                        lstNotUserId.Add(form.Fg1hAuditedBy.Value);


                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.FormG1G2Submitted)
                    {
                        form.Fg1hSubmitSts = false;
                    }
                }

                form.Fg1hAuditLog = Utility.ProcessLog(form.Fg1hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form G1G2 (" + form.Fg1hPkRefNo + ")";
                strNotURL = "/FormG1G2/Edit/" + form.Fg1hPkRefNo.ToString() + "?View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormR1R2(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormR1Hdr.Where(x => x.Fr1hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.FormR1R2Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Fr1hStatus = process.IsApprove ? Common.StatusList.FormR1R2Verified : Common.StatusList.FormR1R2Saved;
                    strTitle = "Verified By";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.FormR1R2Saved;
                }
                else if (process.Stage == Common.StatusList.FormR1R2Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.Fr1hStatus = process.IsApprove ? Common.StatusList.FormR1R2Approved : Common.StatusList.FormR1R2Submitted;
                    strTitle = "Audited By";
                    strStatus = "Audited";
                    strNotStatus = Common.StatusList.FormR1R2Verified;
                    form.Fr1hAuditedBy = Convert.ToInt32(process.UserID);
                    form.Fr1hAuditedName = process.UserName;
                    form.Fr1hAuditedDesig = process.UserDesignation;
                    form.Fr1hAuditedDt = process.ApproveDate;
                    form.Fr1hAuditedSign = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();

                    if (form.Fr1hInspectedBy.HasValue)
                        lstNotUserId.Add(form.Fr1hInspectedBy.Value);
                    if (form.Fr1hAuditedBy.HasValue)
                        lstNotUserId.Add(form.Fr1hAuditedBy.Value);


                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.FormR1R2Submitted)
                    {
                        form.Fr1hSubmitSts = false;
                    }
                }

                form.Fr1hAuditLog = Utility.ProcessLog(form.Fr1hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form R1R2 (" + form.Fr1hPkRefNo + ")";
                strNotURL = "/FormR1R2/Edit/" + form.Fr1hPkRefNo.ToString() + "?View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormX(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormXHdr.Where(x => x.FxhPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (process.Stage == Common.StatusList.FormXInit)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.Supervisor : GroupNames.Supervisor;
                    strTitle = "Assigned";
                }
                else if (process.Stage == Common.StatusList.FormXWorkCompleted)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    strTitle = "Work Completed";
                }
                else if (process.Stage == Common.StatusList.FormXVetted)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperationsExecutive;
                    strTitle = "Vetted";
                }
                else if (process.Stage == Common.StatusList.FormXVerified)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperationsExecutive;
                    strTitle = "Agreed";

                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FxhUseridAssgn.HasValue)
                            lstNotUserId.Add(form.FxhUseridAssgn.Value);
                        if (form.FxhUseridVer.HasValue)
                            lstNotUserId.Add(form.FxhUseridVer.Value);
                        if (form.FxhUseridVet.HasValue)
                            lstNotUserId.Add(form.FxhUseridVet.Value);
                        if (form.FxhUseridSchdVer.HasValue)
                            lstNotUserId.Add(form.FxhUseridSchdVer.Value);
                        if (form.FxhUseridPrp.HasValue)
                            lstNotUserId.Add(form.FxhUseridPrp.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                //form.FsiihAuditLog = Utility.ProcessLog(form.FsiihAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form X (" + form.FxhRefId + ")";
                strNotURL = "/ERT/FormX?vid=" + form.FxhPkRefNo.ToString();
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormD(DTO.RequestBO.ProcessDTO process)
        {

            var form = context.RmFormDHdr.Where(x => x.FdhPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FdhStatus == Common.StatusList.Executive)
                {
                    if (!process.IsApprove) { form.FdhSubmitSts = false; }
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FdhStatus = process.IsApprove ? Common.StatusList.HeadMaintenance : Common.StatusList.Supervisor;
                    form.FdhUseridVer = !process.IsApprove ? null : process.UserID;
                    form.FdhUsernameVer = !process.IsApprove ? null : process.UserName;
                    form.FdhDesignationVer = !process.IsApprove ? null : process.UserDesignation;
                    form.FdhDtVer = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = "Verified By";
                }
                else if (form.FdhStatus == Common.StatusList.HeadMaintenance)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperationsExecutive;
                    form.FdhStatus = process.IsApprove ? Common.StatusList.VerifiedJKRSSuperior : StatusList.Executive;
                    form.FdhUseridVet = !process.IsApprove ? null : process.UserID;
                    form.FdhUsernameVet = !process.IsApprove ? null : process.UserName;
                    form.FdhDesignationVet = !process.IsApprove ? null : process.UserDesignation;
                    form.FdhDtVet = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = "Vetted By";
                }
                else if (form.FdhStatus == Common.StatusList.VerifiedJKRSSuperior)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FdhStatus = process.IsApprove ? Common.StatusList.ProcessedJKRSSuperior : StatusList.HeadMaintenance;
                    form.FdhUseridVerSo = !process.IsApprove ? null : process.UserID;
                    form.FdhUsernameVerSo = !process.IsApprove ? null : process.UserName;
                    form.FdhDesignationVerSo = !process.IsApprove ? null : process.UserDesignation;
                    form.FdhDtVerSo = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = "S.O Verified By";
                }
                else if (form.FdhStatus == Common.StatusList.ProcessedJKRSSuperior)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.JKRSSuperiorOfficerSO;
                    form.FdhStatus = process.IsApprove ? Common.StatusList.AgreedJKRSSuperior : StatusList.VerifiedJKRSSuperior;
                    form.FdhUseridPrcdSo = !process.IsApprove ? null : process.UserID;
                    form.FdhUsernamePrcdSo = !process.IsApprove ? null : process.UserName;
                    form.FdhDesignationPrcdSo = !process.IsApprove ? null : process.UserDesignation;
                    form.FdhDtPrcdSo = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = "S.O Processed By";
                }
                else if (form.FdhStatus == Common.StatusList.AgreedJKRSSuperior)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.JKRSSuperiorOfficerSO;
                    form.FdhStatus = process.IsApprove ? Common.StatusList.Completed : StatusList.ProcessedJKRSSuperior;
                    form.FdhUseridAgrdSo = !process.IsApprove ? null : process.UserID;
                    form.FdhUsernameAgrdSo = !process.IsApprove ? null : process.UserName;
                    form.FdhDesignationAgrdSo = !process.IsApprove ? null : process.UserDesignation;
                    form.FdhDtAgrdSo = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = "S.O Agreed By";
                    if (process.IsApprove)
                    {
                        List<string> lstNotUserId = new List<string>();
                        lstNotUserId.Add(form.FdhUseridAgrdSo);
                        lstNotUserId.Add(form.FdhUseridPrcdSo);
                        lstNotUserId.Add(form.FdhUseridVerSo);
                        lstNotUserId.Add(form.FdhUseridVet);
                        lstNotUserId.Add(form.FdhUseridVer);
                        lstNotUserId.Add(form.FdhUseridPrp);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FdhAuditLog = Utility.ProcessLog(form.FdhAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form D (" + form.FdhRefId + ")";
                strNotURL = "/ERT/EditFormD?id=" + form.FdhPkRefNo.ToString() + "&view=1";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormA(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormAHdr.Where(x => x.FahPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FahSubmitSts == true && (form.FahStatus == Common.StatusList.FormAInit))
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FahStatus = process.IsApprove ? Common.StatusList.FormAInspected : StatusList.FormAInit;
                    strTitle = StatusList.FormFCInspected;
                }
                else if (form.FahStatus == Common.StatusList.FormAInspected)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.OperationsExecutive;
                    form.FahStatus = process.IsApprove ? Common.StatusList.FormAExecutiveApproved : StatusList.FormAInit;
                    if (!process.IsApprove) { form.FahSubmitSts = false; }
                    strTitle = Common.StatusList.FormAExecutiveApproved;
                }
                else if (form.FahStatus == Common.StatusList.FormAExecutiveApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FahStatus = process.IsApprove ? Common.StatusList.FormAHeadMaintenanceApproved : StatusList.FormAExecutiveApproved;
                    strTitle = Common.StatusList.FormAHeadMaintenanceApproved;
                }
                else if (form.FahStatus == Common.StatusList.FormAHeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperRegionManager;
                    form.FahStatus = process.IsApprove ? Common.StatusList.FormARegionManagerApproved : StatusList.FormAExecutiveApproved;
                    strTitle = Common.StatusList.FormARegionManagerApproved;
                }
                else if (form.FahStatus == Common.StatusList.FormARegionManagerApproved)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.JKRSSuperiorOfficerSO;
                    form.FahStatus = process.IsApprove ? Common.StatusList.Completed : StatusList.FormAHeadMaintenanceApproved;
                    strTitle = "Verified";
                    form.FahUseridVer = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FahUsernameVer = !process.IsApprove ? null : process.UserName;
                    form.FahDesignationVer = !process.IsApprove ? null : process.UserDesignation;
                    form.FahDtVer = !process.IsApprove ? null : process.ApproveDate;

                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FahUseridVer.HasValue)
                            lstNotUserId.Add(form.FahUseridVer.Value);
                        if (form.FahUseridPrp.HasValue)
                            lstNotUserId.Add(form.FahUseridPrp.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FahAuditLog = Utility.ProcessLog(form.FahAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form A (" + form.FahRefId + ")";
                strNotURL = "/NOD?vid=" + form.FahPkRefNo.ToString();
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormJ(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormJHdr.Where(x => x.FjhPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FjhSubmitSts == true && (form.FjhStatus == Common.StatusList.FormJInit))
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FjhStatus = process.IsApprove ? Common.StatusList.FormJInspected : StatusList.FormJInit;
                    strTitle = StatusList.FormFCInspected;
                }
                else if (form.FjhStatus == Common.StatusList.FormJInspected)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.OperationsExecutive;
                    form.FjhStatus = process.IsApprove ? Common.StatusList.FormJChecked : StatusList.FormJInit;
                    if (!process.IsApprove) { form.FjhSubmitSts = false; }
                    form.FjhUseridVer = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FjhUsernameVer = !process.IsApprove ? null : process.UserName;
                    form.FjhDesignationVer = !process.IsApprove ? null : process.UserDesignation;
                    form.FjhDtVer = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = Common.StatusList.FormJChecked;
                }
                else if (form.FjhStatus == Common.StatusList.FormJChecked)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FjhStatus = process.IsApprove ? Common.StatusList.FormJHeadMaintenanceApproved : StatusList.FormJInspected;
                    strTitle = Common.StatusList.FormJHeadMaintenanceApproved;
                }
                else if (form.FjhStatus == Common.StatusList.FormJHeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperRegionManager;
                    form.FjhStatus = process.IsApprove ? Common.StatusList.FormJRegionManagerApproved : StatusList.FormJChecked;
                    strTitle = Common.StatusList.FormJRegionManagerApproved;
                }
                else if (form.FjhStatus == Common.StatusList.FormJRegionManagerApproved)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.JKRSSuperiorOfficerSO;
                    form.FjhStatus = process.IsApprove ? Common.StatusList.Completed : StatusList.FormJHeadMaintenanceApproved;
                    strTitle = "Audited";
                    form.FjhUseridVet = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FjhUsernameVet = !process.IsApprove ? null : process.UserName;
                    form.FjhDesignationVet = !process.IsApprove ? null : process.UserDesignation;
                    form.FjhDtVet = !process.IsApprove ? null : process.ApproveDate;

                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FjhUseridVer.HasValue)
                            lstNotUserId.Add(form.FjhUseridVer.Value);
                        if (form.FjhUseridPrp.HasValue)
                            lstNotUserId.Add(form.FjhUseridPrp.Value);
                        if (form.FjhUseridVet.HasValue)
                            lstNotUserId.Add(form.FjhUseridVet.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FjhAuditLog = Utility.ProcessLog(form.FjhAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form J (" + form.FjhRefId + ")";
                strNotURL = "/NOD/FormJ?vid=" + form.FjhPkRefNo.ToString();
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormH(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormHHdr.Where(x => x.FhhPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FhhSubmitSts == true && (form.FhhStatus == Common.StatusList.FormHInit))
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.OperationsExecutive;
                    form.FhhStatus = process.IsApprove ? Common.StatusList.FormHReported : StatusList.FormHInit;
                    strTitle = Common.StatusList.FormJChecked;
                }
                else if (form.FhhStatus == Common.StatusList.FormHReported)
                {
                    if (!process.IsApprove) { form.FhhSubmitSts = false; }
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FhhStatus = process.IsApprove ? Common.StatusList.FormHVerified : StatusList.FormHInit;
                    form.FhhUseridVer = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FhhUsernameVer = !process.IsApprove ? null : process.UserName;
                    form.FhhDesignationVer = !process.IsApprove ? null : process.UserDesignation;
                    form.FhhDtVer = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = Common.StatusList.FormHVerified;
                }
                else if (form.FhhStatus == Common.StatusList.FormHVerified)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OperRegionManager;
                    form.FhhStatus = process.IsApprove ? Common.StatusList.FormHRegionManagerApproved : StatusList.FormHReported;
                    strTitle = Common.StatusList.FormHRegionManagerApproved;
                }
                else if (form.FhhStatus == Common.StatusList.FormHRegionManagerApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.JKRSSuperiorOfficerSO;
                    form.FhhStatus = process.IsApprove ? Common.StatusList.FormHReceived : StatusList.FormHVerified;
                    form.FhhUseridRcvdAuth = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FhhUsernameRcvdAuth = !process.IsApprove ? null : process.UserName;
                    form.FhhDesignationRcvdAuth = !process.IsApprove ? null : process.UserDesignation;
                    form.FhhDtRcvdAuth = !process.IsApprove ? null : process.ApproveDate;
                    strTitle = Common.StatusList.FormHReceived;
                }
                else if (form.FhhStatus == Common.StatusList.FormHReceived)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.JKRSSuperiorOfficerSO;
                    form.FhhStatus = process.IsApprove ? Common.StatusList.Completed : StatusList.FormHRegionManagerApproved;
                    strTitle = "Vetted";
                    form.FhhUseridVetAuth = !process.IsApprove ? null : Utility.ToNullInt(process.UserID);
                    form.FhhUsernameVetAuth = !process.IsApprove ? null : process.UserName;
                    form.FhhDesignationVetAuth = !process.IsApprove ? null : process.UserDesignation;
                    form.FhhDtVetAuth = !process.IsApprove ? null : process.ApproveDate;

                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FhhUseridVer.HasValue)
                            lstNotUserId.Add(form.FhhUseridVer.Value);
                        if (form.FhhUseridVetAuth.HasValue)
                            lstNotUserId.Add(form.FhhUseridVetAuth.Value);
                        if (form.FhhUseridRcvdAuth.HasValue)
                            lstNotUserId.Add(form.FhhUseridRcvdAuth.Value);
                        if (form.FhhUseridPrp.HasValue)
                            lstNotUserId.Add(form.FhhUseridPrp.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FhhAuditLog = Utility.ProcessLog(form.FhhAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form H (" + form.FhhRefId + ")";
                strNotURL = "/NOD/FormH?vid=" + form.FhhPkRefNo.ToString();
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormC1C2(DTO.RequestBO.ProcessDTO process)
        {

            var form = context.RmFormCvInsHdr.Where<RmFormCvInsHdr>(x => x.FcvihPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FcvihSubmitSts && form.FcvihStatus == StatusList.FormC1C2Init)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FcvihStatus = process.IsApprove ? StatusList.FormC1C2Inspected : StatusList.FormC1C2Init;
                    strTitle = StatusList.FormC1C2Inspected;
                }
                else if (form.FcvihStatus == StatusList.FormC1C2Inspected)
                {
                    if (!process.IsApprove)
                        form.FcvihSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FcvihStatus = process.IsApprove ? StatusList.FormC1C2ExecutiveApproved : StatusList.FormC1C2Init;
                    strTitle = StatusList.FormC1C2ExecutiveApproved;
                }
                else if (form.FcvihStatus == StatusList.FormC1C2ExecutiveApproved)
                {
                    if (!process.IsApprove)
                        form.FcvihSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FcvihStatus = process.IsApprove ? StatusList.FormC1C2HeadMaintenanceApproved : StatusList.FormC1C2Inspected;
                    strTitle = StatusList.FormC1C2HeadMaintenanceApproved;
                }
                else if (form.FcvihStatus == StatusList.FormC1C2HeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FcvihStatus = process.IsApprove ? StatusList.RegionManager : StatusList.FormC1C2ExecutiveApproved;
                    strTitle = StatusList.RegionManager;
                }
                else if (form.FcvihStatus == StatusList.RegionManager)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperRegionManager;
                    form.FcvihStatus = process.IsApprove ? StatusList.Completed : StatusList.FormC1C2HeadMaintenanceApproved;
                    strTitle = "Completed / Audited";
                    form.FcvihUserIdAud = !process.IsApprove ? new int?() : Utility.ToNullInt((object)process.UserID);
                    form.FcvihUserNameAud = !process.IsApprove ? (string)null : process.UserName;
                    form.FcvihUserDesignationAud = !process.IsApprove ? (string)null : process.UserDesignation;
                    form.FcvihDtAud = !process.IsApprove ? new DateTime?() : process.ApproveDate;
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();

                        if (form.FcvihSerProviderUserId.HasValue)
                            lstNotUserId.Add(form.FcvihSerProviderUserId.Value);

                        if (form.FcvihUserIdAud.HasValue)
                            lstNotUserId.Add(form.FcvihUserIdAud.Value);

                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FcvihAuditLog = Utility.ProcessLog(form.FcvihAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                string strNotMsg = (process.IsApprove ? "Approved - " : "Rejected - ") + strTitle + ":" + process.UserName + " - Form C1C2 (" + form.FcvihCInspRefNo + ")";
                string strNotURL = "/FormC1C2/View/" + form.FcvihPkRefNo.ToString();

                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormB1B2(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormB1b2BrInsHdr.Where(x => x.FbrihPkRefNo == process.RefId).FirstOrDefault();

            if (form != null)
            {
                string strTitle = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FbrihSubmitSts && form.FbrihStatus == Common.StatusList.FormB1B2Init)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FbrihStatus = process.IsApprove ? Common.StatusList.FormB1B2Inspected : StatusList.FormB1B2Init;
                    strTitle = Common.StatusList.FormB1B2Inspected;
                }
                else if (form.FbrihStatus == StatusList.FormB1B2Inspected)
                {
                    if (!process.IsApprove)
                        form.FbrihSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FbrihStatus = process.IsApprove ? StatusList.FormB1B2ExecutiveApproved : StatusList.FormB1B2Init;
                    strTitle = Common.StatusList.FormB1B2ExecutiveApproved;
                }
                else if (form.FbrihStatus == StatusList.FormB1B2ExecutiveApproved)
                {
                    if (!process.IsApprove)
                        form.FbrihSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FbrihStatus = process.IsApprove ? Common.StatusList.FormB1B2HeadMaintenanceApproved : StatusList.FormB1B2Inspected;
                    strTitle = Common.StatusList.FormB1B2HeadMaintenanceApproved;
                }
                else if (form.FbrihStatus == StatusList.FormB1B2HeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FbrihStatus = process.IsApprove ? StatusList.RegionManager : StatusList.FormB1B2ExecutiveApproved;
                    strTitle = StatusList.RegionManager;
                }
                else if (form.FbrihStatus == StatusList.RegionManager)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperRegionManager;
                    form.FbrihStatus = process.IsApprove ? StatusList.Completed : StatusList.FormB1B2HeadMaintenanceApproved;
                    strTitle = "Completed / Audited";
                    form.FbrihUserIdAud = !process.IsApprove ? new int?() : Utility.ToNullInt((object)process.UserID);
                    form.FbrihUserNameAud = !process.IsApprove ? (string)null : process.UserName;
                    form.FbrihUserDesignationAud = !process.IsApprove ? (string)null : process.UserDesignation;
                    form.FbrihDtAud = !process.IsApprove ? new DateTime?() : process.ApproveDate;

                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();

                        if (form.FbrihSerProviderUserId.HasValue)
                            lstNotUserId.Add(form.FbrihSerProviderUserId.Value);

                        if (form.FbrihUserIdAud.HasValue)
                            lstNotUserId.Add(form.FbrihUserIdAud.Value);


                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FbrihAuditLog = Utility.ProcessLog(form.FbrihAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                string strNotMsg = (process.IsApprove ? "Approved - " : "Rejected - ") + strTitle + ":" + process.UserName + " - Form B1B2 (" + form.FbrihCInspRefNo + ")";
                string strNotURL = "/FormB1B2/Add?id=" + form.FbrihPkRefNo.ToString() + "&isview=true";

                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormF2(DTO.RequestBO.ProcessDTO process)
        {

            var form = context.RmFormF2GrInsHdr.Where<RmFormF2GrInsHdr>(x => x.FgrihPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FgrihSubmitSts && form.FgrihStatus == StatusList.FormF2Init)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FgrihStatus = process.IsApprove ? StatusList.FormF2Inspected : StatusList.FormF2Init;
                    strTitle = StatusList.FormF2Inspected;
                }
                else if (form.FgrihStatus == StatusList.FormF2Inspected)
                {
                    if (!process.IsApprove)
                        form.FgrihSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FgrihStatus = process.IsApprove ? StatusList.FormF2ExecutiveApproved : StatusList.FormF2Init;
                    strTitle = StatusList.FormF2ExecutiveApproved;
                }
                else if (form.FgrihStatus == StatusList.FormF2ExecutiveApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FgrihStatus = process.IsApprove ? StatusList.FormB1B2HeadMaintenanceApproved : StatusList.FormF2Inspected;
                    strTitle = StatusList.FormB1B2HeadMaintenanceApproved;
                }
                else if (form.FgrihStatus == StatusList.FormB1B2HeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FgrihStatus = process.IsApprove ? StatusList.RegionManager : StatusList.FormF2ExecutiveApproved;
                    strTitle = StatusList.RegionManager;
                }
                else if (form.FgrihStatus == StatusList.RegionManager)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperRegionManager;
                    form.FgrihStatus = process.IsApprove ? StatusList.Completed : StatusList.FormB1B2HeadMaintenanceApproved;
                    strTitle = "Completed";
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();

                        if (form.FgrihUserIdInspBy.HasValue)
                            lstNotUserId.Add(form.FgrihUserIdInspBy.Value);
                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FgrihAuditLog = Utility.ProcessLog(form.FgrihAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                string strNotMsg = (process.IsApprove ? "Approved - " : "Rejected - ") + strTitle + ":" + process.UserName + " - Form F2 (" + form.FgrihFormRefId + ")";
                string strNotURL = "/FormF2/Add?id=" + form.FgrihPkRefNo.ToString() + "&isview=true";

                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormF4(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormF4InsHdr.Where<RmFormF4InsHdr>(x => x.FivahPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string title = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FivahSubmitSts && form.FivahStatus == StatusList.FormF4Init)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FivahStatus = process.IsApprove ? StatusList.FormF4Inspected : StatusList.FormF4Init;
                    title = StatusList.FormF4Inspected;
                }
                else if (form.FivahStatus == StatusList.FormF4Inspected)
                {
                    if (!process.IsApprove)
                        form.FivahSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FivahStatus = process.IsApprove ? StatusList.FormF4ExecutiveApproved : StatusList.FormF4Init;
                    title = StatusList.FormF4ExecutiveApproved;
                }
                else if (form.FivahStatus == StatusList.FormF4ExecutiveApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FivahStatus = process.IsApprove ? StatusList.FormF4HeadMaintenanceApproved : StatusList.FormF4Inspected;
                    title = StatusList.FormF4HeadMaintenanceApproved;
                }
                else if (form.FivahStatus == StatusList.FormF4HeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FivahStatus = process.IsApprove ? StatusList.RegionManager : StatusList.FormF4ExecutiveApproved;
                    title = StatusList.RegionManager;
                }
                else if (form.FivahStatus == StatusList.RegionManager)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperRegionManager;
                    form.FivahStatus = process.IsApprove ? StatusList.Completed : StatusList.FormF4HeadMaintenanceApproved;
                    title = "Completed";
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();

                        if (form.FivahUserIdInspBy.HasValue)
                            lstNotUserId.Add(form.FivahUserIdInspBy.Value);

                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FivahAuditLog = Utility.ProcessLog(form.FivahAuditLog, title, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                string strNotMsg = (process.IsApprove ? "Approved - " : "Rejected - ") + title + ":" + process.UserName + " - Form F4 (" + form.FivahFormRefId + ")";
                string strNotURL = "/FormF4/View/" + form.FivahPkRefNo.ToString();

                SaveNotification(new RmUserNotification
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormF5(DTO.RequestBO.ProcessDTO process)
        {

            var form = context.RmFormF5InsHdr.Where<RmFormF5InsHdr>(x => x.FvahPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FvahSubmitSts && form.FvahStatus == StatusList.FormF5Init)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FvahStatus = process.IsApprove ? StatusList.FormF5Inspected : StatusList.FormF5Init;
                    strTitle = StatusList.FormF5Inspected;
                }
                else if (form.FvahStatus == StatusList.FormF5Inspected)
                {
                    if (!process.IsApprove)
                        form.FvahSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FvahStatus = process.IsApprove ? StatusList.FormB1B2ExecutiveApproved : StatusList.FormF5Init;
                    strTitle = StatusList.FormB1B2ExecutiveApproved;
                }
                else if (form.FvahStatus == StatusList.FormB1B2ExecutiveApproved)
                {
                    if (!process.IsApprove)
                        form.FvahSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FvahStatus = process.IsApprove ? StatusList.FormF5HeadMaintenanceApproved : StatusList.FormF5Inspected;
                    strTitle = StatusList.FormF5HeadMaintenanceApproved;
                }
                else if (form.FvahStatus == StatusList.FormF5HeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FvahStatus = process.IsApprove ? StatusList.RegionManager : StatusList.FormB1B2ExecutiveApproved;
                    strTitle = StatusList.RegionManager;
                }
                else if (form.FvahStatus == StatusList.RegionManager)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperRegionManager;
                    form.FvahStatus = process.IsApprove ? StatusList.Completed : StatusList.FormF5HeadMaintenanceApproved;
                    strTitle = "Completed";
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();
                        if (form.FvahUserIdInspBy.HasValue)
                            lstNotUserId.Add(form.FvahUserIdInspBy.Value);

                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FvahAuditLog = Utility.ProcessLog(form.FvahAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                string strNotMsg = (process.IsApprove ? "Approved - " : "Rejected - ") + strTitle + ":" + process.UserName + " - Form F5 (" + form.FvahFormRefId + ")";
                string strNotURL = "/FormF5/View/" + form.FvahPkRefNo.ToString();

                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormFC(DTO.RequestBO.ProcessDTO process)
        {

            var form = context.RmFormFcInsHdr.Where<RmFormFcInsHdr>(x => x.FcihPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FcihSubmitSts && form.FcihStatus == StatusList.FormFCInit)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FcihStatus = process.IsApprove ? StatusList.FormFCInspected : StatusList.FormFCInit;
                    strTitle = StatusList.FormFCInspected;
                }
                else if (form.FcihStatus == StatusList.FormFCInspected)
                {
                    if (!process.IsApprove)
                        form.FcihSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FcihStatus = process.IsApprove ? StatusList.FormB1B2ExecutiveApproved : StatusList.FormFCInit;
                    strTitle = StatusList.FormB1B2ExecutiveApproved;
                }
                else if (form.FcihStatus == StatusList.FormB1B2ExecutiveApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FcihStatus = process.IsApprove ? StatusList.FormB1B2HeadMaintenanceApproved : StatusList.FormFCInspected;
                    strTitle = StatusList.FormB1B2HeadMaintenanceApproved;
                }
                else if (form.FcihStatus == StatusList.FormB1B2HeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FcihStatus = process.IsApprove ? StatusList.RegionManager : StatusList.FormB1B2ExecutiveApproved;
                    strTitle = StatusList.RegionManager;
                }
                else if (form.FcihStatus == StatusList.RegionManager)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperRegionManager;
                    form.FcihStatus = process.IsApprove ? StatusList.Completed : StatusList.FormB1B2HeadMaintenanceApproved;
                    strTitle = "Completed";
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();

                        if (form.FcihUserIdInspBy.HasValue)
                            lstNotUserId.Add(form.FcihUserIdInspBy.Value);

                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FcihAuditLog = Utility.ProcessLog(form.FcihAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                string strNotMsg = (process.IsApprove ? "Approved - " : "Rejected - ") + strTitle + ":" + process.UserName + " - Form FC (" + form.FcihFormRefId + ")";
                string strNotURL = "/FormFC/View/" + form.FcihPkRefNo.ToString();

                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormFD(DTO.RequestBO.ProcessDTO process)
        {

            var form = context.RmFormFdInsHdr.Where<RmFormFdInsHdr>(x => x.FdihPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string title = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FdihSubmitSts && form.FdihStatus == StatusList.FormFDInit)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperationsExecutive : GroupNames.Supervisor;
                    form.FdihStatus = process.IsApprove ? StatusList.FormFDInspected : StatusList.FormFDInit;
                    title = StatusList.FormFDInspected;
                }
                else if (form.FdihStatus == StatusList.FormFDInspected)
                {
                    if (!process.IsApprove)
                        form.FdihSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FdihStatus = process.IsApprove ? StatusList.FormFDExecutiveApproved : StatusList.FormFDInit;
                    title = StatusList.FormFDExecutiveApproved;
                }
                else if (form.FdihStatus == StatusList.FormFDExecutiveApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FdihStatus = process.IsApprove ? StatusList.FormFDHeadMaintenanceApproved : StatusList.FormFDInspected;
                    title = StatusList.FormFDHeadMaintenanceApproved;
                }
                else if (form.FdihStatus == StatusList.FormFDHeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FdihStatus = process.IsApprove ? StatusList.RegionManager : StatusList.FormFDExecutiveApproved;
                    title = StatusList.RegionManager;
                }
                else if (form.FdihStatus == StatusList.RegionManager)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperRegionManager;
                    form.FdihStatus = process.IsApprove ? StatusList.Completed : StatusList.FormFDHeadMaintenanceApproved;
                    title = "Completed";
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();

                        if (form.FdihUserIdInspBy.HasValue)
                            lstNotUserId.Add(form.FdihUserIdInspBy.Value);

                        strNotUserID = string.Join(",", lstNotUserId.Distinct());
                    }
                }
                form.FdihAuditLog = Utility.ProcessLog(form.FdihAuditLog, title, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                string strNotMsg = (process.IsApprove ? "Approved - " : "Rejected - ") + title + ":" + process.UserName + " - Form FD (" + form.FdihFormRefId + ")";
                string strNotURL = "/FormFD/View/" + form.FdihPkRefNo.ToString();

                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormFS(DTO.RequestBO.ProcessDTO process)
        {

            var form = context.RmFormFsInsHdr.Where<RmFormFsInsHdr>(x => x.FshPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                if (form.FshSubmitSts && form.FshStatus == StatusList.FormFSInit)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : "Supervisor";
                    form.FshStatus = process.IsApprove ? StatusList.FormFSSummarized : StatusList.FormFSInit;
                    strTitle = StatusList.FormFSSummarized;
                }
                else if (form.FshStatus == StatusList.FormFSSummarized)
                {
                    if (!process.IsApprove)
                        form.FshSubmitSts = false;
                    strNotGroupName = process.IsApprove ? GroupNames.OperRegionManager : GroupNames.OperationsExecutive;
                    form.FshStatus = process.IsApprove ? StatusList.FormFSHeadMaintenanceApproved : StatusList.FormFSInit;
                    strTitle = StatusList.FormFSHeadMaintenanceApproved;
                }
                else if (form.FshStatus == StatusList.FormFSHeadMaintenanceApproved)
                {
                    strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.FshStatus = process.IsApprove ? StatusList.FormFSRegionManagerApproved : StatusList.FormFSSummarized;
                    strTitle = StatusList.FormFSRegionManagerApproved;
                }
                else if (form.FshStatus == StatusList.FormFSRegionManagerApproved)
                {
                    strNotGroupName = process.IsApprove ? "" : GroupNames.OperRegionManager;
                    form.FshStatus = process.IsApprove ? StatusList.Completed : StatusList.FormFSHeadMaintenanceApproved;
                    strTitle = "Completed";
                    form.FshUserIdChckdBy = !process.IsApprove ? new int?() : Utility.ToNullInt((object)process.UserID);
                    form.FshUserNameChckdBy = !process.IsApprove ? (string)null : process.UserName;
                    form.FshUserDesignationChckdBy = !process.IsApprove ? (string)null : process.UserDesignation;
                    form.FshDtChckdBy = !process.IsApprove ? new DateTime?() : process.ApproveDate;
                    if (process.IsApprove)
                    {
                        List<int> lstNotUserId = new List<int>();

                        if (form.FshUserIdInspBy.HasValue)
                            lstNotUserId.Add(form.FshUserIdInspBy.Value);

                        if (form.FshUserIdChckdBy.HasValue)
                            lstNotUserId.Add(form.FshUserIdChckdBy.Value);

                        if (form.FshUserIdSmzdBy.HasValue)
                            lstNotUserId.Add(form.FshUserIdSmzdBy.Value);

                        strNotUserID = string.Join<int>(",", lstNotUserId.Distinct<int>());
                    }
                }
                form.FshAuditLog = Utility.ProcessLog(form.FshAuditLog, strTitle, process.IsApprove ? "Approved" : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                string strNotMsg = (process.IsApprove ? "Approved - " : "Rejected - ") + strTitle + ":" + process.UserName + " - Form FS (" + form.FshRoadName + ")";
                string strNotURL = "/FormFS/Add?id=" + form.FshPkRefNo.ToString() + "&isview=true";

                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormV1(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormV1Hdr.Where(x => x.Fv1hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.FormV1Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Fv1hStatus = process.IsApprove ? Common.StatusList.FormV1Verified : Common.StatusList.FormV1Saved;
                    strTitle = "Verified By";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.FormV1Saved;
                    form.Fv1hUseridAgr = Convert.ToInt32(process.UserID);
                    form.Fv1hUsernameAgr = process.UserName;
                    form.Fv1hDesignationAgr = process.UserDesignation;
                    form.Fv1hDtAgr = process.ApproveDate;
                    form.Fv1hSignAgr = true;
                }
                else if (process.Stage == Common.StatusList.FormV1Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.Fv1hStatus = process.IsApprove ? Common.StatusList.FormV1Approved : Common.StatusList.FormV1Submitted;
                    strTitle = "Approved By";
                    strStatus = "Facilitated";
                    strNotStatus = Common.StatusList.FormV1Verified;
                    form.Fv1hUseridAck = Convert.ToInt32(process.UserID);
                    form.Fv1hUsernameAck = process.UserName;
                    form.Fv1hDesignationAck = process.UserDesignation;
                    form.Fv1hDtAck = process.ApproveDate;
                    form.Fv1hSignAck = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();
                    if (form.Fv1hUseridAgr.HasValue)
                        lstNotUserId.Add(form.Fv1hUseridAck.Value);
                    if (form.Fv1hUseridAck.HasValue)
                        lstNotUserId.Add(form.Fv1hUseridAck.Value);

                    form.Fv1hUseridAck = Convert.ToInt32(process.UserID);
                    form.Fv1hUsernameAck = process.UserName;
                    form.Fv1hDesignationAck = process.UserDesignation;
                    form.Fv1hDtAck = process.ApproveDate;
                    form.Fv1hSignAck = true;

                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.FormV1Submitted)
                    {
                        form.Fv1hSubmitSts = false;
                    }
                }

                form.Fv1hAuditLog = Utility.ProcessLog(form.Fv1hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form V1 (" + form.Fv1hPkRefNo + ")";
                strNotURL = "/MAM/EditFormV1?id=" + form.Fv1hPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormV3(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormV3Hdr.Where(x => x.Fv3hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Fv3hStatus = process.IsApprove ? Common.StatusList.Verified : Common.StatusList.Saved;
                    strTitle = "Verified By";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.Saved;
                    form.Fv3hUseridRec = Convert.ToInt32(process.UserID);
                    form.Fv3hUsernameRec = process.UserName;
                    form.Fv3hDesignationRec = process.UserDesignation;
                    form.Fv3hDtRec = process.ApproveDate;
                    form.Fv3hSignRec = true;
                }
                else if (process.Stage == Common.StatusList.Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.Fv3hStatus = process.IsApprove ? Common.StatusList.Approved : Common.StatusList.Submitted;
                    strTitle = "Approved By";
                    strStatus = "Facilitated";
                    strNotStatus = Common.StatusList.Verified;
                    form.Fv3hUseridAgr = Convert.ToInt32(process.UserID);
                    form.Fv3hUsernameAgr = process.UserName;
                    form.Fv3hDesignationAgr = process.UserDesignation;
                    form.Fv3hDtAgr = process.ApproveDate;
                    form.Fv3hSignAgr = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();
                    if (form.Fv3hUseridAgr.HasValue)
                        lstNotUserId.Add(form.Fv3hUseridAgr.Value);
                    if (form.Fv3hUseridAgr.HasValue)
                        lstNotUserId.Add(form.Fv3hUseridAgr.Value);

                    form.Fv3hUseridAgr = Convert.ToInt32(process.UserID);
                    form.Fv3hUsernameAgr = process.UserName;
                    form.Fv3hDesignationAgr = process.UserDesignation;
                    form.Fv3hDtAgr = process.ApproveDate;
                    form.Fv3hSignAgr = true;

                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.Submitted)
                    {
                        form.Fv3hSubmitSts = false;
                    }
                }

                form.Fv3hAuditLog = Utility.ProcessLog(form.Fv3hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form V3 (" + form.Fv3hPkRefNo + ")";
                strNotURL = "/MAM/EditFormV3?id=" + form.Fv3hPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormV4(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormV4Hdr.Where(x => x.Fv4hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Fv4hStatus = process.IsApprove ? Common.StatusList.Verified : Common.StatusList.Saved;
                    strTitle = "Vetted By";
                    strStatus = "Vetted";
                    strNotStatus = Common.StatusList.Verified;
                    form.Fv4hUseridVet = Convert.ToInt32(process.UserID);
                    form.Fv4hUsernameVet = process.UserName;
                    form.Fv4hDesignationVet = process.UserDesignation;
                    form.Fv4hDtVet = process.ApproveDate;
                    form.Fv4hSignVet = true;
                }
                else if (process.Stage == Common.StatusList.Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.Fv4hStatus = process.IsApprove ? Common.StatusList.Approved : Common.StatusList.Submitted;
                    strTitle = "Agreed By";
                    strStatus = "Agreed";
                    strNotStatus = Common.StatusList.Verified;
                    form.Fv4hUseridAgr = Convert.ToInt32(process.UserID);
                    form.Fv4hUsernameAgr = process.UserName;
                    form.Fv4hDesignationAgr = process.UserDesignation;
                    form.Fv4hDtAgr = process.ApproveDate;
                    form.Fv4hSignAgr = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();
                    if (form.Fv4hUseridAgr.HasValue)
                        lstNotUserId.Add(form.Fv4hUseridAgr.Value);
                    if (form.Fv4hUseridAgr.HasValue)
                        lstNotUserId.Add(form.Fv4hUseridAgr.Value);

                    form.Fv4hUseridAgr = Convert.ToInt32(process.UserID);
                    form.Fv4hUsernameAgr = process.UserName;
                    form.Fv4hDesignationAgr = process.UserDesignation;
                    form.Fv4hDtAgr = process.ApproveDate;
                    form.Fv4hSignAgr = true;

                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.Submitted)
                    {
                        form.Fv4hSubmitSts = false;
                    }
                }

                form.Fv4hAuditLog = Utility.ProcessLog(form.Fv4hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form V4 (" + form.Fv4hPkRefNo + ")";
                strNotURL = "/MAM/EditFormV4?id=" + form.Fv4hPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormQA1(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormQa1Hdr.Where(x => x.Fqa1hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.FormQA1Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Fqa1hStatus = process.IsApprove ? Common.StatusList.FormQA1Verified : Common.StatusList.FormQA1Saved;
                    strTitle = "Executed By";
                    strStatus = "Executed";
                    strNotStatus = Common.StatusList.FormQA1Saved;
                    form.Fqa1hUseridExec = Convert.ToInt32(process.UserID);
                    form.Fqa1hUsernameExec = process.UserName;

                    form.Fqa1hDtExec = process.ApproveDate;
                    form.Fqa1hInitialExec = true;
                }
                else if (process.Stage == Common.StatusList.FormQA1Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.Fqa1hStatus = process.IsApprove ? Common.StatusList.FormQA1Approved : Common.StatusList.FormQA1Submitted;
                    strTitle = "Checked By";
                    strStatus = "Checked";
                    strNotStatus = Common.StatusList.FormQA1Verified;
                    form.Fqa1hUseridChked = Convert.ToInt32(process.UserID);
                    form.Fqa1hUsernameChked = process.UserName;

                    form.Fqa1hDtChked = process.ApproveDate;
                    form.Fqa1hInitialChked = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();
                    if (form.Fqa1hUseridExec.HasValue)
                        lstNotUserId.Add(form.Fqa1hUseridExec.Value);
                    if (form.Fqa1hUseridChked.HasValue)
                        lstNotUserId.Add(form.Fqa1hUseridChked.Value);

                    form.Fqa1hUseridExec = Convert.ToInt32(process.UserID);
                    form.Fqa1hUsernameExec = process.UserName;

                    form.Fqa1hDtExec = process.ApproveDate;
                    form.Fqa1hInitialExec = true;

                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.FormQA1Submitted)
                    {
                        form.Fqa1hSubmitSts = false;
                    }
                }

                form.Fqa1hAuditLog = Utility.ProcessLog(form.Fqa1hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form V2 (" + form.Fqa1hPkRefNo + ")";
                strNotURL = "/FormQA1/EditFormQa1?id=" + form.Fqa1hPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormF3(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormF3Hdr.Where(x => x.Ff3hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Ff3hStatus = process.IsApprove ? Common.StatusList.Verified : Common.StatusList.Saved;
                    strTitle = "Verified By";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.Saved;
                    form.Ff3hInspectedBy = Convert.ToInt32(process.UserID);
                    form.Ff3hInspectedName = process.UserName;
                    //   form.ins = process.UserDesignation;
                    form.Ff3hInspectedDate = process.ApproveDate;
                    form.Ff3hInspectedBySign = true;
                }
                else
                {
                    if (process.Stage == Common.StatusList.Submitted)
                    {
                        form.Ff3hSubmitSts = false;
                    }
                }
                form.Ff3hAuditLog = Utility.ProcessLog(form.Ff3hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form f3 (" + form.Ff3hPkRefNo + ")";
                strNotURL = "/MAM/EditFormf3?id=" + form.Ff3hPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormV2(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormV2Hdr.Where(x => x.Fv2hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.FormV2Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Fv2hStatus = process.IsApprove ? Common.StatusList.FormV2Verified : Common.StatusList.FormV2Saved;
                    strTitle = "Verified By";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.FormV2Saved;
                    form.Fv2hUseridAgr = Convert.ToInt32(process.UserID);
                    form.Fv2hUsernameAgr = process.UserName;
                    form.Fv2hDesignationAgr = process.UserDesignation;
                    form.Fv2hDtAgr = process.ApproveDate;
                    form.Fv2hSignAck = true;
                }
                else if (process.Stage == Common.StatusList.FormV2Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.Fv2hStatus = process.IsApprove ? Common.StatusList.FormV2Approved : Common.StatusList.FormV2Submitted;
                    strTitle = "Approved By";
                    strStatus = "Facilitated";
                    strNotStatus = Common.StatusList.FormV2Verified;
                    form.Fv2hUseridAck = Convert.ToInt32(process.UserID);
                    form.Fv2hUsernameAck = process.UserName;
                    form.Fv2hDesignationAck = process.UserDesignation;
                    form.Fv2hDtAck = process.ApproveDate;
                    form.Fv2hSignAck = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();
                    if (form.Fv2hUseridAgr.HasValue)
                        lstNotUserId.Add(form.Fv2hUseridAgr.Value);
                    if (form.Fv2hUseridAck.HasValue)
                        lstNotUserId.Add(form.Fv2hUseridAck.Value);

                    form.Fv2hUseridAck = Convert.ToInt32(process.UserID);
                    form.Fv2hUsernameAck = process.UserName;
                    form.Fv2hDesignationAck = process.UserDesignation;
                    form.Fv2hDtAck = process.ApproveDate;
                    form.Fv2hSignAck = true;

                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.FormV2Submitted)
                    {
                        form.Fv2hSubmitSts = false;
                    }
                }

                form.Fv2hAuditLog = Utility.ProcessLog(form.Fv2hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form V2 (" + form.Fv2hPkRefNo + ")";
                strNotURL = "/MAM/EditFormV2?id=" + form.Fv2hPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormF1(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormF1Hdr.Where(x => x.Ff1hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Ff1hStatus = process.IsApprove ? Common.StatusList.Verified : Common.StatusList.Saved;
                    strTitle = "Verified By";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.Saved;
                    form.Ff1hInspectedBy = Convert.ToInt32(process.UserID);
                    form.Ff1hInspectedName = process.UserName;
                    //   form.ins = process.UserDesignation;
                    form.Ff1hInspectedDate = process.ApproveDate;
                    form.Ff1hInspectedBySign = true;
                }
                else
                {
                    if (process.Stage == Common.StatusList.Submitted)
                    {
                        form.Ff1hSubmitSts = false;
                    }
                }
                form.Ff1hAuditLog = Utility.ProcessLog(form.Ff1hAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form f1 (" + form.Ff1hPkRefNo + ")";
                strNotURL = "/MAM/EditFormf1?id=" + form.Ff1hPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }
        private async Task<int> SaveFormT(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormTHdr.Where(x => x.FmtPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.FmtStatus = process.IsApprove ? Common.StatusList.Approved : Common.StatusList.Submitted;
                    strTitle = "Headed By";
                    strStatus = "Approved";
                    strNotStatus = Common.StatusList.Saved;
                    form.FmtUseridHdd = Convert.ToInt32(process.UserID);
                    form.FmtUsernameHdd = process.UserName;
                    form.FmtDesignationHdd = process.UserDesignation;
                    form.FmtDateHdd = process.ApproveDate;
                    form.FmtSignHdd = true;
                }
                else
                {
                    if (process.Stage == Common.StatusList.Submitted)
                    {
                        form.FmtSubmitSts = false;
                    }
                }
                form.FmtAuditLog = Utility.ProcessLog(form.FmtAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form f1 (" + form.FmtPkRefNo + ")";
                strNotURL = "/FormsT/EditFormT?id=" + form.FmtPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormM(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmFormMHdr.Where(x => x.FmhPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.FormMSubmitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.Fmhstatus = process.IsApprove ? Common.StatusList.FormMVerified : Common.StatusList.FormMSaved;
                    strTitle = "Verified By";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.FormMSaved;
                    form.FmhUseridWit = Convert.ToInt32(process.UserID);
                    form.FmhUsernameWit = process.UserName;
                    form.FmhDesignationWit = process.UserDesignation;
                    form.FmhDateWit = process.ApproveDate;
                    form.FmhSignWit = true;
                }
                else if (process.Stage == Common.StatusList.FormMVerified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.Fmhstatus = process.IsApprove ? Common.StatusList.FormMApproved : Common.StatusList.FormMSubmitted;
                    strTitle = "Witnessed By";
                    strStatus = "Witness";
                    strNotStatus = Common.StatusList.FormMVerified;
                    form.FmhUseridWitone = Convert.ToInt32(process.UserID);
                    form.FmhUsernameWitone = process.UserName;
                    form.FmhDesignationWitone = process.UserDesignation;
                    form.FmhDateWitone = process.ApproveDate;
                    form.FmhSignWitone = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();

                    if (form.FmhUseridAudit.HasValue)
                        lstNotUserId.Add(form.FmhUseridAudit.Value);
                    if (form.FmhUseridWit.HasValue)
                        lstNotUserId.Add(form.FmhUseridWit.Value);
                    if (form.FmhUseridWitone.HasValue)
                        lstNotUserId.Add(form.FmhUseridWitone.Value);

                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.FormMSubmitted)
                    {
                        form.FmhSubmitSts = false;
                    }
                }

                form.FmhAuditLog = Utility.ProcessLog(form.FmhAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form R1R2 (" + form.FmhPkRefNo + ")";
                strNotURL = "/FormM/Edit/" + form.FmhPkRefNo.ToString() + "?View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormB15(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmB15Hdr.Where(x => x.B15hPkRefNo== process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.FormB15Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.B15hStatus = process.IsApprove ? Common.StatusList.FormB15Verified : Common.StatusList.FormB15Saved;
                    strTitle = "Verified (EC)";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.FormB15Saved;
                    form.B15hUseridProsd = form.B15hUseridProsd;
                    form.B15hUseridFclitd = Convert.ToInt32(process.UserID);
                    form.B15hUserNameFclitd = process.UserName;
                    form.B15hUserDesignationFclitd = process.UserDesignation;
                    form.B15hDtFclitd = process.ApproveDate;
                    form.B15hSignFclitd = true;
                }
                else if (process.Stage == Common.StatusList.FormB15Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.B15hStatus = process.IsApprove ? Common.StatusList.FormB15Agreed : Common.StatusList.FormB15Submitted;
                    strTitle = "Verified (JKRS)";
                    strStatus = "Agreed";
                    strNotStatus = Common.StatusList.FormB15Verified;
                    form.B15hUserNameFclitd = form.B15hUserNameFclitd;
                    form.B15hUseridAgrd = Convert.ToInt32(process.UserID);
                    form.B15hUserNameAgrd = process.UserName;
                    form.B15hUserDesignationAgrd = process.UserDesignation;
                    form.B15hDtAgrd = process.ApproveDate;
                    form.B15hSignAgrd = true;
                }
                else if (process.Stage == Common.StatusList.FormB15Agreed)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.B15hStatus = process.IsApprove ? Common.StatusList.FormB15Approved : Common.StatusList.FormB15Submitted;
                    strTitle = "Approved (Endrosed By)";
                    strStatus = "Approved";
                    strNotStatus = Common.StatusList.FormB15Agreed;
                    form.B15hUseridEndosd = Convert.ToInt32(process.UserID);
                    form.B15hUserNameEndosd = process.UserName;
                    form.B15hUserDesignationEndosd = process.UserDesignation;
                    form.B15hDtEndosd = process.ApproveDate;
                    form.B15hSignEndosd = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();

                    if (form.B15hUseridProsd.HasValue)
                        lstNotUserId.Add(form.B15hUseridProsd.Value);
                    if (form.B15hUseridFclitd.HasValue)
                        lstNotUserId.Add(form.B15hUseridFclitd.Value);
                    if (form.B15hUseridAgrd.HasValue)
                        lstNotUserId.Add(form.B15hUseridAgrd.Value);
                    if (form.B15hUseridEndosd.HasValue)
                        lstNotUserId.Add(form.B15hUseridEndosd.Value);

                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.FormB15Submitted)
                    {
                        form.B15hSubmitSts = false;
                    }
                }

                form.B15hAuditlog = Utility.ProcessLog(form.B15hAuditlog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form B15 (" + form.B15hPkRefNo + ")";
                strNotURL = "/FormB15/Edit/" + form.B15hPkRefNo.ToString() + "?View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormB13(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmB13ProposedPlannedBudget.Where(x => x.B13pPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";


                if (process.Stage == Common.StatusList.Submitted)
                {
                    form.B13pStatus = process.IsApprove ? Common.StatusList.Facilitated : Common.StatusList.Saved;
                    strTitle = "Facilitated by";
                    strStatus = "Facilitated";
                    form.B13pUseridFclitd = Convert.ToInt32(process.UserID);
                    form.B13pUserNameFclitd = process.UserName;
                    form.B13pUserDesignationFclitd = process.UserDesignation;
                    form.B13pDtFclitd = process.ApproveDate;
                    form.B13pSignFclitd = true;
                }
                else if (process.Stage == Common.StatusList.Facilitated)
                {
                    form.B13pStatus = process.IsApprove ? Common.StatusList.Agreed : Common.StatusList.Saved;
                    strTitle = "Agreed by";
                    strStatus = "Agreed";
                    form.B13pUseridAgrd = Convert.ToInt32(process.UserID);
                    form.B13pUserNameAgrd = process.UserName;
                    form.B13pUserDesignationAgrd = process.UserDesignation;
                    form.B13pDtAgrd = process.ApproveDate;
                    form.B13pSignAgrd = true;
                }
                else if (process.Stage == Common.StatusList.Agreed)
                {
                    form.B13pStatus = process.IsApprove ? Common.StatusList.Endorsed : Common.StatusList.Saved;
                    strTitle = "Endorsed by";
                    strStatus = "Endorsed";
                    form.B13pUseridEdosd = Convert.ToInt32(process.UserID);
                    form.B13pUserNameEdosd = process.UserName;
                    form.B13pUserDesignationEdosd = process.UserDesignation;
                    form.B13pDtEdosd = process.ApproveDate;
                    form.B13pSignEdosd = true;
                }

                if (!process.IsApprove)
                {
                    if (process.Stage == Common.StatusList.Submitted)
                    {
                        form.B13pSubmitSts = false;
                    }
                }

                form.B13pAuditLog = Utility.ProcessLog(form.B13pAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form B13 (" + form.B13pPkRefNo + ")";
                strNotURL = "/FormB13/Add?id=" + form.B13pPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }

        private async Task<int> SaveFormB14(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmB14Hdr.Where(x => x.B14hPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";
                string strNotStatus = "";

                if (process.Stage == Common.StatusList.FormB14Submitted)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.OpeHeadMaintenance : GroupNames.Supervisor;
                    form.B14hStatus = process.IsApprove ? Common.StatusList.FormB14Verified : Common.StatusList.FormB14Saved;
                    strTitle = "Verified (EC)";
                    strStatus = "Verified";
                    strNotStatus = Common.StatusList.FormB14Saved;
                    form.B14hUseridProsd = form.B14hUseridProsd;
                    form.B14hUseridFclitd = Convert.ToInt32(process.UserID);
                    form.B14hUserNameFclitd = process.UserName;
                    form.B14hUserDesignationFclitd = process.UserDesignation;
                    form.B14hDtFclitd = process.ApproveDate;
                    form.B14hSignFclitd = true;
                }
                else if (process.Stage == Common.StatusList.FormB14Verified)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.B14hStatus = process.IsApprove ? Common.StatusList.FormB14Agreed : Common.StatusList.FormB14Submitted;
                    strTitle = "Verified (JKRS)";
                    strStatus = "Agreed";
                    strNotStatus = Common.StatusList.FormB14Verified;
                    form.B14hUserNameFclitd = form.B14hUserNameFclitd;
                    form.B14hUseridAgrd = Convert.ToInt32(process.UserID);
                    form.B14hUserNameAgrd = process.UserName;
                    form.B14hUserDesignationAgrd = process.UserDesignation;
                    form.B14hDtAgrd = process.ApproveDate;
                    form.B14hSignAgrd = true;
                }
                else if (process.Stage == Common.StatusList.FormB14Agreed)
                {
                    //strNotGroupName = process.IsApprove ? GroupNames.JKRSSuperiorOfficerSO : GroupNames.OpeHeadMaintenance;
                    form.B14hStatus = process.IsApprove ? Common.StatusList.FormB14Approved : Common.StatusList.FormB14Submitted;
                    strTitle = "Approved (Endrosed By)";
                    strStatus = "Approved";
                    strNotStatus = Common.StatusList.FormB14Agreed;
                    form.B14hUseridEndosd = Convert.ToInt32(process.UserID);
                    form.B14hUserNameEndosd = process.UserName;
                    form.B14hUserDesignationEndosd = process.UserDesignation;
                    form.B14hDtEndosd = process.ApproveDate;
                    form.B14hSignEndosd = true;
                }

                if (process.IsApprove)
                {
                    List<int> lstNotUserId = new List<int>();

                    if (form.B14hUseridProsd.HasValue)
                        lstNotUserId.Add(form.B14hUseridProsd.Value);
                    if (form.B14hUseridFclitd.HasValue)
                        lstNotUserId.Add(form.B14hUseridFclitd.Value);
                    if (form.B14hUseridAgrd.HasValue)
                        lstNotUserId.Add(form.B14hUseridAgrd.Value);
                    if (form.B14hUseridEndosd.HasValue)
                        lstNotUserId.Add(form.B14hUseridEndosd.Value);

                    strNotUserID = string.Join(",", lstNotUserId.Distinct());
                }
                else
                {
                    if (process.Stage == Common.StatusList.FormB14Submitted)
                    {
                        form.B14hSubmitSts = false;
                    }
                }

                form.B14hAuditlog = Utility.ProcessLog(form.B14hAuditlog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form B14 (" + form.B14hPkRefNo + ")";
                strNotURL = "/FormB14/Edit/" + form.B14hPkRefNo.ToString() + "?View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }



        private async Task<int> SaveFormPA(DTO.RequestBO.ProcessDTO process)
        {
            var form = context.RmPaymentCertificateMamw.Where(x => x.PcmamwPkRefNo == process.RefId).FirstOrDefault();
            if (form != null)
            {
                string strTitle = "";
                string strNotURL = "";
                string strNotMsg = "";
                string strNotGroupName = "";
                string strNotUserID = "";
                string strStatus = "";


                if (process.Stage == Common.StatusList.Submitted)
                {
                    form.PcmamwStatus = process.IsApprove ? Common.StatusList.CertifiedbyEC : Common.StatusList.Saved;
                    strTitle = "Certified by EC";
                    strStatus = "Certified by EC";
                    form.PcmamwUseridEc = Convert.ToInt32(process.UserID);
                    form.PcmamwUsernameEc = process.UserName;
                    form.PcmamwDesignationEc = process.UserDesignation;
                    form.PcmamwSignDateEc = process.ApproveDate;
                    form.PcmamwSignEc = true;
                }
                else if (process.Stage == Common.StatusList.CertifiedbyEC)
                {
                    form.PcmamwStatus = process.IsApprove ? Common.StatusList.CertifiedbySO : Common.StatusList.Saved;
                    strTitle = "Certified by SO";
                    strStatus = "Certified by SO";
                    form.PcmamwUseridSo = Convert.ToInt32(process.UserID);
                    form.PcmamwUsernameSo = process.UserName;
                    form.PcmamwDesignationSo = process.UserDesignation;
                    form.PcmamwSignDateSo = process.ApproveDate;
                    form.PcmamwSignSo = true;
                }
                

                if (!process.IsApprove)
                {
                    if (process.Stage == Common.StatusList.Submitted)
                    {
                        form.PcmamwSubmitSts = false;
                    }
                }

                form.PcmamwAuditLog = Utility.ProcessLog(form.PcmamwAuditLog, strTitle, process.IsApprove ? strStatus : "Rejected", process.UserName, process.Remarks, process.ApproveDate, security.UserName);
                strNotMsg = (process.IsApprove ? "" : "Rejected - ") + strTitle + ":" + process.UserName + " - Form B13 (" + form.PcmamwPkRefNo + ")";
                strNotURL = "/FormPA/Add?id=" + form.PcmamwPkRefNo.ToString() + "&View=0";
                SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = security.UserName,
                    RmNotGroup = strNotGroupName,
                    RmNotMessage = strNotMsg,
                    RmNotOn = DateTime.Now,
                    RmNotUrl = strNotURL,
                    RmNotUserId = strNotUserID,
                    RmNotViewed = ""
                }, false);
            }
            return await context.SaveChangesAsync();
        }


    }
}
