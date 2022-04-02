using System;
using System.Collections.Generic;
using System.Text;
using coderush.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace coderush.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //custom entity, override identity user with new column
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        //custom entity, for simple todo app
        public DbSet<Todo> Todo { get; set; }
        //custom entity, for log
        public DbSet<Log> Log { get; set; }

        ///HRM
        ///

        public DbSet<Designation> Designation { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Leave> Leave { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<AllowanceType> AllowanceType { get; set; }
        public DbSet<Asset> Asset { get; set; }
        public DbSet<AssetType> AssetType { get; set; }
        public DbSet<DeductionType> DeductionType { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<ExpenseType> ExpenseType { get; set; }
        public DbSet<ThirdParty> ThirdParty { get; set; }
        public DbSet<JobVacancy> JobVacancy { get; set; }
        public DbSet<OnBoarding> OnBoarding { get; set; }
        public DbSet<Resignation> Resignation { get; set; }
        public DbSet<Layoff> Layoff { get; set; }
        public DbSet<Applicant> Applicant { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Award> Award { get; set; }
        public DbSet<AwardType> AwardType { get; set; }
        public DbSet<Information> Information { get; set; }
        public DbSet<InformationType> InformationType { get; set; }
        public DbSet<Appraisal> Appraisal { get; set; }
        public DbSet<AppraisalType> AppraisalType { get; set; }
        public DbSet<TodoType> TodoType { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<PublicHoliday> PublicHoliday { get; set; }
        public DbSet<PublicHolidayLine> PublicHolidayLine { get; set; }
        public DbSet<BenefitTemplate> BenefitTemplate { get; set; }
        public DbSet<BenefitTemplateLine> BenefitTemplateLine { get; set; }
        public DbSet<Payroll> Payroll { get; set; }
        public DbSet<PayrollLineBasic> PayrollLineBasic { get; set; }
        public DbSet<PayrollLineAllowance> PayrollLineAllowance { get; set; }
        public DbSet<PayrollLineDeduction> PayrollLineDeduction { get; set; }
        public DbSet<PayrollLineCashAdvance> PayrollLineCashAdvance { get; set; }
        public DbSet<PayrollLineReimburse> PayrollLineReimburse { get; set; }
        public DbSet<PayrollLineUnpaidLeave> PayrollLineUnpaidLeave { get; set; }
        public DbSet<JobPost> JobPost { get; set; }
        public DbSet<JobApplication> JobApplication { get; set; }

    }
}
