using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace concatenate_strings_benchmark
{
    public class BenchmarkTestPerformance
    {
        [Params(10, 100, 1000)]
        public int Length;

        public string testString = "Test";

        public string[] appendix;

        [GlobalSetup]
        public void Setup()
        {
            appendix = new string[Length];

            for (var i = 0; i < Length; i++)
            {
                appendix[i] = "Test";
            }
        }

        // using + operator 
        [Benchmark]
        public string UsingPlus()
        {
            testString = "test";

            for(int i = 0; i < Length; i++)
            {
                testString += appendix[i];
            }
            return testString;
        }

        // using Concat
        [Benchmark]
        public string UsingConcat()
        {
            testString = "test";

            for(int i = 0; i < Length; i++)
            {
                testString = string.Concat(testString, appendix[i]);
            }
            return testString;
        }

        // using Concat with array
        [Benchmark]
        public string UsingConcatWithArray()
        {
            testString = "test";

            for(int i = 0; i < Length; i++)
            {
                testString = string.Concat(appendix);
            }
            return testString;
        }

        // using join
        [Benchmark]
        public string UsingJoin()
        {
            testString = "test";

            for(int i = 0; i < Length; i++)
            {
                testString += string.Join(string.Empty, appendix);
            }
            return testString;
        }

        // using Format
        [Benchmark]
        public string UsingFormat()
        {
            string testString = "test";

            for( int i = 0; i < Length; i++)
            {
                testString = string.Format("{0}{1}",testString, appendix[i]);
            }
            return testString;
        }

        // using StringBuilder
        [Benchmark]
        public string UsingStringBuilder()
        {
            testString = "test";
            StringBuilder builder = new StringBuilder(testString);

            for(int i = 0; i < Length; i++)
            {
                builder.Append(appendix[i]);
            }

            testString = builder.ToString();

            return testString;
        }

        // using StringBuilder.AppendJoin
        [Benchmark]
        public string UsingBuilderStringAppendJoin()
        {
            testString = "test";

            StringBuilder builder = new StringBuilder(testString);

            builder.AppendJoin(string.Empty, appendix);

            testString = builder.ToString();
            return testString;
        }

        // using Linq Aggregate
        [Benchmark]
        public string UsingLinqAggregate()
        {
            testString = "test";
            testString += appendix.Aggregate((partialPhrase,word) => $"{partialPhrase}{word}");

            return testString;
        }

        // using string create
        [Benchmark]
        public string UsingStringCreate()
        {
            var bufferSize = Length * testString.Length;

            return string.Create(bufferSize, (testString, Length), static (buffer, state) =>
             {
                 var testStringSpan = state.testString.AsSpan();

                 for(var i = 0; i < state.Length; i++)
                 {
                     var pos = i * state.testString.Length;
                     testStringSpan.CopyTo(buffer[pos..]);
                 }
             }
            );
        }


    }
}
