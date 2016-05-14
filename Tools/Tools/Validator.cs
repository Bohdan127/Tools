using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Validator
    {
        /// <summary>
        /// Check if incoming object isn't null.
        /// </summary>
        /// <exception cref="ArgumentNullException">If checked object is null</exception>
        /// <param name="instance">object to check</param>
        /// <param name="instanceName">Name of the argument for check</param>
        public static void IsNotNull(object instance, string instanceName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException($"The {instanceName} should not be null.");
            }
        }

        /// <summary>
        /// Check if incoming isn't blank 
        /// </summary>
        /// <exception cref="ArgumentNullException">If checked object is null</exception>
        /// <param name="instance">string to check</param>
        /// <param name="instanceName">Name of the argument for check/param>
        public static void IsNotEmptyString(string instance, string instanceName)
        {
            if (instance.Trim().Length == 0)
            {
                throw new ArgumentNullException($"The {instanceName} should not be empty or white space only.");
            }
        }                    
    }
}
