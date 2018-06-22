using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Reflection;
using System.Linq.Expressions;
using EntityFramework.Filters;
using RefactorThis.GraphDiff;
using System.Data.Entity.Core.Objects;
using EFSecondLevelCache;
using System.Data.Entity.Validation;
using MyWeb.DomainClasses.Configurations;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DataLayer.Context
{
    public class MyWebContext : DbContext, IUnitOfWork
    {
        #region Ctor
        public MyWebContext()
            : base("MyWebConnection")
        {
        }

        #endregion

        #region Override OnModelCreating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Configurations.AddFromAssembly(typeof(UserConfig).Assembly);
            LoadEntities(typeof(User).Assembly, modelBuilder, "MyWeb.DomainClasses.Entities");
        }

        #endregion


        #region SaveChanges
        public int SaveAllChanges(bool invalidateCacheDependencies = true)
        {
            var result = SaveChanges();
            if (!invalidateCacheDependencies) return result;
            var changedEntityNames = GetChangedEntityNames();
            new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            return result;
        }
        public Task<int> SaveAllChangesAsync(bool invalidateCacheDependencies = true)
        {

            try
            {

                var result = SaveChangesAsync();
                if (!invalidateCacheDependencies) return result;
                var changedEntityNames = GetChangedEntityNames();
                new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);

                return result;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        #endregion

        private string[] GetChangedEntityNames()
        {
            return ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added ||
                            x.State == EntityState.Modified ||
                            x.State == EntityState.Deleted)
                .Select(x => ObjectContext.GetObjectType(x.Entity.GetType()).FullName)
                .Distinct()
                .ToArray();
        }

        private static void LoadEntities(Assembly asm, DbModelBuilder modelBuilder, string nameSpace)
        {
            var entityTypes = asm.GetTypes()
                .Where(type => type.BaseType != null &&
                               type.BaseType != Type.GetType("System.Enum") &&
                               //type.Name != "Entity" &&
                               !type.Name.Contains("Entity") &&
                               !type.Name.Contains("Base") &&
                               //type.Name != "BaseEntity" &&
                               type.Namespace != null &&
                               type.Namespace.Contains(nameSpace))
                //.Where(type => type.BaseType != null &&
                //               type.Namespace == nameSpace &&
                //               type.BaseType == null)
                .ToList();

            entityTypes.ForEach(modelBuilder.RegisterEntityType);
        }


        #region Dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion



        #region IUnitOfWork
        public T Update<T>(T entity, Expression<Func<IUpdateConfiguration<T>, object>> mapping)
            where T : class, new()
        {
            return this.UpdateGraph(entity, mapping);
        }
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }
        public void MarkAsDetached<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Detached;
        }
        public void MarkAsAdded<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Added;
        }

        public void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public IList<T> GetRows<T>(string sql, params object[] parameters) where T : class
        {
            return Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public void AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            ((DbSet<TEntity>)Set<TEntity>()).AddRange(entities);
        }
        public void ForceDatabaseInitialize()
        {
            Database.Initialize(force: true);
        }

        public void EnableFiltering(string name)
        {
            this.EnableFilter(name);
        }

        public void EnableFiltering(string name, string parameter, object value)
        {
            this.EnableFilter(name).SetParameter(parameter, value);
        }

        public void DisableFiltering(string name)
        {
            this.DisableFilter(name);
        }

        public bool ValidateOnSaveEnabled
        {
            get { return Configuration.ValidateOnSaveEnabled; }
            set { Configuration.ValidateOnSaveEnabled = value; }
        }

        public bool ProxyCreationEnabled
        {
            get { return Configuration.ProxyCreationEnabled; }
            set { Configuration.ProxyCreationEnabled = value; }
        }

        bool IUnitOfWork.AutoDetectChangesEnabled
        {
            get { return Configuration.AutoDetectChangesEnabled; }
            set { Configuration.AutoDetectChangesEnabled = value; }
        }

        public bool ForceNoTracking { get; set; }
        #endregion

        //public DbSet<IncomingMessage> IncomingMessages { get; set; }

        //public DbSet<OutgoingMessage> OutgoingMessage { get; set; }

        public DbSet<User> Users { get; set; }

        //public DbSet<Setting> Settings { get; set; }

        //public DbSet<Contact> Contacts { get; set; }

        public DbSet<Role> Roles { get; set; }

    }
}
