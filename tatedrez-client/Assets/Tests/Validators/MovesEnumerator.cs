using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MovementValidatorTests
{
    public class MovesEnumerator : IEnumerable<TestCaseData>
    {
        private IEnumerable<TestCaseData> allMoves;
            
        public MovesEnumerator(params IEnumerable<TestCaseData>[] caseProviders)
        {
            this.allMoves = new LinkedList<TestCaseData>(); 
            foreach (var provider in caseProviders) {
                this.allMoves = this.allMoves.Concat(provider);
            }
        }

        public IEnumerator<TestCaseData> GetEnumerator()
        {
            return this.allMoves.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}