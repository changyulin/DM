using System;

namespace DM.Infrastructure.Domain
{
    public abstract class EntityBase
    {
        private object key;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected EntityBase()
            : this(null)
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="key">An <see cref="System.Object"/> that 
        /// represents the primary identifier value for the 
        /// class.</param>
        protected EntityBase(object key)
        {
            this.key = key;
            if (this.key == null)
            {
                this.key = EntityBase.NewKey();
            }
        }

        /// <summary>
        /// An <see cref="System.Object"/> that represents the 
        /// primary identifier value for the class.
        /// </summary>
        public object Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        public static object NewKey()
        {
            return Guid.NewGuid();
        }
    }
}
