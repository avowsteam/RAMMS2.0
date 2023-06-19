using Microsoft.AspNetCore.Mvc;
using RAMMS.Business.ServiceProvider;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.DTO;
using RAMMS.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Web.UI.Controllers
{
    [CAuthorize]
    public class ProcessController : Models.BaseController
    {
        private readonly IProcessService _processService;
        private readonly IUserService userService;
        private readonly ISecurity _security;
        private readonly IMailService _mailService;
        private readonly IFormDService _formDService;
        public ProcessController(IProcessService processService, IUserService userSer, ISecurity security, IMailService mailService, IFormDService formDService)
        {
            _processService = processService;
            userService = userSer;
            _security = security;
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _formDService = formDService ?? throw new ArgumentNullException(nameof(formDService));
        }
        public IActionResult ViewApprove(string group)
        {
            if (group == "admin") group = "User";
            ViewBag.GroupName = group;
            base.LoadLookupService(group);
            var user = (List<CSelectListItem>)ViewData[group];
            if (user != null)
            {
                var cs = user.Find(c => c.Value == _security.UserID.ToString());
                if (cs != null)
                    cs.Selected = true;
            }
            return PartialView("~/Views/Process/ViewApprove.cshtml");
        }
        public async Task<JsonResult> Save(DTO.RequestBO.ProcessDTO process)
        {
            Common.Result<int> result = new Common.Result<int>();
            try
            {
                await _processService.Save(process);

                #region Email

                if (process.Stage == Common.StatusList.Executive || process.Stage == Common.StatusList.HeadMaintenance)
                {
                    var response = await _processService.GetReferenceId(process);
                    if (response != null && !string.IsNullOrEmpty(process.UserID) && process.Form == "FormD" && process.IsApprove)
                    {
                        string emails = "";
                        string subject = "eRAMS: Form " + response.RefNo + " has been " + response.Title.ToLower() + " for your review/approval";
                        if (response.Title.ToLower() == "vetted")
                        {
                            subject = "eRAMS: Form " + response.RefNo + " has been " + response.Title.ToLower();
                            emails = await _formDService.GetUserEmailIds(Convert.ToInt32(response.VerifierId),true);

                            var selectedUserEmail = await _formDService.GetUserEmailIds(Convert.ToInt32(process.UserID),false);
                            if (!string.IsNullOrEmpty(emails))
                            {
                                emails += "," + selectedUserEmail;
                            }
                            else
                            {
                                emails = selectedUserEmail;
                            }                           
                        }
                        else
                        {
                            emails = await _formDService.GetUserEmailIds(Convert.ToInt32(process.UserID),true);
                        }

                        if (response.SubmittedByUserId > 0)
                        {
                            var userEmails = await _formDService.GetUserEmailIds(Convert.ToInt32(response.SubmittedByUserId),true);
                            if (!string.IsNullOrEmpty(userEmails))
                            {
                                if (!string.IsNullOrEmpty(emails))
                                    emails += "," + userEmails;
                                else
                                    emails = userEmails;
                            }
                        }

                        List<string> uniques = emails.Split(',').Reverse().Distinct().Reverse().ToList();
                        emails = string.Join(",", uniques);

                        if (!string.IsNullOrEmpty(emails))
                        {
                            var location = new Uri($"{Request.Scheme}://{Request.Host}");
                            var eRAMSLink = location.AbsoluteUri;

                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.AppendFormat(@"Dear, {0} {1}", "<br>", "<br>");
                            stringBuilder.AppendFormat("Form {0} : Form System {1}", response.RefNo, "<br><br>");
                            stringBuilder.AppendFormat("Reference No. {0} {1}", response.RefNo, "<br><br>");
                            stringBuilder.AppendFormat("{0} on {1} {2}", response.Title, DateTime.Now.ToString("dd/MM/yyyy"), "<br><br>");
                            stringBuilder.AppendFormat("Access this task in the <a href={0}>eRAMS Link</a>", eRAMSLink);

                            stringBuilder.AppendFormat("{0}{1}Thank you.", "<br>", "<br>");
                            stringBuilder.AppendFormat("{0}{1}Regards,", "<br>", "<br>");

                            var request = new MailRequestDto();
                            request.PrepareRequest(emails, subject, stringBuilder.ToString());
                            await _mailService.SendEmailAsync(request);
                        }
                    }
                }

                #endregion

                result.Message = "Sucessfully Saved";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return Json(result, base.JsonOption());
        }
        public async Task<JsonResult> GetNotification(int Count, DateTime? from)
        {
            return Json(await userService.GetUserNotification(Count, from), JsonOption());
        }
    }
}
