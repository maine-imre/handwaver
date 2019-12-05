using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.Kernel
{
    
    public static class elementNameManager
    //TODO: fix wrap to enumerate aa, ab, ac...
    {
        private static int highestIndex;
        public static string GenerateName(int index)
        {
            return ((char) ((index+65) % 26)).ToString();
        }
        public static string GenerateName()
        {
            return ((char) (((highestIndex++)+65) % 26)).ToString();
        }
    }    

}