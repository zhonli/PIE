namespace PIEM.Database
{
    using System.Data.Entity;
    using PIEM.Common.Model;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Config;
    public partial class PIEMContext : DbContext
    {
        public PIEMContext()
            : base("name=PIEMContext")
        {
            //Database.Log = s => Debug.WriteLine(s);
        }

        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        //public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }

        public virtual DbSet<Process> Processes { get; set; }
        public virtual DbSet<ProcessTask> ProcessTasks { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<ResultSummary> ResultSummaries { get; set; }

        public virtual DbSet<ResultStats> ResultStats { get; set; }

        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<TaskLink> TaskLinks { get; set; }
        //public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TestCollateral> TestCollaterals { get; set; }
        public virtual DbSet<TestCollateralItem> TestCollateralItems { get; set; }
        //public virtual DbSet<SanityCheck> SanityChecks { get; set; }

        //public virtual DbSet<TaskHistoryWork> TaskHistoryWorks { get; set; }
        //public virtual DbSet<Email> Emails { get; set; }
        //public virtual DbSet<EmailSchedule> EmailSchedules { get; set; }
        //public virtual DbSet<TaskBugMapping> TaskBugMappings { get; set; }
        //public virtual DbSet<BugTestResultMapping> BugTestResultMappings { get; set; }
        public virtual DbSet<WorkItemSource> WorkItemSources { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Configurations.Add(new FeatureConfiguration());
            modelBuilder.Configurations.Add(new LanguageConfiguration());
            modelBuilder.Configurations.Add(new PlanConfiguration());
            modelBuilder.Configurations.Add(new ProcessConfig());
            modelBuilder.Configurations.Add(new TransactionConfig()); 
            modelBuilder.Configurations.Add(new ResultSummaryConfiguration());
            modelBuilder.Configurations.Add(new ResultStatsConfiguration());
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Configurations.Add(new ResourceConfiguration());
            //modelBuilder.Configurations.Add(new SprintConfiguration());
            modelBuilder.Configurations.Add(new TaskLinkConfiguration());
            modelBuilder.Configurations.Add(new AssignmentConfiguration());
            modelBuilder.Configurations.Add(new TeamConfiguration());
            modelBuilder.Configurations.Add(new TestCollateralConfiguration());
            modelBuilder.Configurations.Add(new TestCollateralItemConfiguration());

            //modelBuilder.Entity<Department>().MapToStoredProcedures();
        }
    }
}
