//#define USESAMPLE
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Problems.Year2020
{
    class Day25ComboBreaker : ProblemBase<long>
    {
        public Day25ComboBreaker(ILogger logger) : base(logger, "Combo Breaker", 2020, 25) { }

        protected override long ExecutePart1()
        {
            int cardLoopSize = DetermineLoopSize(cardPublicKey);
            return ComputeEncryptionKey(doorPublicKey, cardLoopSize);
        }

        protected override long ExecutePart2()
        {
            return 0;
        }

        private static long ComputeEncryptionKey(int subjectNumber, int loopSize)
        {
            long encryptionKey = 1;
            for (int i = 0; i < loopSize; i++)
            {
                encryptionKey = Transform(encryptionKey, subjectNumber);
            }
            return encryptionKey;
        }

        private static int DetermineLoopSize(long publicKey)
        {
            int loopSize = 0;
            long currKey = 1;
            while (publicKey != currKey)
            {
                currKey = Transform(currKey, initialSubjectNumber);
                loopSize++;
            }
            return loopSize;
        }

        private static long Transform(long input, long subjectNumber)
        {
            input = input * subjectNumber;
            return input % 20201227L;
        }

        private const int initialSubjectNumber = 7;
#if USESAMPLE
        private const int cardPublicKey = 5764801;
        private const int doorPublicKey = 17807724;
#else
        private const int cardPublicKey = 3469259;
        private const int doorPublicKey = 13170438;
#endif
    }
}
