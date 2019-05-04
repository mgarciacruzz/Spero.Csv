using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class CsvSerializableAttribute : Attribute
    {
        #region Private members
        private int _order;
        #endregion

        #region Properties
        /// <summary>
        /// Name of the column or the table
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Column position starting at 0
        /// </summary>
        public int Order
        {
            get
            {
                return _order;
            }
            set
            {
                // Column Order can't be negative
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Order must be greater or equal than 0.");

                _order = value;
            }
        }

        /// <summary>
        /// Reports if the order has been specified or not
        /// </summary>
        public bool OrderSpecified => Order >= 0;
        #endregion

        /// <summary>
        /// Marks a class or a property as Csv serializable
        /// </summary>
        /// <param name="name"> Name of the Table or the column. Default value is the name of the member</param>
        /// <param name="order"> Order of the column</param>
        public CsvSerializableAttribute([CallerMemberName]string name = null, int order = -1)
        {
            Name = name;

            if (order == -1)
                _order = order;
            else

                Order = order;
        }

    }
}
