using System;
using System.Collections.Generic;

namespace ADLS_Explorer
{
    public class AZContainer : IComparable, IEquatable<AZContainer>
    {
        public string Account { get; set; }
        public string Container { get; set; }

        public override string ToString() => $"{Account} : {Container}";
        
        public int CompareTo(object obj) => ToString().CompareTo(((AZContainer)obj).ToString());

        public override bool Equals(object obj) => Equals(obj as AZContainer);

        public bool Equals(AZContainer other) => other != null && Account == other.Account && Container == other.Container;
        
        public static bool operator ==(AZContainer left, AZContainer right) => EqualityComparer<AZContainer>.Default.Equals(left, right);

        public static bool operator !=(AZContainer left, AZContainer right) => !(left == right);
    }
}
