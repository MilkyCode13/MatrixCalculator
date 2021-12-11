using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MatrixCalculator
{
    /// <summary>
    /// Main class of the program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// <c>Random</c> object used for pseudo-random number generation.
        /// </summary>
        private static Random s_rand = new();

        /// <summary>
        /// Dictionary of matrices saved by user.
        /// </summary>
        private static Dictionary<char, double[,]> s_savedMatrices = new();

        /// <summary>
        /// Main entrypoint of the program.
        /// </summary>
        private static void Main()
        {
            WelcomeUser();

            bool exit;
            do
            {
                try
                {
                    exit = GetCommand();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Поздравляю, ты смог сломать программу так, как я не мог ожидать!\n" +
                                      "Мне аж интересно, как ты это сделал. Но я ожидал, что это может произойти\n" +
                                      "и сделал так, чтобы программа не упала. :)\n");
                    exit = false;
                }
            } while (!exit);
        }

        /// <summary>
        /// Displays welcome message.
        /// </summary>
        private static void WelcomeUser()
        {
            Console.WriteLine("Добро пожаловать в калькулятор матриц! Это самый продвинутый калькулятор матриц!*\n" + 
                              "Он позволяет выполнять все** необходимые операции над матрицами!\n" +
                              "Чтобы вызвать справку, введите команду \"help\".\n\n" +
                              "*  Среди всех калькуляторов матриц, созданных мной\n" +
                              "** Все, которые требуются на 1 модуль 1 курса\n");
        }

        /// <summary>
        /// Gets user command and sends it for processing.
        /// </summary>
        /// <returns><c>true</c>, if user decided to exit, <c>false</c> otherwise.</returns>
        private static bool GetCommand()
        {
            // Get user input.
            Console.Write("> ");
            string input = Console.ReadLine();
            
            // Check if EOF.
            if (input == null)
            {
                Console.WriteLine();
                return false;
            }

            // Parse input.
            string[] cmd = input.Split(' ',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (cmd.Length < 1)
            {
                return false;
            }

            return ProcessCommand(cmd);
        }

        /// <summary>
        /// Processes user command.
        /// </summary>
        /// <param name="cmd">Array of words in command.</param>
        /// <returns><c>true</c>, if user decided to exit, <c>false</c> otherwise.</returns>
        private static bool ProcessCommand(string[] cmd)
        {
            // FOR REVIEWER: Unable to decompose that method any more.
            // ДЛЯ ПРОВЕРЯЮЩЕГО: Дальше декомпозировать этот метод невозможно.
            switch (cmd[0])
            {
                case "help":
                    ShowHelp();
                    return false;
                case "fileformat":
                    ShowFileFormat();
                    return false;
                case "exit":
                    Console.WriteLine("Пока!\n");
                    return true;
                case "random":
                    DisplaySave(CreateRandomMatrix());
                    return false;
                case "manual":
                    DisplaySave(CreateUserDefinedMatrix());
                    return false;
                case "readfile":
                    ReadMatrixFile();
                    return false;
                case "writefile":
                    WriteMatrixFile();
                    return false;
                case "list":
                    ListSavedMatrices();
                    return false;
                case "display":
                    PrintMatrix(GetMatrix());
                    return false;
                case "trace":
                    MatrixTrace();
                    return false;
                case "transpose":
                    MatrixTranspose();
                    return false;
                case "sum":
                    MatrixSum();
                    return false;
                case "difference":
                    MatrixDifference();
                    return false;
                case "product":
                    MatrixProduct();
                    return false;
                case "factorize":
                    MatrixFactorize();
                    return false;
                case "determinant":
                    MatrixDeterminant();
                    return false;
                case "solvegauss":
                    MatrixSolveGaussian();
                    return false;
                default:
                    Console.WriteLine("Такой команды нет. Введите \"help\" для справки.\n");
                    return false;
            }
        }

        /// <summary>
        /// Displays help.
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("Основные команды:\n" +
                              "help        Показать эту справку.\n" +
                              "fileformat  Описание формата файла с матрицами.\n" +
                              "exit        Выход из программы\n\n" +
                              "Команды ввода/вывода:\n" +
                              "random      Сгенерировать случайную матрицу заданного размера и выполнить операцию\n" +
                              "            над ней или сохранить её в памяти.\n" +
                              "manual      Ввести матрицу заданного размера вручную и выполнить операцию над ней\n" +
                              "            или сохранить её в памяти.\n" +
                              "readfile    Считать матрицы из файла и сохранить их в памяти.\n" +
                              "writefile   Сохранить матрицы из памяти в файл.\n" +
                              "list        Вывести список матриц, сохранённых в памяти, и их размеров.\n" +
                              "display     Вывести матрицу из памяти на экран.\n\n" +
                              "Команды операций над матрицами:\n" +
                              "trace       Подсчёт следа квадратной матрицы.\n" +
                              "transpose   Транспонирование матрицы.\n" +
                              "sum         Сложение двух матриц одного типа.\n" +
                              "difference  Вычитание двух матриц одного типа.\n" +
                              "product     Умножение двух матриц.\n" +
                              "factorize   Умножение матрицы на число.\n" +
                              "determinant Нахождение определителя (детерминанта) квадратной матрицы.\n" +
                              "solvegauss  Приведение матрицы к ступенчатому или каноническому виду методом Гаусса.\n" +
                              "            Решение СЛАУ, представленной в матричном виде.\n");
        }

        /// <summary>
        /// Displays file format description.
        /// </summary>
        private static void ShowFileFormat()
        {
            Console.WriteLine("Описание формата файла:\n" +
                              "Файл состоит из одного или нескольких блоков, каждый блок содержит описание\n" +
                              "одной матрицы.\n" +
                              "Формат блока:\n\n" +
                              "[C:n:m]\n" +
                              "c11 c12 \u2026 c1m\n" +
                              "c21 c22 \u2026 c2m\n" +
                              " \u22ee   \u22f1  \u22f1  \u22ee\n" +
                              "cn1 cn2 \u2026 cnm\n\n" +
                              "Здесь C - буква, которой обозначена матрица (по ней можно будет обратиться\n" +
                              "к данной матрице), n — количество строк, m — количество столбцов,\n" +
                              "cij — элемент матрицы в i-ой строке и j-ом столбце.\n\n" +
                              "Например (матрица A размера 4x5):\n\n" +
                              "[A:4:5]\n" +
                              "5.3 0 -3 2e6 -4\n" +
                              "1 4.64 0 20001 46\n" +
                              "-1e-5 0 -9.8 234 0\n" +
                              "4.35 1 -5 -0.1 29.4\n\n" +
                              "Строки, начинающиеся с '#', а также пустые строки не учитываются.\n" +
                              "ВНИМАНИЕ! Разделитель целой и дробной части — точка.\n");
        }

        /// <summary>
        /// Displays matrix on screen and prompts user to save it.
        /// </summary>
        /// <param name="matrix">Matrix to display.</param>
        /// <param name="isSolution">Whether to print matrix as a formatted solution.</param>
        private static void DisplaySave(double[,] matrix, bool isSolution = false)
        {
            PrintMatrix(matrix, isSolution);
            if (isSolution)
            {
                PrintSolutions(matrix);
            }

            // Prompt user to save or discard the matrix.
            ConsoleKey key;
            do
            {
                Console.Write("Вы хотите сохранить эту матрицу в память (Y/N)? ");
                key = Console.ReadKey().Key;
                Console.WriteLine();
            } while (key != ConsoleKey.Y && key != ConsoleKey.N);
            
            if (key == ConsoleKey.Y)
            {
                // Prompts user to select a latin letter to refer the matrix.
                Console.WriteLine("Введите букву латинского алфавита, которой хотите обозначить матрицу.");
                char matrixName;
                do
                {
                    Console.Write("Буква> ");
                    string input = Console.ReadLine();
                    if (input is {Length: 1})
                    {
                        matrixName = input[0];
                        if (matrixName is >= 'a' and <= 'z' or >= 'A' and <= 'Z')
                        {
                            break;
                        }
                    }
                    
                    Console.WriteLine("Неверный ввод.\n");
                } while (true);
                UpdateSavedMatrices(matrixName, matrix);
                Console.WriteLine($"\nТеперь вы можете обращаться к этой матрице через её название: {matrixName}.\n");
            }
        }

        /// <summary>
        /// Prints the solution of system of linear equations. 
        /// </summary>
        /// <param name="matrix">Solved matrix representation of SLE.</param>
        private static void PrintSolutions(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            var isDependent = new bool[columns - 1];
            var solutions = new List<string> {"Решения системы уравнений:"};

            for (var i = 0; i < rows; i++)
            {
                // If the system has no solutions, tell user so and return.
                if (!ProcessSolutionMatrixString(in matrix, i, ref isDependent, out string solutionString))
                {
                    Console.WriteLine("\nДанная система решений не имеет.\n");
                    return;
                }

                if (solutionString != "")
                {
                    solutions.Add(solutionString);
                }
            }

            // If some variables are free, print them as being in set of real numbers.
            string freeVars = string.Join(", ",
                Enumerable.Range(0, columns - 1).Where(x => !isDependent[x]).Select(x => $"x{x + 1}"));
            if (freeVars != "")
            {
                solutions.Add(freeVars + " \u2208 R");
            }

            foreach (string solution in solutions)
            {
                Console.WriteLine(solution);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Processes string of solved matrix for printing.
        /// </summary>
        /// <param name="matrix">Matrix to process.</param>
        /// <param name="rowNumber">Number of row to process.</param>
        /// <param name="isDependent">Array of flags if each variable of equation is dependent or free.</param>
        /// <param name="solutionString">Result string to display.</param>
        /// <returns><c>true</c> if the equation has solutions, <c>false</c> otherwise.</returns>
        private static bool ProcessSolutionMatrixString(in double[,] matrix, int rowNumber, ref bool[] isDependent,
            out string solutionString)
        {
            int columns = matrix.GetLength(1);
            var metNonZero = 0;
            solutionString = "";
            
            // Process each term.
            for (var j = 0; j < columns; j++)
            {
                bool isFreeTerm = (j == columns - 1);
                // If system has no solutions, return false.
                if (!ProcessSolutionMatrixStringTurn(j, matrix[rowNumber, j], isFreeTerm, ref isDependent,
                    ref metNonZero, ref solutionString))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Processes a term of an equation.
        /// </summary>
        /// <param name="columnNumber">Index of a term in a row.</param>
        /// <param name="element">Element of a term.</param>
        /// <param name="isFreeTerm">Is the term is free.</param>
        /// <param name="isDependent">Array of flags if each variable of equation is dependent or free.</param>
        /// <param name="metNonZero">Number of times processes met non-zero term in the row.</param>
        /// <param name="solutionString">Result string to display.</param>
        /// <returns><c>true</c> if the equation has solutions, <c>false</c> otherwise.</returns>
        private static bool ProcessSolutionMatrixStringTurn(int columnNumber, double element, bool isFreeTerm,
            ref bool[] isDependent, ref int metNonZero, ref string solutionString)
        {
            if (Math.Abs(element) > 1e-12)
            {
                metNonZero++;
                switch (metNonZero)
                {
                    case 1:
                        // If the only non-zero term is free term, the system has no solutions.
                        if (isFreeTerm)
                        {
                            return false;
                        }

                        isDependent[columnNumber] = true;
                        solutionString = $"x{columnNumber + 1} =";
                        break;
                    case 2:
                        // This term is first after the equation sign — do not print '+' and print '-'
                        // without space to the term. Invert signs if not the free term.
                        solutionString += $" {((element < 0) ^ isFreeTerm ? "" : "-")}" +
                                          $"{FormatNumber(Math.Abs(element))}" +
                                          (isFreeTerm ? "" : $"*x{columnNumber + 1}");
                        break;
                    default:
                        // This term is other than first after the equation sign. Invert signs if not the free term.
                        solutionString += $" {((element < 0) ^ isFreeTerm ? "+" : "-")}" +
                                          $" {FormatNumber(Math.Abs(element))}" +
                                          (isFreeTerm ? "" : $"*x{columnNumber + 1}");
                        break;
                }
            }
            // If there are no terms after the equation sign and this is the free term equal 0, add 0 to the end.
            else if (metNonZero == 1 && isFreeTerm)
            {
                solutionString += " 0";
            }

            return true;
        }

        /// <summary>
        /// Gets a matrix by different means.
        /// </summary>
        /// <param name="operandNumber">Number to display user.</param>
        /// <returns>Matrix created or fetched from memory.</returns>
        private static double[,] GetMatrix(int? operandNumber = null)
        {
            ShowGetMatrixHelp();
            do
            {
                Console.Write($"Матрица {operandNumber}> ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case null:
                        break;
                    case "manual":
                        return CreateUserDefinedMatrix();
                    case "random":
                        return CreateRandomMatrix();
                    case "file":
                        ReadMatrixFile();
                        break;
                    case "list":
                        ListSavedMatrices();
                        break;
                    case "help":
                        ShowGetMatrixHelp();
                        break;
                    case {Length: 1}:
                        // Check that character is a latin letter. If matrix referred by it exists, return it.
                        char matrixName = input[0];
                        double[,] matrix = GetMatrixByName(matrixName);
                        if (matrix != null)
                        {
                            return matrix;
                        }
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.\n");
                        break;
                }
            } while (true);
        }

        
        /// <summary>
        /// Shows help of get matrix mode.
        /// </summary>
        private static void ShowGetMatrixHelp()
        {
            Console.WriteLine("Допустимые выражения в режиме выбора матрицы:\n" +
                              "A       Буква — имя матрицы в памяти.\n" +
                              "manual  Ввод матрицы из консоли.\n" +
                              "random  Генерация случайной матрицы.\n" +
                              "file    Чтение матриц из файла в память.\n" +
                              "list    Вывод списка матриц в памяти в консоль.\n" +
                              "help    Показать эту справку.\n");
        }

        /// <summary>
        /// Gets a matrix by its name in memory.
        /// </summary>
        /// <param name="matrixName"></param>
        /// <returns></returns>
        private static double[,] GetMatrixByName(char matrixName)
        {
            if (matrixName is >= 'a' and <= 'z' or >= 'A' and <= 'Z')
            {
                if (s_savedMatrices.ContainsKey(matrixName))
                {
                    return s_savedMatrices[matrixName];
                }
                Console.WriteLine("Такой матрицы в памяти нет.");
            }
            else
            {
                Console.WriteLine("Неверный ввод.\n");
            }

            return null;
        }

        /// <summary>
        /// Prints a matrix to screen.
        /// </summary>
        /// <param name="matrix">Matrix to display.</param>
        /// <param name="makeDivisor">Whether to make visual divisor (for matrix of system of equations).</param>
        private static void PrintMatrix(double[,] matrix, bool makeDivisor = false)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);
            
            var outputs = new string[rows, columns];
            var columnWidths = new int[columns];
            
            // Get formatted numbers and calculate maximum length for column length.
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    outputs[i, j] = FormatNumber(matrix[i, j]);
                    columnWidths[j] = Math.Max(columnWidths[j], outputs[i, j].Length);
                }
            }
            
            // Output the matrix.
            Console.WriteLine();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    if (makeDivisor && j == columns - 2)
                    {
                        Console.Write($"{outputs[i, j].PadLeft(columnWidths[j])} | ");
                    }
                    else
                    {
                        Console.Write($"{outputs[i, j].PadLeft(columnWidths[j])}  ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Formats the number to occupy reasonable number of characters.
        /// </summary>
        /// <param name="number">Number to format.</param>
        /// <param name="culture">Culture to represent number in. Current by default.</param>
        /// <returns>String representation of the number.</returns>
        private static string FormatNumber(double number, CultureInfo culture = null)
        {
            culture ??= CultureInfo.DefaultThreadCurrentCulture;

            return Math.Abs(number) switch
            {
                // If number is (almost) zero, return zero.
                <= 1e-12 => "0",
                // If number is too large, use exponential form.
                >= 1e7 => number.ToString("0.0####e0", culture),
                // If number is too small, use exponential form.
                <= 1e-3 => number.ToString("0.0###e-0", culture),
                // Else return general form.
                _ => number.ToString("G8", culture)
            };
        }

        /// <summary>
        /// Gets a matrix row from user input.
        /// </summary>
        /// <param name="length">Desired length of a row.</param>
        /// <param name="index">Index of a row to get.</param>
        /// <returns>Array of elements of the row.</returns>
        private static double[] GetInputRow(int length, int index)
        {
            var row = new double[length];
            var valid = false;
            // Repeat input until it is valid.
            do
            {
                Console.Write($"[{index + 1}]> ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }

                // Split the input and check that number of elements is correct.
                string[] array = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (array.Length != length)
                {
                    Console.WriteLine("Неверное число элементов.\n");
                    continue;
                }

                // Parse each element and check it is valid.
                valid = true;
                for (var i = 0; i < length; i++)
                {
                    if (!double.TryParse(array[i], out row[i]))
                    {
                        valid = false;
                        Console.WriteLine($"Неверный элемент {i + 1}.\n");
                        break;
                    }
                }
            } while (!valid);

            return row;
        }

        /// <summary>
        /// Gets matrix dimensions from the user.
        /// </summary>
        /// <param name="rows">Number of rows entered.</param>
        /// <param name="columns">Number of columns entered.</param>
        private static void GetMatrixDimensionsInput(out int rows, out int columns)
        {
            rows = 0;
            columns = 0;
            
            Console.WriteLine("Введите размеры матрицы: количество строк и количество столбцов через пробел.");
            var valid = false;
            do
            {
                Console.Write("Размеры> ");
                string input = Console.ReadLine();
                
                if (input == null)
                {
                    continue;
                }
                
                // Get user input and validate it.
                string[] array = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (array.Length != 2)
                {
                    Console.WriteLine("Неверный ввод.\n");
                    continue;
                }

                valid = true;
                if (!int.TryParse(array[0], out rows) || rows is <= 0 or > 25)
                {
                    valid = false;
                    Console.WriteLine("Неверное количество строк, должно быть натуральным числом между 1 и 25.\n");
                }
                else if (!int.TryParse(array[1], out columns) || columns is <= 0 or > 25)
                {
                    valid = false;
                    Console.WriteLine("Неверное количество столбцов, должно быть натуральным числом между 1 и 25.\n");
                }
            } while (!valid);
        }

        /// <summary>
        /// Creates a matrix based on user input.
        /// </summary>
        /// <returns>Created matrix.</returns>
        private static double[,] CreateUserDefinedMatrix()
        {
            GetMatrixDimensionsInput(out int rows, out int columns);
            
            var matrix = new double[rows, columns];

            Console.WriteLine("Введите все элементы (вещественные числа), по одной строке, через пробел.\n");
            for (var i = 0; i < rows; i++)
            {
                double[] row = GetInputRow(columns, i);
                for (var j = 0; j < columns; j++)
                {
                    matrix[i, j] = row[j];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Creates a random matrix.
        /// </summary>
        /// <returns>Created matrix.</returns>
        private static double[,] CreateRandomMatrix()
        {
            GetMatrixDimensionsInput(out int rows, out int columns);
            
            var matrix = new double[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    matrix[i, j] = s_rand.NextDouble() * s_rand.Next(-1000, 1000);
                }
            }

            return matrix;
        }

        /// <summary>
        /// Reads matrices from a file specified by user.
        /// </summary>
        private static void ReadMatrixFile()
        {
            Console.WriteLine("Введите абсолютный или относительный путь к файлу:");
            string filePath;
            do
            {
                Console.Write("Путь> ");
                filePath = Console.ReadLine();
            } while (string.IsNullOrEmpty(filePath));

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Такого файла не существует.\n");
                return;
            }

            int badLineNumber = ProcessMatrixFileContents(filePath);
            if(badLineNumber != 0)
            {
                Console.WriteLine(badLineNumber != -1
                    ? $"Ошибка при обработке {badLineNumber} строки файла.\n"
                    : "Ошибка при чтении файла.");
                return;
            }

            Console.WriteLine("Файл успешно прочитан.\n");
        }

        /// <summary>
        /// Processes and validates file contents.
        /// </summary>
        /// <param name="filePath">Path of the file.</param>
        /// <returns>0 if there are no errors, -1 if file read error occured,
        /// number of line if processing error occured.</returns>
        private static int ProcessMatrixFileContents(string filePath)
        {
            // Working with files — needing extreme caution.
            try
            {
                using StreamReader streamReader = File.OpenText(filePath);
                string line;
                char matrixName = default;
                double[,] matrix = null;
                var currentRow = 0;
                for (var lineNum = 1; (line = streamReader.ReadLine()) != null; lineNum++)
                {
                    // Process the line.
                    if (!ProcessFileMatrixString(line, ref matrixName, ref matrix, ref currentRow))
                    {
                        return lineNum;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка чтения файла: {e.Message}");
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Processes and validates file string contents.
        /// </summary>
        /// <param name="line">String to validate</param>
        /// <param name="matrixName">Name of processing matrix.</param>
        /// <param name="matrix">Matrix processing.</param>
        /// <param name="currentRow">Number of current row.</param>
        /// <returns><c>false</c> if an error occured, <c>true</c> otherwise.</returns>
        private static bool ProcessFileMatrixString(string line, ref char matrixName, ref double[,] matrix,
            ref int currentRow)
        {
            line = line.Trim();
            // If line begins with '#' or empty, skip it.
            if (line.Length == 0 || line[0] == '#')
            {
                return true;
            }
            // Matrix header.
            if (line[0] == '[')
            {
                if (!ParseFileMatrixHeader(line, out matrixName, out int rows, out int columns))
                {
                    return false;
                }

                matrix = new double[rows, columns];
                currentRow = 0;
                return true;
            }
            // Matrix row.
            if (!ParseMatrixRow(line, ref matrix, currentRow))
            {
                return false;
            }
            currentRow++;
            if (currentRow == matrix.GetLength(0))
            {
                UpdateSavedMatrices(matrixName, matrix);
            }

            return true;
        }

        /// <summary>
        /// Updates memory with a matrix.
        /// </summary>
        /// <param name="matrixName">Name of a matrix.</param>
        /// <param name="matrix">The matrix to save.</param>
        /// <param name="forceOverwrite">Whether to overwrite without prompting.</param>
        private static void UpdateSavedMatrices(char matrixName, double[,] matrix, bool forceOverwrite = false)
        {
            // If matrix with that name is already present, need to make decision.
            if (s_savedMatrices.ContainsKey(matrixName))
            {
                // If forceOverwrite flag is present, do not prompt user, overwrite matrix.
                if (forceOverwrite)
                {
                    s_savedMatrices[matrixName] = matrix;
                    return;
                }
                
                // Prompt user otherwise. If user agrees to overwrite matrix, do so.
                Console.Write($"Матрица с именем {matrixName} уже существует в памяти. Перезаписать? (Y/N) ");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    s_savedMatrices[matrixName] = matrix;
                    return;
                }
                Console.WriteLine();
            }
            
            // There are no conflicts, just add the matrix.
            s_savedMatrices.Add(matrixName, matrix);
        }

        /// <summary>
        /// Lists all matrices saved.
        /// </summary>
        private static void ListSavedMatrices()
        {
            Console.WriteLine("\nСохранённые в памяти матрицы:");
            foreach (KeyValuePair<char, double[,]> keyValuePair in s_savedMatrices)
            {
                Console.WriteLine($"{keyValuePair.Key} - {keyValuePair.Value.GetLength(0)}" +
                                  $"x{keyValuePair.Value.GetLength(1)}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Parses the header of matrix in the file.
        /// </summary>
        /// <param name="header">String of the header.</param>
        /// <param name="matrixName">Name of the matrix parsed.</param>
        /// <param name="rows">Number of rows of the matrix parsed.</param>
        /// <param name="columns">Number of columns of the matrix parsed.</param>
        /// <returns>Whether the header is valid.</returns>
        private static bool ParseFileMatrixHeader(string header, out char matrixName, out int rows, out int columns)
        {
            matrixName = default;
            rows = default;
            columns = default;
            
            // Check that header is not empty and begins and ends appropriately.
            if (header.Length < 1 || header[0] != '[' || header[^1] != ']')
            {
                return false;
            }
            
            // Split header into parameters of matrix.
            string[] matrixDef = header.Substring(1, header.Length - 2).Split(':');
            
            // Check that header contains correct number of parameters and matrix name has appropriate length.
            if (matrixDef.Length != 3 || matrixDef[0].Length != 1)
            {
                return false;
            }

            matrixName = matrixDef[0][0];
            // Check that matrix name is a latin letter.
            if (!(matrixName is >= 'a' and <= 'z' or >= 'A' and <= 'Z'))
            {
                return false;
            }

            // Check that row and column numbers are valid.
            if (!int.TryParse(matrixDef[1], out rows) || rows is <= 0 or > 25 ||
                !int.TryParse(matrixDef[2], out columns) || columns is <= 0 or > 25)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parses a row of matrix from a string.
        /// </summary>
        /// <param name="line">String to parse.</param>
        /// <param name="matrix">Matrix where to add parsed elements.</param>
        /// <param name="currentRow">Number of row parsing.</param>
        /// <returns>Whether the row is valid.</returns>
        private static bool ParseMatrixRow(string line, ref double[,] matrix, int currentRow)
        {
            // Check if row is not out of bounds of the matrix.
            if (matrix == null || currentRow >= matrix.GetLength(0))
            {
                return false;
            }

            string[] array = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Check if number of elements is correct.
            int columns = matrix.GetLength(1);
            if (array.Length != columns)
            {
                return false;
            }

            // Parse each element.
            for (var i = 0; i < columns; i++)
            {
                if (!double.TryParse(array[i], NumberStyles.Any, CultureInfo.InvariantCulture,
                    out matrix[currentRow, i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Writes all matrices in memory to file.
        /// </summary>
        private static void WriteMatrixFile()
        {
            // Get file path from the user.
            Console.WriteLine("Введите абсолютный или относительный путь к файлу для записи:");
            string filePath;
            do
            {
                Console.Write("Путь> ");
                filePath = Console.ReadLine();
            } while (string.IsNullOrEmpty(filePath));

            // If file exists, prompt user to overwrite it. 
            if (File.Exists(filePath))
            {
                ConsoleKey key;
                do
                {
                    Console.Write("Файл уже существует. Перезаписать? (Y/N)? ");
                    key = Console.ReadKey().Key;
                    Console.WriteLine();
                } while (key != ConsoleKey.Y && key != ConsoleKey.N);

                if (key == ConsoleKey.Y)
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Ошибка записи файла: {e.Message}");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            
            WriteDataToFile(filePath);
        }

        /// <summary>
        /// Writes data to the file.
        /// </summary>
        /// <param name="filePath">Path to the file to write.</param>
        private static void WriteDataToFile(string filePath)
        {
            // Working with files — needing extreme caution.
            try
            {
                using var streamWriter = new StreamWriter(filePath);
                foreach (KeyValuePair<char, double[,]> keyValuePair in s_savedMatrices)
                {
                    int rows = keyValuePair.Value.GetLength(0);
                    int columns = keyValuePair.Value.GetLength(1);
                    
                    // Write the header.
                    streamWriter.WriteLine($"[{keyValuePair.Key}:{rows}:{columns}]");

                    // Write all rows.
                    for (var i = 0; i < rows; i++)
                    {
                        for (var j = 0; j < columns; j++)
                        {
                            streamWriter.Write(
                                $"{FormatNumber(keyValuePair.Value[i, j], CultureInfo.InvariantCulture)} ");
                        }
                        streamWriter.WriteLine();
                    }
                    streamWriter.WriteLine();
                }
                Console.WriteLine("Успешно данные записаны в файл.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка записи файла: {e.Message}");
            }
        }

        /// <summary>
        /// Get the trace of input matrix.
        /// </summary>
        private static void MatrixTrace()
        {
            double[,] matrix = GetMatrix();
            
            // Check that matrix is square.
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                Console.WriteLine("След можно найти только для квадратной матрицы.\n");
                return;
            }
            
            Console.WriteLine($"След матрицы равен {FormatNumber(Matrix.Trace(matrix))}.\n");
        }

        /// <summary>
        /// Get the determinant of input matrix.
        /// </summary>
        private static void MatrixDeterminant()
        {
            double[,] matrix = GetMatrix();
            
            // Check that matrix is square.
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                Console.WriteLine("Определитель можно найти только для квадратной матрицы.\n");
                return;
            }
            
            Console.WriteLine($"Определитель матрицы равен {FormatNumber(Matrix.Determinant(matrix))}.\n");
        }

        /// <summary>
        /// Get the transpose of input matrix.
        /// </summary>
        private static void MatrixTranspose()
        {
            double[,] matrix = GetMatrix();
            DisplaySave(Matrix.Transpose(matrix));
        }

        /// <summary>
        /// Get solutions of input system of equations in matrix form.
        /// </summary>
        private static void MatrixSolveGaussian()
        {
            Console.WriteLine("Необходимо ввести матрицу СЛАУ для решения.\n");
            double[,] matrix = GetMatrix();
            Matrix.SolveGaussian(matrix, out double[,] solution);
            DisplaySave(solution, true);
        }

        /// <summary>
        /// Get the sum of the matrices.
        /// </summary>
        private static void MatrixSum()
        {
            double[,] matrix1 = GetMatrix(1);
            double[,] matrix2 = GetMatrix(2);
            
            // Check that matrices have the same dimensions.
            if (matrix1.GetLength(0) != matrix2.GetLength(0) ||
                matrix1.GetLength(1) != matrix2.GetLength(1))
            {
                Console.WriteLine("Складывать можно только матрицы одного типа.\n");
                return;
            }
            
            DisplaySave(Matrix.Sum(matrix1, matrix2));
        }

        /// <summary>
        /// Get the difference of the matrices.
        /// </summary>
        private static void MatrixDifference()
        {
            double[,] matrix1 = GetMatrix(1);
            double[,] matrix2 = GetMatrix(2);
            
            // Check that matrices have the same dimensions.
            if (matrix1.GetLength(0) != matrix2.GetLength(0) ||
                matrix1.GetLength(1) != matrix2.GetLength(1))
            {
                Console.WriteLine("Вычитать можно только матрицы одного типа.\n");
                return;
            }
            
            DisplaySave(Matrix.Difference(matrix1, matrix2));
        }

        /// <summary>
        /// Get the product of the matrices.
        /// </summary>
        private static void MatrixProduct()
        {
            double[,] matrix1 = GetMatrix(1);
            double[,] matrix2 = GetMatrix(2);
            
            // Check that matrices are suitable for multiplication.
            if (matrix1.GetLength(1) != matrix2.GetLength(0))
            {
                Console.WriteLine("Умножать можно только такие матрицы, что число столбцов первой равно " +
                                  "числу строк второй.\n");
                return;
            }
            
            DisplaySave(Matrix.Product(matrix1, matrix2));
        }

        /// <summary>
        /// Factorize the matrix.
        /// </summary>
        private static void MatrixFactorize()
        {
            double[,] matrix = GetMatrix();
            
            // Get the factor.
            double factor;
            bool valid;
            Console.WriteLine("Введите число, на которое нужно умножить матрицу:");
            do
            {
                Console.Write("Множитель> ");
                string input = Console.ReadLine();
                valid = double.TryParse(input, out factor);

                if (!valid)
                {
                    Console.WriteLine("Неверный ввод.\n");
                }
            } while (!valid);
            
            DisplaySave(Matrix.Factorize(matrix, factor));
        }
    }
}