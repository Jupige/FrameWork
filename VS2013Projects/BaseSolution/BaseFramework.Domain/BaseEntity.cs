using System;

namespace BaseFramework.Domain
{
        /// <summary>
        /// The highest abstract entity base, all concreate entities should derive from it.
        /// 
        /// The <see cref="BaseEntity"/> defines a default key named 'Id'.
        /// </summary>
        public abstract class BaseEntity
        {
            public const int ID_NONE = -1;

            /// <summary>
            /// Entity Id (the default key)
            /// </summary>
            private int _id = ID_NONE;
            public virtual int Id
            {
                get { return _id; }
                set { _id = value; }
            }

            /// <summary>
            /// Assert if one entity object is equal with another,
            /// return true when object hash code or Id is equal with another's 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                if ((obj as BaseEntity) == null)
                    return false;

                if (this.Id == ID_NONE && (obj as BaseEntity).Id == ID_NONE)
                    return Object.ReferenceEquals(this, obj);

                return this.Id == ((BaseEntity)obj).Id;
            }

            /// <summary>
            /// Override operation ==
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            /// <returns>if both Ids are equal, return true; otherwise return false;</returns>
            public static bool operator ==(BaseEntity v1, BaseEntity v2)
            {
                if (object.ReferenceEquals(v1, null) && object.ReferenceEquals(v2, null))
                    return true;

                if (object.ReferenceEquals(v1, null) || object.ReferenceEquals(v2, null))
                    return false;

                return v1.Equals(v2);
            }

            /// <summary>
            /// Override operation !=, refer to ==
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            /// <returns></returns>
            public static bool operator !=(BaseEntity v1, BaseEntity v2)
            {
                return !(v1 == v2);
            }

            /// <summary>
            /// if Id !=0 return Id, otherwise return raw hash code
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                if (this.Id == ID_NONE)
                    return base.GetHashCode();

                return this.Id;
            }

            /// <summary>
            /// If Id is ID_NONE, indicate this is new entity 
            /// </summary>
            public bool IsNew
            {
                get { return Id == ID_NONE; }
            }
        }
}
