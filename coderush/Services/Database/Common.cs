using coderush.Data;
using coderush.Models;
using coderush.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Services.Database
{
    //special service provided for db initialization / data seed
    public class Common : ICommon
    {
        private readonly ApplicationDbContext _context;
        private readonly Security.ICommon _security;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly App.ICommon _app;

        public Common(ApplicationDbContext context, 
            Security.ICommon security,
            RoleManager<IdentityRole> roleManager,
            IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
            App.ICommon app,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _security = security;
            _userManager = userManager;
            _roleManager = roleManager;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _app = app;
        }

        public async Task Initialize()
        {
            try
            {
                _context.Database.EnsureCreated();

                //check for users
                if (_context.ApplicationUser.Any())
                {
                    return; //if user is not empty, DB has been seed
                }

                //init app with super admin user
                await _security.CreateDefaultSuperAdmin();

                //init app with demo data
                await InsertDemoData();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task InsertDemoData()
        {
            try
            {
                //get super admin user
                ApplicationUser superAdmin = new ApplicationUser();
                superAdmin = await _userManager.FindByEmailAsync(_superAdminDefaultOptions.Email);

                //insert leave type
                LeaveType familyAndMedicalLeave = new LeaveType() {
                    LeaveTypeId = Guid.NewGuid().ToString(),
                    Name = "Family and Medical Leave",
                    Description = ""
                };
                LeaveType militaryLeave = new LeaveType()
                {
                    LeaveTypeId = Guid.NewGuid().ToString(),
                    Name = "Military Leave",
                    Description = ""
                };
                LeaveType pregnancyLeave = new LeaveType()
                {
                    LeaveTypeId = Guid.NewGuid().ToString(),
                    Name = "Pregnancy and Maternity Leave",
                    Description = ""
                };
                LeaveType sickLeave = new LeaveType()
                {
                    LeaveTypeId = Guid.NewGuid().ToString(),
                    Name = "Sick Leave",
                    Description = ""
                };
                LeaveType personalLeave = new LeaveType()
                {
                    LeaveTypeId = Guid.NewGuid().ToString(),
                    Name = "Personal Leave",
                    Description = ""
                };
                LeaveType vacationLeave = new LeaveType()
                {
                    LeaveTypeId = Guid.NewGuid().ToString(),
                    Name = "Vacation Leave",
                    Description = ""
                };

                await _context.LeaveType.AddAsync(familyAndMedicalLeave);
                await _context.LeaveType.AddAsync(militaryLeave);
                await _context.LeaveType.AddAsync(pregnancyLeave);
                await _context.LeaveType.AddAsync(sickLeave);
                await _context.LeaveType.AddAsync(personalLeave);
                await _context.LeaveType.AddAsync(vacationLeave);

                await _context.SaveChangesAsync();

                //insert award type
                AwardType topPerformerAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Top Performer",
                    Description = "Top Performer Recognition"
                };
                AwardType customerServiceAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Customer Service",
                    Description = "Customer Service Awards"
                };
                AwardType aboveAndBeyondAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Above Beyond",
                    Description = "Above and Beyond Recognition"
                };
                AwardType peerToPeerAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Peer-to-Peer",
                    Description = "Peer-to-Peer Recognition"
                };
                AwardType perfectAttendanceAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Perfect Attendance",
                    Description = "Perfect Attendance Programs"
                };
                AwardType welcomeAboardAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Welcome Aboard",
                    Description = "Welcome Aboard Recognition"
                };
                AwardType salesIncentiveAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Incentives",
                    Description = "Sales Incentives"
                };
                AwardType safetyAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Awards",
                    Description = "Safety Awards"
                };
                AwardType retirementAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Retirement",
                    Description = "Retirement Awards"
                };
                AwardType volunteerAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Volunteer",
                    Description = "Volunteer Awards"
                };
                AwardType teamAward = new AwardType()
                {
                    AwardTypeId = Guid.NewGuid().ToString(),
                    Name = "Team",
                    Description = "Team Recognition"
                };

                await _context.AwardType.AddAsync(topPerformerAward);
                await _context.AwardType.AddAsync(customerServiceAward);
                await _context.AwardType.AddAsync(aboveAndBeyondAward);
                await _context.AwardType.AddAsync(peerToPeerAward);
                await _context.AwardType.AddAsync(perfectAttendanceAward);
                await _context.AwardType.AddAsync(welcomeAboardAward);
                await _context.AwardType.AddAsync(salesIncentiveAward);
                await _context.AwardType.AddAsync(safetyAward);
                await _context.AwardType.AddAsync(retirementAward);
                await _context.AwardType.AddAsync(volunteerAward);
                await _context.AwardType.AddAsync(teamAward);

                await _context.SaveChangesAsync();

                //insert information type
                InformationType annualReportInformation = new InformationType()
                {
                    InformationTypeId = Guid.NewGuid().ToString(),
                    Name = "Annual Reports",
                    Description = ""
                };
                InformationType manualInformation = new InformationType()
                {
                    InformationTypeId = Guid.NewGuid().ToString(),
                    Name = "Manual and Handbooks",
                    Description = ""
                };
                InformationType newsletterInformation = new InformationType()
                {
                    InformationTypeId = Guid.NewGuid().ToString(),
                    Name = "Newsletters",
                    Description = ""
                };
                InformationType trainingInformation = new InformationType()
                {
                    InformationTypeId = Guid.NewGuid().ToString(),
                    Name = "Trainings and Seminars",
                    Description = ""
                };
                InformationType whitePapperInformation = new InformationType()
                {
                    InformationTypeId = Guid.NewGuid().ToString(),
                    Name = "White Pappers",
                    Description = ""
                };
                InformationType caseStudyInformation = new InformationType()
                {
                    InformationTypeId = Guid.NewGuid().ToString(),
                    Name = "Case Studies",
                    Description = ""
                };
                InformationType blogInformation = new InformationType()
                {
                    InformationTypeId = Guid.NewGuid().ToString(),
                    Name = "Blogs",
                    Description = ""
                };
                InformationType salesMaterialInformation = new InformationType()
                {
                    InformationTypeId = Guid.NewGuid().ToString(),
                    Name = "Brochures and Printed Sales Materials",
                    Description = ""
                };

                await _context.InformationType.AddAsync(annualReportInformation);
                await _context.InformationType.AddAsync(manualInformation);
                await _context.InformationType.AddAsync(newsletterInformation);
                await _context.InformationType.AddAsync(trainingInformation);
                await _context.InformationType.AddAsync(whitePapperInformation);
                await _context.InformationType.AddAsync(caseStudyInformation);
                await _context.InformationType.AddAsync(blogInformation);
                await _context.InformationType.AddAsync(salesMaterialInformation);

                await _context.SaveChangesAsync();

                //insert asset type
                AssetType ppeAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "PPE",
                    Description = "(Property, Plant, and Equipment)"
                };
                AssetType inventoryAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Inventory",
                    Description = ""
                };
                AssetType landAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Land",
                    Description = ""
                };
                AssetType buildingAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Buildings",
                    Description = ""
                };
                AssetType vehicleAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Vehicles",
                    Description = ""
                };
                AssetType furnitureAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Furniture",
                    Description = ""
                };
                AssetType patentAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Patents",
                    Description = ""
                };
                AssetType machineryAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Machinery",
                    Description = ""
                };
                AssetType equipmentAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Equipment",
                    Description = ""
                };
                AssetType laptopAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Laptop",
                    Description = ""
                };
                AssetType pcAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "PC",
                    Description = ""
                };
                AssetType tvAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "TV",
                    Description = ""
                };
                AssetType infocusAsset = new AssetType()
                {
                    AssetTypeId = Guid.NewGuid().ToString(),
                    Name = "Infocus",
                    Description = ""
                };

                await _context.AssetType.AddAsync(ppeAsset);
                await _context.AssetType.AddAsync(inventoryAsset);
                await _context.AssetType.AddAsync(landAsset);
                await _context.AssetType.AddAsync(buildingAsset);
                await _context.AssetType.AddAsync(vehicleAsset);
                await _context.AssetType.AddAsync(furnitureAsset);
                await _context.AssetType.AddAsync(patentAsset);
                await _context.AssetType.AddAsync(machineryAsset);
                await _context.AssetType.AddAsync(equipmentAsset);
                await _context.AssetType.AddAsync(laptopAsset);
                await _context.AssetType.AddAsync(pcAsset);
                await _context.AssetType.AddAsync(tvAsset);
                await _context.AssetType.AddAsync(infocusAsset);

                await _context.SaveChangesAsync();

                //insert expense type
                ExpenseType marketingExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Marketing",
                    Description = ""
                };
                ExpenseType advertisingExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Advertising",
                    Description = ""
                };
                ExpenseType promotionExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Promotion",
                    Description = ""
                };
                ExpenseType trainingExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Training",
                    Description = ""
                };
                ExpenseType seminarExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Seminar",
                    Description = ""
                };
                ExpenseType projectExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Project",
                    Description = ""
                };
                ExpenseType transportationExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Transportation",
                    Description = ""
                };
                ExpenseType accomodationExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Accomodation",
                    Description = ""
                };
                ExpenseType mealExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Meal",
                    Description = ""
                };
                ExpenseType medicalExpense = new ExpenseType()
                {
                    ExpenseTypeId = Guid.NewGuid().ToString(),
                    Name = "Medical",
                    Description = ""
                };

                await _context.ExpenseType.AddAsync(marketingExpense);
                await _context.ExpenseType.AddAsync(advertisingExpense);
                await _context.ExpenseType.AddAsync(promotionExpense);
                await _context.ExpenseType.AddAsync(trainingExpense);
                await _context.ExpenseType.AddAsync(seminarExpense);
                await _context.ExpenseType.AddAsync(projectExpense);
                await _context.ExpenseType.AddAsync(transportationExpense);
                await _context.ExpenseType.AddAsync(accomodationExpense);
                await _context.ExpenseType.AddAsync(mealExpense);
                await _context.ExpenseType.AddAsync(medicalExpense);

                await _context.SaveChangesAsync();
                


                //insert allowance type
                AllowanceType housingAllowance = new AllowanceType()
                {
                    AllowanceTypeId = Guid.NewGuid().ToString(),
                    Name = "Housing", 
                    Description = "Housing Allowance"
                };
                AllowanceType schoolAllowance = new AllowanceType()
                {
                    AllowanceTypeId = Guid.NewGuid().ToString(),
                    Name = "School",
                    Description = "Children School Allowance"
                };
                AllowanceType projectAllowance = new AllowanceType()
                {
                    AllowanceTypeId = Guid.NewGuid().ToString(),
                    Name = "Project",
                    Description = "Project Allowance"
                };
                AllowanceType entertainmentAllowance = new AllowanceType()
                {
                    AllowanceTypeId = Guid.NewGuid().ToString(),
                    Name = "Entertainment",
                    Description = "Entertainment Allowance"
                };
                AllowanceType taxAllowance = new AllowanceType()
                {
                    AllowanceTypeId = Guid.NewGuid().ToString(),
                    Name = "Tax",
                    Description = "Tax Allowance"
                };
                AllowanceType healthInsuranceAllowance = new AllowanceType()
                {
                    AllowanceTypeId = Guid.NewGuid().ToString(),
                    Name = "Health Insurance",
                    Description = "Health Insurance Allowance"
                };

                await _context.AllowanceType.AddAsync(housingAllowance);
                await _context.AllowanceType.AddAsync(schoolAllowance);
                await _context.AllowanceType.AddAsync(projectAllowance);
                await _context.AllowanceType.AddAsync(entertainmentAllowance);
                await _context.AllowanceType.AddAsync(taxAllowance);
                await _context.AllowanceType.AddAsync(healthInsuranceAllowance);
                await _context.SaveChangesAsync();

                //insert deduction
                DeductionType taxDeduction = new DeductionType()
                {
                    DeductionTypeId = Guid.NewGuid().ToString(),
                    Name = "Tax",
                    Description = "Tax Deduction"
                };
                DeductionType healthInsuranceDeduction = new DeductionType()
                {
                    DeductionTypeId = Guid.NewGuid().ToString(),
                    Name = "Health Insurance",
                    Description = "Health Insurance"
                };

                await _context.DeductionType.AddAsync(taxDeduction);
                await _context.DeductionType.AddAsync(healthInsuranceDeduction);
                await _context.SaveChangesAsync();

                //insert apparisal type
                AppraisalType straightRankingAppraisal = new AppraisalType()
                {
                    AppraisalTypeId = Guid.NewGuid().ToString(),
                    Name = "Straight Ranking",
                    Description = ""
                };
                AppraisalType gradingAppraisal = new AppraisalType()
                {
                    AppraisalTypeId = Guid.NewGuid().ToString(),
                    Name = "Grading",
                    Description = ""
                };
                AppraisalType managementObjectiveAppraisal = new AppraisalType()
                {
                    AppraisalTypeId = Guid.NewGuid().ToString(),
                    Name = "Management Objective",
                    Description = ""
                };
                AppraisalType traitBasedAppraisal = new AppraisalType()
                {
                    AppraisalTypeId = Guid.NewGuid().ToString(),
                    Name = "Trait Based",
                    Description = ""
                };
                AppraisalType behaviourBasedAppraisal = new AppraisalType()
                {
                    AppraisalTypeId = Guid.NewGuid().ToString(),
                    Name = "Behaviour Based",
                    Description = ""
                };
                AppraisalType review360Appraisal = new AppraisalType()
                {
                    AppraisalTypeId = Guid.NewGuid().ToString(),
                    Name = "360 Reviews",
                    Description = ""
                };

                await _context.AppraisalType.AddAsync(straightRankingAppraisal);
                await _context.AppraisalType.AddAsync(gradingAppraisal);
                await _context.AppraisalType.AddAsync(managementObjectiveAppraisal);
                await _context.AppraisalType.AddAsync(traitBasedAppraisal);
                await _context.AppraisalType.AddAsync(behaviourBasedAppraisal);
                await _context.AppraisalType.AddAsync(review360Appraisal);


                await _context.SaveChangesAsync();

                //insert ticket type
                TicketType attendanceTicket = new TicketType()
                {
                    TicketTypeId = Guid.NewGuid().ToString(),
                    Name = "Attendance",
                    Description = ""
                };
                TicketType payrollTicket = new TicketType()
                {
                    TicketTypeId = Guid.NewGuid().ToString(),
                    Name = "Payroll",
                    Description = ""
                };
                TicketType leaveTicket = new TicketType()
                {
                    TicketTypeId = Guid.NewGuid().ToString(),
                    Name = "Leave",
                    Description = ""
                };
                TicketType reimburseTicket = new TicketType()
                {
                    TicketTypeId = Guid.NewGuid().ToString(),
                    Name = "Reimburse",
                    Description = ""
                };
                TicketType miscellaneousTicket = new TicketType()
                {
                    TicketTypeId = Guid.NewGuid().ToString(),
                    Name = "Miscellaneous",
                    Description = ""
                };

                await _context.TicketType.AddAsync(attendanceTicket);
                await _context.TicketType.AddAsync(payrollTicket);
                await _context.TicketType.AddAsync(leaveTicket);
                await _context.TicketType.AddAsync(reimburseTicket);
                await _context.TicketType.AddAsync(miscellaneousTicket);


                await _context.SaveChangesAsync();

                //insert todo type
                TodoType payrollTodo = new TodoType()
                {
                    TodoTypeId = Guid.NewGuid().ToString(),
                    Name = "Payroll",
                    Description = ""
                };
                TodoType onboardingTodo = new TodoType()
                {
                    TodoTypeId = Guid.NewGuid().ToString(),
                    Name = "Onboarding",
                    Description = ""
                };
                TodoType recruitmentTodo = new TodoType()
                {
                    TodoTypeId = Guid.NewGuid().ToString(),
                    Name = "Recruitment",
                    Description = ""
                };

                await _context.TodoType.AddAsync(payrollTodo);
                await _context.TodoType.AddAsync(onboardingTodo);
                await _context.TodoType.AddAsync(recruitmentTodo);


                await _context.SaveChangesAsync();

                //insert department
                Department itDepartment = new Department()
                {
                    DepartmentId = Guid.NewGuid().ToString(),
                    Name = "IT Department",
                    Description = ""
                };
                Department hrDepartment = new Department()
                {
                    DepartmentId = Guid.NewGuid().ToString(),
                    Name = "HR Department",
                    Description = ""
                };
                Department financeDepartment = new Department()
                {
                    DepartmentId = Guid.NewGuid().ToString(),
                    Name = "Finance Department",
                    Description = ""
                };
                Department salesDepartment = new Department()
                {
                    DepartmentId = Guid.NewGuid().ToString(),
                    Name = "Sales Department",
                    Description = ""
                };
                Department warehouseDepartment = new Department()
                {
                    DepartmentId = Guid.NewGuid().ToString(),
                    Name = "Warehouse Department",
                    Description = ""
                };

                await _context.Department.AddAsync(itDepartment);
                await _context.Department.AddAsync(hrDepartment);
                await _context.Department.AddAsync(financeDepartment);
                await _context.Department.AddAsync(salesDepartment);
                await _context.Department.AddAsync(warehouseDepartment);


                await _context.SaveChangesAsync();

                //insert designation
                Designation designationVPIT = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "VP IT / COO (ExComm)",
                    Description = ""
                };
                Designation designationHeadIT = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Head of IT",
                    Description = ""
                };
                Designation designationSeniorManagerIT = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "IT Senior Manager",
                    Description = ""
                };
                Designation designationManagerIT = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "IT Manager",
                    Description = ""
                };
                Designation designationStaffIT = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "IT Staff",
                    Description = ""
                };
                Designation designationVPFinance = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "VP Finance / CEO (ExComm)",
                    Description = ""
                };
                Designation designationHeadFinance = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Head of Finance",
                    Description = ""
                };
                Designation designationSeniorManagerFinance = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Finance Senior Manager",
                    Description = ""
                };
                Designation designationManagerFinance = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Finance Manager",
                    Description = ""
                };
                Designation designationStaffFinance = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Finance Staff",
                    Description = ""
                };
                Designation designationVPHR = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "VP HR (ExComm)",
                    Description = ""
                };
                Designation designationHeadHR = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Head of HR",
                    Description = ""
                };
                Designation designationSeniorManagerHR = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "HR Senior Manager",
                    Description = ""
                };
                Designation designationManagerHR = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "HR Manager",
                    Description = ""
                };
                Designation designationStaffHR = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "HR Staff",
                    Description = ""
                };
                Designation designationVPSales = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "VP Sales / CMO (ExComm)",
                    Description = ""
                };
                Designation designationHeadSales = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Head of Sales",
                    Description = ""
                };
                Designation designationSeniorManagerSales = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Sales Senior Manager",
                    Description = ""
                };
                Designation designationManagerSales = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Sales Manager",
                    Description = ""
                };
                Designation designationStaffSales = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Sales Staff",
                    Description = ""
                };
                Designation designationVPWarehouse = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "VP Warehouse (ExComm)",
                    Description = ""
                };
                Designation designationHeadWarehouse = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Head of Warehouse",
                    Description = ""
                };
                Designation designationSeniorManagerWarehouse = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Warehouse Senior Manager",
                    Description = ""
                };
                Designation designationManagerWarehouse = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Warehouse Manager",
                    Description = ""
                };
                Designation designationStaffWarehouse = new Designation()
                {
                    DesignationId = Guid.NewGuid().ToString(),
                    Name = "Warehouse Staff",
                    Description = ""
                };

                await _context.Designation.AddAsync(designationVPIT);
                await _context.Designation.AddAsync(designationHeadIT);
                await _context.Designation.AddAsync(designationSeniorManagerIT);
                await _context.Designation.AddAsync(designationManagerIT);
                await _context.Designation.AddAsync(designationStaffIT);

                await _context.Designation.AddAsync(designationVPHR);
                await _context.Designation.AddAsync(designationHeadHR);
                await _context.Designation.AddAsync(designationSeniorManagerHR);
                await _context.Designation.AddAsync(designationManagerHR);
                await _context.Designation.AddAsync(designationStaffHR);

                await _context.Designation.AddAsync(designationVPFinance);
                await _context.Designation.AddAsync(designationHeadFinance);
                await _context.Designation.AddAsync(designationSeniorManagerFinance);
                await _context.Designation.AddAsync(designationManagerFinance);
                await _context.Designation.AddAsync(designationStaffFinance);

                await _context.Designation.AddAsync(designationVPSales);
                await _context.Designation.AddAsync(designationHeadSales);
                await _context.Designation.AddAsync(designationSeniorManagerSales);
                await _context.Designation.AddAsync(designationManagerSales);
                await _context.Designation.AddAsync(designationStaffSales);

                await _context.Designation.AddAsync(designationVPWarehouse);
                await _context.Designation.AddAsync(designationHeadWarehouse);
                await _context.Designation.AddAsync(designationSeniorManagerWarehouse);
                await _context.Designation.AddAsync(designationManagerWarehouse);
                await _context.Designation.AddAsync(designationStaffWarehouse);


                await _context.SaveChangesAsync();

                //random data source: https://www.summet.com/dmsi/html/codesamples/addresses.html

                //insert employee (finance)
                Employee empCeciliaChapman = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Cecilia",
                    LastName = "Chapman",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 1, 1),
                    PlaceOfBirth = "Mankato",
                    MaritalStatus = "Single",
                    Email = "Cecilia.Chapman@hrmlite.com",
                    Phone = "(257) 563-7401",
                    Address1 = "711-2880 Nulla St.",
                    City = "Mankato",
                    StateProvince = "Mississippi",
                    ZipCode = "96522",
                    Country = "USA",
                    EmployeeIDNumber = "EMPFI001",
                    Designation = designationVPFinance,
                    Department = financeDepartment,
                    //Supervisor
                    JoiningDate = new DateTime(2017, 1, 1),
                    BasicSalary = 25000m,
                    UnpaidLeavePerDay = 1250m,
                    AccountTitle = "Cecilia Chapman Payroll",
                    BankName = "JPMorgan-Chase",
                    AccountNumber = "1111-CHASE"
                };
                await _context.Employee.AddAsync(empCeciliaChapman);
                await _context.SaveChangesAsync();

                Employee empIrisWatson = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Iris",
                    LastName = "Watson",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 1, 2),
                    PlaceOfBirth = "Frederick",
                    MaritalStatus = "Single",
                    Email = "Iris.Watson@hrmlite.com",
                    Phone = "(372) 587-2335",
                    Address1 = "P.O. Box 283 8562 Fusce Rd.",
                    City = "Frederick",
                    StateProvince = "Nebraska",
                    ZipCode = "20620",
                    Country = "USA",
                    EmployeeIDNumber = "EMPFI002",
                    Designation = designationHeadFinance,
                    Department = financeDepartment,
                    Supervisor = empCeciliaChapman,
                    JoiningDate = new DateTime(2017, 1, 2),
                    BasicSalary = 20000m,
                    UnpaidLeavePerDay = 1000m,
                    AccountTitle = "Iris Watson Payroll",
                    BankName = "Bank of America",
                    AccountNumber = "1111-AMERICA"
                };
                await _context.Employee.AddAsync(empIrisWatson);
                await _context.SaveChangesAsync();

                Employee empCelesteSlater = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Celeste",
                    LastName = "Slater",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 1, 3),
                    PlaceOfBirth = "Roseville",
                    MaritalStatus = "Single",
                    Email = "Celeste.Slater@hrmlite.com",
                    Phone = "(786) 713-8616",
                    Address1 = "606-3727 Ullamcorper. Street",
                    City = "Roseville",
                    StateProvince = "NH",
                    ZipCode = "11523",
                    Country = "USA",
                    EmployeeIDNumber = "EMPFI003",
                    Designation = designationSeniorManagerFinance,
                    Department = financeDepartment,
                    Supervisor = empIrisWatson,
                    JoiningDate = new DateTime(2017, 1, 3),
                    BasicSalary = 15000m,
                    UnpaidLeavePerDay = 750m,
                    AccountTitle = "Celeste Slater Payroll",
                    BankName = "Wells Fargo",
                    AccountNumber = "1111-FARGO"
                };
                await _context.Employee.AddAsync(empCelesteSlater);
                await _context.SaveChangesAsync();

                Employee empTheodoreLowe = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Theodore",
                    LastName = "Lowe",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 1, 4),
                    PlaceOfBirth = "Azusa",
                    MaritalStatus = "Single",
                    Email = "Theodore.Lowe@hrmlite.com",
                    Phone = "(793) 151-6230",
                    Address1 = "Ap #867-859 Sit Rd.",
                    City = "Azusa",
                    StateProvince = "New York",
                    ZipCode = "39531",
                    Country = "USA",
                    EmployeeIDNumber = "EMPFI004",
                    Designation = designationManagerFinance,
                    Department = financeDepartment,
                    Supervisor = empCelesteSlater,
                    JoiningDate = new DateTime(2017, 1, 4),
                    BasicSalary = 10000m,
                    UnpaidLeavePerDay = 500m,
                    AccountTitle = "Theodore Lowe Payroll",
                    BankName = "Bank of New York Mellon",
                    AccountNumber = "1111-MELLON"
                };
                await _context.Employee.AddAsync(empTheodoreLowe);
                await _context.SaveChangesAsync();

                Employee empCalistaWise = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Calista",
                    LastName = "Wise",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 1, 5),
                    PlaceOfBirth = "San Antonio",
                    MaritalStatus = "Single",
                    Email = "Calista.Wise@hrmlite.com",
                    Phone = "(492) 709-6392",
                    Address1 = "7292 Dictum Av.",
                    City = "San Antonio",
                    StateProvince = "MI",
                    ZipCode = "47096",
                    Country = "USA",
                    EmployeeIDNumber = "EMPFI005",
                    Designation = designationStaffFinance,
                    Department = financeDepartment,
                    Supervisor = empTheodoreLowe,
                    JoiningDate = new DateTime(2017, 1, 5),
                    BasicSalary = 9000m,
                    UnpaidLeavePerDay = 450m,
                    AccountTitle = "Calista Wise Payroll",
                    BankName = "Capital One",
                    AccountNumber = "1111-ONE"
                };
                await _context.Employee.AddAsync(empCalistaWise);
                await _context.SaveChangesAsync();


                //insert employee (IT)
                Employee empKylaOlsen = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Kyla",
                    LastName = "Olsen",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 2, 1),
                    PlaceOfBirth = "Tamuning",
                    MaritalStatus = "Single",
                    Email = "Kyla.Olsen@hrmlite.com",
                    Phone = "(654) 393-5734",
                    Address1 = "Ap #651-8679 Sodales Av.",
                    City = "Tamuning",
                    StateProvince = "PA",
                    ZipCode = "10855",
                    Country = "USA",
                    EmployeeIDNumber = "EMPIT001",
                    Designation = designationVPIT,
                    Department = itDepartment,
                    Supervisor = empCeciliaChapman,
                    JoiningDate = new DateTime(2017, 2, 1),
                    BasicSalary = 25000m,
                    UnpaidLeavePerDay = 1250m,
                    AccountTitle = "Kyla Olsen Payroll",
                    BankName = "JPMorgan-Chase",
                    AccountNumber = "2222-CHASE"
                };
                await _context.Employee.AddAsync(empKylaOlsen);
                await _context.SaveChangesAsync();

                Employee empForrestRay = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Forrest",
                    LastName = "Ray",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 2, 2),
                    PlaceOfBirth = "Corona",
                    MaritalStatus = "Single",
                    Email = "Forrest.Ray@hrmlite.com",
                    Phone = "(404) 960-3807",
                    Address1 = "191-103 Integer Rd.",
                    City = "Corona",
                    StateProvince = "New Mexico",
                    ZipCode = "08219",
                    Country = "USA",
                    EmployeeIDNumber = "EMPIT002",
                    Designation = designationHeadIT,
                    Department = itDepartment,
                    Supervisor = empKylaOlsen,
                    JoiningDate = new DateTime(2017, 2, 2),
                    BasicSalary = 20000m,
                    UnpaidLeavePerDay = 1000m,
                    AccountTitle = "Forrest Ray Payroll",
                    BankName = "Bank of America",
                    AccountNumber = "22222-AMERICA"
                };
                await _context.Employee.AddAsync(empForrestRay);
                await _context.SaveChangesAsync();

                Employee empHirokoPotter = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Hiroko",
                    LastName = "Potter",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 2, 3),
                    PlaceOfBirth = "Muskegon",
                    MaritalStatus = "Single",
                    Email = "Hiroko.Potter@hrmlite.com",
                    Phone = "(314) 244-6306",
                    Address1 = "P.O. Box 887 2508 Dolor. Av.",
                    City = "Muskegon",
                    StateProvince = "KY",
                    ZipCode = "12482",
                    Country = "USA",
                    EmployeeIDNumber = "EMPIT003",
                    Designation = designationSeniorManagerIT,
                    Department = itDepartment,
                    Supervisor = empForrestRay,
                    JoiningDate = new DateTime(2017, 2, 3),
                    BasicSalary = 15000m,
                    UnpaidLeavePerDay = 750m,
                    AccountTitle = "Hiroko Potter Payroll",
                    BankName = "Wells Fargo",
                    AccountNumber = "2222-FARGO"
                };
                await _context.Employee.AddAsync(empHirokoPotter);
                await _context.SaveChangesAsync();

                Employee empLawrenceMoreno = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Lawrence",
                    LastName = "Moreno",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 2, 4),
                    PlaceOfBirth = "Santa Rosa",
                    MaritalStatus = "Single",
                    Email = "Lawrence.Moreno@hrmlite.com",
                    Phone = "(684) 579-1879",
                    Address1 = "935-9940 Tortor. Street",
                    City = "Santa Rosa",
                    StateProvince = "MN",
                    ZipCode = "98804",
                    Country = "USA",
                    EmployeeIDNumber = "EMPIT004",
                    Designation = designationManagerIT,
                    Department = itDepartment,
                    Supervisor = empHirokoPotter,
                    JoiningDate = new DateTime(2017, 2, 4),
                    BasicSalary = 10000m,
                    UnpaidLeavePerDay = 500m,
                    AccountTitle = "Lawrence Moreno Payroll",
                    BankName = "Bank of New York Mellon",
                    AccountNumber = "2222-MELLON"
                };
                await _context.Employee.AddAsync(empLawrenceMoreno);
                await _context.SaveChangesAsync();

                Employee empAaronHawkins = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Aaron",
                    LastName = "Hawkins",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 2, 5),
                    PlaceOfBirth = "Erie",
                    MaritalStatus = "Single",
                    Email = "Aaron.Hawkins@hrmlite.com",
                    Phone = "(660) 663-4518",
                    Address1 = "5587 Nunc. Avenue",
                    City = "Erie",
                    StateProvince = "Rhode Island",
                    ZipCode = "24975",
                    Country = "USA",
                    EmployeeIDNumber = "EMPIT005",
                    Designation = designationStaffIT,
                    Department = itDepartment,
                    Supervisor = empLawrenceMoreno,
                    JoiningDate = new DateTime(2017, 2, 5),
                    BasicSalary = 9000m,
                    UnpaidLeavePerDay = 450m,
                    AccountTitle = "Aaron Hawkins Payroll",
                    BankName = "Capital One",
                    AccountNumber = "2222-ONE"
                };
                await _context.Employee.AddAsync(empAaronHawkins);
                await _context.SaveChangesAsync();


                //insert employee (HR)
                Employee empHedyGreene = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Hedy",
                    LastName = "Greene",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 3, 1),
                    PlaceOfBirth = "Latrobe",
                    MaritalStatus = "Single",
                    Email = "Hedy.Greene@hrmlite.com",
                    Phone = "(608) 265-2215",
                    Address1 = "Ap #696-3279 Viverra. Avenue",
                    City = "Latrobe",
                    StateProvince = "DE",
                    ZipCode = "38100",
                    Country = "USA",
                    EmployeeIDNumber = "EMPHR001",
                    Designation = designationVPHR,
                    Department = hrDepartment,
                    Supervisor = empCeciliaChapman,
                    JoiningDate = new DateTime(2017, 3, 1),
                    BasicSalary = 25000m,
                    UnpaidLeavePerDay = 1250m,
                    AccountTitle = "Hedy Greene Payroll",
                    BankName = "JPMorgan-Chase",
                    AccountNumber = "3333-CHASE"
                };
                await _context.Employee.AddAsync(empHedyGreene);
                await _context.SaveChangesAsync();

                Employee empMelvinPorter = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Melvin",
                    LastName = "Porter",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 3, 2),
                    PlaceOfBirth = "Bandera",
                    MaritalStatus = "Single",
                    Email = "Melvin.Porter@hrmlite.com",
                    Phone = "(959) 119-8364",
                    Address1 = "P.O. Box 132 1599 Curabitur Rd.",
                    City = "Bandera",
                    StateProvince = "South Dakota",
                    ZipCode = "45149",
                    Country = "USA",
                    EmployeeIDNumber = "EMPHR002",
                    Designation = designationHeadHR,
                    Department = hrDepartment,
                    Supervisor = empHedyGreene,
                    JoiningDate = new DateTime(2017, 3, 2),
                    BasicSalary = 20000m,
                    UnpaidLeavePerDay = 1000m,
                    AccountTitle = "Melvin Porter Payroll",
                    BankName = "Bank of America",
                    AccountNumber = "3333-AMERICA"
                };
                await _context.Employee.AddAsync(empMelvinPorter);
                await _context.SaveChangesAsync();

                Employee empJoanRomero = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Joan",
                    LastName = "Romero",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 3, 3),
                    PlaceOfBirth = "Idaho Falls ",
                    MaritalStatus = "Single",
                    Email = "Joan.Romero@hrmlite.com",
                    Phone = "(248) 675-4007",
                    Address1 = "666-4366 Lacinia Avenue",
                    City = "Idaho Falls ",
                    StateProvince = "Ohio",
                    ZipCode = "19253",
                    Country = "USA",
                    EmployeeIDNumber = "EMPHR003",
                    Designation = designationSeniorManagerHR,
                    Department = hrDepartment,
                    Supervisor = empMelvinPorter,
                    JoiningDate = new DateTime(2017, 3, 3),
                    BasicSalary = 15000m,
                    UnpaidLeavePerDay = 750m,
                    AccountTitle = "Joan Romero Payroll",
                    BankName = "Wells Fargo",
                    AccountNumber = "3333-FARGO"
                };
                await _context.Employee.AddAsync(empJoanRomero);
                await _context.SaveChangesAsync();

                Employee empLeilaniBoyer = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Leilani",
                    LastName = "Boyer",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 3, 4),
                    PlaceOfBirth = "San Bernardino",
                    MaritalStatus = "Single",
                    Email = "Leilani.Boyer@hrmlite.com",
                    Phone = "(570) 873-7090",
                    Address1 = "557-6308 Lacinia Road",
                    City = "San Bernardino",
                    StateProvince = "ND",
                    ZipCode = "09289",
                    Country = "USA",
                    EmployeeIDNumber = "EMPHR004",
                    Designation = designationManagerHR,
                    Department = hrDepartment,
                    Supervisor = empJoanRomero,
                    JoiningDate = new DateTime(2017, 3, 4),
                    BasicSalary = 10000m,
                    UnpaidLeavePerDay = 500m,
                    AccountTitle = "Leilani Boyer Payroll",
                    BankName = "Bank of New York Mellon",
                    AccountNumber = "3333-MELLON"
                };
                await _context.Employee.AddAsync(empLeilaniBoyer);
                await _context.SaveChangesAsync();

                Employee empColbyBernard = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Colby",
                    LastName = "Bernard",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 3, 5),
                    PlaceOfBirth = "Amesbury",
                    MaritalStatus = "Single",
                    Email = "Colby.Bernard@hrmlite.com",
                    Phone = "(302) 259-2375",
                    Address1 = "Ap #285-7193 Ullamcorper Avenue",
                    City = "Amesbury",
                    StateProvince = "HI",
                    ZipCode = "93373",
                    Country = "USA",
                    EmployeeIDNumber = "EMPHR005",
                    Designation = designationStaffHR,
                    Department = hrDepartment,
                    Supervisor = empLeilaniBoyer,
                    JoiningDate = new DateTime(2017, 3, 5),
                    BasicSalary = 9000m,
                    UnpaidLeavePerDay = 450m,
                    AccountTitle = "Colby Bernard Payroll",
                    BankName = "Capital One",
                    AccountNumber = "3333-ONE"
                };
                await _context.Employee.AddAsync(empColbyBernard);
                await _context.SaveChangesAsync();

                //insert employee (Sales)
                Employee empNoelleAdams = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Noelle",
                    LastName = "Adams",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 4, 1),
                    PlaceOfBirth = "Gardena",
                    MaritalStatus = "Single",
                    Email = "Noelle.Adams@hrmlite.com",
                    Phone = "(559) 104-5475",
                    Address1 = "6351 Fringilla Avenue",
                    City = "Gardena",
                    StateProvince = "Colorado",
                    ZipCode = "37547",
                    Country = "USA",
                    EmployeeIDNumber = "EMPSLS001",
                    Designation = designationVPSales,
                    Department = salesDepartment,
                    Supervisor = empCeciliaChapman,
                    JoiningDate = new DateTime(2017, 4, 1),
                    BasicSalary = 25000m,
                    UnpaidLeavePerDay = 1250m,
                    AccountTitle = "Noelle Adams Payroll",
                    BankName = "JPMorgan-Chase",
                    AccountNumber = "4444-CHASE"
                };
                await _context.Employee.AddAsync(empNoelleAdams);
                await _context.SaveChangesAsync();

                Employee empLillithDaniel = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Lillith",
                    LastName = "Daniel",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 4, 2),
                    PlaceOfBirth = "Centennial",
                    MaritalStatus = "Single",
                    Email = "Lillith.Daniel@hrmlite.com",
                    Phone = "(387) 142-9434",
                    Address1 = "935-1670 Neque. St.",
                    City = "Centennial",
                    StateProvince = "Delaware",
                    ZipCode = "48432",
                    Country = "USA",
                    EmployeeIDNumber = "EMPSLS002",
                    Designation = designationHeadSales,
                    Department = salesDepartment,
                    Supervisor = empNoelleAdams,
                    JoiningDate = new DateTime(2017, 4, 2),
                    BasicSalary = 20000m,
                    UnpaidLeavePerDay = 1000m,
                    AccountTitle = "Lillith Daniel Payroll",
                    BankName = "Bank of America",
                    AccountNumber = "4444-AMERICA"
                };
                await _context.Employee.AddAsync(empLillithDaniel);
                await _context.SaveChangesAsync();

                Employee empAdriaRussell = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Adria",
                    LastName = "Russell",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 4, 3),
                    PlaceOfBirth = "Miami Beach",
                    MaritalStatus = "Single",
                    Email = "Adria.Russell@hrmlite.com",
                    Phone = "(516) 745-4496",
                    Address1 = "414-7533 Non Rd.",
                    City = "Miami Beach",
                    StateProvince = "North Dakota",
                    ZipCode = "58563",
                    Country = "USA",
                    EmployeeIDNumber = "EMPSLS003",
                    Designation = designationSeniorManagerSales,
                    Department = salesDepartment,
                    Supervisor = empLillithDaniel,
                    JoiningDate = new DateTime(2017, 4, 3),
                    BasicSalary = 15000m,
                    UnpaidLeavePerDay = 750m,
                    AccountTitle = "Adria Russell Payroll",
                    BankName = "Wells Fargo",
                    AccountNumber = "4444-FARGO"
                };
                await _context.Employee.AddAsync(empAdriaRussell);
                await _context.SaveChangesAsync();

                Employee empHildaHaynes = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Hilda",
                    LastName = "Haynes",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 4, 4),
                    PlaceOfBirth = "Weirton",
                    MaritalStatus = "Single",
                    Email = "Hilda.Haynes@hrmlite.com",
                    Phone = "(570) 873-7090",
                    Address1 = "778-9383 Suspendisse Av.",
                    City = "Weirton",
                    StateProvince = "IN",
                    ZipCode = "93479",
                    Country = "USA",
                    EmployeeIDNumber = "EMPSLS004",
                    Designation = designationManagerSales,
                    Department = salesDepartment,
                    Supervisor = empAdriaRussell,
                    JoiningDate = new DateTime(2017, 4, 4),
                    BasicSalary = 10000m,
                    UnpaidLeavePerDay = 500m,
                    AccountTitle = "Hilda Haynes Payroll",
                    BankName = "Bank of New York Mellon",
                    AccountNumber = "4444-MELLON"
                };
                await _context.Employee.AddAsync(empHildaHaynes);
                await _context.SaveChangesAsync();

                Employee empRebeccaChambers = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Rebecca",
                    LastName = "Chambers",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1992, 4, 5),
                    PlaceOfBirth = "Liberal",
                    MaritalStatus = "Single",
                    Email = "Rebecca.Chambers@hrmlite.com",
                    Phone = "(455) 430-0989",
                    Address1 = "P.O. Box 813 5982 Sit Ave",
                    City = "Liberal",
                    StateProvince = "Vermont",
                    ZipCode = "51324",
                    Country = "USA",
                    EmployeeIDNumber = "EMPSLS005",
                    Designation = designationStaffSales,
                    Department = salesDepartment,
                    Supervisor = empHildaHaynes,
                    JoiningDate = new DateTime(2017, 4, 5),
                    BasicSalary = 9000m,
                    UnpaidLeavePerDay = 450m,
                    AccountTitle = "Rebecca Chambers Payroll",
                    BankName = "Capital One",
                    AccountNumber = "4444-ONE"
                };
                await _context.Employee.AddAsync(empRebeccaChambers);
                await _context.SaveChangesAsync();

                //insert employee (Warehouse)
                Employee empChristianEmerson = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Christian",
                    LastName = "Emerson",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 5, 1),
                    PlaceOfBirth = "Rolling Hills",
                    MaritalStatus = "Single",
                    Email = "Christian.Emerson@hrmlite.com",
                    Phone = "(490) 936-4694",
                    Address1 = "P.O. Box 886 4118 Arcu St.",
                    City = "Rolling Hills",
                    StateProvince = "Georgia",
                    ZipCode = "92358",
                    Country = "USA",
                    EmployeeIDNumber = "EMPWH001",
                    Designation = designationVPWarehouse,
                    Department = warehouseDepartment,
                    Supervisor = empCeciliaChapman,
                    JoiningDate = new DateTime(2017, 5, 1),
                    BasicSalary = 25000m,
                    UnpaidLeavePerDay = 1250m,
                    AccountTitle = "Christian Emerson Payroll",
                    BankName = "JPMorgan-Chase",
                    AccountNumber = "5555-CHASE"
                };
                await _context.Employee.AddAsync(empChristianEmerson);
                await _context.SaveChangesAsync();

                Employee empEdwardNieves = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Edward",
                    LastName = "Nieves",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 5, 2),
                    PlaceOfBirth = "Idaho Falls",
                    MaritalStatus = "Single",
                    Email = "Edward.Nieves@hrmlite.com",
                    Phone = "(802) 668-8240",
                    Address1 = "928-3313 Vel Av.",
                    City = "Idaho Falls",
                    StateProvince = "Rhode Island",
                    ZipCode = "37232",
                    Country = "USA",
                    EmployeeIDNumber = "EMPWH002",
                    Designation = designationHeadWarehouse,
                    Department = warehouseDepartment,
                    Supervisor = empChristianEmerson,
                    JoiningDate = new DateTime(2017, 5, 2),
                    BasicSalary = 20000m,
                    UnpaidLeavePerDay = 1000m,
                    AccountTitle = "Edward Nieves Payroll",
                    BankName = "Bank of America",
                    AccountNumber = "5555-AMERICA"
                };
                await _context.Employee.AddAsync(empEdwardNieves);
                await _context.SaveChangesAsync();

                Employee empChesterBennett = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Chester",
                    LastName = "Bennett",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 5, 3),
                    PlaceOfBirth = "Minot",
                    MaritalStatus = "Single",
                    Email = "Chester.Bennett@hrmlite.com",
                    Phone = "(837) 196-3274",
                    Address1 = "3476 Aliquet. Ave",
                    City = "Minot",
                    StateProvince = "AZ",
                    ZipCode = "95302",
                    Country = "USA",
                    EmployeeIDNumber = "EMPWH003",
                    Designation = designationSeniorManagerWarehouse,
                    Department = warehouseDepartment,
                    Supervisor = empEdwardNieves,
                    JoiningDate = new DateTime(2017, 5, 3),
                    BasicSalary = 15000m,
                    UnpaidLeavePerDay = 750m,
                    AccountTitle = "Chester Bennett Payroll",
                    BankName = "Wells Fargo",
                    AccountNumber = "5555-FARGO"
                };
                await _context.Employee.AddAsync(empChesterBennett);
                await _context.SaveChangesAsync();

                Employee empCastorRichardson = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Castor",
                    LastName = "Richardson",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 5, 4),
                    PlaceOfBirth = "Lynchburg",
                    MaritalStatus = "Single",
                    Email = "Castor.Richardson@hrmlite.com",
                    Phone = "(268) 442-2428",
                    Address1 = "P.O. Box 902 3472 Ullamcorper Street",
                    City = "Lynchburg",
                    StateProvince = "DC",
                    ZipCode = "29738",
                    Country = "USA",
                    EmployeeIDNumber = "EMPWH004",
                    Designation = designationManagerWarehouse,
                    Department = warehouseDepartment,
                    Supervisor = empChesterBennett,
                    JoiningDate = new DateTime(2017, 5, 4),
                    BasicSalary = 10000m,
                    UnpaidLeavePerDay = 500m,
                    AccountTitle = "Castor Richardson Payroll",
                    BankName = "Bank of New York Mellon",
                    AccountNumber = "5555-MELLON"
                };
                await _context.Employee.AddAsync(empCastorRichardson);
                await _context.SaveChangesAsync();

                Employee empHarrisonMcguire = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Harrison",
                    LastName = "Mcguire",
                    Gender = "Male",
                    DateOfBirth = new DateTime(1992, 5, 5),
                    PlaceOfBirth = "San Fernando",
                    MaritalStatus = "Single",
                    Email = "Harrison.Mcguire@hrmlite.com",
                    Phone = "(861) 546-5032",
                    Address1 = "P.O. Box 813 5982 Sit Ave",
                    City = "San Fernando",
                    StateProvince = "ID",
                    ZipCode = "77373",
                    Country = "USA",
                    EmployeeIDNumber = "EMPWH005",
                    Designation = designationStaffWarehouse,
                    Department = warehouseDepartment,
                    Supervisor = empCastorRichardson,
                    JoiningDate = new DateTime(2017, 5, 5),
                    BasicSalary = 9000m,
                    UnpaidLeavePerDay = 450m,
                    AccountTitle = "Harrison Mcguire Payroll",
                    BankName = "Capital One",
                    AccountNumber = "5555-ONE"
                };
                await _context.Employee.AddAsync(empHarrisonMcguire);
                await _context.SaveChangesAsync();

                //insert and connect system user with employee
                List<Employee> employees = new List<Employee>();
                employees =  _context.Employee.ToList();
                foreach (var item in employees)
                {
                    ApplicationUser appUser = new ApplicationUser();

                    appUser.Email = item.Email;
                    appUser.UserName = item.Email;
                    appUser.EmailConfirmed = true;
                    appUser.isSuperAdmin = false;

                    //create system user
                    await _userManager.CreateAsync(appUser, "123456");

                    //connect employee with their system user
                    
                    if (item.Equals(empCeciliaChapman))
                    {
                        //the CEO connect with super admin
                        item.SystemUser = superAdmin;
                    }
                    else
                    {
                        item.SystemUser = appUser;
                    }
                    

                    _context.Employee.Update(item);
                    await _context.SaveChangesAsync();

                    //assign role SelfService to employee
                    await _userManager.AddToRoleAsync(appUser, Services.App.Pages.SelfService.RoleName);
                    
                }

                //insert benefit template default for each employee
                foreach (var item in employees)
                {
                    BenefitTemplate benefit = new BenefitTemplate();
                    benefit.BenefitTemplateId = Guid.NewGuid().ToString();
                    benefit.Name = item.EmployeeIDNumber + " Template";
                    benefit.Description = "Benefit template for " + item.FirstName + " " + item.LastName + " " + item.EmployeeIDNumber;

                    await _context.BenefitTemplate.AddAsync(benefit);
                    await _context.SaveChangesAsync();

                    //salary components allowance
                    foreach (var allowance in _context.AllowanceType.ToList())
                    {
                        BenefitTemplateLine component = new BenefitTemplateLine();
                        component.BenefitTemplateLineId = Guid.NewGuid().ToString();
                        component.BenefitTemplateId = benefit.BenefitTemplateId;
                        component.Description = allowance.Description;
                        component.AllowanceTypeId = allowance.AllowanceTypeId;
                        component.Amount = item.BasicSalary * 0.10m;
                        await _context.BenefitTemplateLine.AddAsync(component);
                    }

                    //salary components deduction
                    foreach (var deduction in _context.DeductionType.ToList())
                    {
                        BenefitTemplateLine component = new BenefitTemplateLine();
                        component.BenefitTemplateLineId = Guid.NewGuid().ToString();
                        component.BenefitTemplateId = benefit.BenefitTemplateId;
                        component.Description = deduction.Description;
                        component.DeductionTypeId = deduction.DeductionTypeId;
                        component.Amount = item.BasicSalary * 0.10m * -1m;
                        await _context.BenefitTemplateLine.AddAsync(component);
                    }

                    item.BenefitTemplateId = benefit.BenefitTemplateId;
                    await _context.SaveChangesAsync();
                }

                //insert leave
                List<LeaveType> leaveTypes = new List<LeaveType>();
                leaveTypes = _context.LeaveType.ToList();

                DateTime startLeave = new DateTime(2021, 1, 1);
                DateTime endLeave = DateTime.Now;

                //set random leave dates
                List<int> leaveDates = new List<int>();
                for (int i = 0; i < 15; i++)
                {
                    int randomDate = new Random().Next(1, 30);
                    if (!leaveDates.Contains(randomDate))
                    {
                        leaveDates.Add(randomDate);
                    }
                    
                }

                for (DateTime date = startLeave.Date; date < endLeave.Date; date = date.AddDays(1))
                {
                    if (leaveDates.Contains(date.Day)
                        && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                    {
                        Leave leave = new Leave();
                        leave.LeaveId = Guid.NewGuid().ToString();
                        leave.LeaveType = leaveTypes[new Random().Next(0, leaveTypes.Count - 1)];
                        leave.OnBehalf = employees[new Random().Next(0, employees.Count - 1)];
                        leave.LeaveName = date.ToString("yyyy-MM-dd") + " " + leave.OnBehalf.EmployeeIDNumber;
                        leave.Description = leave.OnBehalf.FirstName + " " + leave.OnBehalf.LastName + " on " + leave.LeaveType.Name;
                        leave.IsPaidLeave = (new Random().Next(0, 2) == 1) ? true : false;
                        leave.FromDate = date;
                        leave.ToDate = date;
                        leave.EmergencyCall = leave.OnBehalf.Phone;
                        leave.IsApproved = new Random().Next(0, 2) == 1 ? true : false;
                        await _context.Leave.AddAsync(leave);
                    }
                    
                }
                await _context.SaveChangesAsync();

                //insert award
                List<AwardType> awardTypes = new List<AwardType>();
                awardTypes = _context.AwardType.ToList();

                DateTime startAward = new DateTime(2021, 1, 1);
                DateTime endAward = DateTime.Now;

                //set random award dates
                List<int> awardDates = new List<int>();
                for (int i = 0; i < 8; i++)
                {
                    int randomDate = new Random().Next(1, 30);
                    if (!awardDates.Contains(randomDate))
                    {
                        awardDates.Add(randomDate);
                    }

                }

                for (DateTime date = startAward.Date; date < endAward.Date; date = date.AddDays(1))
                {
                    if (awardDates.Contains(date.Day)
                        && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                    {
                        Award award = new Award();
                        award.AwardId = Guid.NewGuid().ToString();
                        award.AwardType = awardTypes[new Random().Next(0, awardTypes.Count - 1)];
                        award.AwardRecipient = employees[new Random().Next(0, employees.Count - 1)];
                        award.AwardName = date.ToString("yyyy-MM-dd") + " " + award.AwardRecipient.EmployeeIDNumber;
                        award.Description = award.AwardRecipient.FirstName + " " + award.AwardRecipient.LastName + " winner of " + award.AwardType.Name;
                        award.ReleaseDate = date;
                        award.IsApproved = new Random().Next(0, 2) == 1 ? true : false;
                        await _context.Award.AddAsync(award);
                    }

                }
                await _context.SaveChangesAsync();

                //insert appraisal
                List<AppraisalType> appraisalTypes = new List<AppraisalType>();
                appraisalTypes = _context.AppraisalType.ToList();

                DateTime startAppraisal = new DateTime(2021, 1, 1);
                DateTime endAppraisal = DateTime.Now;

                //set random appraisal dates
                List<int> appraisalDates = new List<int>();
                for (int i = 0; i < 8; i++)
                {
                    int randomDate = new Random().Next(1, 30);
                    if (!appraisalDates.Contains(randomDate))
                    {
                        appraisalDates.Add(randomDate);
                    }

                }

                for (DateTime date = startAppraisal.Date; date < endAppraisal.Date; date = date.AddDays(1))
                {
                    if (appraisalDates.Contains(date.Day)
                        && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                    {
                        Appraisal appraisal = new Appraisal();
                        appraisal.AppraisalId = Guid.NewGuid().ToString();
                        appraisal.AppraisalType = appraisalTypes[new Random().Next(0, appraisalTypes.Count - 1)];
                        appraisal.OnBehalf = employees[new Random().Next(0, employees.Count - 1)];
                        appraisal.AppraisalName = date.ToString("yyyy-MM-dd") + " " + appraisal.OnBehalf.EmployeeIDNumber;
                        appraisal.Description = appraisal.OnBehalf.FirstName + " " + appraisal.OnBehalf.LastName + " " + appraisal.AppraisalType.Name;
                        appraisal.SubmitDate = date;
                        appraisal.IsApproved = new Random().Next(0, 2) == 1 ? true : false;
                        await _context.Appraisal.AddAsync(appraisal);
                    }
                    
                }
                await _context.SaveChangesAsync();

                //insert ticket
                List<Employee> agents = new List<Employee>();
                agents = _context.Employee.Where(x => x.Department.Name.Equals("HR Department")).ToList();

                List<Employee> nonHREmployees = new List<Employee>();
                nonHREmployees = _context.Employee.Where(x => !x.Department.Name.Equals("HR Department")).ToList();

                List<TicketType> ticketTypes = new List<TicketType>();
                ticketTypes = _context.TicketType.ToList();

                DateTime startTicket = new DateTime(2021, 1, 1);
                DateTime endTicket = DateTime.Now;

                //set random ticket dates
                List<int> ticketDates = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    int randomDate = new Random().Next(1, 30);
                    if (!ticketDates.Contains(randomDate))
                    {
                        ticketDates.Add(randomDate);
                    }

                }

                foreach (var item in nonHREmployees)
                {
                    for (DateTime date = startTicket.Date; date < endTicket.Date; date = date.AddDays(1))
                    {
                        if (ticketDates.Contains(date.Day)
                            && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                        {
                            Ticket ticket = new Ticket();
                            ticket.TicketId = Guid.NewGuid().ToString();
                            ticket.TicketType = ticketTypes[new Random().Next(0, ticketTypes.Count - 1)];
                            ticket.OnBehalf = item;
                            ticket.Agent = agents[new Random().Next(0, agents.Count - 1)];
                            ticket.TicketName = date.ToString("yyyy-MM-dd") + " " + ticket.OnBehalf.EmployeeIDNumber;
                            ticket.Description = ticket.OnBehalf.FirstName + " " + ticket.OnBehalf.LastName + " Asking About " + ticket.TicketType.Name;
                            ticket.SubmitDate = date;
                            ticket.IsSolve = new Random().Next(0, 2) == 1 ? true : false;
                            ticket.SolutionNote = "Update info with adjustment.";
                            await _context.Ticket.AddAsync(ticket);
                        }

                    }

                    await _context.SaveChangesAsync();

                }
                

                //insert todo
                List<TodoType> todoTypes = new List<TodoType>();
                todoTypes = _context.TodoType.ToList();

                DateTime startTodo = new DateTime(2021, 1, 1);
                DateTime endTodo = DateTime.Now;

                //set random todo dates
                List<int> todoDates = new List<int>();
                for (int i = 0; i < 8; i++)
                {
                    int randomDate = new Random().Next(1, 30);
                    if (!todoDates.Contains(randomDate))
                    {
                        todoDates.Add(randomDate);
                    }

                }

                for (DateTime date = startTodo.Date; date < endTodo.Date; date = date.AddDays(1))
                {
                    if (todoDates.Contains(date.Day)
                        && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                    {
                        Todo todo = new Todo();
                        todo.TodoId = Guid.NewGuid().ToString();
                        todo.TodoType = todoTypes[new Random().Next(0, todoTypes.Count - 1)];
                        todo.OnBehalf = agents[new Random().Next(0, agents.Count - 1)];
                        todo.TodoItem = date.ToString("yyyy-MM-dd") + " " + todo.OnBehalf.EmployeeIDNumber;
                        todo.Description = todo.OnBehalf.FirstName + " " + todo.OnBehalf.LastName + " Handling " + todo.TodoType.Name;
                        todo.StartDate = date;
                        todo.EndDate = date;
                        todo.IsDone = new Random().Next(0, 2) == 1 ? true : false;
                        await _context.Todo.AddAsync(todo);
                    }

                }
                await _context.SaveChangesAsync();


                //insert expense
                List<decimal> expenseAmount = new List<decimal>();
                expenseAmount.Add(250m);
                expenseAmount.Add(270m);
                expenseAmount.Add(300m);
                expenseAmount.Add(310m);
                expenseAmount.Add(325m);
                expenseAmount.Add(350m);
                expenseAmount.Add(357m);
                expenseAmount.Add(410m);
                expenseAmount.Add(450m);

                List<ExpenseType> expenseTypes = new List<ExpenseType>();
                expenseTypes = _context.ExpenseType.ToList();

                foreach (var item in employees)
                {
                    DateTime startExpense = new DateTime(2021, 1, 1);
                    DateTime endExpense = DateTime.Now;

                    //set random expense dates
                    List<int> expenseDates = new List<int>();
                    for (int i = 0; i < 5; i++)
                    {
                        int randomDate = new Random().Next(1, 30);
                        if (!expenseDates.Contains(randomDate))
                        {
                            expenseDates.Add(randomDate);
                        }

                    }

                    for (DateTime date = startExpense.Date; date < endExpense.Date; date = date.AddDays(1))
                    {
                        if (expenseDates.Contains(date.Day)
                            && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                        {
                            Expense expense = new Expense();
                            expense.ExpenseId = Guid.NewGuid().ToString();
                            expense.ExpenseType = expenseTypes[new Random().Next(0, expenseTypes.Count - 1)];
                            expense.OnBehalf = item;
                            expense.ExpenseName = date.ToString("yyyy-MM-dd") + " " + expense.OnBehalf.EmployeeIDNumber;
                            expense.Description = expense.OnBehalf.FirstName + " " + expense.OnBehalf.LastName + " " + expense.ExpenseType.Name;
                            expense.FromDate = date;
                            expense.ToDate = date;
                            expense.IsApproved = new Random().Next(0, 2) == 1 ? true : false;
                            expense.ExpenseAmount = expenseAmount[new Random().Next(0, 8)];
                            expense.IsCashAdvance = new Random().Next(0, 2) == 1 ? true : false;
                            await _context.Expense.AddAsync(expense);
                        }

                    }
                }


                await _context.SaveChangesAsync();


                //insert information
                List<InformationType> informationTypes = new List<InformationType>();
                informationTypes = _context.InformationType.ToList();

                DateTime startInformation = new DateTime(2021, 1, 1);
                DateTime endInformation = DateTime.Now;

                //set random information dates
                List<int> informationDates = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    int randomDate = new Random().Next(1, 30);
                    if (!informationDates.Contains(randomDate))
                    {
                        informationDates.Add(randomDate);
                    }

                }

                for (DateTime date = startInformation.Date; date < endInformation.Date; date = date.AddDays(1))
                {
                    if (informationDates.Contains(date.Day)
                        && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                    {
                        Information info = new Information();
                        info.InformationId = Guid.NewGuid().ToString();
                        info.InformationType = informationTypes[new Random().Next(0, informationTypes.Count - 1)];
                        info.InformationName = date.ToString("yyyy-MM-dd") + " " + info.InformationType.Name;
                        info.Description = "";
                        info.ExternalLink = "https://www.google.com";
                        info.ReleaseDate = date;
                        info.IsActive = true;
                        await _context.Information.AddAsync(info);
                    }

                }
                await _context.SaveChangesAsync();

                //insert asset
                List<string> excludeAssets = new List<string>();
                excludeAssets.Add("Laptop");
                excludeAssets.Add("PPE");
                excludeAssets.Add("Patents");
                excludeAssets.Add("Land");
                excludeAssets.Add("Buildings");

                List<decimal> prices = new List<decimal>();
                prices.Add(1500m);
                prices.Add(1700m);
                prices.Add(2500m);
                prices.Add(2700m);
                prices.Add(3200m);

                List<AssetType> assetTypes = new List<AssetType>();
                assetTypes = _context.AssetType.Where(x => !excludeAssets.Contains(x.Name))
                    .ToList();

                DateTime startAsset = new DateTime(2021, 2, 1);
                DateTime endAsset = DateTime.Now;

                //set random asset dates
                List<int> assetDates = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    int randomDate = new Random().Next(1, 30);
                    if (!assetDates.Contains(randomDate))
                    {
                        assetDates.Add(randomDate);
                    }

                }

                //laptop asset
                AssetType assetLaptop = new AssetType();
                assetLaptop = _context.AssetType.Where(x => x.Name.Equals("Laptop")).FirstOrDefault();
                int laptop = 0;
                foreach (var item in employees)
                {
                    laptop++;

                    Asset asset = new Asset();
                    asset.AssetId = Guid.NewGuid().ToString();
                    asset.AssetType = assetLaptop;
                    asset.PurchaseDate = new DateTime(2021, 1, new Random().Next(1, 28));
                    asset.PurchasePrice = prices[new Random().Next(0, 4)];
                    asset.AssetName = asset.PurchaseDate.ToString("yyyy-MM-dd") + " " + asset.AssetType.Name + " " + laptop.ToString();
                    asset.Description = "";
                    asset.UsedBy = item;
                    asset.IsActive = true;
                    await _context.Asset.AddAsync(asset);
                }

                //other asset
                for (DateTime date = startAsset.Date; date < endAsset.Date; date = date.AddDays(1))
                {
                    if (assetDates.Contains(date.Day)
                        && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                    {
                        Asset asset = new Asset();
                        asset.AssetId = Guid.NewGuid().ToString();
                        asset.AssetType = assetTypes[new Random().Next(0, assetTypes.Count - 1)];
                        asset.AssetName = date.ToString("yyyy-MM-dd") + " " + asset.AssetType.Name;
                        asset.Description = "";
                        asset.PurchaseDate = date;
                        asset.PurchasePrice = prices[new Random().Next(0, 4)];
                        asset.UsedBy = new Random().Next(0, 2) == 1 ? employees[new Random().Next(0, employees.Count - 1)] : null;
                        asset.IsActive = true;
                        await _context.Asset.AddAsync(asset);
                    }

                }
                await _context.SaveChangesAsync();


                //insert public holiday 2021
                PublicHoliday publicHoliday2021 = new PublicHoliday();
                publicHoliday2021.PublicHolidayId = Guid.NewGuid().ToString();
                publicHoliday2021.Name = "2021 Holiday";
                publicHoliday2021.Description = "Federal and Regional";

                _context.PublicHoliday.Add(publicHoliday2021);
                await _context.SaveChangesAsync();

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "New Year's Day",
                    PublicHolidayDate = new DateTime(2021, 1, 1),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Martin Luther King Jr. Day",
                    PublicHolidayDate = new DateTime(2021, 1, 21),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "President's Day",
                    PublicHolidayDate = new DateTime(2021, 2, 18),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Mother's Day",
                    PublicHolidayDate = new DateTime(2021, 5, 12),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Memorial Day",
                    PublicHolidayDate = new DateTime(2021, 5, 27),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Father's Day",
                    PublicHolidayDate = new DateTime(2021, 6, 16),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Independence Day",
                    PublicHolidayDate = new DateTime(2021, 7, 4),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Labor Day",
                    PublicHolidayDate = new DateTime(2021, 9, 2),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Columbus Day",
                    PublicHolidayDate = new DateTime(2021, 10, 14),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "US Indigenous People's Day",
                    PublicHolidayDate = new DateTime(2021, 10, 14),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Veterans Day",
                    PublicHolidayDate = new DateTime(2021, 11, 11),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Thanksgiving",
                    PublicHolidayDate = new DateTime(2021, 11, 28),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Day after Thanksgiving",
                    PublicHolidayDate = new DateTime(2021, 11, 29),
                    PublicHolidayYear = 2021
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHolidayLineId = Guid.NewGuid().ToString(),
                    PublicHoliday = publicHoliday2021,
                    Description = "Chirstmas Day",
                    PublicHolidayDate = new DateTime(2021, 12, 25),
                    PublicHolidayYear = 2021
                });

                await _context.SaveChangesAsync();

                //insert attendance
                DateTime startAttendance = new DateTime(2021, 1, 1);
                DateTime endAttendance = DateTime.Now;
                List<DateTime> dateHolidays = _context.PublicHolidayLine
                    .Select(x => x.PublicHolidayDate.Date)
                    .ToList();

                List<Employee> employeesAttendance = new List<Employee>();
                employeesAttendance = _context.Employee.ToList();

                for (DateTime date = startAttendance.Date; date < endAttendance.Date; date = date.AddDays(1))
                {
                    if (
                        date.DayOfWeek != DayOfWeek.Saturday 
                        && date.DayOfWeek != DayOfWeek.Sunday                        
                       )
                    {
                        //and not holidays
                        if (!dateHolidays.Contains(date.Date))
                        {
                            //apply to all employee                        
                            foreach (var item in employeesAttendance)
                            {

                                //randomize choose employee for attendance
                                if (new Random().Next(0, 4) != 1)
                                {

                                    Attendance attIn = new Attendance();
                                    attIn.AttendanceId = Guid.NewGuid().ToString();
                                    //randomize the minute
                                    DateTime clockIn = new DateTime(date.Year, date.Month, date.Day, 7, new Random().Next(0, 30), 0);

                                    attIn.AttendanceName = clockIn.ToString("yyyy-MM-dd HH:mm:ss") + " " + item.EmployeeIDNumber;
                                    attIn.Clock = clockIn;
                                    attIn.OnBehalf = item;
                                    await _context.Attendance.AddAsync(attIn);


                                    Attendance attOut = new Attendance();
                                    attOut.AttendanceId = Guid.NewGuid().ToString();
                                    //randomize the minute
                                    DateTime clockOut = new DateTime(date.Year, date.Month, date.Day, 17, new Random().Next(0, 30), 0);

                                    attOut.AttendanceName = clockOut.ToString("yyyy-MM-dd HH:mm:ss") + " " + item.EmployeeIDNumber;
                                    attOut.Clock = clockOut;
                                    attOut.OnBehalf = item;
                                    await _context.Attendance.AddAsync(attOut);
                                }



                            }

                            await _context.SaveChangesAsync();
                        }                        

                    }
                }


                //generate salary per employee per month
                int endMonth = DateTime.Now.Month;
                for (int i = 1; i <= endMonth; i++)
                {
                    DateTime periodDate = new DateTime(2021, i, 1);
                    string period = periodDate.ToString("yyyy-MM");

                    foreach (var item in employees)
                    {
                        await _app.GenerateSalaryByEmployeeByPeriod(
                            item.EmployeeId, 
                            period, 
                            new Random().Next(0, 2) == 1, 
                            new Random().Next(0, 2) == 1
                            );
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
