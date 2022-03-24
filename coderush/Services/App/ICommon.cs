using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.ViewModels;

namespace coderush.Services.App
{
    public interface ICommon
    {
        Task GenerateSalaryByEmployeeByPeriod(string employeeId, string period, bool isApproved, bool isPaid);

        IEnumerable<SelectListItem> GetEmployeeSelectList();

        IEnumerable<SelectListItem> GetDesignationSelectList();

        IEnumerable<SelectListItem> GetDepartmentSelectList();

        IEnumerable<SelectListItem> GetSystemUserSelectList();

        IEnumerable<SelectListItem> GetGenderSelectList();

        IEnumerable<SelectListItem> GetMaritalStatusSelectList();

        IEnumerable<SelectListItem> GetTicketTypeSelectList();

        IEnumerable<SelectListItem> GetTicketSelectList();

        IEnumerable<SelectListItem> GetTicketSelectListByEmployeeId(string employeeId);

        IEnumerable<SelectListItem> GetAppraisalTypeSelectList();

        IEnumerable<SelectListItem> GetAssetTypeSelectList();

        IEnumerable<SelectListItem> GetInformationTypeSelectList();

        IEnumerable<SelectListItem> GetAwardTypeSelectList();

        IEnumerable<SelectListItem> GetLeaveTypeSelectList();

        IEnumerable<SelectListItem> GetLeaveSelectList();

        IEnumerable<SelectListItem> GetExpenseTypeSelectList();

        IEnumerable<SelectListItem> GetTodoTypeSelectList();

        IEnumerable<SelectListItem> GetAllowanceTypeSelectList();

        IEnumerable<SelectListItem> GetDeductionTypeSelectList();

        IEnumerable<SelectListItem> GetBenefitTemplateSelectList();

        TodoSummary GetTodoSummaryByPeriod(string period);
        
        EmployeeSummary GetEmployeeSummary();

        InformationSummary GetInformationSummaryByPeriod(string period);

        AwardSummary GetAwardSummaryByPeriod(string period);

        TicketSummary GetTicketSummaryByPeriod(string period);

        AppraisalSummary GetAppraisalSummaryByPeriod(string period);

        AssetSummary GetAssetSummaryByPeriod(string period);

        ExpenseSummary GetExpenseSummaryByPeriod(string period);

        PayrollSummary GetPayrollSummaryByPeriod(string period);

        PayrollSummary GetPayrollSummaryByPeriodByEmployeeId(string employeeId, string period);

        ChartDoughnut GetPayrollDoughnutByPeriodByEmployeeId(string employeeId, string period);

        AttendanceSummary GetAttendanceSummaryByPeriod(string period);

        LeaveSummary GetLeaveSummaryByPeriod(string period);

        SelfServiceAttendanceSummary GetAttendanceSummaryByPeriodByEmployeeId(string period, string employeeId);

        SelfServiceLeaveSummary GetLeaveSummaryByPeriodByEmployeeId(string period, string employeeId);

        SelfServiceExpenseSummary GetExpenseSummaryByPeriodByEmployeeId(string period, string employeeId);

        SelfServiceTicketSummary GetTicketSummaryByPeriodByEmployeeId(string period, string employeeId);

        ChartDoughnut GetTicketDoughnutByPeriodByEmployeeId(string period, string employeeId);

        ChartDoughnut GetExenseDoughnutByPeriodByEmployeeId(string period, string employeeId);

        ChartDoughnut GetTodoDoughnutByPeriod(string period);

        ChartDoughnut GetLeaveDoughnutByPeriod(string period);

        ChartDoughnut GetAwardDoughnutByPeriod(string period);

        ChartDoughnut GetInformationDoughnutByPeriod(string period);

        ChartDoughnut GetAssetDoughnutByPeriod(string period);

        ChartDoughnut GetExpenseDoughnutByPeriod(string period);

        ChartDoughnut GetAppraisalDoughnutByPeriod(string period);

        ChartDoughnut GetTicketDoughnutByPeriod(string period);

        ChartDoughnut GetAttendanceDoughnutByPeriod(string period);

        ChartDoughnut GetEmployeeDoughnut();

        ChartDoughnut GetSelfServiceDoughnutByPeriod(string period, string employeeId);

        ChartDoughnut GetHolidayVsWorkdayDoughnutByPeriod(string period);

        ChartDoughnut GetLeaveDoughnutByPeriodByEmployeeId(string period, string employeeId);
    }

}
