using System;
using System.Text.RegularExpressions;

namespace ReverseDateFormats
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter a date in MM/DD/YYYY or MM/DD/YY format (or type 'exit' to quit):");
                string inputDate = Console.ReadLine();
                
                if (inputDate.ToLower() == "exit")
                {
                    break;
                }

                try
                {
                    // Validate the input date format
                    if (DateTime.TryParse(inputDate, out _))
                    {
                        // Reverse the date format to "YYYY-MM-DD"
                        string reversedDate = ReverseDateFormat(inputDate);
                        Console.WriteLine("Reversed Date (YYYY-MM-DD): " + reversedDate);
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter a valid date.");
                    }
                }
                catch (RegexMatchTimeoutException ex)
                {
                    Console.WriteLine("The regex operation timed out. Returning the original input date.");
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Invalid input date format.");
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid date format.");
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine();
            }
        }

        public static string ReverseDateFormat(string date)
        {
            // Regular expression with named capturing groups
            string pattern = @"^(?<mon>\d{1,2})/(?<day>\d{1,2})/(?<year>\d{2}|\d{4})$";
            Regex regex = new Regex(pattern);

            // Set a timeout of 1 second
            TimeSpan timeout = TimeSpan.FromSeconds(1);

            try
            {
                Match match = regex.Match(date, 0, timeout);
                if (match.Success)
                {
                    // Extract the named groups
                    string month = match.Groups["mon"].Value.PadLeft(2, '0');
                    string day = match.Groups["day"].Value.PadLeft(2, '0');
                    string year = match.Groups["year"].Value;

                    // Handle 2-digit year by converting it to 4 digits
                    // Assuming dates are in the 1900s if year is less than or equal to current year
                    if (year.Length == 2)
                    {
                        int twoDigitYear = int.Parse(year);
                        year = (twoDigitYear <= DateTime.Now.Year % 100 ? "20" : "19") + year;
                    }

                    // Return the reversed date format
                    return $"{year}-{month}-{day}";
                }
                else
                {
                    throw new ArgumentException("Date must be in a valid format (MM/DD/YY or MM/DD/YYYY).");
                }
            }
            catch (RegexMatchTimeoutException)
            {
                throw;
            }
        }
    }
}
