namespace BaseFramework.Domain
{
    /// <summary>
    /// <see cref="IUserBase"/> expects being implemented by <see cref="UserBaseEntity"/> and User in the Authentication.
    /// </summary>
    public interface IUserBase
    {
        /// <summary>
        /// User Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// User code, it should be an unique number of system user, such as Staff Id, Job Id etc.
        /// </summary>
        string UserCode { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        string UserName { get; set; }
    }

    /// <summary>
    /// The user base used to track who made creation,update,deleting etc.   
    /// </summary>
    public class UserBaseEntity : BaseEntity, IUserBase
    {
        /// <summary>
        /// User code, it should be an unique number of system user, such as Staff Id, Job Id etc.
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public virtual string UserName { get; set; }

    }
}
