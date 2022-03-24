using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    //view model for changeroles screen
    public class ChangeRoles
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsTodoRegistered { get; set; }
        public bool IsMembershipRegistered { get; set; }
        public bool IsRoleRegistered { get; set; }
        public bool IsSelfServiceRegistered { get; set; }
        public bool IsEmployeeRegistered { get; set; }
        public bool IsRecruitmentRegistered { get; set; }
        public bool IsAttendanceRegistered { get; set; }
        public bool IsLeaveRegistered { get; set; }
        public bool IsAwardRegistered { get; set; }
        public bool IsInformationRegistered { get; set; }
        public bool IsAssetRegistered { get; set; }
        public bool IsExpenseRegistered { get; set; }
        public bool IsPayrollRegistered { get; set; }
        public bool IsAppraisalRegistered { get; set; }
        public bool IsTicketRegistered { get; set; }
        public bool IsSettingsRegistered { get; set; }
    }
}
