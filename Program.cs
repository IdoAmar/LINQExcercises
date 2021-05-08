using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQExercises
{
    class Program
    {
        static void Main(string[] args)
        {
            Question1();
            Question2();
            Question3();
            Question4();
            Question5();
            Question6();
            Question7();
            Question8();
            Question9();
            Question10();
            Question11();
            Question12();
            Question13();
        }

        public static void Question1()
        {
            string str = "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis, Schneiderlin, Cork, Lallana, Rodriguez, Lambert";
            Console.WriteLine(str.Split(",")
                                 .Select((s, i) => i + 1 + "." + s.Trim())
                                 .Aggregate((acc, s) => acc + ", " + s) + "\n");
        }

        public static void Question2()
        {
            string str = "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988";
            var peopleEnum = str.Split(";").Select(s => s.Trim().Split(",")).Select(s => (Name: s[0].Trim(), Age: PersonAgeByDate(s[1]))).OrderBy(p => p.Age);
            foreach (var item in peopleEnum)
                Console.WriteLine($"{item.Name,-20} | {item.Age,-3}");
            Console.WriteLine();
        }

        public static void Question3()
        {
            string str = "4:12,2:43,3:51,4:29,3:24,3:14,4:46,3:25,4:52,3:27";
            TimeSpan time = str.Split(",").Select(s => TimeSpan.ParseExact(s.Trim(), "c", null)).Aggregate(TimeSpan.Zero, (acc, t) => acc + t);
            Console.WriteLine($"Total time is : {time.Minutes,2}:{time.Seconds}" + "\n");
        }

        public static void Question4()
        {
            Console.WriteLine("Question 4 answer : " +
                              Enumerable.Range(0, 3)
                                        .Select((x, i) => (index: i, range: Enumerable.Range(0, 3)))
                                        .SelectMany(t => t.range, (t, n) => (index: t.index, number: n))
                                        .Aggregate("", (acc, c) => acc + c.index + "," + c.number + " | ") + "\n");
        }

        public static void Question5()
        {
            string str = "00:45,01:32,02:18,03:01,03:44,04:31,05:19,06:01,06:47,07:35";
            Console.WriteLine("Question 5 answer : " +
                              str.Split(",")
                                 .Select(s => TimeSpan.ParseExact(s.Trim(), "c", null))
                                 .Window(2)
                                 .Select(w => w.Last() - w.First())
                                 .Aggregate("", (acc, t) => acc + t.ToString() + " ") + "\n");
        }

        public static void Question6()
        {
            string str = "2,5,7-10,11,17-18";
            Console.WriteLine("Question 6 answer : " +
                              str.Split(",")
                                 .Select(s => s.Split("-"))
                                 .SelectMany(s => s.Count() > 1 ? (CreateStringsEnumFromStringArray(s)) : s)
                                 .Aggregate("", (acc, s) => acc + s + " ") + "\n");
        }

        public static void Question7()
        {
            string str = "10,5,0,8,10,1,4,0,10,1";
            Console.WriteLine("Question 7 answer : " + str.Split(",")
                                                          .Select(s => int.Parse(s))
                                                          .OrderBy(s => s)
                                                          .Skip(3)
                                                          .Sum(s => s) + "\n");
        }

        public static void Question8BAD()
        {
            int boardSize = 8;
            string str = "c6";
            var l = new List<int>() { Convert.ToInt32(str[0] - 'a' + 1), Convert.ToInt32(str[1].ToString()) };

            var sUpRight = Enumerable.Range(1, boardSize - l.Max())
                                     .Select(s => (l[0] + s, l[1] + s));

            var sUpLeft = Enumerable.Range(1, (l[0] - 1) < (boardSize - l[1]) ? l[0] - 1 : boardSize - l[1])
                                    .Select(s => (l[0] - s, l[1] + s));

            var sDownLeft = Enumerable.Range(1, l.Min() - 1)
                                     .Select(s => (l[0] - s, l[1] - s));

            var sDownRight = Enumerable.Range(1, (boardSize - l[0]) < (l[1] - 1) ? boardSize - l[0] : l[1] - 1)
                                    .Select(s => (l[0] + s, l[1] - s));

            var joinedMoves = sUpRight.Concat(sUpLeft).Concat(sDownLeft).Concat(sDownRight);

            foreach (var item in joinedMoves)
            {
                Console.Write($"{(Convert.ToChar('a' + item.Item1 - 1))}{item.Item2} | ");
            }

        }
        public static void Question8()
        {
            int boardSize = 8;
            string str = "c6";
            var l = new List<int>() { Convert.ToInt32(str[0] - 'a' + 1), Convert.ToInt32(str[1].ToString()) };

            var joinedMoves = Enumerable.Range(1, boardSize)
                                        .Select((r, i) => (i, Enumerable.Range(1, boardSize)))
                                        .SelectMany(s => s.Item2, (s, i) => (x: s.i + 1, y: i))
                                        .Where(i => Math.Abs(i.x - l[0]) == Math.Abs(i.y - l[1]));

            foreach (var item in joinedMoves)
                Console.Write($"({Convert.ToChar('a' + item.x - 1)},{item.y}) | ");
            Console.WriteLine("\n");
        }

        public static void Question9()
        {
            string str = "0,6,12,18,24,30,36,42,48,53,58,63,68,72,77,80,84,87,90,92,95,96,98,99,99,100," +
                         "99,99,98,96,95,92,90,87,84,80,77,72,68,63,58,53,48,42,36,30,24,18,12,6,0,-6,-12" +
                         ",-18,-24,-30,-36,-42,-48,-53,-58,-63,-68,-72,-77,-80,-84,-87,-90,-92,-95,-96,-98,-99" +
                         ",-99,-100,-99,-99,-98,-96,-95,-92,-90,-87,-84,-80,-77,-72,-68,-63,-58,-53,-48,-42,-36,-30,-24,-18,-12,-6";
            var stringEnum = str.Split(",").Select((s, i) => (value: s, index: i + 1)).Where(t => t.index % 5 == 0);

            foreach (var item in stringEnum)
                Console.Write(item.value + ",");
            Console.WriteLine("\n");
        }

        public static void Question10()
        {
            string str = "Yes,Yes,No,Yes,No,Yes,No,No,No,Yes,Yes,Yes,Yes,No,Yes,No,No,Yes,Yes";
            var winningChoiceTuple = str.Split(",")
                                        .Select(s => s.Trim().ToUpper())
                                        .GroupBy(s => s)
                                        .OrderByDescending(g => g.Count())
                                        .Take(2)
                                        .Aggregate((choice: "", amount: 0), (acc, g) => g.Count() > acc.amount ? (g.Key, g.Count()) : (acc.choice, acc.amount - g.Count()));
            Console.WriteLine($"The choice {winningChoiceTuple.choice} won with {winningChoiceTuple.amount} votes difference.\n");
        }

        public static void Question11()
        {
            string str = "Dog,Cat,Rabbit,Dog,Dog,Lizard,Cat,Cat,Dog,Rabbit,Guinea Pig,Dog";
            var sequence = str.Split(",")
                              .Select(s => s.Trim())
                              .Select(s => s == "Dog" || s == "Cat" ? s : "Other")
                              .GroupBy(s => s)
                              .Select(g => (kind: g.Key, amount: g.Count()));
            foreach (var item in sequence)
                Console.WriteLine($"{item.kind} : {item.amount}");
            Console.WriteLine();
        }

        public static void Question12()
        {
            string str = "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0";
            var answer = str.Split(",")
                             .Select(s => int.Parse(s.Trim()))
                             .Aggregate((previous: 0, current: 0), (acc, c) => (c == 0) ? 
                                                                   (acc.previous ,acc.current + 1) : 
                                                                   (acc.current > acc.previous ? (acc.current, 0) : (acc.previous , 0))).previous;
            Console.WriteLine("Question 12 answer is : " + answer);
        }

        public static void Question13()
        {
            string str = "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson, Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell";
            var answer = str.Split(",").Select(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries)).GroupBy(s => s[0][0].ToString() + s[1][0].ToString()).TakeWhile( g => g.Count() > 1);
            foreach (var group in answer)
            {
                Console.WriteLine($"\nAmount of people with \"{group.Key}\" initials is {group.Count()}:");
                foreach (var item in group)
                {
                    foreach (var innerItem in item)
                        Console.Write(innerItem + " ");
                    if(item != group.Last())
                        Console.Write("and ");
                }
                Console.WriteLine();
            }
        }

        public static IEnumerable<string> CreateStringsEnumFromStringArray(string[] strArr)
        {
            return Enumerable.Range(Int32.Parse(strArr[0]), Int32.Parse(strArr[1]) - Int32.Parse(strArr[0]) + 1)
                             .Select(s => s.ToString());
        }

        public static int PersonAgeByDate(string dateString)
        {
            DateTime date = DateTime.Parse(dateString);
            return date.Month < DateTime.Now.Month ?
                   DateTime.Now.Year - date.Year :
                   DateTime.Now.Year - 1 - date.Year;
        }
    }
}
