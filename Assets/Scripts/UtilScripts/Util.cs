using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Util 
{
    // All methods should be public static.


    // Adds commas every 3rd character starting from the right, intended to take string ints as param.
    public static string AddCommas(string s)
    {
        string result = "";
        while (s.Length > 3)
        {
            result = result.Insert(0, "," + s.Substring(s.Length - 3));
            s = s.Substring(0, s.Length - 3);
        }
        result = result.Insert(0, s);

        return result;
    }
    public static string AddCommas(int num)
    {
        return AddCommas(num.ToString());
    }

    public static bool CaseInsensitiveStringCompare(string string1, string string2)
    {
        return string1.Equals(string2, StringComparison.OrdinalIgnoreCase);
    }

    

}
