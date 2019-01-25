using System.Text.RegularExpressions;

namespace Rcp.Utilities
{
    public class CommonRegex
    {
        /// <summary>
        ///     Validates SSN:
        ///     REF: http://www.codeproject.com/Articles/651609/Validating-Social-Security-Numbers-through-Regular
        ///     REF: https://github.com/rionmonster/SocialSecurityValidation/blob/master/SocialSecurityValidation/Example.aspx.cs
        ///     REF: https://en.wikipedia.org/wiki/Social_Security_number
        ///     REF: https://www.ssa.gov/employer/ssnvshandbk/SSNresults.htm
        ///     REF: https://en.wikipedia.org/wiki/List_of_Social_Security_Area_Numbers
        ///     REF: http://www.usrecordsearch.com/ssn.htm
        /// </summary>
        public static Regex SsnRegex = new Regex(@"^(?!\b([0-9])\1+-([0-9])\1+-([0-9])\1+\b)(?!123-45-6789|219-09-9999|078-05-1120)(?!666|000|9[0-9]{2})[0-9]{3}-(?!00)[0-9]{2}-(?!0{4})[0-9]{4}$");
    }
}