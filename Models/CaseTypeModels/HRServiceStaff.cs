using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resolve.Models
{
    public enum RequestType
    {
        [Display(Name = "Compensation Base Pay Change")]
        Base,
        [Display(Name = "Compensation Allowance Change")]
        Allowance,
        [Display(Name = "Distribution Change")]
        Distribution,
        [Display(Name = "Employment End Date Change")]
        Enddate,
        [Display(Name = "FTE Change")]
        FTE,
        [Display(Name = "Move Worker")]
        Move,
        [Display(Name = "Termination")]
        Termination,
        [Display(Name = "Other")]
        Other
    }
    public enum BasePayChange
    {
        [Display(Name = "Ingrade – Change in Responsibilities")]
        IngradeChange,
        [Display(Name = "Ingrade – Competitive Offer(non-UW)")]
        IngradeCompete,
        [Display(Name = "Ingrade – Market/Retention")]
        IngradeRetain,
        [Display(Name = "Ingrade – Merit/Increased Functioning")]
        IngradeMerit,
        [Display(Name = "Ingrade – Preemptive Offer(non-UW)")]
        IngradePrempt,
        [Display(Name = "Internal Equity")]
        InternalEquity,
        [Display(Name = "Salary / Hourly Rate Reduction")]
        HourlyReduction,
        [Display(Name = "Step Adjustment – Career Enhancement/Growth Program(CEGP)")]
        StepCareer,
        [Display(Name = "Step Adjustment – Contractual Change")]
        StepContract,
        [Display(Name = "Step Adjustment – Other")]
        StepOther,
        [Display(Name = "Step Adjustment –Recruitment / Retention Adjustment")]
        StepRecruit,
        [Display(Name = "Maintain Progression Start Date")]
        MaintainProgress
    }
    public enum AllowanceChange
    {
        [Display(Name = "Administrative Supplement")]
        AdministrativeSupplement,
        [Display(Name = "Alaska RN Assignment Pay")]
        AlaskaRN,
        [Display(Name = "Car Allowance")]
        CarAllowance,
        [Display(Name = "Chief Resident Supplement")]
        ChiefResident,
        [Display(Name = "Car Allowance")]
        DentistryPractice,
        [Display(Name = " Employment Contract Allowance")]
        EmploymentContract,
        [Display(Name = "Endowed Supplement")]
        EndowedSupplement,
        [Display(Name = "Excess Compensation")]
        ExcessCompensation,
        [Display(Name = "Extension Lecture Summer Quarter Premium")]
        ExtensionPremium,
        [Display(Name = "International Location Allowance")]
        InternationalAllowance,
        [Display(Name = "K9 Officer Premium")]
        K9Premium,
        [Display(Name = "Language Premium")]
        LanguagePremium,
        [Display(Name = "Mobile Service Agreement")]
        MobileService,
        [Display(Name = "Teaching Assistant Summer Quarter Premium")]
        TeachingAssistant,
        [Display(Name = "Temporary Pay Increase")]
        TemporaryIncrease,
        [Display(Name = "Temporary Pay Supplement")]
        TemporarySupplement,
        [Display(Name = "Temporary Recruitment and Retention Increase CNU")]
        TemporaryRetention,
        [Display(Name = "Temporary Salary Increase")]
        TemporarySalary,
        [Display(Name = "UWPMA/Teamsters117 Longevity Pay")]
        LongevityPay
    }

    public enum WorkerType
    {
        [Display(Name = "Student")]
        Student,
        [Display(Name = "Staff/Classified")]
        Classified,
        [Display(Name = "Staff/Professional")]
        Professional
    }

    public enum SupOrg
    {
        [Display(Name = "Clinical Accounting: Cash Applications")] Clinical,
        [Display(Name = "Deans Office: Clinical Affairs")] Dean1,
        [Display(Name = "Deans Office: Compliance (Brown, Robert S.)")] Dean2,
        [Display(Name = "Deans Office: Compliance JM Student (Brown, Robert S.)")] Dean3,
        [Display(Name = "Deans Office: Deans Office Administration")] Dean4,
        [Display(Name = "Deans Office: Deans Office Administration JM Student (Wee, Christina)")] Dean5,
        [Display(Name = "Deans Office: Dental Maintenance (Fox, David G.)")] Dean6,
        [Display(Name = "Deans Office: Educational Partnerships & Diversity JM Contingent Worker")] Dean7,
        [Display(Name = "Deans Office: Educational Partnerships & Diversity (Gandara, Beatrice K)")] Dean8,
        [Display(Name = "Deans Office: Finance Administration (Farris, Gary E)")] Dean9,
        [Display(Name = "Deans Office: Information Management (Farris, Gary E)")] Dean10,
        [Display(Name = "Deans Office: Qi & Safety (Phillips, Sandra L)")] Dean11,
        [Display(Name = "Deans Office: School of Dentistry IT (Ruddle, Tom)")] Dean12,
        [Display(Name = "Deans Office: School of Dentistry IT JM Student")] Dean13,
        [Display(Name = "Deans Office: Student Services (Sperry, Chanira R.)")] Dean14,
        [Display(Name = "Deans Office: Student Services JM Student (Sperry, Chanira R.)")] Dean15,
        [Display(Name = "Dental Clinic: Clinic Operations (Mihwa Kim)")] DentalClinic1,
        [Display(Name = "Dental Clinic: Clinic Operations- DUCC (Serquinia, Cherylene)")] DentalClinic2,
        [Display(Name = "Dental Clinic: Patient Intake (Charlene Strong)")] DentalClinic3,
        [Display(Name = "Dental Clinic: Patient Relations (Phillips, Sandra L)")] DentalClinic4,
        [Display(Name = "Dental Clinic: Pre-doc Clinic Coordination (Schwedhelm, E. Ricardo)")] DentalClinic5,
        [Display(Name = "Dental Clinic: Pre-doc Clinics ")] DentalClinic6,
        [Display(Name = "Dental Clinic: Pre-doc Clinic Coordination JM Student (Schwedhelm, E. Ricardo)")] DentalClinic7,
        [Display(Name = "Dental Clinic: Sterilization (Harvey, Carol J)")] DentalClinic8,
        [Display(Name = "Department of Endodontics (Johnson, James D)")] Endo1,
        [Display(Name = "Department of Endodontics JM Academic (Johnson, James D)")] Endo2,
        [Display(Name = "Endodontics: Administration (Collins, Margaret M)")] Endo3,
        [Display(Name = "Endodontics: Grad Endo Clinic (Ullberg, Moira)")] Endo4,
        [Display(Name = "Endodontics: Grad Endo Clinic JM Students (Ullberg, Moira)")] Endo5,
        [Display(Name = "External Affairs: Advancement (Newquist, Randall)")] ExternalAffairs1,
        [Display(Name = "External Affairs: Advancement JM Student (Newquist, Randall)")] ExternalAffairs2,
        [Display(Name = "External Affairs: Continuing Dental Education (Gee, Sally A.)")] ExternalAffairs3,
        [Display(Name = "External Affairs: Continuing Dental Education JM Student (Gee, Sally A.)")] ExternalAffairs4,
        [Display(Name = "Faculty Practice Administration (Eanes, Debbie)")] FacPrac,
        [Display(Name = "Finance Admin: Banking Operations (Pike, Pam)")] Finance1,
        [Display(Name = "Finance Admin: Revenue Cycles (Lowe, LaMar Andre)")] Finance2,
        [Display(Name = "Finance Admin: Clinical Coding (Lowe, LaMar Andre)")] Finance3,
        [Display(Name = "Finance Admin: Dental Central Purchasing (Douglas, Teresa N.)")] Finance4,
        [Display(Name = "Finance Admin: Clinical Services Accounting (Alpert, Lara)")] Finance5,
        [Display(Name = "Office of Academic Affairs (Gordon, Sara C)")] Academic1,
        [Display(Name = "Office of Regional Affairs (RIDE) (Grant, Jennifer H.)")] RIDE1,
        [Display(Name = "Office of Research (LeResche, Linda)")] Research1,
        [Display(Name = "Office of Research: Research Administration JM Resident/Fellow (Sim, Sang)")] Research2,
        [Display(Name = "Office of Research: Research Administration JM Student (Sim, Sang)")] Research3,
        [Display(Name = "Office of Student Services & Admissions (Coldwell, Susan)")] StudentService1,
        [Display(Name = "Office of Student Services & Admissions JM Student (Brock, Memory)")] StudentService2,
        [Display(Name = "Department of Oral And Maxillo Facial Surgery (Dodson, Thomas B)")] OMS1,
        [Display(Name = "Department of Oral And Maxillo Facial Surgery JM Academic (Dodson, Thomas B)")] OMS2,
        [Display(Name = "Department of Oral And Maxillo Facial Surgery JM Resident/Fellow (Dillon, Jasjit K)")] OMS3,
        [Display(Name = "Department of Oral Health Sciences (Ramsay, Douglas S)")] OHS1,
        [Display(Name = "Department of Oral Health Sciences JM Academic (Ramsay, Douglas S)")] OHS2,
        [Display(Name = "Department of Oral Health Sciences JM Resident/Fellow (Ramsay, Douglas S)")] OHS3,
        [Display(Name = "Oral Health Sciences: Administation (Kakida, Eileen)")] OHS4,
        [Display(Name = "Oral Health Sciences: Clinical (Fears Clinic) (Aw, Tar-Chee)")] OHS5,
        [Display(Name = "Oral Health Sciences: Maxpro Clinic (Rubenstein, Jeffrey E)")] OHS6,
        [Display(Name = "Oral Health Sciences: RCDRC (Rothen, Marilynn L.)")] OHS7,
        [Display(Name = "Oral Health Sciences: Research- Chi JM Resident/Fellow (Ramsay, Douglas S)")] OHS8,
        [Display(Name = "Oral Health Sciences: Research- Chi JM Student (Chi, Donald L)")] OHS9,
        [Display(Name = "Oral Health Sciences: Research- Leroux (Leroux, Brian G)")] OHS10,
        [Display(Name = "Oral Health Sciences: Research- Ramsay (Ramsay, Douglas S)")] OHS11,
        [Display(Name = "Oral Health Sciences: Research- Silva (Cunha-Cruz, Joana)")] OHS12,
        [Display(Name = "Oral Health Sciences: Research- Chi Staff (Chi, Donald L)")] OHS13,
        [Display(Name = "Department of Oral Medicine (Drangsholt, Mark Thomas)")] OralMed1,
        [Display(Name = "Department of Oral Medicine JM Academic (Drangsholt, Mark Thomas)")] OralMed2,
        [Display(Name = "Department of Oral Medicine JM Resident/Fellow (Dean, David R.)")] OralMed3,
        [Display(Name = "Oral Medicine: Administration & OM Clinic (Sebring, Dalila V.)")] OralMed4,
        [Display(Name = "Oral Medicine: Administration & OM Clinic JM Student (Sebring, Dalila V.)")] OralMed5,
        [Display(Name = "Oral Medicine: DECOD Clinic (Espinoza, Kimberly)")] OralMed6,
        [Display(Name = "Department of Oral Medicine: Oral Maxillofacial Radiology JM Resident/Fellow (Chen, Curtis SK)")] OralMed7,
        [Display(Name = "Oral Surgery: Administration (Doyle, Bridget A.)")] OralSurg1,
        [Display(Name = "Oral Surgery: Administration JM Student (Doyle, Bridget A.)")] OralSurg2,
        [Display(Name = "Oral Surgery: AGD_JM_Student (Herrera Velasco, Ana)")] OralSurg3,
        [Display(Name = "Oral Surgery: GPR Residency JM Resident/Fellow (O'Connor, Ryan T.)")] OralSurg4,
        [Display(Name = "Oral Surgery: OP (Oda, Dolphine)")] OralSurg5,
        [Display(Name = "Oral Surgery: OP JM Student (Oda, Dolphine)")] OralSurg6,
        [Display(Name = "Oral Surgery: OS Clinical (Doyle, Bridget A.)")] OralSurg7,
        [Display(Name = "Oral Surgery: OS Clinical Services (McCloud, Mesha)")] OralSurg8,
        [Display(Name = "Oral Surgery: OS Clinical Services JM Student (McCloud, Mesha)")] OralSurg9,
        [Display(Name = "Oral Surgery: Patient Services (Johnson, Rebecca C)")] OralSurg10,
        [Display(Name = "Oral Surgery: Patient Services JM Student (Johnson, Rebecca C)")] OralSurg11,
        [Display(Name = "Department of Orthodontics (Huang, Greg J.)")] Ortho1,
        [Display(Name = "Department of Orthodontics JM Academic (Huang, Greg J.)")] Ortho2,
        [Display(Name = "Department of Orthodontics JM Contingent Worker (Huang, Greg J.)")] Ortho3,
        [Display(Name = "Orthodontics: Administrative (Horishige, Bette) ")] Ortho4,
        [Display(Name = "Orthodontics: Administrative JM Student (Horishige, Bette) ")] Ortho5,
        [Display(Name = "Orthodontics: Clinic (Greenlee, Geoffrey M)")] Ortho6,
        [Display(Name = "Orthodontics: Research (Herring, Susan W)")] Ortho7,
        [Display(Name = "Othodontics: Research JM Student (Herring, Susan W)")] Ortho8,
        [Display(Name = "Department of Pediatric Dentistry (Nelson, Travis)")] Pedo1,
        [Display(Name = "Department of Pediatric Dentistry JM Academic (Nelson, Travis)")] Pedo2,
        [Display(Name = "Department of Pediatric Dentistry JM Resident/Fellow (Xu, Zheng)")] Pedo3,
        [Display(Name = "Pediatric Dentistry: ABCD Research (Kim, Amy S)")] Pedo4,
        [Display(Name = "Pediatric Dentistry: Administration (Phantumvanit, Eve)")] Pedo5,
        [Display(Name = "Pediatric Dentistry: CPD Clinic (Phantumvanit, Eve)")] Pedo6,
        [Display(Name = "Pediatric Dentistry: Dental Assistants (Harris, Essence)")] Pedo7,
        [Display(Name = "Pediatric Dentistry: Dental Surgical Center ")] Pedo8,
        [Display(Name = "Pediatric Dentistry: Patient Services (Ogden, Joanne)")] Pedo9,
        [Display(Name = "Pediatric Dentistry: Patient Services JM Student (Ogden, Joanne)")] Pedo10,
        [Display(Name = "Pediatric Dentistry: Support Staff (Quintero, Elide)")] Pedo11,
        [Display(Name = "Department of Periodontics (Darveau, Richard P.)")] Perio1,
        [Display(Name = "Department of Periodontics JM Academic (Darveau, Richard P.)")] Perio2,
        [Display(Name = "Periodontics: Administrative (Collins, Margaret M)")] Perio3,
        [Display(Name = "Periodontics: Administrative JM Student (Collins, Margaret M)")] Perio4,
        [Display(Name = "Periodontics: Grad Perio Clinic Staff (Daubert, Diane M.)")] Perio5,
        [Display(Name = "Periodontics: Research Darveau (Darveau, Richard P.)")] Perio6,
        [Display(Name = "Periodontics: Research Mclean (McLean, Jeffrey S)")] Perio7,
        [Display(Name = "Periodontics: Research Mclean JM Resident/Fellow (McLean, Jeffrey S)")] Perio8,
        [Display(Name = "Department of Restorative Dentistry (Chan, Daniel C. N.)")] Restorative1,
        [Display(Name = "Department of Restorative Dentistry JM Academic (Chan, Daniel C. N.)")] Restorative2,
        [Display(Name = "Restorative Dentistry: Administration (Low, Betty)")] Restorative3,
        [Display(Name = "Restorative Dentistry: Administration JM Student (Low, Betty)")] Restorative4,
        [Display(Name = "Restorative Dentistry: Clinic Staff (Schwedhelm, E. Ricardo)")] Restorative5,
        [Display(Name = "Restorative Dentistry: Grad Pros Clinic Admin (RAMOS, SERVANDO)")] Restorative6,
        [Display(Name = "Restorative Dentistry: Grad Pros Clinic staff (Green, Carole K.)")] Restorative7,
        [Display(Name = "School of Dentistry (Chiodo, Gary T)")] SOD1,
        [Display(Name = "Office of Regional Affairs JM Student (RIDE) (Grant, Jennifer H.)")] RIDE2,
        [Display(Name = "UW Dentistry Campus Dental Center (UWDCDC)")] UWDCDC1,
        [Display(Name = "UW Dentistry Campus Dental Center Operations")] UWDCDC2,
        [Display(Name = "Deans Office: D1 Simulation JM Student")] Dean16,
        [Display(Name = "Deans Office: Derouen Center")] Dean17,
        [Display(Name = "Deans Office: Human Resources Operations")] Dean18,
        [Display(Name = "Dental Clinic: Patient Intake JM Student")] DentalClinic9,
        [Display(Name = "Office of Regional Affairs Leadership")] RIDE3,
        [Display(Name = "Oral Health Sciences: Research- Silva JM Student (Cunha-Cruz, Joana)")] OHS14,
        [Display(Name = "Dental Clinic: Pre-Doc Clinics (Baird, Christine)")] DentalClinic10

    }
    public class HRServiceStaff
    {
        [Required, Key, ForeignKey("Case")]
        public int CaseID { get; set; }
        public Case Case { get; set; }

       [Display(Name = "Effective Start Date")]
        [DataType(DataType.Date)]
        public DateTime EffectiveStartDate { get; set; }

        [Display(Name = "Effective End/Termination Date")]
        [DataType(DataType.Date)]
        public DateTime? EffectiveEndDate { get; set; }

        [Display(Name = "Request Type"), Required]
        public virtual RequestType RequestType { get; set; }

        [Display(Name = "Worker Type"), Required]
        public virtual WorkerType WorkerType { get; set; }

        [Display(Name = "Compensation Base Pay Change")]
        public virtual BasePayChange? BasePayChange { get; set; }

        [Display(Name = "Compensation Allowance Change")]
        public virtual AllowanceChange? AllowanceChange { get; set; }

        [Display(Name = "Termination Reason")]
        public virtual TerminationReason? TerminationReason { get; set; }

        public virtual SupOrg? SupOrg { get; set; }

        public string EmployeeEID { get; set; }

        [Display(Name = "Employee Name"), Required]
        public string EmployeeName { get; set; }

        [Display(Name = "Budget Numbers")]
        public string BudgetNumbers { get; set; }

        [Display(Name = "Amount/Percent/Step Increase")]
        public string Amount { get; set; }


        [MaxLength(1024)]
        public string Note { get; set; }

        [Display(Name = "Offboarding?")]
        public bool Offboarding { get; set; }

        [Display(Name = "Close Position?")]
        public bool ClosePosition { get; set; }

        [Display(Name = "Leave WA?")]
        public bool LeaveWA { get; set; }
    }
}
