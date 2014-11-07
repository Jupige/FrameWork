using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.Domain
{
    /// <summary>
    /// Auditable entity defualtly implemented IAuitable.
    /// Please see also <seealso cref="IAuditable"/>
    /// </summary>
    public abstract class AuditableEntity : BaseEntity, IAuditable
    {
        /// <summary>
        /// User who create the entity
        /// </summary>
        public virtual UserBaseEntity CreatedBy { get; set; }

        /// <summary>
        /// Date when create the entity
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// User who update the entity
        /// </summary>
        public virtual UserBaseEntity UpdatedBy { get; set; }

        /// <summary>
        /// Date when update the entity
        /// </summary>
        public DateTime UpdatedDate { get; set; }

    }

    /// <summary>
    /// Interface for auditable entities by which determinate whether need to inject the Created User and Updated User or not when persisting.
    /// If a entity implemented <see cref="IAuditable"/>, persistence model will automatically fill the auditing infomation including CreatedBy, CreatedDate,UpdatedBy,UpdatedDate. 
    /// </summary>
    public interface IAuditable
    {
        /// <summary>
        /// User who create the entity
        /// </summary>
        UserBaseEntity CreatedBy { get; set; }

        /// <summary>
        /// Date when create the entity
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// User who update the entity
        /// </summary>
        UserBaseEntity UpdatedBy { get; set; }

        /// <summary>
        /// Date when update the entity
        /// </summary>
        DateTime UpdatedDate { get; set; }
    }
}
