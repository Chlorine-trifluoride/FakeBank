using System;
using System.Collections.Generic;
using System.Text;

namespace BankModel
{
    public static class Utils
    {
        public static bool IsAnyEmptyOrNull<T>(params T[] ts)
        {
            foreach (var v in ts)
            {
                if (v is null)
                    return true;

                if ((v as string) == string.Empty)
                    return true;
            }

            return false;
        }
    }
}
