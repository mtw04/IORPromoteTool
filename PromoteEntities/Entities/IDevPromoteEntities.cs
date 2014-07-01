namespace IORPromoteTool.IORPromoteEntities
{
    using System;
    using System.Data.Entity;
    using System.Data.Objects;

    /// <summary>
    /// This extracts the interface from the generated DevAnswersEntities class.
    /// </summary>
    public interface IDevPromoteEntities : IDbContext
    {
        /// <summary>
        /// This is the same as the DbContext Set only it returns IDbSet instead of DbSet.
        /// Returns a non-generic DbSet instance for access to entities of the given type in the context, the ObjectStateManager, and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns> A set for the given entity type. </returns>
        new IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        #region Extracted interface from generated DevAnswersEntities class.

        //DbSet<AccountType> AccountTypes { get; set; }
        //DbSet<Action> Actions { get; set; }
        //DbSet<Badge> Badges { get; set; }
        //DbSet<Case> Cases { get; set; }
        //DbSet<ExcerptPost> ExcerptPosts { get; set; }
        //DbSet<IndustryGroupTopicMapping> IndustryGroupTopicMappings { get; set; }
        //DbSet<LDAPServer> LDAPServers { get; set; }
        //DbSet<Option> Options { get; set; }
        //DbSet<Post> Posts { get; set; }
        //DbSet<PostTag> PostTags { get; set; }
        //DbSet<PostType> PostTypes { get; set; }
        //DbSet<PostView> PostViews { get; set; }
        //DbSet<ReputationRule> ReputationRules { get; set; }
        //DbSet<RoleRelation> RoleRelations { get; set; }
        //DbSet<Role> Roles { get; set; }
        //DbSet<RoleTag> RoleTags { get; set; }
        //DbSet<SearchCatalog> SearchCatalogs { get; set; }
        //DbSet<SiteSetting> SiteSettings { get; set; }
        //DbSet<TagFollower> TagFollowers { get; set; }
        //DbSet<TagFollowersRemovedLog> TagFollowersRemovedLogs { get; set; }
        //DbSet<Tag> Tags { get; set; }
        //DbSet<Tenant> Tenants { get; set; }
        //DbSet<TopicCategory> TopicCategories { get; set; }
        //DbSet<Upload> Uploads { get; set; }
        //DbSet<UploadType> UploadTypes { get; set; }
        //DbSet<UserAction> UserActions { get; set; }
        //DbSet<UserActionType> UserActionTypes { get; set; }
        //DbSet<UserBadge> UserBadges { get; set; }
        //DbSet<UserExtension> UserExtensions { get; set; }
        //DbSet<UserJoiningType> UserJoiningTypes { get; set; }
        //DbSet<UserRole> UserRoles { get; set; }
        //DbSet<UserRolesLog> UserRolesLogs { get; set; }
        //DbSet<User> Users { get; set; }
        //DbSet<UserSetting> UserSettings { get; set; }
        //DbSet<UserTag> UserTags { get; set; }
        //DbSet<UserType> UserTypes { get; set; }
        //DbSet<UserView> UserViews { get; set; }
        //DbSet<Vote> Votes { get; set; }
        //DbSet<VoteType> VoteTypes { get; set; }
        //ObjectResult<sp_RankStats_Result> sp_RankStats(string tagname, DateTime? startdate, int? userid, bool? withinIndustry);
        //int sp_ResetReputation();
        //ObjectResult<sp_SimilarPeople_Result> sp_SimilarPeople(int? userid);
        //ObjectResult<sp_TopAnswerers_Result> sp_TopAnswerers(DateTime? startdate, int? tagid);
        //ObjectResult<decimal?> sp_UserRank(int? tagid, DateTime? startdate, int? userid, bool? withinIndustry);

        #endregion
    }
}
