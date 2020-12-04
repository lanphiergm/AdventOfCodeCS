﻿using System.Linq;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Problems.Year2015
{
    class Day01NotQuiteLisp : ProblemBase<int>
    {
        public Day01NotQuiteLisp(ILogger logger) : base(logger, "Not Quite Lisp", 2015, 1) { }

        protected override int ExecutePart1()
        {

            int up = directions.Count(c => c == '(');
            int down = directions.Count(c => c == ')');
            return up - down;
        }

        protected override int ExecutePart2()
        {
            int answer = 0;
            int currFloor = 0;
            for (int i = 0; i < directions.Length; i++)
            {
                currFloor += directions[i] == '(' ? 1 : -1;
                if (currFloor == -1)
                {
                    answer = i + 1;
                    break;
                }
            }
            return answer;
        }

        #region Data

        private const string directions = "()()(()()()(()()((()((()))((()((((()()((((()))()(((((" +
            "))(((((((()(((((((((()(((())(()()(()((()()(()(())(()((((()((()()()((((())((((((()((" +
            ")(((()())(()((((()))())(())(()(()()))))))))((((((((((((()())()())())(())))(((()()()" +
            "((((()(((()(()(()()(()(()()(()(((((((())(())(())())))((()())()((((()()((()))(((()()" +
            "()())))(())))((((())(((()())(())(()))(()((((()())))())((()(())(((()((((()((()(())()" +
            ")))((()))()()(()(()))))((((((((()())((((()()((((()(()())(((((()(()())()))())(((()))" +
            "()(()(()(()((((()(())(()))(((((()()(()()()(()(((())())(((()()(()()))(((()()(((())()" +
            ")(()(())())()()(())()()()((()(((()(())((()()((())()))((()()))((()()())((((()(()()((" +
            ")(((()))()(()))))((()(((()()()))(()(((())()(()((()())(()(()()(()())(())()(((()(()()" +
            ")()((((()((()))))())()))((()()()()(())()())()()()((((()))))(()(((()()(((((((())()))" +
            "()((((()((())()(()())(())()))(()(()())(((((((())))(((()))())))))()))())((())(()()((" +
            "())()())()))))()((()()())(())((())((((()())())()()()(((()))())))()()))())(()()()(()" +
            "((((((()()))())()))()(((()(((())((((()()()(()))())()()))))())()))())((())()())(((((" +
            "())())((())())))(((())(((())(((((()(((((())(()(()())())(()(())(()))(()((((()))())()" +
            "))))())))((()(()))))())))(((((())()))())()))))()))))(((()))()))))((()))((()((()(()(" +
            "())()())))(()()()(())()))()((((())))))))(())(()((()()))(()))(()))(()((()))))))()()(" +
            "(((()()))()())()))))))()()()))(()((())(()))((()()()())()(((()((((())())))()((((()((" +
            ")))))))())))()()())()))(()))))(()())()))))))((())))))))())()))()((())())))(()((()))" +
            "()))(())))))(()))()())()()))((()(()))()()()()))))())()()))())(())()()))()))((()))))" +
            "()()(()())))))()()()))((((()))()))))(()(())))(()())))((())())(()))()))))()())))()()" +
            ")()())))))))))()()))))())))((())((()))))())))(((()())))))))(()))()()))(()))()))))()" +
            "())))))())((((()())))))))())))()()))))))))()))()))))()))))))(())))))))))())))))))))" +
            "))))))())())((())))))))))()))((())))()))))))))())()(()))))))())))))()()()())()(()()" +
            "()(()())(()))()()()(()())))())())))()))))())))))))()()()()())(())())()())()))))(()(" +
            ")()()()))))()))())())))((()())()())))()))()))))(()())))()))))))))(((()))()())))))))" +
            ")))))))))))))(()))(()((()))())))())(()))(()(()(())))))()(()))()))()()))))))))))))()" +
            "((()())(())())()(())))))())()())((()()))))(()()))))())()(())()))))))))))))))))))))(" +
            ")))(()(()())))))))()()((()))()))))))((())))()))))))))((()))())()()))())()()))((()))" +
            "())))))))))))(()())()))(())((()(()()))(()())(())))()())(()(())()()))))()))()(()))))" +
            "))(()))))))))))(()))())))))))))())))))())))(())))))()))))(())())))))))))()(()))))()" +
            "())))())(()))()())))))))))))))())()()))))()))))))())))))()))))(())(()()()()((())())" +
            ")())(()))((())()))())())(())(()()))))()))(())()()((())(())))(())))()))())))))))))()" +
            "(((((())())))(())()))))(())))((()))()(((((((()))))()()))(())))))()(()))))(()()))())" +
            ")())))))))(()())()))))))))())))(()))())()))(())()((())())()())())(()(()))))()))))))" +
            "((()())(())()()(()())))()()))(())(())(()))())))()))(()))()()))((((()))))()))((()()(" +
            ")))))()))()))())))(()))()))))(())))()))())()(()))()())))())))))))())))())))()()))))" +
            "))(()))())())))()))()()())())))))))))))))())))()))(()()))))())))())()(())))()))))))" +
            ")))))))))))()()())())))))()()()((()(()))()()(())()())()))()))))()()()))))))((()))))" +
            "))))()(()(()((((((()()((()())))))))))))()))())))))((())())(()))())))())))))())()()(" +
            "))(())))())))()())())(())))))))()()(())))()))())))())())())()))))))))()))(()()()())" +
            "())())))(())())))))))()()())()))))())))())()(())())))))))()())()))(()()(())())))()(" +
            "()((()()((()()(((((())(()())()))(())()))(())))(())))))))()))()))((()))()))())))))))" +
            ")()))))))))((()()())(()))(((()))(())))()))((())(((())))()())))())))))((())))))(())(" +
            "))((((((())())()(()))()(()((()())))((())()(()(()))))(())(()()())(())))())((()(((())" +
            "())))(((()())())))())()(())())((((()()))))())((()))()()()()(())(((((((()()()((()))(" +
            "))(()())))(())())((((()()(()))))()((())))((())()))()(((()))())))()))((()(()))(())((" +
            ")((((())((((()()(()()))(((())(()))))((((()(()))(())))))((()))(()))((()(((()(()))(()" +
            "(()((()(())(()(()(()(()()((()))())(((())(()(()))))(()))()()))(())))(())()(((())(())" +
            ")()((((()()))))())(()))))((())()((((()(((()))())())(((()))()())((())(())())(())()((" +
            "))()(()()((((((()()))))()()(((()()))))()())()(((()(()))(()(()())(()(()))))(((((()((" +
            "(())())))))(((((()((()()((())())((((((()(())(()()((()()()()()()()(()()))()(((()))()" +
            "))(((((((())(((()((()())()((((())(((()(())))()((()(()()()((())((()())()))()))())))(" +
            "))((((((()))(()(()()()))(()((()(()(()))()((()(((()()()((())(((((())()(()))())())(((" +
            ")(())))(()(()())(())((())())())(((()()()(())))))())(()))))))()))))))())((()()()))((" +
            "()((((((()))(((()((((()()()(((()))())()(()()(((()((()()()()())()()))()()()(()(())((" +
            "()))))(()))())))))))()(()()(((((())()(()(((((()((()(()()())(()((((((((()((((((())()" +
            "((((()()()((()((()((((((()))((())))))))())()))((()(()))()(()()(()((())((()()(((((((" +
            "(((((()())(()()()))((((()((((((())(()))())(()()((()()))()(((((((()((()()((((((()(((" +
            "())))((())))((((((((()()(((((((())(((((()())(((())((())()((((()(((((((()(()(((()(((" +
            "(((()(((()(((((((((((()()((()()(()))((()()(((()(((())))((((())()(()(((())()(()(((()" +
            ")(((((((((((()))())))((((((())((()()((((()())())((((()()))((())(((((()(()()(()()()(" +
            "(())(()((()()((((()(((((()((()(()((((()())((((((()(((((()()(()(()((((())))(())(())(" +
            "())((((()(()()((((()((((()()((()((((((())))(((((()))))()))(()((((((((()(((())())(((" +
            "())))(()(()((())(((()((()()(((((()((()()(((())()(()))(((((((())(()(((((()))((()((()" +
            "((()))(())())((((()((((())()(()))(((()(((((((((((((((())(((((((((()))(((()(()()()()" +
            "((((((()((())()((((((((()(())(((((((((((()(()((())()((()()(()(()()((((()()((())(()(" +
            "(()()(()()((((()(((((((())))((((())(())()(((()()((()()((((()((()(((()((())(((()()()" +
            "((((()((((()()(()(()((((((((())(()(((((())(()())(((((((()())()(()((((()((())(()()()" +
            ")((((()()(((()((((())(())(()()(((((((((()()))()(((())(()(()((((((())(()()())(()))()" +
            "()(((()(((()((())(()(((((((()(()(()((()(((((()(()((()(()((((((()((((()()((((()(((()" +
            "((())(()(()((()()((((()()(())()(())(((())(()((((((((()())(((((((((()(())()((((())))" +
            "()))()()(((((()()((((((())(()()(((()(()(((((((()(()(((((((())(())((((()((()(())))((" +
            "((()()())(()))((()())((((()(((((()(()(())(()(()()())(((((()(((((()((((()()(((((((((" +
            ")()))(()((((((())((((())()(()(((()()()(((()(()(())(())(((((()(())())((((())(())(()(" +
            "((()(((((())((((())())((()(((((((()(((())(()(()))(((((((((()((()((()()(()((((())(((" +
            "()((())((((())(()(((()(((()(()((((()(((())(()(((()(()()(()(()((()()(()())(())())(((" +
            ")(()(((()(((()(((()()(((((((((()(((((((((()()(((()(((()())((((()(()(((()()()((())((" +
            "((((((((())(()(((()((((()())((((()((()))(((()()()(((((()(((((((())((()())(()((((())" +
            "((((((((())(()((()((((((((((()()((()((()()))(((()())()())()(((()())()()(()(()((((((" +
            "(())()))(())()))())()()((())()((()((((()((()((())(((((()((((((()(())))(()))())(((()" +
            "))((()()(()(((()))((((())()(((()))))()(()(())()(((((())(()(()(())(())()((()()()((((" +
            "()(())((()())(()(()))(()(()(()()(())()()(()((())()((()))))()))((()(()()()()((()())(" +
            "()))())()(()(((((((((())())((()((()((((((())()((((())(((())((()(()()()((())(()((())" +
            "(((()((((()()((()(()(((((())()))()((((((()))((())(((()()))(((())(())()))(((((((())(" +
            "())())()(())(((((()))()((()))()(()()((()()()()()())(((((((";

        #endregion Data
    }
}
